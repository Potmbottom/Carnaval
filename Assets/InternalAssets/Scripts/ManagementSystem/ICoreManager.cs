using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Management
{
	public interface ICoreManager
	{
		T GetData<T>() where T : IManager, new();
		void Init<T>() where T : IManager, new();
		void ClearAllData();
	}

}
