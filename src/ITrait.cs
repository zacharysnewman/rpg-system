interface ITrait
{
    string Name { get; }
    float BaseValue { get; set; }
    List<TraitModifier> Modifiers { get; }

    // Computed: (BaseValue + Σ flat modifiers) × (1 + Σ percentage modifiers)
    // Percentage modifiers are summed additively before being applied as a single multiplier.
    float Value { get; }
}
