#include <FS.h>
#include "cgeRunner.h"

#include <Esp.h>
#include <WiFiManager.h>

CgeRunner::CgeRunner(AbstractBaseModule *modules[], int modulesCount)
{
    m_pModules = new AbstractBaseModule*[modulesCount]; 

    for(int i = 0 ; i < modulesCount; i++)
        m_pModules[i] = modules[i];
    
    m_iModulesCount = modulesCount;

    strcpy(device_id, "uninitialized_esp_device");
    strcpy(server_ip, "ip_not_set");

    //CgeRunner::_shouldSaveConfig = false;
}

void CgeRunner::Setup()
{
    pinMode(LED_BUILTIN, OUTPUT);

    bool configMode = MayEnterConfigMode();
    SetupWiFi(configMode);

    pinMode(LED_BUILTIN, HIGH);

    Serial.print("connecting to server '");
    Serial.print(server_ip);
    Serial.println("' ...");
    if(!m_WiFiClient.connect(server_ip, server_port))
    {
        Serial.println("Connection to server failed!");
        delay(10000);
        ESP.reset();
        return;
    }
    
    Serial.println("Connected to server!");

    JsonObject& obj = m_JsonBuffer.createObject();
    obj["type"] = "definition";
    obj["deviceId"] = device_id;
    
    JsonArray& array = obj.createNestedArray("modules");
    for(int i = 0; i < m_iModulesCount; i++)
        m_pModules[i]->Initialize(array.createNestedObject());

    obj.printTo(m_WiFiClient);
    m_WiFiClient.println();
}

void CgeRunner::Update()
{
    m_JsonBuffer.clear();
    JsonArray& eventContext = m_JsonBuffer.createArray();
    for(int i=0; i < m_iModulesCount; i++)
        m_pModules[i]->SetEventContext(eventContext);

    if (m_WiFiClient.available())
    {
        char c = m_WiFiClient.read();
        m_sNetStringBuffer += c;
        if (c == '\n')
        {
            JsonObject& msg = m_JsonBuffer.parseObject(m_sNetStringBuffer);
            if(msg.success())
            {
                Serial.print("[<--] ");
                msg.printTo(Serial);
                Serial.println();

                //TODO: filter commands per module ?
                for (int i = 0; i < m_iModulesCount; i++)
                    m_pModules[i]->ProcessCommand(msg); 
            }
            m_sNetStringBuffer = "";
        }
    }

    for(int i=0; i < m_iModulesCount; i++)
        m_pModules[i]->Update();
    
    if(eventContext.size() > 0)
    {
        eventContext.printTo(m_WiFiClient);
        m_WiFiClient.println();

        Serial.print("[-->] ");
        eventContext.printTo(Serial);
        Serial.println();
    }
}

bool CgeRunner::MayEnterConfigMode()
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

//nasty callbacks for WiFi manager
bool CgeRunner::_shouldSaveConfig;
void OnSetSaveConfig() { CgeRunner::_shouldSaveConfig = true; }
void OnEnterConfigMode(WiFiManager *mgr) { pinMode(LED_BUILTIN, LOW);}

void CgeRunner::SetupWiFi(bool enterConfigMode)
{
    WiFiManager mgr;
    mgr.setBreakAfterConfig(true);
    mgr.setConnectTimeout(20);
    mgr.setSaveConfigCallback(OnSetSaveConfig);
    mgr.setAPCallback(OnEnterConfigMode);

    if(SPIFFS.begin())
    {
        if(SPIFFS.exists("/config.json"))
        {
            File configFile = SPIFFS.open("/config.json", "r");
            JsonObject &config = m_JsonBuffer.parse(configFile);

            if(config.success())
            {
                strcpy(device_id, config["device_id"]);
                strcpy(server_ip, config["server_ip"]);
            }
            else
                enterConfigMode = true;
        }
        else
            enterConfigMode = true;
    }
    else
        enterConfigMode = true;

    WiFiManagerParameter deviceIdParam("device id", "", device_id, 30);
    WiFiManagerParameter serverIpParam("server ip", "", server_ip, 16);

    mgr.addParameter(&deviceIdParam);
    mgr.addParameter(&serverIpParam);
    
    if(enterConfigMode)
        mgr.startConfigPortal(device_id);
    else
        mgr.autoConnect(device_id);

    digitalWrite(LED_BUILTIN, HIGH);

    strcpy(device_id, deviceIdParam.getValue());
    strcpy(server_ip, serverIpParam.getValue());

    if(CgeRunner::_shouldSaveConfig)
    {
        Serial.println("saving config ...");

        JsonObject& json = m_JsonBuffer.createObject();
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

