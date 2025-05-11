using System.Reflection;
using System.Text;
using BepInEx;
using HarmonyLib;
using SOD.Common;
using SOD.Common.BepInEx;
using SOD.Common.Helpers;

namespace ConfigurableMurderCooldown;

[BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
public class Plugin : PluginController<Plugin>
{

    public const string PLUGIN_GUID = "Severedsolo.SOD.ConfigurableMurderCooldown";
    public const string PLUGIN_NAME = "ConfigurableMurderCooldown";
    public const string PLUGIN_VERSION = "1.0.0";

    public override void Load()
    {
        Harmony.PatchAll(Assembly.GetExecutingAssembly());
        BindConfigs();
        Lib.SaveGame.OnAfterNewGame += OnNewGame;
        Lib.SaveGame.OnAfterLoad += OnLoadGame;
        Lib.SaveGame.OnAfterSave += OnSaveGame;
        Lib.SaveGame.OnAfterDelete += OnDeleteGame;
        ModLog("Initialised and patched");
    }

    private static string GetSavePath(string savePath)
    {
        string path = Lib.SaveGame.GetUniqueString(savePath);
        return Lib.SaveGame.GetSavestoreDirectoryPath(Assembly.GetExecutingAssembly(), $"CMC_{path}.txt");
    }

    private void OnDeleteGame(object? sender, SaveGameArgs e)
    {
        string path = GetSavePath(e.FilePath);
        if (!File.Exists(path)) return;
        File.Delete(path);
        ModLog("Save deleted");
    }

    private void OnSaveGame(object? sender, SaveGameArgs e)
    {
        if (MurderController.Instance == null) return;
        StringBuilder saveData = new StringBuilder();
        saveData.AppendLine(MurderController.Instance.pauseBetweenMurders.ToString());
        Log.LogInfo("Saved line 1");
        if(MurderController.Instance.GetCurrentMurder() != null) saveData.AppendLine(MurderController.Instance.GetCurrentMurder().preset.minimumTimeBetweenMurders.ToString());
        Log.LogInfo("Saved line 2");
        using StreamWriter writer = new StreamWriter(GetSavePath(e.FilePath));
        writer.Write(saveData.ToString());
        ModLog("Data saved");
    }

    private void OnLoadGame(object? sender, SaveGameArgs e)
    {
        PatchPickNewMurderer.IsNewGame = false;
        string path = GetSavePath(e.FilePath);
        if (!File.Exists(path)) return;
        List<string> saveData = new List<string>();
        using (StreamReader reader = new StreamReader(path))
        {
            while (true)
            {
                string? line = reader.ReadLine();
                if (line == null)
                {
                    break;
                }

                saveData.Add(line);
            }

            if (saveData.Count == 0) return;
            float.TryParse(saveData[0], out float f);
            MurderController.Instance.pauseBetweenMurders = f;
            if(saveData.Count >1) float.TryParse(saveData[1], out f);
            if(MurderController.Instance.GetCurrentMurder()!= null) MurderController.Instance.GetCurrentMurder().preset.minimumTimeBetweenMurders = f;
            ModLog("Data Restored");
        }
    }

    public static void ModLog(string output)
    {
        Log.LogInfo(output);
    }

    private void OnNewGame(object? sender, EventArgs e)
    {
        PatchPickNewMurderer.IsNewGame = true;
        MurderController.Instance.pauseBetweenMurders = 0;
        ModLog("New Game: - Resetting to Default");
    }

    private void BindConfigs()
    {
        Settings.ApplyToNewGames = Config.Bind("General", "Severedsolo.CMC.applyToNewGames", false, "Should the first murder of a new game also have a cooldown?");
        Settings.NewMurdererMinCooldown = Config.Bind("Murderer First Kill", "Severedsolo.CMC.newKillerMinCooldown", 12, "Minimum time (in hours) before murderer makes their first kill");
        Settings.NewMurdererMaxCooldown = Config.Bind("Murderer First Kill", "Severedsolo.CMC.newKillerMaxCooldown", 48, "Maximum time (in hours) before murderer makes their first kill");
        Settings.ExistingMurdererMinCooldown = Config.Bind("Murderer Second+ Kill", "Severedsolo.CMC.existingKillerMinCooldown", 1, "Minimum time (in hours) before murderer makes their second (or more) kill");
        Settings.ExistingMurdererMaxCooldown = Config.Bind("Murderer Second+ Kill", "Severedsolo.CMC.existingKillerMaxCooldown", 12, "Minimum time (in hours) before murderer makes their second (or more) kill");
        ModLog("Configs Bound");
    }
}