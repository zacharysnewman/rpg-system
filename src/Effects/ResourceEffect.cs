class ResourceEffect : IEffect
{
    public string ResourceName; // "Health", "Mana", etc.
    public float Amount; // Positive = heal, Negative = damage
    public bool IsDrain; // If true, caster gains what target loses

    public void Apply(Character target) { ... }
}