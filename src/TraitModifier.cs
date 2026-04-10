class TraitModifier
{
    public IEffect Source;       // The effect that added this modifier; used to remove it on expiry
    public ModifierType ModifierType;
    public float Amount;         // Flat units, or decimal percentage (e.g. 0.5 = +50%)
}
