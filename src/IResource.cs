interface IResource
{
    string Name { get; }
    float CurrentValue { get; set; }
    float MaxValue { get; }
    void Modify(float amount);
}