using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Core.Management
{
	public class CoreManagerData : ICoreManagerData
	{
		private List<BasicManagerData> _managerList;

		private void CheckForNullManagerList()
		{
			if (_managerList == null)
			{
				_managerList = new List<BasicManagerData>();
			}
		}

		public T CreateInstanceT<T>() where T : IManager, new()
		{
			return new T();
		}

		public void Init<T>() where T : IManager, new()
		{
			CheckForNullManagerList();

			var findedType = typeof(T);

			if (_managerList.Select(managerData => managerData.Manager)
				.Select(manager => manager.GetType())
				.Any(managerType => findedType == managerType))
			{
				return;
			}

			CreateBasicData<T>();
		}

		public void ClearAll()
		{
			_managerList = null;
		}

		private T CreateBasicData<T>() where T : IManager, new()
		{
			CheckForNullManagerList();

			var findedManager = CreateInstanceT<T>();
			findedManager.Init();

			var basicManagerData = new BasicManagerData { Manager = findedManager };
			_managerList.Add(basicManagerData);

			return findedManager;
		}

		public T GetManager<T>() where T : IManager, new()
		{
			CheckForNullManagerList();

			var findedType = typeof(T);
			
			foreach (var managerData in _managerList)
			{
				var manager = managerData.Manager;
				var managerType = manager.GetType();

				if (managerType == findedType)
				{
					return (T)manager;
				}
			}

			var findedManager = CreateBasicData<T>();

			return findedManager;
		}

		public void RegisterManager<T>(T customManager) where T: IManager
		{
			CheckForNullManagerList();
			
			var basicManagerData = new BasicManagerData { Manager = customManager };
			_managerList.Add(basicManagerData);
		}

		public void UnRegisterManager<T>(T customManager) where T : IManager
		{
			var findedType = typeof(T);
			BasicManagerData findedManager = null;
			foreach (var managerData in _managerList)
			{
				var manager = managerData.Manager;
				var managerType = manager.GetType();

				if (managerType == findedType)
				{
					findedManager = managerData;
					break;
				}
			}

			if(findedManager != null)
				_managerList.Remove(findedManager);
		}
	}

	public class BasicManagerData
	{
		public IManager Manager;
	}
}
