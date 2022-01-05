using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

using agora_gaming_rtc;

#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

public class ConnManager : MonoBehaviourPunCallbacks{

    public string AppID;
    public string ChannelName;
    VideoSurface myView;
    VideoSurface remoteView;
    IRtcEngine mRtcEngine;

    GameObject dialog = null;
    public List<string> permissionList = new List<string>();

    // Start is called before the first frame update
    void Start()
    {

        permissionList.Add(Permission.Camera);
        CheckPermission();

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

    private void CheckPermission()
    {
    #if (UNITY_2018_3_OR_NEWER)
            foreach (string permission in permissionList)
            {
                if (Permission.HasUserAuthorizedPermission(permission))
                {
                }
                else
                {
                    Permission.RequestUserPermission(permission);
                }
            }
    #endif
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

        /*if (photonView.IsMine)
        {

            SetupUI();
        }*/
        SetupUI();
        SetupAgora();
    }


    void SetupUI()
    {

        GameObject go = GameObject.Find("Plane");
        //go.transform.rotation = Quaternion.Euler(Vector3.right * -180);
        myView = go.AddComponent<VideoSurface>();
    }

    void SetupAgora()
    {
        mRtcEngine = IRtcEngine.GetEngine(AppID);


        Debug.Log("WOOGIFACT. Setup Agora");
        Join();
    }

    void Join()
    {
        Debug.Log("WOOGIFACT. calling join (channel = " + ChannelName + ")");

        mRtcEngine.JoinChannel(ChannelName, "", 0);
        myView.SetEnable(true);

        mRtcEngine.OnJoinChannelSuccess += OnJoinChannelSuccessHandler;
        mRtcEngine.OnUserJoined += OnUserJoined;
        mRtcEngine.OnUserOffline += OnUserOffline;
        mRtcEngine.OnLeaveChannel += OnLeaveChannelHandler;

        mRtcEngine.EnableVideo();
        mRtcEngine.EnableVideoObserver();



        //Debug.Log(test);
    }


    void Leave()
    {

        mRtcEngine.LeaveChannel();
        mRtcEngine.DisableVideo();
        mRtcEngine.DisableVideoObserver();
    }

    void OnJoinChannelSuccessHandler(string channelName, uint uid, int elapsed)
    {

        // join successfully
        Debug.Log("WOOGIFACT. OK??");
        Debug.LogFormat("WOOGIFACT. Joined channel {0} successfully, my uid={1}", channelName, uid);


    }

    void OnLeaveChannelHandler(RtcStats stats)
    {
        myView.SetEnable(false);
        if (remoteView != null)
        {
            remoteView.SetEnable(false);
        }
    }

    void OnUserJoined(uint uid, int elapsed)
    {
        if (remoteView == null)
        {
            remoteView = GameObject.Find("Plane").AddComponent<VideoSurface>();
        }
        remoteView.SetEnable(true);
        remoteView.SetForUser(uid);
        //remoteView.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);
        remoteView.SetVideoSurfaceType(AgoraVideoSurfaceType.Renderer);
        remoteView.SetGameFps(30);
    }

    void OnUserOffline(uint uid, USER_OFFLINE_REASON reason)
    {

        remoteView.SetEnable(false);
    }

    void OnApplicationQuit()
    {
        if (mRtcEngine != null)
        {
            IRtcEngine.Destroy();
            mRtcEngine = null;
        }
    }
}
