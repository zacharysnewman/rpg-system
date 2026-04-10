# Known Gaps

Gaps between this system and what modern RPG engines commonly support. Split into two categories:

- **Actual gaps** — hard to work around without pushing significant logic into the game layer, or cases where the system makes a promise it doesn't keep.
- **Opinionated gaps** — reasonable omissions; depend on game design, genre, or can be cleanly handled outside the engine.

---

## Actual Gaps

### Effect System

**No stat layering (base vs. modified)**
`ITrait.Value` is a single mutable float. Effects mutate it directly with no record of the original base value. There is no way to ask "what is the character's base Speed before effects?" and no clean way to remove an effect and restore the prior value. This makes temporary trait modifications fragile — removing one effect correctly requires knowing exactly what every other active effect contributed.

**No effect ordering / priority**
When multiple `TraitEffect` instances modify the same trait, evaluation order is undefined. Flat additive bonuses and percentage multipliers produce different results depending on order (e.g. base 100 + flat 10, then ×150% = 165 vs. base 100 ×150%, then + 10 = 160). Without a defined model, results are unpredictable when effects are mixed.

**No effect expiry enforcement**
`ActiveEffect.ExpiresAt` is recorded but nothing removes expired effects or fires expiry callbacks. The system declares that effects expire but provides no mechanism to act on it. The game layer must poll and clean up manually.

### Ability System

**No resource costs**
Abilities have no mechanism to require or consume a resource (mana, energy, stamina) on activation. The game layer must check and deduct costs separately before calling `Activate()`. This is a near-universal RPG mechanic.

**No multi-target or AoE targeting**
`Activate(Character caster, Character target)` accepts exactly one optional target. Cone, line, radius-from-caster, and multi-target selection require the game layer to resolve targets and call `Activate()` once per character. `EnvironmentalEffect` approximates a placed AoE but the system provides no mechanism to detect which characters are inside an area and apply effects to them.

**No ability source on ActiveEffect**
`ActiveEffect.Source` records which character applied an effect but not which ability triggered it. It is impossible to distinguish two different abilities from the same caster, or to implement mechanics like "dispel all effects applied by ability X" or "this passive only reacts to effects I applied."

### Character Model

**No teams or factions**
Characters have no affiliation. Abilities cannot distinguish allies from enemies, making it impossible to implement friendly-fire prevention, heal-ally-only targeting, or faction-based passives without external wiring on every ability.

**No death state**
When a resource (e.g. Health) reaches zero, no behavior is defined. There is no dead/alive flag, no `OnDeath` trigger for passives to react to, and no respawn or elimination mechanic. The system has no answer for what happens next.

### Conditions and Triggers

**Limited trigger vocabulary**
`TriggerType` covers reactive events like `OnHitTaken` and `OnLowHealth`. Missing triggers that are difficult to approximate: `OnKill`, `OnDeath`, `OnEffectApplied`, `OnEffectExpired`, `OnAbilityActivated`, `OnEnterArea`, `OnLeaveArea`.

---

## Opinionated Gaps

These are absent by reasonable design choice. Whether they belong in a base engine depends on the game being built.

### Effect System

**No effect magnitude caps**
No way to declare "Speed cannot be increased by more than 100% from effects." Enforcing this externally is straightforward; it doesn't require engine-level support.

**No shield / absorption layer**
No temporary-HP or damage-absorption mechanic. Not universal — many RPGs have no shield mechanic at all.

**No periodic (over-time) effects**
DoT and HoT require a tick/update loop, which was explicitly deferred. The data model for `TickInterval` also doesn't exist yet, but this is a deliberate scope decision.

### Crowd Control

**No first-class CC types**
Stun, root, and fear can be approximated: root = `TraitEffect` setting Speed to 0, silence = `NegationEffect`. A dedicated `CrowdControlEffect` would be cleaner but isn't strictly necessary given the extensible `IEffect` interface.

**No CC immunity or resistance**
Characters cannot be marked immune to a specific effect category. Highly game-specific — many RPGs have no immunity system.

**No diminishing returns**
Repeated application of the same CC does not reduce duration. This is primarily a PvP concern; most single-player RPGs omit it entirely.

### Ability System

**No cast time or channel mechanics**
`Ability.Activate()` is instantaneous. Many genres (action RPGs, fighting games) use instant abilities throughout. Cast time is genre-dependent, not universal.

**No combo or chain mechanics**
No concept of ability B being empowered after ability A. Highly game-specific and commonly implemented at the game layer rather than the engine.

### Character Model

**No character progression**
No XP, levels, or stat growth. This is intentionally out of scope for a combat engine — progression systems vary too widely across games to belong here.

**No equipment or inventory**
Items that modify traits or grant abilities are not modeled. Equipment is a game-layer concern that sits above the combat engine.

### Conditions and Triggers

**No multi-condition gating**
`Condition` evaluates a single trait against a single threshold. For games with simple passives this is sufficient; compound logic ("health < 20% AND mana > 50%") can be handled with chained passives or game-layer checks.

### Spatial

**No line of sight**
Range is checked but path obstruction is not. LoS is highly dependent on the world representation (tile grid, navmesh, physics) and is typically handled outside a combat engine.

**Only circular area shapes**
`EnvironmentArea` supports center + radius only. Many games use only circular areas; cones and lines are genre-specific additions.
