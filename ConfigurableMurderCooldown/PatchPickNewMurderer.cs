using HarmonyLib;
using Il2CppSystem.Linq;

namespace ConfigurableMurderCooldown;

[HarmonyPatch(typeof(MurderController), nameof(MurderController.PickNewMurderer))]
public static class PatchPickNewMurderer
{
    public static bool IsNewGame { get; set; }
    
    [HarmonyPostfix]
    private static void Postfix(MurderController __instance)
    {
        if (IsNewGame && !Settings.ApplyToNewGames.Value) return;
        Plugin.ModLog("New Murderer. Resetting cooldown");
        CooldownController.SetCooldown(true);
    }
}