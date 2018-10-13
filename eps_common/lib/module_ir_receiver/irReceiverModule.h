#include <baseModule.h>

#include <IRremoteESP8266.h>
#include <IRrecv.h>

class IrReceiverModule : public AbstractBaseModule
{
    enum e_ReceiveState
    {
        DISABLED, ENABLED
    };

private:
    const char* COMMAND_ENABLE = "ir_enable";
    const char* COMMAND_DISABLE = "ir_disable";

    const char* EVENT_RECEIVED = "ir_received";

public:
    IrReceiverModule(uint8_t pin);
    void ProcessCommand(JsonObject& command);
    void Update();

protected:
    const char* GetTypeIdentifier() const { return "ir_receiver"; };

private:
    uint8_t m_iReceiverPin;
    e_ReceiveState m_eCurrentState;
    IRrecv *m_pIRecv;
};

