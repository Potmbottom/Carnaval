using System;
using UnityEngine;

namespace InternalAssets.Scripts.Common.Slots
{
	[Serializable]
	public class SlotLanePositions
	{
		public Transform Start;
		public Transform End;

		public Vector3 Direction
		{
			get
			{
				var direction = (End.position - Start.position).normalized;
				return direction;
			}
		}
	}
}