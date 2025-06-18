class Character
{
    public string Id;
    public string Name;
    
    public Dictionary<string, IResource> Resources;
    public Dictionary<string, ITrait> Traits;
    
    public List<PassiveAbility> PassiveAbilities;
    public List<Ability> ActiveAbilities;

    public Position Position; // for spatial logic
    public Dimension CurrentDimension; // for teleportation/summoning
}