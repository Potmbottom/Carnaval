using System.Collections.Generic;
using UnityEngine;

namespace InternalAssets.Resources.Database.Models
{
	[CreateAssetMenu(fileName = "SpawnObjectTable", menuName = "SpawnObjectTable", order = 51)]
	public class SpawnObjectTable : ScriptableObject
	{
		public List<SpawnObjectRecord> Records;
	}
}