#include <Arduino.h>
#include <WiFiClient.h>
#include <ArduinoJson.h>

#include "baseModule.h"

class CgeRunner
{
public: 
    static bool _shouldSaveConfig;

public:
    CgeRunner(AbstractBaseModule *modules[], int modulesCount);

    void Setup();
    void Update();

    

private:
    bool MayEnterConfigMode();
    void SetupWiFi(bool configMode);

    DynamicJsonBuffer m_JsonBuffer;
    
    AbstractBaseModule **m_pModules;
    int m_iModulesCount;

    char device_id[30];
    char server_ip[16];
    const int server_port = 11000;

    WiFiClient m_WiFiClient;
    String m_sNetStringBuffer;

    void SetConfigFlag(bool enabled);
};