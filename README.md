# Bot Census

An in-raid overlay that tallies the AI you're sharing the map with, custom factions included. RUAF,
UNTAR, Black Division and friends register above the vanilla bot range, and Bot Census reads them off
MoreBotsAPI's live registry so each lands on its own line.

## What it counts

- **Custom factions** (via MoreBotsAPI): RUAF, Remnant, UNTAR, Black Division, each on its own line.
  They spawn as `WildSpawnType` values above the vanilla ceiling of 67. Bot Census pulls names from
  MoreBotsAPI's live registry by reflection rather than hardcoding a list, which means factions added
  after this was written show up too; with MoreBotsAPI absent it falls back to the known ID ranges.
- **Custom bosses, split from their escort.** A faction that fields a boss gets him on his own line with
  his guards beneath — Wedge and three guards read as `Wedge 1` / `Wedge Guard 3` rather than a single
  `Wedge 4` you can't take anything from. Which factions those are comes off the registry, not a list of
  names here, so a boss added by some mod later splits the same way. Factions that are a flat squad with
  nobody in charge keep their single line.
- **The Goons.** Knight, Big Pipe and Birdeye get their own line, so you know the trio is on the map.
- **The BTR gunner**, on its own line.
- **AI PMCs**, separated from the scav side by faction (USEC / BEAR).
- **Raiders and Rogues** on separate lines by default. Fold them into one row from F12 if you'd rather
  keep the panel short.
- **Bosses and their guards** on separate lines too, so you can tell whether the boss himself is still up
  or only his guards are left. Merge them from F12 as well.
- **Scavs, bosses, cultists and the infected event**, with the vanilla sorting oddities put right:
  `crazyAssaultEvent` reads as a Scav instead of a boss, and `cursedAssault`,
  `assaultGroup` and the misspelled `sectactPriestEvent` all land where they should. Anything genuinely
  unrecognised still lands under **Other** rather than disappearing.
- **Everything, added up.** A **Total Bots** line under a rule at the foot of the panel. It counts off the
  tally rather than the lines above it, so rows you've merged or hidden still make the total.

Every line carries a glyph, drawn in that row's colour — a scav in his balaclava, the BTR on its eight
wheels, the cultists' own sigil, and Black Division under the scorpion. Turn them off from F12 if you want
the panel narrower.

The panel re-scans every few seconds, so patrols that spawn well into a raid (a RUAF push, a Black
Division hunt) turn up on the next tick. It eases in as the raid loads in rather than snapping onto the
screen, so it settles alongside the rest of the HUD instead of popping over it.

## Install

