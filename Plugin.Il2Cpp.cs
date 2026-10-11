#if SPT5
using System;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using Il2CppInterop.Runtime.Injection;
using UnityEngine;

namespace BotCensus
{
    // SPT 5.0 / BepInEx 6 (IL2CPP). A BasePlugin is not a MonoBehaviour, so Unity never calls into it.
    // Register a small managed behaviour with the IL2CPP runtime and let BepInEx host it on a persistent
    // GameObject; that behaviour is what receives Update and OnGUI.
    //
    // DRAFT: written ahead of SPT 5.0's modding setup, and not yet built against its interop assemblies.
    [BepInPlugin(PluginId, "Bot Census", "2.0.0")]
    [BepInDependency("com.morebotsapi.tacticaltoaster", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.fika.core", BepInDependency.DependencyFlags.SoftDependency)]
    public class BotCensusPlugin : BasePlugin
    {
        public const string PluginId = "com.sipto.botcensus";

        internal static Census Census;

        public override void Load()
        {
            Census = new Census(Config, Log);
            if (!ClassInjector.IsTypeRegisteredInIl2Cpp<CensusDriver>())
                ClassInjector.RegisterTypeInIl2Cpp<CensusDriver>();
            AddComponent<CensusDriver>();
        }
    }

    public class CensusDriver : MonoBehaviour
    {
        // Il2CppInterop builds the managed wrapper around an object the IL2CPP side has already made, and
        // needs this constructor to do it.
        public CensusDriver(IntPtr ptr) : base(ptr) { }

        void Update() => BotCensusPlugin.Census?.Tick();

        void OnGUI() => BotCensusPlugin.Census?.Paint();
    }
}
#endif
