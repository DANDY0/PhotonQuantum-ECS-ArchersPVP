using System;
using Quantum.QuantumUser.Simulation.Common.Extensions;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Statuses.Factory
{
    public class StatusFactory: IStatusFactory
    {
        public EntityRef CreateStatus(Frame f, StatusSetup setup, EntityRef producerRef, EntityRef targetRef,
            Owner owner)
        {
            EntityRef status;
            
            switch (setup.StatusTypeId)
            {
                case EStatusTypeId.Fire:
                    status = CreateFireStatus(f, setup, producerRef, targetRef, owner);
                    break;
                case EStatusTypeId.Freeze:
                    status = CreateFreezeStatus(f, setup, producerRef, targetRef, owner);
                    break;
                case EStatusTypeId.Poison:
                    status = CreatePoisonStatus(f, setup, producerRef, targetRef, owner);
                    break;
                
                default:
                    throw new Exception($"Effect with typeId: {setup.StatusTypeId} does not exist");
            }

            if (setup.Duration > 0)
            {
                f.Set(status, new Duration { Value = setup.Duration});
                f.Set(status, new TimeLeft { Value = setup.Duration});
            }

            if (setup.Period > 0)
            {
                f.Set(status, new Period { Value = setup.Period});
                f.Set(status, new TimeSinceLastTick { Value = 0});
            }
            
            return status;
        }

        private EntityRef CreateFireStatus(Frame f, StatusSetup setup, EntityRef producerRef, EntityRef targetRef,
            Owner owner)
        {
            var entityRef = CreateStatusBase(f, setup, producerRef, targetRef, owner);
         
            f.Set(entityRef, new StatusTypeId { Value = EStatusTypeId.Fire});
            f.Add<Fire>(entityRef);

            return entityRef;
        }

        private EntityRef CreateFreezeStatus(Frame f, StatusSetup setup, EntityRef producerRef, EntityRef targetRef,
            Owner owner)
        {
            var entityRef = CreateStatusBase(f, setup, producerRef, targetRef, owner);

            f.Set(entityRef, new StatusTypeId { Value = EStatusTypeId.Freeze});
            f.Add<Freeze>(entityRef);

            return entityRef;
        }

        private EntityRef CreatePoisonStatus(Frame f, StatusSetup setup, EntityRef producerRef, EntityRef targetRef, Owner owner)
        {
            var entityRef = CreateStatusBase(f, setup, producerRef, targetRef, owner);

            f.Set(entityRef, new StatusTypeId { Value = EStatusTypeId.Poison});
            f.Add<Poison>(entityRef);

            return entityRef;
        }

        private static EntityRef CreateStatusBase(Frame f, StatusSetup setup, EntityRef producerRef, EntityRef targetRef,
            Owner owner)
        {
            EntityRef entityRef = f.CreateEmpty(owner);

            f.Set(entityRef, new EffectValue { Value = setup.Value});
            f.Set(entityRef, new ProducerId { Value = producerRef});
            f.Set(entityRef, new TargetId { Value = targetRef});
            f.Set(entityRef, new StatusEffectTypeId { Value = setup.StatusEffectTypeId});
            
            f.Add<Status>(entityRef);
            
            return entityRef;
        }
    }
}