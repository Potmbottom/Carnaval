using System.Collections.Generic;
using Core.Management;
using InternalAssets.Resources.Database.Manager;
using InternalAssets.Resources.Database.Models;

namespace InternalAssets.Resources.Database
{
	public static class ScriptableUtils
	{
		public static LevelRecord GetLevelRecord()
		{
			var manager = CoreManager.Instance.GetData<DataManager>();
			var table = manager.GetScriptableObjectDictionary<LevelRecord>();
			return table;
		}

		public static List<SpawnObjectRecord> GetSpawnObjects()
		{
			var manager = CoreManager.Instance.GetData<DataManager>();
			var table = manager.GetScriptableObjectDictionary<SpawnObjectTable>();
			return table.Records;
		}
	}
}