
namespace Quantum.QuantumUser.Simulation.Common.Extensions
{
    public static class FrameExtensions
    {
        public static EntityRef CreateEmpty(this Frame f, Owner owner)
        {
            var entity = f.Create();

            return CreateWithOwner(f, entity, owner);
        }

        public static EntityRef CreateEmpty(this Frame f, AssetRef<EntityPrototype> prototype, Owner owner)
        {
            var entity = f.Create(prototype);
            
            return CreateWithOwner(f, entity, owner);
        }

        private static EntityRef CreateWithOwner(Frame f, EntityRef entity, Owner owner)
        {
            f.Set(entity, owner);
            return entity;
        }
    }
}