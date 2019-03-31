using Core.Management;
using UnityEngine;

namespace InternalAssets.Resources.Database.Manager
{
	public class DataManager : IManager
	{
		private DataManagerModel _dataManagerModel;
		private DataManagerModelDictionary _dataManagerModelDictionary;

		public T GetScriptableObjectDictionary<T>() where T: ScriptableObject
		{
			var tableType = typeof(T);
			
			var result = _dataManagerModelDictionary.GetScriptableObject(tableType);
			return (T)result;
		}

		public void Init()
		{
			_dataManagerModelDictionary = new DataManagerModelDictionary();
		}
	}
}