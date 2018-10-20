#include "irShooterModule.h"
#include <Arduino.h>

IrShooterModule::IrShooterModule(uint8_t pin_ir, uint8_t pin_trigger, uint8_t pin_shotVis, uint8_t pin_salveVis)
{
    m_pISend = new IRsend(pin_ir);
    m_pISend->begin();
    
    m_iPinTrigger = pin_trigger;
    
    pinMode(pin_trigger, INPUT);

    if(pin_shotVis > 0)
    {
        m_iPinShotVis = pin_shotVis;
        pinMode(pin_shotVis, OUTPUT);
    }

    if(pin_salveVis > 0)
    {
        m_iPinSalveVis = pin_salveVis;
        pinMode(pin_salveVis, OUTPUT);
    }

    m_iCfgWarmup = 1000;
    m_iCfgSalveDelay = 100;
    m_iCfgSalveCount = 1;
    m_iCfgCooldown = 1000;
}

/*void IrShooterModule::Initialize(JsonObject& declaration)
{
    declaration["HasShotVis"] = m_iPinShotVis > 0;
    declaration["HasSalveVis"] = m_iPinSalveVis > 0;
}*/

void IrShooterModule::ProcessCommand(JsonObject& command)
{
    String type = command["Type"];

    if(type == COMMAND_CONFIG)
    {
        m_iCfgWarmup = command["Warmup"].as<int>();
        m_iCfgSalveDelay = command["SalveDelay"].as<int>();
        m_iCfgSalveCount = command["SalveCount"].as<int>();
        m_iCfgCooldown = command["Cooldown"].as<int>();
    }
    else if(type == COMMAND_SHOOT)
    {
        Shoot();
    }
}

void IrShooterModule::Update()
{
    if(digitalRead(m_iPinTrigger) == HIGH)
    {
        Serial.println("shoot pressed");
        Shoot();
    }
}

void IrShooterModule::Shoot()
{
    Serial.println("shooting ...");

    //TODO: make this not blocking!
    if(m_iPinSalveVis > 0)
        digitalWrite(m_iPinSalveVis, HIGH);

    delay(m_iCfgWarmup);

    for(int i = 0; i < m_iCfgSalveCount; i++)
    {
        delay(m_iCfgSalveDelay);

        if(m_iPinShotVis > 0)
            digitalWrite(m_iPinShotVis, HIGH);

        digitalWrite(LED_BUILTIN, LOW);

        uint64_t msg =
            m_iCfgGameId << 0 | 
            m_iCfgShooterId << 8 |
            m_iCurrentSalve << 16 |
            i << 22;

        m_pISend->sendNEC(msg);
        Serial.println("PEW");

        digitalWrite(LED_BUILTIN, HIGH);

        if(m_iPinShotVis > 0)
            digitalWrite(m_iPinShotVis, LOW);
    }

    delay(m_iCfgCooldown);

    if(m_iPinSalveVis > 0)
        digitalWrite(m_iPinSalveVis, LOW);

    if(m_iCurrentSalve >= 5)
        m_iCurrentSalve = 0;
    else
        m_iCurrentSalve++;

    Serial.println("shot done!");
}