using Code.Player.Camera;
using Quantum;
using UnityEngine;

namespace Code.Player
{
	[RequireComponent(typeof(UnityEngine.Camera))]
	public class FollowCamera : QuantumSceneViewComponent<ViewsContext>
	{
		[SerializeField] private Vector3[] _camerasTeamRotation;
		[SerializeField] private float[] _offsetsZ;
		[SerializeField] private float _borderMinZ = -6;
		[SerializeField] private float _borderMaxZ = 6;
		[SerializeField] private float _followSpeed = 4f;

		private UnityEngine.Camera _localCamera;
		private int _teamIndex;

		public override void OnInitialize()
		{
			_localCamera = GetComponent<UnityEngine.Camera>();
			QuantumEvent.Subscribe<EventPlayerSetupedEvent>(this, OnPlayerSetupedEvent);
		}

		private void OnPlayerSetupedEvent(EventPlayerSetupedEvent callback)
		{
			if (Game.PlayerIsLocal(callback.PlayerLink.Value))
			{
				_teamIndex = callback.TeamIndex;
				transform.eulerAngles = _camerasTeamRotation[_teamIndex];
			}
		}

		public override void OnUpdateView()
		{
			if (ViewContext.OurPlayerGameObject == null)
				return;

			var target = ViewContext.OurPlayerGameObject.transform;
			if (target == null)
				return;

			var currentPosition = _localCamera.transform.position;

			float desiredZ = target.position.z + _offsetsZ[_teamIndex];
			desiredZ = Mathf.Clamp(desiredZ, _borderMinZ, _borderMaxZ);

			float smoothedZ = Mathf.Lerp(currentPosition.z, desiredZ, Time.deltaTime * _followSpeed);

			_localCamera.transform.position = new Vector3(
				currentPosition.x,
				currentPosition.y,
				smoothedZ
			);
		}
	}
}