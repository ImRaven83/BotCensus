using System.Collections.Generic;
using EFT;

namespace BotCensus
{
    // Maps vanilla WildSpawnType values (0-67, or 0-82 on SPT 5.0) to a display bucket. Values a name-substring test
    // would get wrong or miss are pinned by number; the well-behaved boss*/follower* families fall
    // through to the substring test. Verified against EFT's WildSpawnType enum.
    internal static class Classifier
    {
        // The highest vanilla WildSpawnType. Anything above it was registered by a faction mod.
#if SPT5
        public const int VanillaCeiling = 82;
#else
        public const int VanillaCeiling = 67;
#endif

        static readonly Dictionary<int, Bucket> Pinned = new Dictionary<int, Bucket>
        {
            // scavs the keyword test misses (10 cursedAssault, 19 assaultGroup) and the
            // 37 crazyAssaultEvent that BotCounter wrongly counted as a boss.
            { 0, Bucket.Scav }, { 1, Bucket.Scav }, { 10, Bucket.Scav }, { 19, Bucket.Scav }, { 37, Bucket.Scav },

            // 9 pmcBot is the Reserve/Labs Raider.
            { 9, Bucket.Raider },

            // rogues: 24 exUsec on Lighthouse, 34/35 the Bloodhound arena fighters.
            { 24, Bucket.Rogue }, { 34, Bucket.Rogue }, { 35, Bucket.Rogue },

            // the Goons hunt as a trio (Knight + Big Pipe + Birdeye); the keyword test would fold
            // them into Boss/follower, so pin them out.
            { 26, Bucket.Goon }, { 27, Bucket.Goon }, { 28, Bucket.Goon },

            // the zombie event faction.
            { 60, Bucket.Infected }, { 61, Bucket.Infected }, { 62, Bucket.Infected }, { 63, Bucket.Infected }, { 64, Bucket.Infected },

            // event bosses whose names carry no boss/follower keyword (38/40 Zryachiy events).
            { 38, Bucket.Boss }, { 40, Bucket.Boss },

            // 67 tagillaHelperAgro is one of Tagilla's guards, not the boss, and carries no keyword.
            { 67, Bucket.Guard },

            // 46 shooterBTR — the armoured car's turret gunner.
            { 46, Bucket.Btr },

            // non-combatant / special NPCs (test, gifter, spirits, peacemaker, skier).
            { 18, Bucket.Other }, { 25, Bucket.Other },
            { 48, Bucket.Other }, { 49, Bucket.Other }, { 50, Bucket.Other }, { 53, Bucket.Other },
#if SPT5
            // The SPT 5.0 client's own additions past 67 that aren't a faction (those are in Native below).
            // 71 assaultTutorial is a scav; 75 exUsecFree a rogue; 72 sentry and 74 civilian carry no
            // keyword and fight for nobody we count. 77/78 bossBullyBlackDiv / followerBullyBlackDiv fall
            // through to the boss/follower test like the rest of their family.
            { 71, Bucket.Scav }, { 75, Bucket.Rogue }, { 72, Bucket.Other }, { 74, Bucket.Other },
#endif
        };

#if SPT5
        // The SPT 5.0 client ships the factions that SPT 4.1 only had through mods, as ordinary vanilla
        // roles. They land on the same rows the MoreBotsAPI versions do, under the same names, so the
        // glyph table and the boss/escort split treat them alike. Read off the 5.0 WildSpawnType enum.
        static readonly Dictionary<int, (string Faction, bool Escort)> Native = new Dictionary<int, (string, bool)>
        {
            // Black Division: blackDivision, pmcBotBlackDiv, pmcBotBlackDivSeason. A flat squad.
            { 68, ("Black Division", true) }, { 79, ("Black Division", true) }, { 82, ("Black Division", true) },

            // RUAF (ВС РФ): vsRF, vsRFSniper, vsRFFight. A flat squad.
            { 69, ("RUAF", true) }, { 70, ("RUAF", true) }, { 73, ("RUAF", true) },

            // Wedge leads; bossWedge and his Labs variant, then his Labs escort.
            { 76, ("Wedge", false) }, { 80, ("Wedge", false) }, { 81, ("Wedge", true) },
        };

        public static bool NativeFaction(int r, out string faction, out bool escort)
        {
            if (Native.TryGetValue(r, out var hit))
            {
                faction = hit.Faction;
                escort = hit.Escort;
                return true;
            }
            faction = null;
            escort = false;
            return false;
        }

        public static bool NativeFactionHasBoss(string faction)
        {
            foreach (var entry in Native.Values)
                if (!entry.Escort && entry.Faction == faction) return true;
            return false;
        }
#endif

        public static Bucket Vanilla(WildSpawnType role)
        {
            if (Pinned.TryGetValue((int)role, out var bucket)) return bucket;

            string name = role.ToString().ToLowerInvariant();
            if (name.Contains("boss")) return Bucket.Boss;
            if (name.Contains("follower")) return Bucket.Guard;
            // "sectact" covers the 39 sectactPriestEvent typo in EFT's own enum.
            if (name.Contains("sectant") || name.Contains("sectact")) return Bucket.Cultist;
            return Bucket.Other;
        }

        // The floor under both MoreBotsAPI lookups: the faction int ranges are stable and
        // self-documenting, and they still answer when the API is absent or has been reshaped under us.
        // Prefer the live registry (MoreBotsBridge) when it is available.
        public static string RangeFallback(int r)
        {
            if (r >= 848400 && r <= 848405) return "RUAF";
            if (r == 848406) return "Remnant";
            if (r >= 848420 && r <= 848423) return "Black Division";
            if (r >= 848430 && r <= 848431) return "Wedge";
            if (r >= 868588 && r <= 868589) return "Blackout";
            if (r >= 1170 && r <= 1173) return "UNTAR";
            return null;
        }
    }
}
