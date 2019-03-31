using System;
using InternalAssets.Scripts.Common.Components;
using InternalAssets.Scripts.Common.Slots;
using InternalAssets.Scripts.Extensions;
using Photon.Pun;
using UniRx;
using UnityEngine;

namespace InternalAssets.Scripts.MVC.Controller
{
	public class PlayerController
	{
		public event Action Finish;
		
		public MoveComponent Component => _move;

		private readonly GameObject[] _prefabs;
		private readonly Transform _pivot;
		private GameObject _instance;
		private MoveComponent _move;
		private Rigidbody _rigid;
		private FinishComponent _finish;
		private JumpComponent _jumpComponent;
		private ControlComponent _control;

		private bool _disabled;

		public PlayerController(Transform pivot, GameObject[] prefabs)
		{
			_pivot = pivot;
			_prefabs = prefabs;
		}

		private void SetupCarOnTrack(int gridStartIndex)
		{
			Debug.Log("Grid index " + gridStartIndex);
			var value = SlotLanes.Instance.GridPositions[gridStartIndex];
			var pos = value.Start;
			_instance = PhotonNetwork.Instantiate(_prefabs[gridStartIndex].name, pos.position, Quaternion.identity);
			_instance.transform.LookAt(value.End);
            
			PhotonNetwork.RegisterPhotonView(_instance.GetComponent<PhotonView>());
			_rigid = _instance.GetComponent<Rigidbody>();
			_move = _instance.AddComponent<MoveComponent>();
			_finish = _instance.AddComponent<FinishComponent>();
			_control = _instance.AddComponent<ControlComponent>();
			_jumpComponent = _instance.AddComponent<JumpComponent>();
			
			_control.Init(this);
			_finish.Finish += OnFinish;
		}

		public void Jump()
		{
			if(_disabled) return;
			
			_jumpComponent.Jump();
		}

		private void OnFinish()
		{
			if(_disabled) return;

			_disabled = true;
			_move.Dispose();
			Finish.SafeInvoke();
		}
        
		public void StartRun(int receivedGridIndex)
		{                
			SetupCarOnTrack(receivedGridIndex);

			Observable.Timer(TimeSpan.FromSeconds(1)).Subscribe(_ =>
			{
				var dir = SlotLanes.Instance.GridPositions[receivedGridIndex].Direction;
				_move.Move(_rigid, dir);
			});
		}

		public void Disable()
		{
			_disabled = true;
		}

		public void Dispose()
		{
			PhotonNetwork.Destroy(_instance);
		}
	}
}