# Claude Code Guidelines

## Understanding the System

Read `SPEC.md` before making any changes. It is the authoritative reference for how this RPG system works — its classes, interfaces, fields, mechanics, and design patterns.

## Keeping the Spec Current

`SPEC.md` is the source of truth. **Update the spec before changing the code.** This includes:

- Adding, removing, or renaming a class, interface, or field
- Changing the behavior of an existing method or mechanic
- Introducing new enumerations or supporting types
- Restructuring files or directories

Write the intended design in `SPEC.md` first, then implement it. This keeps intent and implementation in sync and makes the spec a reliable reference for future contributors and agents.
