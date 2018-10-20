#include <baseModule.h>

#include <IRremoteESP8266.h>
#include <IRsend.h>

class IrShooterModule : public AbstractBaseModule
{
private:
    const char* COMMAND_CONFIG = "ir_shooter_config";
    const char* COMMAND_SHOOT = "ir_shooter_shoot";

public:
    IrShooterModule(uint8_t pin_ir, uint8_t pin_trigger, uint8_t pin_shotVis = 0, uint8_t pin_salveVis = 0);

    //void Initialize(JsonObject& declaration);
    void ProcessCommand(JsonObject& command);
    void Update();

    void Shoot();

protected:
    const char* GetTypeIdentifier() const { return "ir_shooter"; };

private:
    IRsend *m_pISend;
    int m_iPinTrigger;
    int m_iPinShotVis;
    int m_iPinSalveVis;

    int m_iCfgGameId;
    int m_iCfgShooterId;

    int m_iCfgWarmup;
    int m_iCfgSalveDelay;
    int m_iCfgSalveCount;
    int m_iCfgCooldown;

    int m_iCurrentSalve;
};