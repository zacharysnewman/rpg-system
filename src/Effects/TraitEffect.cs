class TraitEffect : IEffect
{
    public string TraitName; // "Speed", "Bravery", etc.
    public float ModifierAmount;
    public Duration Duration; // Could be permanent or timed
    public bool IsRelative; // true = +=, false = absolute set

    public void Apply(Character target) { ... }
}