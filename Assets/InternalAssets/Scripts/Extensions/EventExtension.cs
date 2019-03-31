using System;

namespace InternalAssets.Scripts.Extensions
{
	public static class EventExtension
	{
		public static void SafeInvoke<T>(this Action<T> action, T value)
		{
			if(action == null) return;
			
			action.Invoke(value);
		}
		
		public static void SafeInvoke(this Action action)
		{
			if(action == null) return;
			
			action.Invoke();
		}
	}
}