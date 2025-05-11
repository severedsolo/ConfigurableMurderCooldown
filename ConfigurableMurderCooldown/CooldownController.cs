namespace ConfigurableMurderCooldown;

public static class CooldownController
{
    private static Random r = new Random();
    public static void SetCooldown(bool newMurderer)
    {
        int minValue = GetMinValue(newMurderer);
        int maxValue = GetMaxValue(newMurderer);
        int valueToSet = r.Next(minValue, maxValue+1);
        MurderController.Instance.pauseBetweenMurders = valueToSet;
        if (MurderController.Instance.GetCurrentMurder() != null)
        {
            MurderController.Instance.GetCurrentMurder().preset.minimumTimeBetweenMurders = valueToSet;
            valueToSet = (int)MurderController.Instance.GetCurrentMurder().preset.minimumTimeBetweenMurders;
        }
        else valueToSet = (int)MurderController.Instance.pauseBetweenMurders;
        Plugin.ModLog("Set cooldown to "+valueToSet);
    }

    private static int GetMinValue(bool newMurderer)
    {
        return newMurderer ? Settings.NewMurdererMinCooldown.Value : Settings.ExistingMurdererMinCooldown.Value;
    }
    
    private static int GetMaxValue(bool newMurderer)
    {
        return newMurderer ? Settings.NewMurdererMaxCooldown.Value : Settings.ExistingMurdererMaxCooldown.Value;
    }
}