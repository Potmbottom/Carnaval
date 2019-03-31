using System;
using InternalAssets.Scripts.Common.Environment;
using UnityEngine;

namespace InternalAssets.Resources.Database.Models
{
	[Serializable]
	public class SpawnObjectRecord
	{
		public SpawnObjectType Type;
		public float SpawnChance;
		public GameObject Prefab;
	}
}