class EnvironmentalEffect : IEffect
{
    public EnvironmentArea Area;
    public List<IEffect> AreaEffects;
    public Duration Duration;

    public void Apply(Character target)
    {
        // Affects area or creates a trap that triggers on contact
    }
}