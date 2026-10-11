# Bot Census release notes

## 1.4.0 (SPT 4.1)

`BotCensus-1.4.0-SPT4.1.zip`. Extract it into the SPT folder, so `BotCensus.dll` lands in
`BepInEx/plugins/BotCensus`.

- **Toggle Key:** a bindable key that shows and hides the panel mid-raid. It works with other keys held,
  so toggling on the move is fine.
- **Hideout fix:** the count no longer sticks at zero for later raids after you visit the hideout.
- **Fika:** clients now see custom factions by name instead of `Custom`.
- **Scaling:** Font Size now scales the whole panel (text, glyphs, row height and width).
- **Faction art:** Wedge and his guards have their own Black Division crests, and the BTR glyph was
  redrawn.
- **Resilience:** the panel keeps running if Fika or MoreBotsAPI change shape. It logs a warning once and
  falls back to what it can still read.

Optional: MoreBotsAPI (custom faction names), Fika (co-op). Both are picked up automatically if installed.

## 2.0.0 (SPT 5.0, beta)

`BotCensus-2.0.0-SPT5.0.zip`. Built against SPT 5.0's own BepInEx 6 and interop assemblies, but not yet
tested in game, so ship it as a pre-release. Extract into the SPT 5.0 folder, like the 4.1 build.

- **Same panel on the IL2CPP client**, built as a BepInEx 6 plugin.
- **Black Division, RUAF and Wedge** are part of the game on 5.0, and get their own rows and crests
  without MoreBotsAPI. Wedge still splits from his guards.
- **New vanilla roles sorted:** the Free Rogue counts as a Rogue, the tutorial scav as a Scav, and
  sentries and civilians go under Other.
- **Toggle Key** is a single key, with no modifiers.
- **No Fika support in this build.** It was built without a 5.0 Fika, so co-op raids are counted locally.
