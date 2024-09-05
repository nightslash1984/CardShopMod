using BepInEx;
using UnboundLib;
using UnboundLib.Cards;
using CardShopMod.Cards;
using BepInEx.Configuration;
using HarmonyLib;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using UnityEngine;
using UnboundLib.Utils.UI;
using TMPro;
using UnboundLib.Networking;
using Photon.Pun;


namespace CardShopMod
{
    // These are the mods required for our mod to work
    [BepInDependency("com.willis.rounds.unbound", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("pykess.rounds.plugins.moddingutils", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("pykess.rounds.plugins.cardchoicespawnuniquecardpatch", BepInDependency.DependencyFlags.HardDependency)]
    // Declares our mod to Bepin
    [BepInPlugin(ModId, ModName, Version)]
    // The game our mod is associated with
    [BepInProcess("Rounds.exe")]
    public class CardShop : BaseUnityPlugin
    {
        private const string ModId = "com.Nightslash.CardShop";
        private const string ModName = "Card Shop";
        public const string Version = "0.0.1"; // What version are we on (major.minor.patch)?
        public const string ModInitials = "CSM";
        public const string CompatibilityModName = "CardShop";

        public static ConfigEntry<float> coinMultiplierConfig;

        internal static float coinMultiplier; 

        public static CardShop instance; 


        void Awake()
        {
            CardShop.instance = this;

            coinMultiplierConfig = Config.Bind(CompatibilityModName, "Multiplier", 1f, "The coin multiplier for doing damage");

            var harmony = new Harmony(ModId);
            harmony.PatchAll();
        }
        void Start()
        {
            Unbound.RegisterMenu(ModName, () => { }, this.NewGUI, null, false);

            Unbound.RegisterHandshake(CardShop.ModId, this.OnHandShakeCompleted);
        }

        private void NewGUI(GameObject menu)
        {
            MenuHandler.CreateText(ModName + " Options", menu, out TextMeshProUGUI _, 60);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 30);
            void CoinMultiplierChanged(float val)
            {
                CardShop.coinMultiplierConfig.Value = Mathf.Clamp(val, 0.1f, 3.0f);
                CardShop.coinMultiplier = CardShop.coinMultiplierConfig.Value;
                
                OnHandShakeCompleted();
            }
            MenuHandler.CreateSlider("Coin multiplier for doing damage", menu, 30, 0f, 3.0f, CardShop.coinMultiplierConfig.Value, CoinMultiplierChanged, out UnityEngine.UI.Slider _, false);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 30);
        }

        private void OnHandShakeCompleted()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                NetworkingManager.RPC_Others(typeof(CardShop), nameof(SyncSettings), new object[] { CardShop.coinMultiplier});
            }
        }

        [UnboundRPC]
        private static void SyncSettings(int host_multiplier)
        {
            CardShop.coinMultiplier = host_multiplier;
        }
    }
}
