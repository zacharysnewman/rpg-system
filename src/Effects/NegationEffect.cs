class NegationEffect : IEffect
{
    public AbilityType TargetAbilityType;
    public float SuccessChance;
    public Duration Duration; // How long a silence applied by this effect lasts

    public void Apply(Character target) { 
        // Cancel ability if it’s being cast, silence, etc.
    }
}