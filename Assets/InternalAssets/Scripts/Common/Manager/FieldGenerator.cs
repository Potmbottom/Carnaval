using System.Collections.Generic;
using System.Linq;
using InternalAssets.Resources.Database;
using InternalAssets.Scripts.Common.Components;
using InternalAssets.Scripts.MVC.View;
using UniRx;
using UnityEngine;

namespace InternalAssets.Scripts.Common.Manager
{
	public class FieldGenerator
	{
		private readonly RoadView _prefabDefault;
		private readonly RoadView _prefabStart;
		private readonly RoadView _prefabFinish;
		private readonly int _finishDistance;
		
		private List<RoadView> _renderComponents;
		private Vector3 _currentPosition;
		private Vector3 _meshSize;
		
		public FieldGenerator(Vector3 startPosition)
		{
			_currentPosition = startPosition;
			_renderComponents = new List<RoadView>();
			var level = ScriptableUtils.GetLevelRecord();
			_prefabDefault = level.FieldPrefabDefault;
			_prefabStart = level.FieldPrefabStart;
			_prefabFinish = level.FieldPrefabFinish;
			_finishDistance = level.FinishDistance;
		}

		public void Generate()
		{
			var start = Object.Instantiate(_prefabStart);
			_meshSize = start.GetMeshSize();
			_renderComponents.Add(start);

			var count = _finishDistance / _meshSize.z; 
			
			for (var i = 0; i < count; i++)
			{
				var item = Object.Instantiate(_prefabDefault);
				_renderComponents.Add(item);
			}
			
			var end = Object.Instantiate(_prefabFinish);
			_renderComponents.Add(end);
			
			foreach (var item in _renderComponents)
			{
				item.transform.position = _currentPosition;
				_currentPosition.z += _meshSize.z;
			}
		}

		public void Dispose()
		{
			foreach (var item in _renderComponents)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
		}
	}
}