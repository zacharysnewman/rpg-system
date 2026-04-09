# RPG System Specification

A flexible, interface-driven RPG game mechanics engine written in C#. Provides core building blocks for real-time combat systems: characters, abilities, effects, and resources.

---

## Project Structure

```
src/
├── Character.cs              # Central entity class
├── Ability.cs                # Active ability class
├── IEffect.cs                # Base interface for all effects
├── ITrait.cs                 # Interface for character attributes
├── IResource.cs              # Interface for resource pools
├── Abilities/
│   ├── PassiveAbility.cs     # Event-triggered ability subclass
│   └── Condition.cs          # Trait-threshold gate for passive abilities
└── Effects/
    ├── TraitEffect.cs         # Modifies character traits
    ├── ResourceEffect.cs      # Damages/heals resources; supports drain
    ├── EnvironmentalEffect.cs # Area effect anchored to a position
    ├── EnvironmentArea.cs     # Spatial definition for environmental effects
    └── NegationEffect.cs      # Cancels casts or silences targets
```

---

## Interfaces

### `IEffect` (`src/IEffect.cs`)
Base contract for every effect in the system.

| Member | Type | Description |
|---|---|---|
| `Name` | `string` | Identifier for the effect |
| `EffectType` | `EffectType` | Enum category (e.g. `ResourceManipulation`, `TraitManipulation`) |
| `Apply(Character target)` | `void` | Applies the effect to the given target |

### `ITrait` (`src/ITrait.cs`)
A single numeric character attribute.

| Member | Type | Description |
|---|---|---|
| `Name` | `string` | Attribute name (e.g. `"Speed"`, `"Bravery"`) |
| `Value` | `float` | Current attribute value (readable and writable) |

### `IResource` (`src/IResource.cs`)
A bounded resource pool.

| Member | Type | Description |
|---|---|---|
| `Name` | `string` | Resource name (e.g. `"Health"`, `"Mana"`) |
| `CurrentValue` | `float` | Current amount (readable and writable) |
| `MaxValue` | `float` | Maximum capacity (read-only) |
| `Modify(float amount)` | `void` | Adjusts `CurrentValue`, respecting min/max bounds |

---

## Core Classes

### `Character` (`src/Character.cs`)
The central game entity. All combat, abilities, and effects operate on characters.

| Field | Type | Description |
|---|---|---|
| `Id` | `string` | Unique identifier |
| `Name` | `string` | Display name |
| `Resources` | `Dictionary<string, IResource>` | Named resource pools (Health, Mana, etc.) |
| `Traits` | `Dictionary<string, ITrait>` | Named numeric attributes (Speed, Bravery, etc.) |
| `ActiveAbilities` | `List<Ability>` | Cooldown-based, manually activated abilities |
| `PassiveAbilities` | `List<PassiveAbility>` | Event-triggered abilities with conditional logic |
| `Position` | `Position` | Spatial location for range and area checks |
| `CurrentDimension` | `Dimension` | Current plane; used for teleportation and summoning |

---

## Ability System

### `Ability` (`src/Ability.cs`)
An active ability a character can use.

| Field | Type | Description |
|---|---|---|
| `Name` | `string` | Ability name |
| `Effects` | `List<IEffect>` | Effects applied on activation |
| `Cooldown` | `float` | Reuse delay in seconds |
| `Range` | `float` | Maximum distance from caster to target |
| `RequiresTarget` | `bool` | Whether a target `Character` must be provided |

**Method:** `Activate(Character caster, Character target)` — validates range and cooldown, then applies all `Effects` to `target`.

### `PassiveAbility` (`src/Abilities/PassiveAbility.cs`)
Extends `Ability`. Fires automatically when a trigger event occurs and a condition is satisfied.

| Field | Type | Description |
|---|---|---|
| `Trigger` | `TriggerType` | Event that evaluates this ability (e.g. `OnHitTaken`, `OnLowHealth`, `Always`) |
| `Condition` | `Condition` | Trait-threshold gate; must pass before effects fire |

### `Condition` (`src/Abilities/Condition.cs`)
Evaluates a character's trait against a threshold to gate passive ability activation.

| Field | Type | Description |
|---|---|---|
| `TraitName` | `string` | Trait to inspect (e.g. `"Bravery"`) |
| `Comparison` | `ComparisonType` | How to compare (`GreaterThan`, `LessThan`, `Equals`, etc.) |
| `Threshold` | `float` | Value to compare against |

