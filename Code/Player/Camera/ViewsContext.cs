using Quantum;
using UnityEngine;

namespace Code.Player.Camera
{
	public class ViewsContext : MonoBehaviour, IQuantumViewContext {
		public GameObject OurPlayerGameObject;
		public GameObject EnemyPlayerGameObject;
		public Owner OurPlayerOwner;
		public Owner EnemyPlayerOwner;
		
		public GameObject Camera;
	}
}