using Core.Management;
using ExitGames.Client.Photon;
using InternalAssets.Resources.Database;
using InternalAssets.Resources.Database.Manager;
using InternalAssets.Scripts.EventSystem.PunEvents;
using InternalAssets.Scripts.MVC.View;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InternalAssets.Scripts.Lobby
{
	public class LobbyLauncher : MonoBehaviourPunCallbacks, IOnEventCallback
	{
		private const byte MaxPlayers = 4;
		
		[SerializeField] private Button _connectButton;
		[SerializeField] private Button _startButton;
		[SerializeField] private GameObject _panel;
		[SerializeField] private TextMeshProUGUI _loadingText;
		[SerializeField] private TextMeshProUGUI _timerText;
		[SerializeField] private FinishView _finishView;

		private LobbyTimer _lobbyTimer;
	    private bool _isConnecting;
	    private string _gameVersion = "1";
		private bool _started;

		void Awake()
		{
			PhotonNetwork.AutomaticallySyncScene = true;
			InitManagers();
			
			_connectButton.onClick.AddListener(Connect);
			_startButton.onClick.AddListener(StartGame);

			var level = ScriptableUtils.GetLevelRecord();
			_lobbyTimer = new LobbyTimer(level.StartTimer);
			_lobbyTimer.OnTimerEnd += StartGame;
			_lobbyTimer.SingleTick += LobbyTimerUpdate;
			
			
			PhotonNetwork.AddCallbackTarget(photonView);
		}

		public void Quit()
		{
			_finishView.Quit.onClick.AddListener(() =>
			{
				
			});
		}

		public void ShowFinishView(bool winner)
		{
			_loadingText.enabled = false;
			_timerText.enabled = false;
			_panel.SetActive(true);
			_finishView.gameObject.SetActive(true);
			_finishView.Init(winner);
		}
		
		private void InitManagers()
		{
			var core = CoreManager.Instance;
			core.Init<DataManager>();
		}

		private void StartGame()
		{
			PhotonNetwork.CurrentRoom.IsOpen = false;
			_started = true;
			_lobbyTimer.Break();
			var option = new RaiseEventOptions
			{
				Receivers = ReceiverGroup.All,
			};
			var grid = new object[] { 0, 1, 2, 3 };
			//grid.Shuffle();
			var dispatch = PhotonNetwork.RaiseEvent(EventName.StartRun, grid, option, SendOptions.SendReliable);
			Debug.Log(dispatch);
		}
		
		private void Connect()
		{
			_isConnecting = true;
			_connectButton.enabled = false;
			
			if (PhotonNetwork.IsConnected)
			{
				PhotonNetwork.JoinRandomRoom();
			}else{
				
				PhotonNetwork.GameVersion = this._gameVersion;
				PhotonNetwork.ConnectUsingSettings();
			}
		}


        #region MonoBehaviourPunCallbacks CallBacks
        public override void OnConnectedToMaster()
		{

			if (_isConnecting)
			{
				Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room.\n Calling: PhotonNetwork.JoinRandomRoom(); Operation will fail if no room found");
		
				// #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
				PhotonNetwork.JoinRandomRoom();
			}
		}

		public override void OnJoinRandomFailed(short returnCode, string message)
		{
			//Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

			// #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
			PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MaxPlayers});
		}

		public override void OnDisconnected(DisconnectCause cause)
		{
			//Debug.LogError("PUN Basics Tutorial/Launcher:Disconnected");

			_isConnecting = false;
		}

		public override void OnJoinedRoom()
		{
			//Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.\nFrom here on, your game would be running.");
		
			_connectButton.gameObject.SetActive(false);
			_loadingText.gameObject.SetActive(true);
			UpdateConnectedLabel();
			UpdateStartButtonState();
		}

		public override void OnPlayerEnteredRoom(Player newPlayer)
		{
			UpdateConnectedLabel();
			UpdateStartButtonState();

			Debug.Log("Player enter " + PhotonNetwork.CurrentRoom.PlayerCount + " " + MaxPlayers);
			var level = ScriptableUtils.GetLevelRecord();
			
			if (PhotonNetwork.CurrentRoom.PlayerCount >= level.MinPlayersToStart)
			{
				Debug.Log("Start timer");
				StartTimer();
			}
		}

		public override void OnPlayerLeftRoom(Player otherPlayer)
		{
			if(_started) return;
			
			UpdateConnectedLabel();
			UpdateStartButtonState();
			
			if (PhotonNetwork.CurrentRoom.PlayerCount < MaxPlayers)
			{
				EndTimer();
			}
		}

		private void LobbyTimerUpdate(int value)
		{
			var option = new RaiseEventOptions
			{
				Receivers = ReceiverGroup.All,
			};
			PhotonNetwork.RaiseEvent(EventName.TimerTick, value, option, SendOptions.SendReliable);
		}

		private void StartTimer()
		{
			_timerText.enabled = true;
			_lobbyTimer.Start();
		}

		private void EndTimer()
		{
			_timerText.enabled = false;
			_lobbyTimer.Break();
		}

		private void UpdateConnectedLabel()
		{
			_loadingText.text = "Players load " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + MaxPlayers;
		}

		private void UpdateStartButtonState()
		{
			if(!PhotonNetwork.IsMasterClient) return;

			var level = ScriptableUtils.GetLevelRecord();
			//var value = PhotonNetwork.CurrentRoom.PlayerCount >= level.MinPlayersToStart;
			_startButton.gameObject.SetActive(true);
		}

		#endregion

		public void OnEvent(EventData photonEvent)
		{
			if (photonEvent.Code == EventName.TimerTick)
			{
				var data = (int)photonEvent.CustomData;
				_timerText.enabled = true;
				_timerText.text = data + " ";
			}

			if (photonEvent.Code == EventName.StartRun)
			{
				Debug.Log("Receive event start run");
				_finishView.gameObject.SetActive(false);
				_panel.SetActive(false);
			}
		}
	}
}

