﻿using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using NoTankYou.Configuration.Overlays;
using NoTankYou.Localization;
using NoTankYou.UserInterface.Components.InfoBox;

namespace NoTankYou.UserInterface.Windows;

internal class BannerOverlayConfigurationWindow : Window
{
    private static BannerOverlaySettings Settings => Service.ConfigurationManager.CharacterConfiguration.BannerOverlay;

    private readonly InfoBox Options = new();
    private readonly InfoBox ModeSelect = new();
    private readonly InfoBox ListModeOptions = new();
    private readonly InfoBox ScaleOptions = new();
    private readonly InfoBox DisplayOptions = new();
    private readonly InfoBox RepositionMode = new();

    public BannerOverlayConfigurationWindow() : base($"{Strings.TabItems.BannerOverlay.ConfigurationLabel} - {Service.ConfigurationManager.CharacterConfiguration.CharacterData.Name}")
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(350 * (4.0f / 3.0f), 350),
            MaximumSize = new Vector2(9999,9999)
        };

        Flags |= ImGuiWindowFlags.AlwaysVerticalScrollbar;
    }

    public override void Draw()
    {
        RepositionMode
            .AddTitle(Strings.Configuration.PreviewMode)
            .AddConfigCheckbox(Strings.Configuration.PreviewMode, Settings.SampleMode)
            .Draw();

        Options
            .AddTitle(Strings.Common.Labels.Options)
            .AddConfigCheckbox(Strings.Configuration.HideInSanctuary, Settings.DisableInSanctuary)
            .Draw();

        ScaleOptions
            .AddTitle(Strings.Common.Labels.Scale)
            .AddDragFloat("", Settings.Scale, 0.1f, 5.0f, 200.0f)
            .Draw();
        
        DisplayOptions
            .AddTitle(Strings.Common.Labels.DisplayOptions)
            .AddConfigCheckbox(Strings.TabItems.BannerOverlay.ExclamationMark, Settings.WarningShield)
            .AddConfigCheckbox(Strings.TabItems.BannerOverlay.WarningText, Settings.WarningText)
            .AddConfigCheckbox(Strings.TabItems.BannerOverlay.PlayerNames, Settings.PlayerNames)
            .AddConfigCheckbox(Strings.TabItems.BannerOverlay.Icon, Settings.Icon)
            .AddConfigCheckbox(Strings.TabItems.BannerOverlay.IconText, Settings.IconText)
            .AddDragFloat(Strings.TabItems.BannerOverlay.BorderThickness, Settings.BorderThickness, 0.5f, 3.0f, 200.0f)
            .Draw();

        ModeSelect
            .AddTitle(Strings.Common.Labels.ModeSelect)
                .BeginTable()
                    .BeginRow()
                    .AddConfigRadio(Strings.TabItems.BannerOverlay.ListMode, Settings.Mode, BannerOverlayDisplayMode.List, Strings.TabItems.BannerOverlay.ListModeDescription)
                    .AddConfigRadio(Strings.TabItems.BannerOverlay.TopPriorityMode, Settings.Mode, BannerOverlayDisplayMode.TopPriority, Strings.TabItems.BannerOverlay.TopPriorityDescription)
                    .EndRow()
                .EndTable()
            .SameLine()
            .Draw();

        if (Settings.Mode.Value == BannerOverlayDisplayMode.List)
        {
            ListModeOptions
                .AddTitle(Strings.TabItems.BannerOverlay.ListModeOptions)
                .AddSliderInt(Strings.TabItems.BannerOverlay.WarningCount, Settings.WarningCount, 1, 8)
                .Draw();
        }
    }
}