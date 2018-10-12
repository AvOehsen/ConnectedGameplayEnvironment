#include <Arduino.h>

#include <cgeRunner.h>
#include <baseModule.h>
#include <blinkModule.h>
#include <triggerModule.h>

AbstractBaseModule *modules[] = 
{ 
    new BlinkModule(LED_BUILTIN),
    new TriggerModule(),
    new TriggerModule()
};

CgeRunner runner(modules, 3);

void setup() 
{
    Serial.begin(9600);
    runner.Setup();
}

void loop()
{
    runner.Update();
}