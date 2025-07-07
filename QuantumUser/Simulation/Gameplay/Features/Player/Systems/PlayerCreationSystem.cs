using System.Collections.Generic;
using Photon.Deterministic;
using Quantum.Prototypes;
using Quantum.QuantumUser.Simulation.Common.Di;
using Quantum.QuantumUser.Simulation.Gameplay.Common.CardsSetup;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Player.Factory;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Weapons.Factory;
using Quantum.QuantumUser.Simulation.Gameplay.StaticData;
using UnityEngine;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Player.Systems
{
    [Preserve]
    public class PlayerCreationSystem : SystemSignalsOnly, ISignalOnPlayerAdded, ISignalRespawnPlayer
    {
        private IStaticDataService _staticDataService;
        private IPlayerFactory _playerFactory;
        private IWeaponFactory _weaponFactory;
        private IDeckUtility _deckUtility;

        public override void OnInit(Frame f)
        {
            _playerFactory = DI.Resolve<IPlayerFactory>();
            _weaponFactory = DI.Resolve<IWeaponFactory>();
            _deckUtility = DI.Resolve<IDeckUtility>();
            
            _deckUtility.Init();
        }

        public void OnPlayerAdded(Frame f, PlayerRef player, bool firstTime)
        {
            var teamIndex = GetTeamIndex(player);
            
            FPVector3 spawnPosition = GetSpawnPositionForTeam(f, teamIndex);
            EntityRef playerEntity = _playerFactory.CreatePlayer(f, player , spawnPosition, teamIndex);
            
            var playerData = f.GetPlayerData(player);
            
            HashSet<EAbilityCardId> addedAbilities = new HashSet<EAbilityCardId>();
            EntityRef basicShotAbility = _weaponFactory.CreateBowShotWeapon(f, level: 1, f.Get<Owner>(playerEntity));
            
            foreach (var cardAbilityData in playerData.cardAbilityData)
            {
                if (cardAbilityData.cardAbility != EAbilityCardId.None && !addedAbilities.Contains(cardAbilityData.cardAbility))
                {
                    
                    //TODO 
                    //make normal card type check to nopt extend a lot this "if checks"
                    if (cardAbilityData.cardAbility == EAbilityCardId.FireArrow || 
                        cardAbilityData.cardAbility == EAbilityCardId.FrostArrow ||
                        cardAbilityData.cardAbility == EAbilityCardId.PoisonArrow)
                    {
                        _deckUtility.AddCardAbilityToBowShotWeapon(f, basicShotAbility, cardAbilityData.cardAbility);
                    }
                    else if (cardAbilityData.cardAbility == EAbilityCardId.FireBall 
                             || cardAbilityData.cardAbility == EAbilityCardId.FrostBall
                             || cardAbilityData.cardAbility == EAbilityCardId.PoisonBall)
                    { 
                        var orbitalShotAbility = _weaponFactory.CreateOrbitingShotWeapon(f, level: 1, f.Get<Owner>(playerEntity));
                        _deckUtility.AddCardAbilityToOrbitalShotWeapon(f, orbitalShotAbility, cardAbilityData.cardAbility);
                    }

                    addedAbilities.Add(cardAbilityData.cardAbility);
                }
            }

            if (playerData.createDummy)
            {
                EntityRef dummy = _playerFactory.CreateBot(f, player, new FPVector3(0, FP._0_50 - FP._0_05, 3), teamIndex == 0 ? 1 : 0);
                f.Signals.SetupPlayer(f.Get<Owner>(dummy), spawnPosition, isRespawn: false, isDummy: true);
            } 
            
            f.Signals.SetupPlayer(f.Get<Owner>(playerEntity), spawnPosition, isRespawn: true, isDummy: false);
        }

        public void RespawnPlayer(Frame f, PlayerLink playerLink)
        {
            var teamIndex = GetTeamIndex(playerLink.Value);
            var playerData = f.GetPlayerData(playerLink.Value);
            FPVector3 spawnPosition = GetSpawnPositionForTeam(f, teamIndex);
            
            if (playerData.createDummy)
            {
                EntityRef dummy = _playerFactory.CreateBot(f, playerLink.Value, new FPVector3(0, FP._0_50 - FP._0_05, 3), teamIndex == 0 ? 1 : 0);
                f.Signals.SetupPlayer(f.Get<Owner>(dummy), spawnPosition, isRespawn: true, isDummy: true);
            } 
            
            f.Signals.SetupPlayer(f.Get<Owner>(playerLink.Entity), spawnPosition, isRespawn: true, isDummy: false);
        }

        private FPVector3 GetSpawnPositionForTeam(Frame f, int teamIndex)
        {
            ComponentPrototypeSet[] mapEntities = f.Map.MapEntities;

            if (teamIndex < mapEntities.Length)
            {
                foreach (var component in mapEntities[teamIndex].Components)
                {
                    if (component is Transform3DPrototype transformPrototype)
                    {
                        Debug.Log($"Spawn Position for Team {teamIndex}: {transformPrototype.Position}");
                        return transformPrototype.Position;
                    }
                }
            }

            Debug.LogError($"No Transform3DPrototype found for team index {teamIndex}");
            return FPVector3.Zero;
        }

        private int GetTeamIndex(PlayerRef player) =>
            player % 2 == 0 ? 0 : 1;
    }
}
