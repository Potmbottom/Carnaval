using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InternalAssets.Scripts.MVC.View
{
	public class FinishView : MonoBehaviour
	{
		public Button Quit;
		[SerializeField] private TextMeshProUGUI _winner;

		public void Init(bool winner)
		{
			var text = winner ? "winner" : "loser";
			_winner.text = text;
		}
	}
}