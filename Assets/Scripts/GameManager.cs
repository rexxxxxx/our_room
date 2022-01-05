using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);

        PhotonNetwork.SendRate = 30;
        PhotonNetwork.SerializationRate = 30;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
