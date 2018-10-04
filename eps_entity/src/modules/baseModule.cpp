# include "baseModule.h"

void AbstractBaseModule::Initialize(JsonObject& declaration)
{
    declaration["identifier"] = GetIdentifier();
}

void AbstractBaseModule::ProcessCommand(JsonObject& command)
{

}

void AbstractBaseModule::Update()
{

}
