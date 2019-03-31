using UnityEngine;
using Random = UnityEngine.Random;

namespace InternalAssets.Scripts.MVC.View
{
	[RequireComponent(typeof(MeshRenderer))]
	public class RoadView : MonoBehaviour
	{
		public GameObject[] Trees;
		
		private MeshRenderer _mesh;
		private bool _startRender;
		private bool _invoked;
		
		private void Start()
		{
			_mesh = GetComponent<MeshRenderer>();
			InitRandom();
		}

		private void InitRandom()
		{
			if(Trees == null || Trees.Length == 0) return;
			for (var i = 0; i < 3; i++)
			{
				var random = Random.Range(0, Trees.Length);
				Trees[random].SetActive(true);
			}
		}

		public Vector3 GetMeshSize()
		{
			if (_mesh == null)
			{
				_mesh = GetComponent<MeshRenderer>();
			}

			return _mesh.bounds.size;
		}
		
	}
}