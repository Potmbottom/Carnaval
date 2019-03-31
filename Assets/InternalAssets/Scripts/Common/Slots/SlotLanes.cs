using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InternalAssets.Scripts.Common.Slots
{
	public class SlotLanes : MonoBehaviour {

		public static SlotLanes Instance;
		public SlotLanePositions[] GridPositions;

		private List<Color> _colors;

		private IEnumerator<Color> _numerator;

		public float CenterX
		{
			get
			{
				var sum = GridPositions.Sum(x => x.Start.transform.position.x);
				return sum / GridPositions.Length;
			}
		}

		void Awake()
		{
			Instance = this;
		}
		
		void OnDrawGizmosSelected()
		{
			if(GridPositions == null) return;
			
			_colors = new List<Color>
			{
				Color.blue, Color.red, Color.green, Color.cyan
			};
			_numerator = _colors.GetEnumerator();
			
			Gizmos.color = Color.blue;
			//Gizmos.DrawLine(transform.position, target.position);
			foreach (var item in GridPositions)
			{	
				var canNext = _numerator.MoveNext();
				if (!canNext)
				{
					_numerator = _colors.GetEnumerator();
				}
				Gizmos.color = _numerator.Current;
				Gizmos.DrawLine(item.Start.position, item.End.position);
			}	
		}
	}
}