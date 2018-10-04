#pragma once
#include <ArduinoJson.h>

class AbstractBaseModule
{
public:
    virtual void Initialize(JsonObject& declaration);
    virtual void ProcessCommand(JsonObject& command);
    virtual void Update();

protected:
    virtual const char* GetIdentifier() const = 0;

private:

};