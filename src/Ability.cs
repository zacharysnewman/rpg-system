class Ability
{
    public string Name;
    public List<IEffect> Effects;
    public float Cooldown;
    public float Range;
    public bool RequiresTarget;

    public void Activate(Character caster, Character target);
}