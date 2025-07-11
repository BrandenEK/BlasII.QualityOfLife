using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace BlasII.QualityOfLife;

/// <summary>
/// Provides customizable options to improve the gameplay experience
/// </summary>
public class QualityOfLife : BlasIIMod
{
    internal QualityOfLife() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    private readonly List<BaseModule> _modules = [];
    private bool _toggleStatus = false;

    /// <inheritdoc cref="QolSettings" />
    public QolSettings CurrentSettings { get; private set; }

    /// <inheritdoc/>
    protected override void OnInitialize()
    {
        // Load settings and modules
        CurrentSettings = ConfigHandler.Load<QolSettings>();
        LoadModules();

        // Initialize handlers
        //MessageHandler.AddGlobalListener(ReceiveSetting);
        InputHandler.RegisterDefaultKeybindings(SetupInput());

        // Call OnStart for all modules
        foreach (var module in _modules)
            module.OnStart();
    }

    /// <inheritdoc/>
    protected override void OnUpdate()
    {
        // If a glitch status was updated, save the config
        if (ProcessInput())
            ConfigHandler.Save(CurrentSettings);

        if (!SceneHelper.GameSceneLoaded)
            return;

        // Call OnUpdate for all modules
        foreach (var module in _modules)
            module.OnUpdate();
    }

    /// <summary>
    /// Creates the keybindings dictionary from all the modules
    /// </summary>
    private Dictionary<string, KeyCode> SetupInput()
    {
        var input = new Dictionary<string, KeyCode>
        {
            { "Activator", KeyCode.F5 },
            { "Display", KeyCode.KeypadPeriod },
            { "Toggle_All", KeyCode.KeypadEnter },
        };

        foreach (var module in _modules)
            input.Add($"Toggle_{module.Name}", Enum.Parse<KeyCode>($"Keypad{module.Order}"));

        return input;
    }

    /// <summary>
    /// Checks for qol input and returns whether the config was updated
    /// </summary>
    private bool ProcessInput()
    {
        // Check if activator key is held
        if (!InputHandler.GetKey("Activator"))
            return false;

        // Check if display key was pressed
        if (InputHandler.GetKeyDown("Display"))
        {
            DisplaySettings(CurrentSettings);
        }

        // Check if toggle all key was pressed
        if (InputHandler.GetKeyDown("Toggle_All"))
        {
            _toggleStatus = !_toggleStatus;

            foreach (var module in _modules)
                SetModuleStatus(module.Name, _toggleStatus);

            DisplayMessage("Toggling all modules to {0}", _toggleStatus);
            return true;
        }

        bool modified = false;
        foreach (var module in _modules)
        {
            // Check if this module's key was pressed
            if (!InputHandler.GetKeyDown($"Toggle_{module.Name}"))
                continue;

            // Toggle the config setting
            ToggleModuleStatus(module.Name);
            modified = true;
        }

        return modified;
    }

    /// <summary>
    /// Toggles the module's setting in the config
    /// </summary>
    private void ToggleModuleStatus(string name)
    {
        PropertyInfo property = typeof(QolSettings).GetProperty(name);
        bool status = !(bool)property.GetValue(CurrentSettings, null);
        property.SetValue(CurrentSettings, status);

        DisplayMessage($"Toggling module '{name}' to {{0}}", status);
    }

    /// <summary>
    /// Sets the module's setting in the config
    /// </summary>
    private void SetModuleStatus(string name, bool status)
    {
        PropertyInfo property = typeof(QolSettings).GetProperty(name);
        property.SetValue(CurrentSettings, status);

        //ModLog.Info($"Setting module '{name}' to {status}");
    }

    /// <summary>
    /// Displays the enabled status of all settings
    /// </summary>
    private void DisplaySettings(QolSettings settings)
    {
        ModLog.Info(string.Empty);
        ModLog.Info("Quality of Life Settings:");

        foreach (var property in typeof(QolSettings).GetProperties())
        {
            string name = property.Name;
            bool status = (bool)property.GetValue(settings, null);

            DisplayMessage($"{name}: {{0}}", status);
        }

        ModLog.Info(string.Empty);
    }

    /// <summary>
    /// Displays a colored message with a status property
    /// </summary>
    private void DisplayMessage(string message, bool status)
    {
        ModLog.Custom(string.Format(message, status ? "enabled" : "disabled"), status ? ENABLED_COLOR : DISABLED_COLOR);
    }

    ///// <summary>
    ///// Handles receiving settings from other mods
    ///// Removed because that would update the config without the user knowing
    ///// </summary>
    //private void ReceiveSetting(string _, string setting, string value)
    //{
    //    BaseModule module = _modules.FirstOrDefault(x => x.Name == setting);

    //    if (module == null)
    //    {
    //        ModLog.Error($"Unknown setting: '{setting}'");
    //        return;
    //    }

    //    SetModuleStatus(setting, bool.Parse(value));
    //}

    /// <summary>
    /// Loads all modules using reflection
    /// </summary>
    private void LoadModules()
    {
        var modules = Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => x.IsSubclassOf(typeof(BaseModule)))
            .Select(x => (BaseModule)Activator.CreateInstance(x))
            .OrderBy(x => x.Order);

        _modules.AddRange(modules);
        ModLog.Info($"Loaded {_modules.Count} modules");
    }

    private static readonly System.Drawing.Color ENABLED_COLOR = System.Drawing.Color.FromArgb(125, 191, 3);
    private static readonly System.Drawing.Color DISABLED_COLOR = System.Drawing.Color.FromArgb(230, 69, 48);
}
