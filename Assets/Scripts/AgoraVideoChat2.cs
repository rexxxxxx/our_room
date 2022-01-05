using UnityEngine;
using agora_gaming_rtc;
using UnityEngine.UI;
using System.Collections.Generic;

using Photon.Pun;
using UnityEngine.Android;

#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif


public class AgoraVideoChat2: MonoBehaviourPun
{

    public string AppID;
    public string ChannelName;
    VideoSurface myView;
    VideoSurface remoteView;
    IRtcEngine mRtcEngine;

    GameObject dialog = null;
    public List<string> permissionList = new List<string>();

    void Awake()
    {
       

    }

    void Start()
    {
        //permissionList.Add(Permission.Microphone);
        permissionList.Add(Permission.Camera);
        CheckPermission();
        SetupUI();
        SetupAgora();

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




    void SetupUI()
    {

        GameObject go = GameObject.Find("Plane");
        //go.transform.rotation = Quaternion.Euler(Vector3.right * -180);
        myView = go.AddComponent<VideoSurface>();
       

        /*        go = GameObject.Find("JoinButton");
                go.GetComponent<Button>().onClick.AddListener(Join);


                go = GameObject.Find("LeaveButton");
                go?.GetComponent<Button>()?.onClick.AddListener(Leave);*/
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
            remoteView = GameObject.Find("RemoteView").AddComponent<VideoSurface>();
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
