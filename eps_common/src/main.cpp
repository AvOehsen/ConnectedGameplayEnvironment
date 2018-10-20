#include <Arduino.h>

#include <cgeRunner.h>
#include <baseModule.h>
#include <blinkModule.h>
#include <irReceiverModule.h>
#include <irShooterModule.h>
#include <triggerModule.h>

AbstractBaseModule *modules[] = 
{ 
    new BlinkModule(LED_BUILTIN),
    new IrShooterModule(D5, D1, /*LED_BUILTIN*/ D6, D8),
    new IrReceiverModule(D3)
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