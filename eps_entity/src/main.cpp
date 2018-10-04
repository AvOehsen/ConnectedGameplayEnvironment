#include <Arduino.h>
#include <ESP8266WiFi.h>
#include <WiFiClient.h>
#include <ArduinoJson.h>

#include "modules/baseModule.h"
#include "modules/triggerModule.h"

//TODO
#define WIFI_SSID "<your wlan ssid>"    
#define WIFI_PASS "<your wlan pass>"
#define SERVER_IP "192.168.2.1"
#define SERVER_PORT 11000
#define UNIQUE_DEVICE_ID "dummy"

WiFiClient _client;
DynamicJsonBuffer _buffer(512);
String _stringBuffer;

const int _modulesCount = 3;
AbstractBaseModule *_modules[_modulesCount];


void defineModules()
{
    _modules[0] = new TriggerModule();
    _modules[1] = new TriggerModule();
    _modules[2] = new TriggerModule();
}

void setup() 
{
    defineModules();

    Serial.begin(9600);
    WiFi.begin(WIFI_SSID, WIFI_PASS);
    while(WiFi.status() != WL_CONNECTED)
    {
        delay(500);
        Serial.print(".");
    }

    Serial.println(" connected to WiFi");
    Serial.println(WiFi.localIP());
    
    if(!_client.connect(SERVER_IP, SERVER_PORT))
    {
        Serial.println("connection to server failed!");
    }
    else
    {
        _client.setTimeout(10);

        Serial.println("conntected to server!");

        JsonObject& obj = _buffer.createObject();
        obj["id"] = UNIQUE_DEVICE_ID;
        
        JsonArray& array = obj.createNestedArray("modules");
        for(int i = 0; i < _modulesCount; i++)
            _modules[i]->Initialize(array.createNestedObject());

        obj.printTo(_client);
        _client.println();
    
        Serial.println("sent:");
        obj.printTo(Serial);
        Serial.println();
    }
}

void parseMessage(const char* json)
{
    JsonObject& msg = _buffer.parseObject(json);
    if(msg.success())
    {
        Serial.println("recieved");
        msg.printTo(Serial);
        Serial.println();

        //TODO: pass commands to modules
    }
}

void loop() 
{
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
    
    //TODO: get and send events

}