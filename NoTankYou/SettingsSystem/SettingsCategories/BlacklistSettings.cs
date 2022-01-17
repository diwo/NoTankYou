﻿using Dalamud.Interface.Components;
using ImGuiNET;
using System.Numerics;
using Dalamud.Interface;

namespace NoTankYou.SettingsSystem.SettingsCategories
{
    internal class BlacklistSettings : TabCategory
    {
        private int ModifyBlacklistValue;

        public BlacklistSettings()
        {
            CategoryName = "Blacklist Settings";
            TabName = "Blacklist";
        }

        protected override void DrawContents()
        {
            PrintBlackList();

            PrintAddRemoveCurrentTerritoryBlacklist();

            PrintAddRemoveManualTerritoryBlacklist();
        }
        private static void PrintBlackList()
        {
            ImGui.Text("Currently Blacklisted Areas");
            ImGui.Spacing();

            if (Service.Configuration.TerritoryBlacklist.Count > 0)
            {
                var blacklist = Service.Configuration.TerritoryBlacklist;
                ImGui.Text("{" + string.Join(", ", blacklist) + "}");
            }
            else
            {
                ImGui.TextColored(new (180, 0, 0, 0.8f), "Blacklist is empty");
            }

            ImGui.Spacing();

        }
        private static void PrintAddRemoveCurrentTerritoryBlacklist()
        {
            ImGui.Text("Blacklist Operations");
            ImGui.Separator();
            ImGui.Spacing();

            ImGui.Text($"Currently in MapID: [{Service.ClientState.TerritoryType}]");
            ImGui.Spacing();

            if (ImGui.Button("Add Here", ImGuiHelpers.ScaledVector2(125, 25)))
            {
                var blacklist = Service.Configuration.TerritoryBlacklist;

                if (!blacklist.Contains(Service.ClientState.TerritoryType))
                {
                    blacklist.Add(Service.ClientState.TerritoryType);
                    Service.Configuration.ForceWindowUpdate = true;
                }
            }

            ImGui.SameLine();

            if (ImGui.Button("Remove Here", ImGuiHelpers.ScaledVector2(125, 25)))
            {
                var blacklist = Service.Configuration.TerritoryBlacklist;

                if (blacklist.Contains(Service.ClientState.TerritoryType))
                {
                    blacklist.Remove(Service.ClientState.TerritoryType);
                    Service.Configuration.ForceWindowUpdate = true;
                }
            }

            ImGuiComponents.HelpMarker("Adds or Removes the current map to or from the blacklist.");

            ImGui.Spacing();
        }
        private void PrintAddRemoveManualTerritoryBlacklist()
        {
            ImGui.Text("Manually Add or Remove");
            ImGui.Spacing();

            ImGui.PushItemWidth(150 * ImGuiHelpers.GlobalScale);
            ImGui.InputInt("##AddToBlacklist", ref ModifyBlacklistValue, 0, 0);
            ImGui.PopItemWidth();

            ImGui.SameLine();

            if (ImGui.Button("Add", ImGuiHelpers.ScaledVector2(75, 25)))
            {
                AddToBlacklist(ModifyBlacklistValue);
            }

            ImGui.SameLine();

            if (ImGui.Button("Remove", ImGuiHelpers.ScaledVector2(75, 25)))
            {
                RemoveFromBlacklist(ModifyBlacklistValue);
            }

            ImGui.SameLine();

            ImGuiComponents.HelpMarker("Adds or Removes specified map to or from the blacklist");

            ImGui.Spacing();
        }
        private static void RemoveFromBlacklist(int territory)
        {
            var blacklist = Service.Configuration.TerritoryBlacklist;

            if (blacklist.Contains(territory))
            {
                blacklist.Remove(territory);
                Service.Configuration.ForceWindowUpdate = true;
            }
        }
        private static void AddToBlacklist(int territory)
        {
            var blacklist = Service.Configuration.TerritoryBlacklist;

            if (!blacklist.Contains(territory))
            {
                blacklist.Add(territory);
                Service.Configuration.ForceWindowUpdate = true;
            }
        }
    }
}