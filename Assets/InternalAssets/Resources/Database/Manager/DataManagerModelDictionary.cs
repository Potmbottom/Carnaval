using System;
using System.Collections.Generic;
using UnityEngine;

namespace InternalAssets.Resources.Database.Manager
{
	public class DataManagerModelDictionary
	{
		private const string ScriptableObjectsPaths = "ScriptableObjects/";
		private Dictionary<string, object> _objectDic;

		private void LazyCached()
		{
			if (_objectDic != null) return;
			
			//var s = new Stopwatch();
			//s.Start();
			_objectDic = new Dictionary<string, object>();
			var results = UnityEngine.Resources.LoadAll(ScriptableObjectsPaths);
			//Debug.Log("Load count" + results.Length);
			foreach (var res in results)
			{
				var resType = res.GetType().FullName;
				if (resType != null) _objectDic[resType] = res;
			}

			//s.Stop();
			//DebugLogger.Log(this, "s: " + s.ElapsedMilliseconds);
		}

		public object GetScriptableObject(Type scriptableTableType)
		{
			LazyCached();
			
			var typeName = scriptableTableType.FullName;
			if (typeName == null) return null;
			
			var contains = _objectDic.ContainsKey(typeName);
			if (contains)
			{
				return _objectDic[typeName];
			}

			return null;
		}
	}
}