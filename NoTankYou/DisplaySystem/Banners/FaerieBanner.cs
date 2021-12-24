﻿using NoTankYou.Utilities;
using System.Diagnostics;
using System.Linq;

namespace NoTankYou.DisplaySystem.Banners
{
    internal class FaerieBanner : WarningBanner
    {
        protected override ref Configuration.ModuleSettings Settings => ref Service.Configuration.FaerieSettings;


        private readonly Stopwatch PetCountStopwatch = new();
        private readonly Stopwatch DissipationCountStopwatch = new();
        private readonly Stopwatch SoloPetStopwatch = new();
        private readonly Stopwatch SoloDissipationStopwatch = new();
        private int LastNumPlayersWithPets = 0;
        private int LastNumPlayersWithDissipation = 0;
        private bool LastSoloPet = false;
        private bool LastSoloDissipation = false;

        private const int ScholarClassID = 28;
        private const int DissipationStatusID = 791;

        public FaerieBanner() : base("NoTankYou Faerie Warning Banner", "Faerie")
        {

        }

        protected override void UpdateInParty()
        {
            var partyMembers = GetFilteredPartyList(p => p.ClassJob.Id == ScholarClassID && IsTargetable(p));

            var partyMemberData = PetUtilities.GetPartyMemberData(partyMembers);

            var numPlayersWithPet = partyMemberData.Count(playerData => playerData.Value.Item1.Any());
            var numPlayersWithDissipation = partyMemberData.Count(playerData => playerData.Value.Item2.Any(s => s.StatusId == DissipationStatusID));

            var numPetsChanged = numPlayersWithPet != LastNumPlayersWithPets;
            var numDissipationChanged = numPlayersWithDissipation != LastNumPlayersWithDissipation;

            if (numPetsChanged)
            {
                if (PetUtilities.DelayMilliseconds(500, PetCountStopwatch))
                {
                    LastNumPlayersWithPets = numPlayersWithPet;
                }
                else
                {
                    return;
                }
            }

            if (numDissipationChanged)
            {
                if (PetUtilities.DelayMilliseconds(500, DissipationCountStopwatch))
                {
                    LastNumPlayersWithDissipation = numPlayersWithDissipation;
                }
                else
                {
                    return;
                }
            }

            Visible = partyMemberData
                .Where(player => !player.Value.Item1.Any())// Where the player doesn't have any pets
                .Any(player => !player.Value.Item2.Any(s => s.StatusId is DissipationStatusID)); // If any of them don't have dissipation
        }

        protected override void UpdateSolo()
        {
            var player = Service.ClientState.LocalPlayer;
            if (player == null) return;

            var playerIsScholar = player.ClassJob.Id == ScholarClassID && player.CurrentHp > 0;

            var isPetPresent = Service.BuddyList.PetBuddyPresent;

            var playerHasDissipation = player.StatusList.Any(s => s.StatusId == DissipationStatusID);

            var petStatusChanged = isPetPresent != LastSoloPet;
            var dissipationStatusChange = playerHasDissipation != LastSoloDissipation;

            if (petStatusChanged)
            {
                if (PetUtilities.DelayMilliseconds(500, SoloPetStopwatch))
                {
                    LastSoloPet = isPetPresent;
                }
                else
                {
                    return;
                }
            }

            if (dissipationStatusChange)
            {
                if (PetUtilities.DelayMilliseconds(500, SoloDissipationStopwatch))
                {
                    LastSoloDissipation = playerHasDissipation;
                }
                else
                {
                    return;
                }
            }

            Visible = playerIsScholar && !(playerHasDissipation || isPetPresent);
        }
    }
}
