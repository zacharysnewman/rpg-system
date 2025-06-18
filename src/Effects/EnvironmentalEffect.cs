class EnvironmentalEffect : IEffect
{
    public EnvironmentArea Area;
    public List<IEffect> AreaEffects;
    public float Duration;

    public void Apply(Character target)
    {
        // Affects area or creates a trap that triggers on contact
    }
}