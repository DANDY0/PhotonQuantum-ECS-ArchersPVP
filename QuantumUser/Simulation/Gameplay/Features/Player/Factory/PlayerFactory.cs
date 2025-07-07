using System.Collections.Generic;
using Photon.Deterministic;
using Quantum.QuantumUser.Simulation.Common.Extensions;
using Quantum.QuantumUser.Simulation.Gameplay.Features.CharacterStats;
using UnityEngine;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Player.Factory
{
public unsafe class PlayerFactory : IPlayerFactory
{
    public EntityRef CreatePlayer(Frame f, PlayerRef player, FPVector3 at, int teamIndex)
    {
        var baseStats = InitPlayerStats();
        return CreateHero(f, player, at, teamIndex, baseStats, isDummy: false);
    }

    public EntityRef CreateBot(Frame f, PlayerRef player, FPVector3 at, int teamIndex)
    {
        var baseStats = InitBotStats();
        return CreateHero(f, player, at, teamIndex, baseStats, isDummy: true);
    }

    private Dictionary<EStats, FP> InitPlayerStats() => InitStats.EmptyStatDictionary()
        .With(x => x[EStats.Speed] = 4)
        .With(x => x[EStats.MaxHp] = 40)
        .With(x => x[EStats.AttackDelay] = FP._0_50);

    private Dictionary<EStats, FP> InitBotStats() => InitStats.EmptyStatDictionary()
        .With(x => x[EStats.Speed] = 0)
        .With(x => x[EStats.MaxHp] = 20)
        .With(x => x[EStats.AttackDelay] = FP._0_50);

    private EntityRef CreateHero(Frame f, PlayerRef player, FPVector3 at, int teamIndex,
        Dictionary<EStats, FP> baseStats, bool isDummy)
    {
        RuntimePlayer runtimePlayer = f.GetPlayerData(player);
        EntityRef heroEntity = f.Create(runtimePlayer.PlayerAvatar);

        var playerLink = new PlayerLink
        {
            Value = player,
            Entity = heroEntity
        };

        f.Set(heroEntity, playerLink);
        f.Set(heroEntity, new Owner { Link = playerLink, TeamIndex = teamIndex });

        f.Unsafe.GetPointer<Transform3D>(heroEntity)->Position = at;
        Debug.Log($"start pos: {f.Unsafe.GetPointer<Transform3D>(heroEntity)->Position}" +
                  $"at: {at}");

        var baseStatsDict = f.AllocateDictionary<EStats, FP>();
        f.Set(heroEntity, new BaseStats { Value = baseStatsDict });
        f.FillEnumDictionaryComponent(baseStatsDict, baseStats);

        if (isDummy)
            f.Add<Dummy>(heroEntity);

        return heroEntity;
    }
}
}