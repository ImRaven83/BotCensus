#if SPT5
using BepInEx.Unity.IL2CPP;
#else
using BepInEx.Bootstrap;
#endif

namespace BotCensus
{
    // The one place that knows which chainloader is running. BepInEx 5 (SPT 4.1, Mono) keeps its plugin
    // table on the static Chainloader; BepInEx 6 (SPT 5.0, IL2CPP) keeps it on the IL2CPP chainloader
    // instance. Both are keyed by GUID, which is all the soft-dependency checks need.
    internal static class Loader
    {
        public static bool HasPlugin(string guid)
        {
#if SPT5
            var chainloader = IL2CPPChainloader.Instance;
            return chainloader != null && chainloader.Plugins.ContainsKey(guid);
#else
            return Chainloader.PluginInfos.ContainsKey(guid);
#endif
        }
    }
}
