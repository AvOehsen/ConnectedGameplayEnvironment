#include "irReceiverModule.h"

#include <IRutils.h>

IrReceiverModule::IrReceiverModule(uint8_t pin)
{
    m_iReceiverPin = pin;
    m_eCurrentState = e_ReceiveState::DISABLED;
    m_pIRecv = new IRrecv(pin, 1024, 15U);
}

void IrReceiverModule::ProcessCommand(JsonObject& command)
{
    String type = command["Type"];
    if(type == COMMAND_ENABLE)
    {
        m_pIRecv->enableIRIn();
        m_eCurrentState = e_ReceiveState::ENABLED;
    }
    else if(type == COMMAND_DISABLE)
    {
        m_pIRecv->disableIRIn();
        m_eCurrentState = e_ReceiveState::DISABLED;
    }
}

void IrReceiverModule::Update()
{
    decode_results results;
    if(m_pIRecv->decode(&results))
    {
        //TODO check if result is valid cge format!

        JsonObject& event = SendEvent(EVENT_RECEIVED);
        event["payload"] = uint64ToString(results.value, 10);

        m_pIRecv->resume();
    }
}