**Example:** `Condition { TraitName = "Health", Comparison = LessThan, Threshold = 0.2 }` triggers only when health trait is below 20%.

---

## Effect System

All effects implement `IEffect` and are applied via `Apply(Character target)`.

### `TraitEffect` (`src/Effects/TraitEffect.cs`)
Modifies a character trait — either as a relative delta or an absolute set, with optional duration.

| Field | Type | Description |
|---|---|---|
| `TraitName` | `string` | Trait to modify (e.g. `"Speed"`) |
| `ModifierAmount` | `float` | Amount to apply |
| `Duration` | `Duration` | `Permanent` or timed (expires after N seconds) |
| `IsRelative` | `bool` | `true` = additive (`+=`); `false` = absolute set (`=`) |

**Example uses:** Speed debuff (`IsRelative = true, Amount = -15`), stat replacement, temporary buffs.

### `ResourceEffect` (`src/Effects/ResourceEffect.cs`)
Damages or heals a resource pool. Supports a drain (lifesteal) mechanic.

| Field | Type | Description |
|---|---|---|
| `ResourceName` | `string` | Resource to modify (e.g. `"Health"`) |
| `Amount` | `float` | Positive = heal, negative = damage |
| `IsDrain` | `bool` | If `true`, the caster gains what the target loses |

**Example uses:** Direct damage, healing, mana burn, vampiric strike (damage + caster heals same amount).

### `EnvironmentalEffect` (`src/Effects/EnvironmentalEffect.cs`)
Creates a persistent area effect anchored to a `Position`. Can act as a trap or lingering hazard.

| Field | Type | Description |
|---|---|---|
| `Area` | `EnvironmentArea` | Spatial bounds of the effect |
| `AreaEffects` | `List<IEffect>` | Effects applied to characters inside the area |
| `Duration` | `float` | Seconds the area persists |

`Apply(Character target)` either affects the area immediately or creates a trap that triggers on contact.

### `EnvironmentArea` (`src/Effects/EnvironmentArea.cs`)
Defines the spatial extent of an environmental effect.

| Field | Type | Description |
|---|---|---|
| `Center` | `Position` | Anchor point |
| `Radius` | `float` | Circular effect radius |
| `ModifiedTerrain` | `TerrainType` | Optional terrain change (ice, lava, mud, etc.) |

### `NegationEffect` (`src/Effects/NegationEffect.cs`)
Interrupts an in-progress ability cast or applies a silence to a target.

| Field | Type | Description |
|---|---|---|
| `TargetAbilityType` | `AbilityType` | Which class of ability to negate |
| `SuccessChance` | `float` | Probability of success (0.0–1.0) |

`Apply(Character target)` cancels a matching cast in progress or silences the target for the effect duration.

---

## Supporting Types

These types are referenced throughout the system. Their definitions are not listed in individual files but are part of the shared type system.

| Type | Description |
|---|---|
| `TriggerType` | Enum of passive ability events: `OnHitTaken`, `OnLowHealth`, `Always`, etc. |
| `ComparisonType` | Enum of comparison operators: `GreaterThan`, `LessThan`, `Equals`, etc. |
| `EffectType` | Enum categorizing effect kinds: `ResourceManipulation`, `TraitManipulation`, etc. |
| `AbilityType` | Enum used by `NegationEffect` to target specific ability categories |
| `Duration` | Represents permanent or timed effect duration |
| `Position` | Spatial coordinate (2D or 3D) used for range and area calculations |
| `Dimension` | Identifies the current plane/realm a character occupies |
| `TerrainType` | Enum of terrain modifications: ice, lava, mud, water, etc. |

---

## Key Design Patterns

- **Interface-driven extensibility** — all effects implement `IEffect`; new effect types are added by implementing the interface without modifying existing code.
- **Composition over inheritance** — abilities are composed of `List<IEffect>`, making it trivial to build complex behaviors from simple parts.
- **Dictionary-keyed collections** — `Resources` and `Traits` are accessed by name string, allowing flexible, data-driven character definitions.
- **Event-driven passives** — `PassiveAbility` reacts to `TriggerType` events and is gated by `Condition`, enabling reactive gameplay without polling.
- **Probabilistic mechanics** — `NegationEffect.SuccessChance` introduces controlled randomness for skill-based countering.
