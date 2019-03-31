using System;
using InternalAssets.Resources.Database;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace InternalAssets.Scripts.Common.Components
{
	[RequireComponent(typeof(Rigidbody))]
	public class MoveComponent : MonoBehaviour, IDisposable
	{
		public bool Kicked;
		public bool Disabled;
		public float TraveledDistance
		{
			get
			{
				var traveledDistance = GetTraveledDistance();
				return traveledDistance;
			}
		}
		
		private Rigidbody _rigidbody;
		private IDisposable _disposable;
		private Vector3 _startPoint;
		private float _startPosX;

		public void Move(Rigidbody rigidbody, Vector3 direction)
		{
			var level = ScriptableUtils.GetLevelRecord();
			_rigidbody = rigidbody;
			_startPoint = rigidbody.transform.position;
			_startPosX = _startPoint.x;
			_disposable = Observable.EveryFixedUpdate().Subscribe(_ =>
			{
				if(_rigidbody == null || Disabled || Kicked) return;
				
				_rigidbody.MovePosition(transform.position + transform.forward * (Time.deltaTime * level.PlayerSpeed));
			});
			
			_disposable = Observable.EveryUpdate().Subscribe(_ =>
			{
				if(_rigidbody == null) return;

				if (_rigidbody.velocity.x > 0)
				{
					_rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, _rigidbody.velocity.z);
					_rigidbody.transform.position = new Vector3(_startPosX, _rigidbody.transform.position.y, _rigidbody.transform.position.z);
				}
			});
		}
		
		private float GetTraveledDistance()
		{
			if (_rigidbody == null)
			{
				_rigidbody = GetComponent<Rigidbody>();
			}
			
			var distance = Vector3.Distance(_startPoint, _rigidbody.transform.position);
			return distance;
		}

		public void Dispose()
		{
			_rigidbody.velocity = Vector3.zero;
			_disposable.Dispose();
		}

		private void OnCollisionEnter(Collision other)
		{
			if(!other.gameObject.CompareTag("Block")) return;

			Disabled = true;
		}
		
		private void OnCollisionExit(Collision other)
		{
			if(!other.gameObject.CompareTag("Block")) return;
			Disabled = false;
		}
	}
}