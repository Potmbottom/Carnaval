using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using InternalAssets.Resources.Database;
using InternalAssets.Resources.Database.Models;
using InternalAssets.Scripts.Common.Components;
using InternalAssets.Scripts.Common.Environment;
using InternalAssets.Scripts.EventSystem.PunEvents;
using InternalAssets.Scripts.Lobby;
using InternalAssets.Scripts.MVC.Controller;
using Photon.Pun;
using Photon.Realtime;
using UniRx;
using UnityEngine;

namespace InternalAssets.Scripts.Common.Manager
{
	public class GameMediator : MonoBehaviourPunCallbacks, IOnEventCallback
	{
	    [SerializeField] private GameObject[] _prefabs;
		
		private CameraManager _cameraManager;
		private HeadPivotWatcher _headPivotWatcher;
	    private PlayerController _playerController;
		private SpawnManager _spawnManager;
		private FieldGenerator _fieldGenerator;
		private LobbyLauncher _launcher;
        private int _index;
		
		private void OnFinish()
		{
			var option = new RaiseEventOptions
			{
				Receivers = ReceiverGroup.All,
			};
			var dispatch = PhotonNetwork.RaiseEvent(EventName.Finish, PhotonNetwork.LocalPlayer.ActorNumber, option, SendOptions.SendReliable);
		}

	    private void StartRun(int receivedGridIndex)
	    {
		    
		    
		    _cameraManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraManager>();
		    _launcher = GameObject.FindGameObjectWithTag("UiManager").GetComponent<LobbyLauncher>();
		    _playerController = new PlayerController(transform, _prefabs);
		    _playerController.Finish += OnFinish;
		    _fieldGenerator = new FieldGenerator(Vector3.zero);
		    _fieldGenerator.Generate();
		    
		    _playerController.StartRun(receivedGridIndex);
		    Observable.Timer(TimeSpan.FromSeconds(1)).Subscribe(_ =>
		    {
			    _cameraManager.StartWatch();
		    
			    _spawnManager = new SpawnManager();
			    _spawnManager.Spawn += OnSpawnElement;
			    _spawnManager.StartWatch();
			    
			    _headPivotWatcher = new HeadPivotWatcher();
			    _headPivotWatcher.HeadChange += OnHeadPivotChange;
			    _headPivotWatcher.KickPlayer += OnKick;
			    _headPivotWatcher.StartWatch();
		    });
	    }
		
		private void DisposeAll()
		{
			_cameraManager?.Dispose();
			_headPivotWatcher?.Dispose();
			_spawnManager?.Dispose();
			_fieldGenerator?.Dispose();
			
			_playerController?.Dispose();
		}

		#region Ingame callbacks

		private void OnSpawnElement(int index)
		{
			if (PhotonNetwork.IsMasterClient)
			{
				var option = new RaiseEventOptions
				{
					Receivers = ReceiverGroup.All,
				};
				Debug.Log("Dispatch spawn");
				var dispatch = PhotonNetwork.RaiseEvent(EventName.SpawnElement, index, option, SendOptions.SendReliable);
			}
		}

		private void OnHeadPivotChange(MoveComponent head)
		{
			_cameraManager.HeadChange(head);
			_spawnManager.ChangeHead(head);
		}

		private void OnKick(PhotonView view, MoveComponent move)
		{
			var index = view.OwnerActorNr;
			if(index != PhotonNetwork.LocalPlayer.ActorNumber) return;
			
			_playerController.Disable();
		}

		private void OnFinish(int playerId)
		{
			DisposeAll();
			var winner = PhotonNetwork.LocalPlayer.ActorNumber == playerId;
			_launcher.ShowFinishView(winner);
		}

		#endregion

        public void OnEvent(EventData photonEvent)
        {
	        
            if (photonEvent.Code == EventName.StartRun)
            {  
                var data = (object[])photonEvent.CustomData;
                var playersList = PhotonNetwork.CurrentRoom.Players.Values.ToList();
	            var player = playersList.FirstOrDefault(x => x.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber);
	            
	            Debug.Log(PhotonNetwork.LocalPlayer.ActorNumber);
	            
                var playerSlotIndex = playersList.IndexOf(player);
                var receivedGridIndex = (int)data[PhotonNetwork.LocalPlayer.ActorNumber]; 
				StartRun(receivedGridIndex);
            }
	        
	        if (photonEvent.Code == EventName.SpawnElement)
	        {
		        var records = ScriptableUtils.GetSpawnObjects();
		        var index = (int)photonEvent.CustomData;
		        var data = records[index];
		        if (data.Type == SpawnObjectType.Arch || data.Type == SpawnObjectType.Wall)
		        {
			        if (PhotonNetwork.IsMasterClient)
			        {
				        _spawnManager.SpawnObject(data);
			        }
		        }
		        else
		        {
			        _spawnManager.SpawnBonus(data, _playerController.Component);
		        }
	        }

	        if (photonEvent.Code == EventName.Finish)
	        {
		        Debug.Log("Finish ");
		        var playerId = (int) photonEvent.CustomData;
		        OnFinish(playerId);
	        }
        }
	}
}