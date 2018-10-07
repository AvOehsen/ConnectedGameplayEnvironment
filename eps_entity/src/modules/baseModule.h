#pragma once
#include <ArduinoJson.h>

class AbstractBaseModule
{
public:
    virtual void Initialize(JsonObject& declaration);
    virtual void ProcessCommand(JsonObject& command);
    virtual void Update();
    void SetEventContext(JsonArray& context);

protected:
    virtual const char* GetTypeIdentifier() const = 0;
    JsonObject& SendEvent(const char *type);

private:
    JsonArray* _eventContext;
};