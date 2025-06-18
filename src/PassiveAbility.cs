class PassiveAbility : Ability
{
    public TriggerType Trigger; // OnHitTaken, OnLowHealth, Always, etc.
    public Condition Condition;
}