#if FIKA
using Fika.Core.Main.Components;

namespace BotCensus
{
    // Kept in its own type so the Fika.Core types are only JIT-resolved when Fika is actually
    // installed (the caller guards this behind a loader plugin check). Only compiled when the build
    // can see Fika.Core.dll; without it the panel counts locally.
    internal static class FikaSource
    {
        public static bool TryClassify(Census census)
        {
            if (!CoopHandler.TryGetCoopHandler(out var handler) || handler == null || handler.Players == null)
                return false;

            foreach (var player in handler.Players.Values)
            {
                if (player == null) continue;
                census.Classify(player, player.IsObservedAI);
            }

            return true;
        }
    }
}
#endif
