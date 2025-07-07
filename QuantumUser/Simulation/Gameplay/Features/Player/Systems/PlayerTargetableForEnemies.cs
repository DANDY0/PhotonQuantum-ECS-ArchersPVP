using Photon.Deterministic;
using UnityEngine.Scripting;

/*namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Player.Systems
{
	[Preserve]
	public unsafe class PlayerTargetableForEnemies : SystemMainThreadFilter<PlayerTargetableForEnemies.Filter>
	{
		public override void Update(Frame f, ref Filter filter)
		{
			if (!f.Has<TemporaryTargetable>(filter.Entity))
				return;

			if (!f.Has<CooldownLeft>(filter.Entity))
			{
				f.Remove<TemporaryTargetable>(filter.Entity);
				if (f.Has<Cooldown>(filter.Entity)) 
					f.Remove<Cooldown>(filter.Entity);
			}
		}

		public struct Filter
		{
			public EntityRef Entity;
			public PlayerLink* Link;
			public Transform3D* Transform3D;
			public PlayerLifeState* PlayerLifeState;
		}
	}
}*/