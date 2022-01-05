using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Photon.Pun;
using Photon.Realtime;


public class ConnManager_v2 : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.GameVersion = "0.1";
        int num = Random.Range(0, 1000);
        PhotonNetwork.NickName = "Player" + num;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Success Joined Lobby.");
        RoomOptions ro = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 8
        };

        PhotonNetwork.JoinOrCreateRoom("NetTest", ro, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room.");

        Vector2 originPos = Random.insideUnitCircle * 2.0f;



        PhotonNetwork.Instantiate("Player", new Vector3(originPos.x, 0, originPos.y), Quaternion.identity);
    }
}
