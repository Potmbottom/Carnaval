using InternalAssets.Scripts.MVC.Controller;
using Photon.Pun;
using UnityEngine;

namespace InternalAssets.Scripts.Common.Components
{
	public class ControlComponent : MonoBehaviour
	{
		[SerializeField] private PhotonView _view;

		private PlayerController _controller;
		
		private void Start()
		{
			_view = GetComponent<PhotonView>();
		}

		public void Init(PlayerController controller)
		{
			_controller = controller;
		}

		private void Update()
		{
			if (_view.IsMine)
			{
				if (Input.GetKeyDown(KeyCode.X))
				{
					_controller.Jump();
				}

				if (Input.touchCount > 0)
				{
					var touch = Input.GetTouch(0);
					if (touch.phase == TouchPhase.Began)
					{
						_controller.Jump();
					}
				}
			}
		}
	}
}