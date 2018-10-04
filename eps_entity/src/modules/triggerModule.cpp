#include "triggerModule.h"
#include <Arduino.h>

void TriggerModule::Initialize(JsonObject& declaration) 
{
    AbstractBaseModule::Initialize(declaration);
    declaration["type"] = "button";
}

//int _lastUpdate;
void TriggerModule::Update()
{
    /*if(_lastUpdate > 0)
    {
        Serial.print("[TM] last update: ");
        Serial.print(millis() - _lastUpdate);
        Serial.println(" ms");
    }

    _lastUpdate = millis();*/
}