#if !SPT5
using BepInEx;

namespace BotCensus
{
    // SPT 4.1 / BepInEx 5 (Mono). The plugin is itself the MonoBehaviour, so Unity calls straight in.
    [BepInPlugin(PluginId, "Bot Census", "1.4.0")]
    [BepInDependency("com.morebotsapi.tacticaltoaster", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.fika.core", BepInDependency.DependencyFlags.SoftDependency)]
    public class BotCensusPlugin : BaseUnityPlugin
    {
        public const string PluginId = "com.sipto.botcensus";

        Census _census;

        void Awake()
        {
            _census = new Census(Config, Logger);
        }

        void Update() => _census.Tick();

        void OnGUI() => _census.Paint();
    }
}
#endif
