using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine.SceneManagement;

namespace JSON_Networking
{
    public class NetworkConnectionManager : MonoBehaviourPunCallbacks
    {
        public Button Button_ConnectToMaster;
        public Button Button_ConnectToRoom;

        public bool TriesToConnectToMaster;
        public bool TriesToConnectToRoom;

        void Start()
        {
            DontDestroyOnLoad(gameObject);
            TriesToConnectToMaster = false;
            TriesToConnectToRoom = false;
        }

        void Update()
        {
            if (Button_ConnectToMaster != null)
            {
                Button_ConnectToMaster.gameObject.SetActive(!PhotonNetwork.IsConnected && !TriesToConnectToMaster);
            }

            if (Button_ConnectToRoom != null)
            {
                Button_ConnectToRoom.gameObject.SetActive(PhotonNetwork.IsConnected && !TriesToConnectToMaster && !TriesToConnectToRoom);
            }
        }

        public void OnClick_ConnectToMaster()
        {
            PhotonNetwork.OfflineMode = false; //true would "fake" an online connection
            PhotonNetwork.NickName = "PlayerName";//to set a player name
            //PhotonNetwork.AutomaticallySyncScene = true;//to call PhotonNetwork.LoadLevel()
            PhotonNetwork.GameVersion = "v1"; //only ppl w the same game version can play together

            TriesToConnectToMaster = true;

            //PhotonNetwork.ConnectToMaster(ip,port,appid); //manual connetion
            if (!PhotonNetwork.OfflineMode)
            {
                PhotonNetwork.ConnectUsingSettings(); //Auto connect based on the config file in Resources folder in photon folde
            }
        }
        public void OnClick_ConnectToRoom()
        {
            if(!PhotonNetwork.IsConnected)
            {
                Debug.Log("Photon Network isn't connected!");
                return;
            }
            TriesToConnectToRoom = true;
            //RoomInfo[] rooms = PhotonNetwork.GetCustomRoomList()
            RoomOptions roomOptions = new RoomOptions();
            PhotonNetwork.JoinOrCreateRoom("test 1", roomOptions, TypedLobby.Default);
            //PhotonNetwork.JoinRoom("test room 1");

            //PhotonNetwork.JoinRandomRoom(); //Join a random Room  - Error: OnJoinRandomRoomFailed
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            TriesToConnectToRoom = false;
            Debug.Log("Master:" + PhotonNetwork.IsMasterClient + " | Players in room: " 
                + PhotonNetwork.CountOfPlayers + PhotonNetwork.CurrentRoom.Name + " Region: " + PhotonNetwork.CloudRegion);
            SceneManager.LoadScene("Network"); //The name of your scene

        }
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            base.OnJoinRoomFailed(returnCode, message);
            Debug.Log("couldn't join room you tried to create " + message + " creating room with name of ManualCreation");
            //no room available
            PhotonNetwork.CreateRoom("ManualCreation", new RoomOptions { MaxPlayers = 20 });
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            base.OnJoinRandomFailed(returnCode, message);
            Debug.Log("no room available...creating room");
            //no room available
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 20 });
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            base.OnCreateRoomFailed(returnCode, message);
            Debug.Log("OnCreateRoomFailed" + message);
            TriesToConnectToRoom = false;
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);

            TriesToConnectToMaster = false;
            TriesToConnectToRoom = false;

            Debug.Log(cause);
        }

        public override void OnCreatedRoom()
        {
            Debug.Log("OnCreatedRoom() created a room");
            base.OnCreatedRoom();
        }
        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();

            TriesToConnectToMaster = false;

            Debug.Log("Connected to master!");
        }
    }
}
