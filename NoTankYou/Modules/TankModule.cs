﻿using System.Collections.Generic;
using System.Linq;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Lumina.Excel.GeneratedSheets;
using NoTankYou.Data.Components;
using NoTankYou.Data.Modules;
using NoTankYou.Interfaces;
using NoTankYou.Localization;
using Action = Lumina.Excel.GeneratedSheets.Action;
using Status = Lumina.Excel.GeneratedSheets.Status;

namespace NoTankYou.Modules
{
    internal class TankModule : IModule
    {
        public List<ClassJob> ClassJobs { get; }
        private static TankModuleSettings Settings => Service.Configuration.ModuleSettings.Tank;
        public GenericSettings GenericSettings => Settings;

        private readonly List<Status> TankStances;

        public TankModule()
        {
            ClassJobs = Service.DataManager.GetExcelSheet<ClassJob>()!
                .Where(job => job.Role is 1)
                .ToList();

            TankStances = Service.DataManager.GetExcelSheet<Action>()
                !.Where(r => r.ClassJob.Value?.Role == 1)
                .Select(r => r.StatusGainSelf.Value!)
                .Where(r => r.IsPermanent)
                .ToList();

            Settings.WarningText = Strings.Modules.Tank.WarningText;
        }

        public bool ShowWarning(PlayerCharacter character)
        {
            if (Service.PartyList.Length == 0)
            {
                return !character.StatusList.Any(status => TankStances.Contains(status.GameData));
            }

            return !Service.PartyList.Where(partyMember => partyMember.CurrentHP > 0 && ClassJobs.Contains(partyMember.ClassJob.GameData!))
                .Any(tanks => tanks.Statuses.Any(status => TankStances.Contains(status.GameData)));
        }
    }
}