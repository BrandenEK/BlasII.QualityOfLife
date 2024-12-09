﻿using BlasII.ModdingAPI;
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

    /// <inheritdoc cref="QolSettings" />
    public QolSettings CurrentSettings { get; private set; }

    /// <inheritdoc/>
    protected override void OnInitialize()
    {
        // Load settings and modules
        CurrentSettings = ConfigHandler.Load<QolSettings>();
        LoadModules();

        // Initialize handlers
        MessageHandler.AddGlobalListener(ReceiveSetting);
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
    /// <returns></returns>
    private Dictionary<string, KeyCode> SetupInput()
    {
        var input = new Dictionary<string, KeyCode>
        {
            { "Activator", KeyCode.F5 }
        };

        foreach (var module in _modules)
            input.Add(module.Name, module.DefaultKey);

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

        bool modified = false;
        foreach (var module in _modules)
        {
            // Check if this module's key was pressed
            if (!InputHandler.GetKeyDown(module.Name))
                continue;

            // Toggle the config setting
            ToggleModule(module.Name);
            modified = true;
        }

        return modified;
    }

    /// <summary>
    /// Toggles the module's setting in the config
    /// </summary>
    private void ToggleModule(string name)
    {
        PropertyInfo property = typeof(QolSettings).GetProperty(name);
        bool status = (bool)property.GetValue(CurrentSettings, null);
        property.SetValue(CurrentSettings, !status);

        ModLog.Info($"Toggling module '{name}' to {!status}");
    }

    private void ReceiveSetting(string _, string setting, string value)
    {
        switch (setting)
        {
            case CONSISTENT_TYPHOON:
            case "ct":
                CurrentSettings.ConsistentTyphoon = bool.Parse(value);
                return;
        }

        ModLog.Error($"Unknown setting: '{setting}'");
    }

    /// <summary>
    /// Loads all modules using reflection
    /// </summary>
    private void LoadModules()
    {
        var modules = Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => x.IsSubclassOf(typeof(BaseModule)))
            .Select(x => (BaseModule)Activator.CreateInstance(x));

        _modules.AddRange(modules);
        ModLog.Info($"Loaded {_modules.Count} modules");
    }

    private const string CONSISTENT_TYPHOON = "ConsistentTyphoon";
}
