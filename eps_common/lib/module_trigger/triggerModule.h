#include <baseModule.h>

class TriggerModule : public AbstractBaseModule
{
public:
    void Initialize(JsonObject& declaration);
    //void Configure(JsonObject& config) const;
    void Update();

protected:
    const char* GetTypeIdentifier() const { return "trigger"; };
};