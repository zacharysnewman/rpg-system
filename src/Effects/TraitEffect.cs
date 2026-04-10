class TraitEffect : IEffect
{
    public string TraitName;        // "Speed", "Bravery", etc.
    public float ModifierAmount;    // Flat units, or decimal percentage (e.g. 0.5 = +50%)
    public ModifierType ModifierType;
    public Duration Duration;

    public void Apply(Character target)
    {
        // Adds a TraitModifier referencing this effect to the target trait's Modifiers list.
        // The trait's computed Value updates immediately.
        // When this effect is removed from Character.ActiveEffects, its TraitModifier
        // is removed by matching Source == this, and Value recomputes automatically.
    }
}
