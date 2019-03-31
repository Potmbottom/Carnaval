using System;
using UniRx;
using UnityEngine;

namespace InternalAssets.Scripts.Common.Components
{
	public class JumpComponent : MonoBehaviour
	{
		private Rigidbody _rigidbody;
		private MoveComponent _move;
		private bool _inJump;
		
		private void Start()
		{
			_rigidbody = GetComponent<Rigidbody>();
			_move = GetComponent<MoveComponent>();
		}
		
		private void OnCollisionEnter(Collision other)
		{
			if(!other.gameObject.CompareTag("Ground")) return;

			_inJump = false;
			_rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
			_rigidbody.freezeRotation = true;
			
		}

		public void Jump()
		{
			if(_inJump) return;

			_inJump = true;
			_rigidbody.AddForce(Vector3.up * 20, ForceMode.VelocityChange);
			Dispose();
			_rigidbody.constraints = RigidbodyConstraints.None;
			_rigidbody.freezeRotation = true;
		}

		public void Dispose()
		{
			
		}
	}
}
