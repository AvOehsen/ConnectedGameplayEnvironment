# include "baseModule.h"

void AbstractBaseModule::Initialize(JsonObject& declaration)
{
    declaration["type"] = GetTypeIdentifier();
}

void AbstractBaseModule::ProcessCommand(JsonObject& command){}
void AbstractBaseModule::Update(){}


void AbstractBaseModule::SetEventContext(JsonArray& context)
{
    _eventContext = &context;
}

JsonObject& AbstractBaseModule::SendEvent(const char* type)
{
    JsonObject& result = _eventContext->createNestedObject();
    result["type"] = type;

    return result;
}