interface IEffect
{
    string Name { get; }
    void Apply(Character target);
    EffectType EffectType { get; } // ResourceManipulation, TraitManipulation, etc.
}