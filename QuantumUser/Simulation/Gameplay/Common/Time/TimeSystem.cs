using UnityEngine;

namespace Quantum.QuantumUser.Simulation.Gameplay.Common.Time
{
	public unsafe class TimeSystem : SystemMainThread {
		
		public override void Update(Frame f) 
		{
			f.Global->ElapsedTime += f.DeltaTime;
			
			// Debug.Log($"Quantum TIME: {f.Global->ElapsedTime}");
		}
	}
}