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

## 2.0.0 (SPT 5.0, pre-release)

`BotCensus-2.0.0-SPT5.0.zip`. **Experimental.** A BepInEx 6 IL2CPP build of the same panel for SPT 5.0's
IL2CPP client. It was written before 5.0's modding setup was public and has not been tested in game.
Requires BepInEx 6 (IL2CPP). See *SPT versions* in the README for what may not work yet.

- **Toggle Key** is a single key with no modifiers.
- **Fika** support is included only if the build saw a 5.0 `Fika.Core.dll`. Otherwise the panel counts
  locally.
- **Custom factions** fall back to their known ID ranges until MoreBotsAPI has a 5.0 version.
