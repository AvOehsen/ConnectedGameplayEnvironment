#include "blinkModule.h"
#include <Arduino.h>

BlinkModule::BlinkModule(uint8_t pin)
{
    m_iLedPin = pin;
    pinMode(m_iLedPin, OUTPUT);
}

void BlinkModule::ProcessCommand(JsonObject& command)
{
    if(command["Type"] == "blink_command" && command.containsKey("State"))
    {
        int state = command["State"].as<int>();
        if(state == 0)
            digitalWrite(m_iLedPin, LOW);
        else if(state == 1)
            digitalWrite(m_iLedPin, HIGH);
    }
}

//int _lastUpdate;
void BlinkModule::Update()
{
}