Put the `BotCensus` folder in `BepInEx/plugins` and launch. Built for SPT 4.1; see
[SPT versions](#spt-versions) for 5.0. Nothing to configure to get going.
**MoreBotsAPI** (custom-faction names) and **Fika** (co-op) are optional; each is used automatically
when it's installed and ignored when it isn't.

MoreBotsAPI is reached entirely by reflection, and Fika's types sit on a single path that only runs when
Fika is actually installed — so neither can stop Bot Census loading, with or without Wedge, Black Division
or any other faction mod. If one of them changes shape in a later version, the panel notes it once in the
log and carries on with what it can still read: custom factions fall back to their known ID ranges.

## Settings

In-game **F12**, or `BepInEx/config/com.sipto.botcensus.cfg`:

| Setting | Default | Notes |
|---|---|---|
| Enable | on | Master toggle |
| Only In Raid | on | Keeps it off the menu and hideout screens |
| Toggle Key | unbound | Show/hide the panel from the keyboard mid-raid. Same switch as Enable. Works with other keys held, so toggling on the move is fine |
| Font Size | 16 | 10–64. Scales the whole panel, not just the text — glyphs, row height and width all key off it. Raise it on a large or high-resolution display; on 4K, 32–40 sits about where 16 does on 1080p |
| Offset Right / Top | 20 / 40 | Where the panel sits |
| Use Tarkov Font | on | The game's Bender face, with fallbacks |
| Background Opacity | 0.72 | Panel backing, 0 (clear) to 1 (solid) |
| Show Icons | on | Off drops the glyph column and narrows the panel |
| Icon Scale | 1.0 | 0.6–1.5. Glyph size relative to the text, capped at the row height so it can't spill into neighbouring rows |
| Split Rogue And Raider | on | Off merges them into a single row |
| Split Boss And Guard | on | Off merges them into a single row. Also merges a custom boss back in with his guards |
| Update Interval | 5s | 5s / 10s / 15s / 30s / 1min |

Every bot type also has its own visibility mode: **Always** keeps the row up even at zero,
**WhenPresent** only shows it while some are alive (nice for bosses and the Goons), **Hidden** takes it
off entirely. PMC, Scav and Total start on Always, the rest on WhenPresent.

## Co-op (Fika)

On a Fika raid it counts off the shared player list, so your numbers match the host's. It only reads
state and draws to your own screen; nothing networked, no patches.

Custom factions are named the same for everyone in the raid. MoreBotsAPI builds its faction list from a
request only the host's machine makes, so a joining client used to see `Custom` where the host saw the
faction's name; the names now come off the metadata each faction mod registers at startup, which every
machine has.

If a Fika update ever moves what that reads, it drops back to counting locally for the rest of the session
rather than leaving the panel stuck. You keep a working tally; it just stops telling a remote client's AI
apart from your own.

## Panel position

It draws in the top-right. If you've already got a counter living there, run one or the other, or bump
**Offset Top** here so the two don't overlap.

## SPT versions

| SPT | Client | Loader | Status |
|---|---|---|---|
| 4.1 | Mono | BepInEx 5 | Supported. The default build |
| 5.0 | IL2CPP | BepInEx 6 | **Draft port, not yet built or tested in game** |

SPT 5.0 moves to the IL2CPP Tarkov client. BepInEx 5 can't load anything there, so the 5.0 build is a
BepInEx 6 IL2CPP plugin made from the same source; the differences sit behind `#if SPT5`. It was
written before 5.0's modding setup was public, so expect to fix a few things against the real interop
assemblies the first time it's built. The parts most likely to need attention:

- **Game type names.** SPT remaps Tarkov's obfuscated names. The draft assumes 5.0 keeps the 4.1 names it
  uses (`GameWorld`, `HideoutGameWorld`, `Player`, `WildSpawnType`, `CurrentScreenSingletonClass`,
  `EEftScreenType`) in the same namespaces.
- **IMGUI.** The panel is drawn with `OnGUI`. IL2CPP builds sometimes strip IMGUI or
  `ImageConversion.LoadImage`; if either is missing the panel (or its glyphs) won't draw, and that part
  needs a rewrite onto uGUI.
- **Custom factions.** MoreBotsAPI is still read by reflection, and with no 5.0 MoreBotsAPI the panel
  falls back to its known ID ranges. Whether factions can still register roles above the vanilla range
  on an IL2CPP client is up to that mod.
- **Fika.** The Fika integration only compiles in when the build finds a `Fika.Core.dll` for the
  install. Without one the 5.0 build counts locally, which still sees every bot.
- **Toggle Key** is a single key on 5.0, with no modifiers.

## Build

`dotnet build -c Release`, with the .NET SDK 8+ installed (`.\package.ps1` builds and zips a release
into `dist/`; add `-Spt 5` for the 5.0 build). Path resolution keys off `SptRoot` in the
csproj; by default it walks three folders up to the SPT install, so override that if your checkout lives
elsewhere. The build drops `BotCensus.dll` into `BepInEx/plugins/BotCensus`.

For SPT 5.0 add `-p:Spt=5` (or `.\build.ps1 -Spt 5`), pointing `SptRoot` at the 5.0 install. Launch the
game once with BepInEx 6 first: the build references the interop assemblies it generates in
`BepInEx/interop`. The output targets `net6.0` and lands in `bin/Release/net6.0`, so it never overwrites
a 4.1 build.
