using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Management
{
	public class CoreManager : ICoreManager
	{
		#region Singletone
		private static readonly CoreManager instance = new CoreManager();

		// Explicit static constructor to tell C# compiler
		// not to mark type as beforefieldinit
		static CoreManager()
		{
		}

		//default constructor
		private CoreManager()
		{
		}

		public static CoreManager Instance
		{
			get
			{
				return instance;
			}
		}

		#endregion

		private ICoreManagerData _coreData;

		private void CheckDataForNull()
		{
			if (_coreData == null)
			{
				_coreData = new CoreManagerData();
			}
		}

		public T GetData<T>() where T : IManager, new()
		{
			CheckDataForNull();

			return _coreData.GetManager<T>();
		}

		public void RegisterManager<T>(T customManager) where T: IManager
		{
			CheckDataForNull();
			_coreData.RegisterManager(customManager);
		}

		public void UnRegisterManager<T>(T customManager) where T: IManager
		{
			CheckDataForNull();
			_coreData.UnRegisterManager(customManager);
		}

		public void Init<T>() where T : IManager, new()
		{
			CheckDataForNull();

			_coreData.Init<T>();
		}

		public void ClearAllData()
		{
			CheckDataForNull();

			_coreData.ClearAll();
		}
	}
}
