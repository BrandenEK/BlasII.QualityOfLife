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
        InputHandler.RegisterDefaultKeybindings(new Dictionary<string, KeyCode>()
        {
            { "Activator", KeyCode.F5 },
            { CONSISTENT_TYPHOON, KeyCode.Keypad1 },
            { SKIP_STORY_LEVEL, KeyCode.Keypad2 },
        });

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
    /// Checks for qol input and returns whether the config was updated
    /// </summary>
    private bool ProcessInput()
    {
        if (!InputHandler.GetKey("Activator"))
            return false;

        if (InputHandler.GetKeyDown(CONSISTENT_TYPHOON))
        {
            CurrentSettings.ConsistentTyphoon = !CurrentSettings.ConsistentTyphoon;
            ModLog.Info($"Toggling module '{CONSISTENT_TYPHOON}' to {CurrentSettings.ConsistentTyphoon}");
            return true;
        }

        if (InputHandler.GetKeyDown(SKIP_STORY_LEVEL))
        {
            CurrentSettings.SkipStoryLevel++;
            if (CurrentSettings.SkipStoryLevel > 4)
                CurrentSettings.SkipStoryLevel = 0;
            ModLog.Info($"Cycling module '{SKIP_STORY_LEVEL}' to {CurrentSettings.SkipStoryLevel}");
            return true;
        }

        return false;
    }

    private void ReceiveSetting(string _, string setting, string value)
    {
        switch (setting)
        {
            case CONSISTENT_TYPHOON:
            case "ct":
                CurrentSettings.ConsistentTyphoon = bool.Parse(value);
                return;
            case SKIP_STORY_LEVEL:
            case "ssl":
                CurrentSettings.SkipStoryLevel = int.Parse(value);
                return;
        }

        ModLog.Error($"Unknown setting: '{setting}'");
    }

    private void LoadModules()
    {
        var modules = Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => x.IsSubclassOf(typeof(BaseModule)))
            .Select(x => (BaseModule)Activator.CreateInstance(x));

        _modules.AddRange(modules);
        ModLog.Info($"Loaded {_modules.Count} modules");
    }

    private const string CONSISTENT_TYPHOON = "ConsistentTyphoon";
    private const string SKIP_STORY_LEVEL = "SkipStoryLevel";
}
