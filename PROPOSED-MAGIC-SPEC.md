# Proposed Magic Framework

A unified framework for describing all RPG abilities in terms of five categories. The core insight is that **Protection and Immunity are just Trait Manipulation** — granting fire immunity is equivalent to setting a `FireResistance` trait to 100%.

---

## Core Framework

### 1. Resource Manipulation

**Definition:** Alters consumable resources like health, mana, stamina, cooldowns, or energy. Can increase, decrease, drain, or regenerate resources.

| Example | Description |
|---|---|
| Healing | Increases health |
| Mana Burn | Reduces mana |
| Energy Drain | Steals energy from an enemy |
| Cooldown Reduction | Modifies cooldown times |

---

### 2. Trait Manipulation

**Definition:** Temporarily or permanently modifies any trait of a character or creature, covering physical, mental, spatial, perceptual, defensive, and identity attributes.

| Example | Description |
|---|---|
| Strength Buff | Increases strength |
| Fear | Reduces bravery |
| Teleport | Modifies position |
| Summon | Manipulates dimension to bring new entities into the current one |
| Invisibility | Increases stealth or decreases visibility |
| Fire Resistance | Increases resistance to fire damage |
| Immunity | Sets a resistance trait to 100% |

#### Trait Categories

**Physical Traits**
Strength, Agility, Dexterity, Speed, Endurance, etc.

**Mental Traits**
Bravery, Discernment, Willpower, Morale, etc. Cover resistance to fear, charm, confusion.

**Spatial Traits**
- `Position` — where the character is on the battlefield
- `Dimension` — the plane of existence they occupy (e.g. summoning from another realm)
- `Range` — how far the character or their abilities can reach

**Perceptual Traits**
- `Visibility` — how easily a character is detected (invisibility increases this)
- `Perception` — ability to detect hidden objects or creatures (True Sight dramatically increases this)

**Resistance / Defensive Traits**
`FireResistance`, `IceResistance`, `MagicResistance`, etc. Immunity is a resistance trait at 100%. Resistance debuffs lower these values, increasing vulnerability.

**Identity Traits**
- `Form` — the character's physical or magical form (shapeshifting, polymorph)
- `Abilities` — which abilities are available based on form or identity

---

### 3. Passive Abilities

**Definition:** Constant or conditional ongoing effects that automatically modify traits or resources over time or under specific conditions, without active user input.

| Example | Description |
|---|---|
| Regeneration | Restores health over time |
| Damage Reflection | Automatically deals damage back to attackers |
| Stat Boost on Low Health | Increases attack power when health drops below a threshold |

---

### 4. Negation / Interruption

**Definition:** Prevents or cancels another ability from executing, disrupting an opponent's actions or stopping a spell mid-cast.

| Example | Description |
|---|---|
| Counterspell | Stops an enemy spell |
| Interrupt | Disrupts a channeled ability |
| Silence | Prevents certain ability types from being cast |

---

### 5. Environmental Effect

**Definition:** Alters the battlefield in ways that affect positioning, behavior, or damage dealt to characters interacting with it.

| Example | Description |
|---|---|
| Wall of Fire | Creates a damaging barrier |
| Earthquake | Shakes the battlefield, changing terrain or affecting movement |
| Trap Creation | Creates traps that trigger on contact |

---

## Ability Examples Mapped to Framework

| Ability | Category |
|---|---|
| Healing Spell | Resource Manipulation |
| Fireball | Resource Manipulation (damage) |
| Speed Buff | Trait Manipulation (physical) |
| Fear | Trait Manipulation (mental) |
| Teleport | Trait Manipulation (spatial — position) |
| Summon Creature | Trait Manipulation (spatial — dimension) |
| Invisibility | Trait Manipulation (perceptual) |
| Fire Immunity | Trait Manipulation (resistance — set to 100%) |
| Counterspell | Negation / Interruption |
| Wall of Fire | Environmental Effect |

---

## Evaluation Against the Current System

### What fits cleanly

**Resource Manipulation → `ResourceEffect`**
Covers health, mana, and drain mechanics directly. Clean match.

**Negation / Interruption → `NegationEffect`**
Cancels casts and applies silences. Clean match.

**Environmental Effect → `EnvironmentalEffect`**
Area, duration, and nested effects. Clean match.

**Passive Abilities → `PassiveAbility`**
Trigger + condition + effects. Clean match for conditional passives. Over-time effects (e.g. Regeneration) still require a tick loop not yet in the system.

**Physical and Mental Trait Manipulation → `TraitEffect`**
Any named float trait (Speed, Bravery, Strength) maps directly. Full support.

**Resistance / Defensive Traits**
Works immediately — just add traits named `FireResistance`, `MagicResistance`, etc. as floats (0.0–1.0) to `Character.Traits`. Setting one to 1.0 is immunity. Reducing one is a vulnerability debuff. No new types needed.

---

### Where there is tension

**Cooldown Reduction as Resource Manipulation**
Cooldown reduction fits as an `IResource` with a base value of 0 (0%). The resource represents how much cooldown time is reduced, not the cooldown itself. Ability cooldowns would consult this resource when calculating their effective duration. No fundamental conflict with the framework — just needs to be wired up.

**Spatial Traits (Position, Dimension)**
`Position` and `Dimension` are typed fields on `Character` — not `ITrait` floats. `TraitEffect` can only modify `ITrait` values. Teleport and summoning cannot be expressed as `TraitEffect` without either:
- Converting `Position` and `Dimension` into named traits (loses type safety)
- Adding a dedicated `SpatialEffect` type
- Giving `TraitEffect` an escape hatch for arbitrary character field mutation

**Resistance Traits and Typed Damage**
Being addressed. `ResourceEffect` will gain `DamageType` (e.g. `Fire`, `Water`, `Air`) and `DamageCategory` (e.g. `Elemental`, `Physical`). Resistance traits named by convention (`FireResistance`, `ElementalResistance`) can then be consulted during damage application. Both type and category checks allow resistances to operate at either granularity.

**Identity Traits (Form, Abilities)**
`Form` could be a string trait, but changing `Abilities` — swapping out a character's `ActiveAbilities` or `PassiveAbilities` list — cannot be expressed as a `TraitEffect`. It would require a new effect type (e.g. `TransformEffect`) with direct access to the character's ability collections.

**Perceptual Traits (Visibility, Perception)**
These work as float traits but have no connection to the targeting or detection systems, which don't exist yet. The traits would be stored but have no behavioral effect without game-layer support.

---

### Summary

| Concept | Status |
|---|---|
| Resource Manipulation | Fully supported |
| Physical / Mental Trait Manipulation | Fully supported |
| Resistance / Immunity as Traits | Supported as-is (just add named traits) |
| Passive Abilities | Supported; over-time passives need tick loop |
| Negation / Interruption | Fully supported |
| Environmental Effects | Fully supported |
| Cooldown as Resource | Resolved — model as an `IResource` starting at 0 (0% reduction) |
| Spatial Traits (Position, Dimension) | Deferred |
| Typed Damage + Resistance Mitigation | Being addressed — adding `DamageType` and `DamageCategory` to `ResourceEffect` |
| Identity Traits (Form, Ability swapping) | Open — see shapeshifting options |
| Perceptual Traits (Visibility, Perception) | Storable but behaviorally inert without targeting/detection systems |
