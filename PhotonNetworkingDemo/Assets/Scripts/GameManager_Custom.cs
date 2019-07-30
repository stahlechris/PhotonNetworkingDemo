using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameManager_Custom : MonoBehaviourPunCallbacks
{
    public ThirdPersonCharacter_Custom PlayerPrefab;

    [HideInInspector]
    public ThirdPersonCharacter_Custom LocalPlayer;

    private void Awake()
    {
        if(!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("Menu");
            Debug.Log("photon network wasn't connected, loaded scene manually");
            return;
        }
    }
    private void Start()
    {
        ThirdPersonCharacter_Custom.ResfreshInstance(ref LocalPlayer, PlayerPrefab);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        
        ThirdPersonCharacter_Custom.ResfreshInstance(ref LocalPlayer, PlayerPrefab);
    }
}
