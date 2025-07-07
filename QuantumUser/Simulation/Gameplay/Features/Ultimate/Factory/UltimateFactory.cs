using System;
using Photon.Deterministic;
using Quantum.QuantumUser.Simulation.Common.Extensions;
using UnityEngine;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Ultimate.Factory
{
    public class UltimateFactory : IUltimateFactory
    {
        public EntityRef CreateBasicUltimate(Frame f, int level, Owner owner, EUltimateId ultimateId, FPVector3 ultDirection)
        {
            EntityRef entityRef = f.CreateEmpty(owner);
            f.Set(entityRef, new ProducerId { Value = owner.Link.Entity });
            
            
            f.Add<Quantum.Ultimate>(entityRef);
            f.Set(entityRef, new UltimateId { Value = ultimateId });
            f.Set(entityRef, new UltDirection { Value = ultDirection });

            switch (ultimateId)
            {
                case EUltimateId.Unknown:
                    break;
                case EUltimateId.Basic:
                    f.Add<BasicUltimate>(entityRef);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ultimateId), ultimateId, null);
            }
            
            return entityRef;
        }
    }
}