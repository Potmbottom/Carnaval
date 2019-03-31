using System.Collections.Generic;

namespace InternalAssets.Resources.Database.Manager
{
	public class DataManagerModel
	{
		private const string ScriptableObjectsPaths = "ScriptableObjects/";
		private static Dictionary<string, object> _objectDic;

		private static void LazyCached()
		{
			if (_objectDic != null) return;
			
			_objectDic = new Dictionary<string, object>();
			var results = UnityEngine.Resources.LoadAll(ScriptableObjectsPaths);
			foreach (var res in results)
			{
				var resType = res.GetType().Name;
				_objectDic[resType] = res;
			}
		}

		public static object GetScriptableObject(string scriptableObjectTypeName)
		{
			LazyCached();
			
			var contains = _objectDic.ContainsKey(scriptableObjectTypeName);
			if (contains)
			{
				return _objectDic[scriptableObjectTypeName];
			}

			return null;
		}
	}
}