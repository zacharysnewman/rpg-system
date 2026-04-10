interface IEffect
{
    string Name { get; }
    EffectType EffectType { get; } // ResourceManipulation, TraitManipulation, etc.
    StackingPolicy StackingPolicy { get; }
    void Apply(Character target);
}