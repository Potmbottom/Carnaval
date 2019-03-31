using System;
using System.Collections.Generic;
using System.Linq;
using InternalAssets.Resources.Database;
using InternalAssets.Scripts.Common.Components;
using InternalAssets.Scripts.Extensions;
using Photon.Pun;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace InternalAssets.Scripts.Common.Manager
{
	public class HeadPivotWatcher
	{
		public event Action<PhotonView, MoveComponent> KickPlayer;
		public event Action<MoveComponent> HeadChange; 
		
		private readonly float _kickDistance;
		
		private IDisposable _disposable;
		private List<MoveComponent> _components;
		private MoveComponent _head;
		
		public HeadPivotWatcher()
		{
			var level = ScriptableUtils.GetLevelRecord();
			_kickDistance = level.KickDistance;
		}

		public void StartWatch()
		{
			_components = Object.FindObjectsOfType<MoveComponent>().ToList();
			HeadChange.SafeInvoke(_components[0]);
			
			_disposable = Observable.EveryUpdate().Subscribe(x =>
			{
				var ordered = _components.OrderByDescending(y => y.TraveledDistance).ToList();
				FindHead(ordered);
				FindKicked(ordered);
			});
		}

		public void Dispose()
		{
			_components = null;
			_disposable.Dispose();
		}
		
		private void FindHead(List<MoveComponent> ordered)
		{
			if(!ordered.Any()) return;
			
			var head = ordered.First();
			HeadChange.SafeInvoke(head);

			_head = head;
		}
		

		private void FindKicked(List<MoveComponent> ordered)
		{
			if(!ordered.Any()) return;
			
			var first = ordered.First().transform;
			var last = ordered.Last();

			var distance = Vector3.Distance(first.position, last.transform.position);
			if (distance >= _kickDistance)
			{
				Kick(last);
			}
		}


		private void Kick(MoveComponent component)
		{
			var view = component.gameObject.GetComponentInParent<PhotonView>();
			KickPlayer?.Invoke(view, component);
		}
	}
}
