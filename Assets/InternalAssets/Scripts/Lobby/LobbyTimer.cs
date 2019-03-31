using System;
using InternalAssets.Scripts.Extensions;
using UniRx;
using UnityEngine;

namespace InternalAssets.Scripts.Lobby
{
	public class LobbyTimer
	{
		public event Action OnTimerEnd;
		public event Action<int> SingleTick;
		
		private IDisposable _updater;
		private bool _alreadyStarted;
		private readonly int _time;
		private int _current;
		
		public LobbyTimer(int time)
		{
			_time = time;
		}

		public void Start()
		{
			if(_alreadyStarted) return;

			_alreadyStarted = true;
			_current = _time;
			_updater = Observable.Timer(TimeSpan.FromSeconds(1)).Repeat().Subscribe(_ =>
			{
				Debug.Log("Current " + _current);
				if (_current == 0)
				{
					Debug.Log("Timer end");
					TimerEnd();
					return;					
				}

				SingleTick.SafeInvoke(_current);
				_current--;
			});
		}

		public void Break()
		{
			if(!_alreadyStarted) return;

			_alreadyStarted = false;
			_updater.Dispose();
		}

		private void TimerEnd()
		{
			_alreadyStarted = false;
			_updater.Dispose();
			OnTimerEnd.SafeInvoke();
		}
	}
}