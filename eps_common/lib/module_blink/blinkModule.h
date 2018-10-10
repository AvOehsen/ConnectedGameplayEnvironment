#include <baseModule.h>

class BlinkModule : public AbstractBaseModule
{
public:
    BlinkModule(uint8_t pin);
    void ProcessCommand(JsonObject& command);
    void Update();

protected:
    const char* GetTypeIdentifier() const { return "blink"; };

private:
    uint8_t m_iLedPin;
};