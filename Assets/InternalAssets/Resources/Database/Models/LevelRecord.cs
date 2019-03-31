using InternalAssets.Scripts.Common.Components;
using InternalAssets.Scripts.MVC.View;
using UnityEngine;

namespace InternalAssets.Resources.Database.Models
{
	[CreateAssetMenu(fileName = "LevelRecord", menuName = "LevelRecord", order = 51)]
	public class LevelRecord : ScriptableObject
	{
		public int StartTimer;
		public int MinPlayersToStart;
		public int PlayerSpeed;
		public float KickDistance;
		public float CameraSmooth;
		public float SpawnRange;
		public float SpawnDistance;
		public int FieldElementMax;
		public int FinishDistance;

		public RoadView FieldPrefabDefault;
		public RoadView FieldPrefabStart;
		public RoadView FieldPrefabFinish;
	}
}