# include "baseModule.h"

void AbstractBaseModule::Initialize(JsonObject& declaration)
{
    declaration["type"] = GetTypeIdentifier();
}

void AbstractBaseModule::ProcessCommand(JsonObject& command)
{

}

void AbstractBaseModule::Update()
{

}
