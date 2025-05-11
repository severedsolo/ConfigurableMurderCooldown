using BepInEx.Configuration;

namespace ConfigurableMurderCooldown;

public static class Settings
{
    public static ConfigEntry<bool> ApplyToNewGames { get; set; }
    public static ConfigEntry<int> NewMurdererMinCooldown { get; set; }
    public static ConfigEntry<int> NewMurdererMaxCooldown { get; set; }
    public static ConfigEntry<int> ExistingMurdererMinCooldown { get; set; }
    public static ConfigEntry<int> ExistingMurdererMaxCooldown { get; set; }



}