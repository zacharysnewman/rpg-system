class ActiveEffect
{
    public IEffect Effect;
    public Character Source;    // Character who applied this effect
    public float ExpiresAt;     // Absolute timestamp; float.MaxValue if permanent
}
