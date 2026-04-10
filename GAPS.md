# Known Gaps

Gaps between this system and what modern RPG engines commonly support. Intended as a design backlog, not a criticism — many of these are intentionally out of scope for a foundational engine.

---

## Effect System

**No update loop / effect expiry enforcement**
`ActiveEffect.ExpiresAt` is recorded but nothing removes expired effects or fires expiry callbacks. Effects must be cleaned up manually by the game layer. Periodic effects (damage-over-time, heal-over-time, Windy City-style reapplication) are similarly unsupported without a tick system.

**No effect magnitude caps**
There is no way to declare "Speed cannot be increased by more than 100% from effects" or to cap the cumulative result of multiple stacking `TraitEffect` instances. The game layer must enforce this externally.

**No effect ordering / priority**
When multiple `TraitEffect` instances modify the same trait, evaluation order is undefined. Most games distinguish flat additive bonuses from percentage multipliers and apply them in a specified order (e.g. base → flat bonuses → percent multipliers). This system has no such model.

**No stat layering (base vs. modified)**
`ITrait.Value` is a single mutable float. Effects mutate it directly with no record of the original base value. There is no way to ask "what is the character's base Speed before effects?" or to remove an effect and restore the prior value cleanly.

**No shield / absorption layer**
Incoming damage hits a resource pool directly. There is no temporary-HP or damage-absorption mechanic that intercepts `ResourceEffect` before it reaches the underlying resource.

**No periodic (over-time) effects**
DoT and HoT require a tick/update loop to fire effects at regular intervals. The data model for this (e.g. a `TickInterval` on `TraitEffect` or `ResourceEffect`) does not exist.

---

## Crowd Control

**No first-class CC types**
Stun, root, knockback, fear, and sleep have no representation in the type system. They would need to be approximated by combining `TraitEffect` (zero speed = root) and `NegationEffect` (silence), with no shared abstraction.

**No CC immunity or resistance**
Characters cannot be marked as immune to a specific effect category (e.g. immune to silence, resistant to stuns). There is no mechanism to intercept or reduce an effect before it applies.

**No diminishing returns**
Repeated application of the same CC type does not reduce duration or effectiveness. This is a standard mechanic in PvP-oriented games to prevent indefinite lockdown.

---

## Ability System

**No resource costs**
Abilities have no mechanism to require or consume a resource (mana, energy, stamina) on activation. The game layer would need to check and deduct costs separately before calling `Activate()`.

**No cast time or channel mechanics**
`Ability.Activate()` is instantaneous. Cast time (delay before effects fire), channel time (effects fire repeatedly while held), and cast interruption are not modeled.

**No multi-target or AoE targeting**
`Activate(Character caster, Character target)` takes exactly one optional target. Cone, line, AoE (radius around caster or point), and multi-target selection are not supported. `EnvironmentalEffect` approximates a placed AoE but requires the game layer to detect and apply it to characters who enter the area.

**No ability source on ActiveEffect**
`ActiveEffect.Source` records which character applied an effect but not which ability triggered it. This makes it impossible to distinguish two different abilities from the same caster, or to implement mechanics like "remove all effects applied by ability X."

**No combo or chain mechanics**
There is no concept of an ability modifying or empowering a subsequent ability (e.g. "next attack deals bonus damage after using ability A").

---

## Character Model

**No teams or factions**
Characters have no affiliation. Abilities cannot distinguish allies from enemies, so friendly-fire prevention, heal-ally-only targeting, and faction-based passives must be handled entirely outside the system.

**No death state**
When a resource (e.g. Health) reaches zero, no behavior is defined. There is no dead/alive flag, no death event for passives to react to, and no respawn or elimination mechanic.

**No character progression**
No XP, levels, or stat growth over time. Ability upgrades, stat scaling, and unlock systems are entirely absent.

**No equipment or inventory**
Items that grant abilities, modify traits, or add resources are not modeled. The system has no concept of gear slots or item effects.

---

## Conditions and Triggers

**No multi-condition gating**
`Condition` evaluates a single trait against a single threshold. Passives cannot be gated on compound logic ("health < 20% AND mana > 50%") without external wiring.

**Limited trigger vocabulary**
`TriggerType` covers reactive events like `OnHitTaken` and `OnLowHealth`. Missing common triggers: `OnEffectApplied`, `OnEffectExpired`, `OnKill`, `OnDeath`, `OnAbilityActivated`, `OnEnterArea`, `OnLeaveArea`.

---

## Spatial

**No line of sight**
`Ability.Activate()` checks range but not whether the path between caster and target is obstructed.

**Only circular area shapes**
`EnvironmentArea` supports a center + radius only. Cone, line, rectangle, and ring shapes are not available.
