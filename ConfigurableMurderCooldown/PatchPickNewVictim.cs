using HarmonyLib;

namespace ConfigurableMurderCooldown;

[HarmonyPatch(typeof(MurderController), nameof(MurderController.PickNewVictim))]
public class PatchPickNewVictim
{
    [HarmonyPostfix]
    internal static void Postfix()
    {
        CooldownController.SetCooldown(false);
        MurderController.Instance.pauseBetweenMurders = 0;
        Plugin.ModLog("New Victim. Resetting cooldown");
    }
}