enum StackingPolicy
{
    Stack,           // Each application is a separate independent instance
    RefreshDuration, // Resets the duration of the existing instance; does not add a new one
    Ignore,          // Does nothing if the effect is already active on the target
}
