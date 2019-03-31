using System;
using System.Collections.Generic;
using System.Linq;
using InternalAssets.Resources.Database;
using InternalAssets.Resources.Database.Models;
using InternalAssets.Scripts.Common.Components;
using InternalAssets.Scripts.Common.Slots;
using InternalAssets.Scripts.Extensions;
using Photon.Pun;
using UniRx;
using UnityEngine;
using Object = System.Object;

namespace InternalAssets.Scripts.Common.Manager
{
	public class SpawnManager
	{
		public event Action<int> Spawn;
	
		private float _lastTraveledDistance;
		private readonly float _spawnDistance;
		private readonly float _spawnRange;

		private IDisposable _disposable;
		private List<GameObject> _items;
		private MoveComponent _head;
		
		public SpawnManager()
		{
			var record = ScriptableUtils.GetLevelRecord();
			_spawnDistance = record.SpawnDistance;
			_spawnRange = record.SpawnRange;
			_items = new List<GameObject>();
		}

		public void StartWatch()
		{
			_disposable = Observable.EveryUpdate().Subscribe(x =>
			{
				if(_head == null) return;

				var dist = _head.TraveledDistance - _lastTraveledDistance;
				if (dist >= _spawnDistance)
				{
					_lastTraveledDistance = _head.TraveledDistance;
					InvokeMasterSpawn();
				}
			});
		}

		public void ChangeHead(MoveComponent head)
		{
			_head = head;
		}

		private void InvokeMasterSpawn()
		{
			var records = ScriptableUtils.GetSpawnObjects();
			var random = records.GetRandomElement();
			var index = records.IndexOf(random);
			Spawn.SafeInvoke(index);
		}

		
		public void SpawnBonus(SpawnObjectRecord record, MoveComponent self)
		{			
			var headPos = _head.transform.position;
			headPos.z += _spawnRange;
			headPos.x = self.transform.position.x;
			headPos.y = 0;
			var item = PhotonNetwork.InstantiateSceneObject(record.Prefab.name, headPos, Quaternion.identity);
			_items.Add(item);
		}

		public void SpawnObject(SpawnObjectRecord record)
		{
			var center = SlotLanes.Instance.CenterX;
			
			var headPos = _head.transform.position;
			headPos.z += _spawnRange;
			headPos.x = center;
			headPos.y = 0;
			var item = PhotonNetwork.InstantiateSceneObject(record.Prefab.name, headPos, Quaternion.identity);
			_items.Add(item);
		}

		public void Dispose()
		{
			_disposable.Dispose();
			foreach (var item in _items)
			{
				PhotonNetwork.Destroy(item.gameObject);
			}
		}
	}
}