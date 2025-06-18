class NegationEffect : IEffect
{
    public AbilityType TargetAbilityType;
    public float SuccessChance;

    public void Apply(Character target) { 
        // Cancel ability if it’s being cast, silence, etc.
    }
}