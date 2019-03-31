using System;
using System.Collections.Generic;
using InternalAssets.Resources.Database;
using InternalAssets.Scripts.Common.Components;
using UniRx;
using UnityEngine;

namespace InternalAssets.Scripts.Common.Manager
{
	public class CameraManager : MonoBehaviour
	{
		private MoveComponent _head;
		private IDisposable _disposable;
		private bool _watch;
		private bool _first;
		
		private float _cameraSmooth;
		
		public CameraManager()
		{
			
		}

		public void StartWatch()
		{
			var level = ScriptableUtils.GetLevelRecord();
			_cameraSmooth = level.CameraSmooth;
			_disposable = Observable.EveryUpdate().Subscribe(x =>
				{
					if(_head == null) return;
					
					var t = _head.transform.position;
					var pos = new Vector3(transform.position.x, transform.position.y, t.z);
					var vector = !_first ? Vector3.Lerp(transform.position, pos, Time.deltaTime * _cameraSmooth) : pos;
					transform.position = vector;
					_first = true;
				});
		}

		public void HeadChange(MoveComponent head)
		{
			_head = head;
		}

		public void Dispose()
		{
			_disposable.Dispose();
		}
	}
}