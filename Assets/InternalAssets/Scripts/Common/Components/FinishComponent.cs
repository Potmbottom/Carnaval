using System;
using InternalAssets.Scripts.Extensions;
using UnityEngine;

namespace InternalAssets.Scripts.Common.Components
{
	public class FinishComponent : MonoBehaviour
	{
		public event Action Finish; 
		
		private void OnTriggerEnter(Collider other)
		{
			if(!other.gameObject.CompareTag("Finish")) return;
			
			Finish.SafeInvoke();
 		}
	}
}