using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Management
{
	public interface ICoreManagerData
	{
		T GetManager<T>() where T : IManager, new();
		T CreateInstanceT<T>() where T : IManager, new();
		void Init<T>() where T : IManager, new();
		void ClearAll();
		void RegisterManager<T>(T customManager) where T : IManager;
		void UnRegisterManager<T>(T customManager) where T : IManager;
	}
}
