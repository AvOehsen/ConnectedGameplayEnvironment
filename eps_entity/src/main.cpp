#include <FS.h>
#include <Arduino.h>
#include <ESP8266WiFi.h>
#include <WiFiClient.h>
#include <ArduinoJson.h>
#include <WiFiManager.h>
#include <Esp.h>


#include "modules/baseModule.h"
#include "modules/triggerModule.h"
#include "modules/blinkModule.h"

#define SERVER_PORT 11000

char device_id[30] = "uninitialized_esp_device";
char server_ip[16] = "ip_not_set";
bool saveConfig = false;

WiFiClient _client;
DynamicJsonBuffer _buffer(512);
String _stringBuffer;

const int _modulesCount = 3;
AbstractBaseModule *_modules[_modulesCount];

void defineModules()
{
    _modules[0] = new BlinkModule(LED_BUILTIN);
    _modules[1] = new TriggerModule();
    _modules[2] = new TriggerModule();
}

bool enableManualConfig()
{
    uint32_t flag = 0;
    ESP.rtcUserMemoryRead(0, &flag, sizeof(flag));

    if(flag == 1)
    {
        flag = 0;
        ESP.rtcUserMemoryWrite(0, &flag, sizeof(flag));
        
        return true;
    }
    else
    {
        flag = 1;
        ESP.rtcUserMemoryWrite(0, &flag, sizeof(flag));
        digitalWrite(LED_BUILTIN, LOW);
        delay(1000);
        flag = 0;
        ESP.rtcUserMemoryWrite(0, &flag, sizeof(flag));
        digitalWrite(LED_BUILTIN, HIGH);

        return false;
    }
}

void onSaveConfig()
{
    saveConfig = true;
}

void onEnterConfigModus(WiFiManager *mgr)
{
    digitalWrite(LED_BUILTIN, LOW);
}

void netConfig(bool force)
{
    WiFiManager mgr;
    mgr.setBreakAfterConfig(true);
    mgr.setConnectTimeout(20);
    mgr.setSaveConfigCallback(onSaveConfig);
    mgr.setAPCallback(onEnterConfigModus);

    if(SPIFFS.begin())
    {
        if(SPIFFS.exists("/config.json"))
        {
            File configFile = SPIFFS.open("/config.json", "r");
            JsonObject &config = _buffer.parse(configFile);

            if(config.success())
            {
                strcpy(device_id, config["device_id"]);
                strcpy(server_ip, config["server_ip"]);
            }
            else
                force = true;
        }
        else
            force = true;
    }
    else
        force = true;

    WiFiManagerParameter deviceIdParam("device id", "", device_id, 30);
    WiFiManagerParameter serverIpParam("server ip", "", server_ip, 16);

    mgr.addParameter(&deviceIdParam);
    mgr.addParameter(&serverIpParam);
    
    if(force)
        mgr.startConfigPortal(device_id);
    else
        mgr.autoConnect(device_id);

    digitalWrite(LED_BUILTIN, HIGH);

    strcpy(device_id, deviceIdParam.getValue());
    strcpy(server_ip, serverIpParam.getValue());

    if(saveConfig)
    {
        Serial.println("saving config ...");

        JsonObject& json = _buffer.createObject();
        json["device_id"] = device_id;
        json["server_ip"] = server_ip;

        File configFile = SPIFFS.open("/config.json", "w");
        json.printTo(configFile);
        configFile.close();

        Serial.println("save config success!");

                        //there is an issue with some ESP modules just not connecting
        ESP.reset();    //reset the ESP once a new config has been saved
    }
}

void setup() 
{
    Serial.begin(9600);
    defineModules();

    pinMode(LED_BUILTIN, OUTPUT);
    digitalWrite(LED_BUILTIN, HIGH);

    bool rebuildConfig = enableManualConfig(); 
    netConfig(rebuildConfig);

    digitalWrite(LED_BUILTIN, HIGH);
        
    if(!_client.connect(server_ip, SERVER_PORT))
    {
        Serial.println("connection to server failed!");
    }
    else
    {
        _client.setTimeout(10);

        Serial.println("conntected to server!");

        JsonObject& obj = _buffer.createObject();
        obj["type"] = "definition";
        obj["deviceId"] = device_id;
        
        JsonArray& array = obj.createNestedArray("modules");
        for(int i = 0; i < _modulesCount; i++)
            _modules[i]->Initialize(array.createNestedObject());

        obj.printTo(_client);
        _client.println();
    
        Serial.print("[sent] ");
        obj.printTo(Serial);
        Serial.println();
    }
}

void parseMessage(const char* json)
{
    JsonObject& msg = _buffer.parseObject(json);
    if(msg.success())
    {
        Serial.print("[read] ");
        msg.printTo(Serial);
        Serial.println();

        //TODO: filter commands per module ?
        for (int i = 0; i < _modulesCount; i++)
            _modules[i]->ProcessCommand(msg); 
    }
}

void loop() 
{
    _buffer.clear();
    JsonArray& eventContext = _buffer.createArray();
    for(int i=0; i < _modulesCount; i++)
        _modules[i]->SetEventContext(eventContext);

    if (_client.available())
    {
        char c = _client.read();
        _stringBuffer += c;
        if (c == '\n')
        {
            parseMessage(_stringBuffer.c_str());
            _stringBuffer = "";
        }
    }

    for(int i=0; i < _modulesCount; i++)
        _modules[i]->Update();
    
    if(eventContext.size() > 0)
    {
        eventContext.printTo(_client);
        _client.println();

        Serial.print("[sent] ");
        eventContext.printTo(Serial);
        Serial.println();
    }
    //TODO: get and send events

}