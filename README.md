# rpg-system

A flexible, interface-driven RPG game mechanics engine written in C#. Provides the core building blocks for turn-based combat systems: characters, abilities, effects, and resources.

## Concepts

### Character
The central entity. Each character has:
- **Resources** — pools with min/max constraints (e.g. Health, Mana)
- **Traits** — numeric attributes (e.g. Speed, Bravery)
- **Active Abilities** — cooldown-based, optionally ranged and targeted
- **Passive Abilities** — trigger-based abilities with conditional logic
- **Position / Dimension** — for spatial and teleportation/summoning mechanics

### Abilities
| Type | Description |
|---|---|
| `Ability` | Active ability with cooldown, range, and a list of effects |
| `PassiveAbility` | Extends `Ability`; fires automatically on a `TriggerType` (e.g. `OnHitTaken`, `OnLowHealth`, `Always`) when a `Condition` is met |

**Condition** — evaluates a trait against a threshold using a `ComparisonType` to gate passive ability triggers.

### Effects
All effects implement `IEffect` and are applied to a `Character` target.

| Effect | Description |
|---|---|
| `TraitEffect` | Modify a trait by relative (`+=`) or absolute value, with optional duration |
| `ResourceEffect` | Damage or heal a resource; set `IsDrain` to transfer the amount to the caster |
| `EnvironmentalEffect` | Create an area effect anchored to a `Position` with a radius, terrain modification, and a list of sub-effects |
| `NegationEffect` | Cancel an in-progress ability cast or silence a target; configurable success chance |

### Interfaces
| Interface | Purpose |
|---|---|
| `IEffect` | `Apply(Character)`, `Name`, `EffectType` |
| `ITrait` | `Name`, `Value` |
| `IResource` | `Name`, `CurrentValue`, `MaxValue`, `Modify(float)` |

## Project Structure

```
src/
├── Character.cs
├── Ability.cs
├── IEffect.cs
├── ITrait.cs
├── IResource.cs
├── Abilities/
│   ├── PassiveAbility.cs
│   └── Condition.cs
└── Effects/
    ├── TraitEffect.cs
    ├── ResourceEffect.cs
    ├── EnvironmentalEffect.cs
    ├── NegationEffect.cs
    └── EnvironmentArea.cs
```

## License

MIT
