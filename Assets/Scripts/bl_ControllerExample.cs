using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Photon.Pun;

public class bl_ControllerExample : MonoBehaviourPun, IPunObservable {

    /// <summary>
    /// Step #1
    /// We need a simple reference of joystick in the script
    /// that we need add it.
    /// </summary>
	[SerializeField]private bl_Joystick Joystick;//Joystick reference for assign in inspector

    [SerializeField]private float Speed = 5;
    [SerializeField] private float rotSpeed = 20;


    public GameObject myCamera;
    public Transform myCharacter;

    Vector3 setPos;
    Quaternion setRot;
    float dir_speed = 0;


    void Start()
    {
        myCamera.SetActive(photonView.IsMine);
    }

    void Update()
    {

        if (photonView.IsMine)
        {

            //Step #2
            //Change Input.GetAxis (or the input that you using) to Joystick.Vertical or Joystick.Horizontal
            float v = Joystick.Vertical; //get the vertical value of joystick
            float h = Joystick.Horizontal;//get the horizontal value of joystick

            //in case you using keys instead of axis (due keys are bool and not float) you can do this:
            //bool isKeyPressed = (Joystick.Horizontal > 0) ? true : false;

            //ready!, you not need more.
            /*Vector3 translate = (new Vector3(h, 0, v) * Time.deltaTime) * Speed;
            transform.Translate(translate);*/

            Vector3 dir = new Vector3(h, 0, v);
            dir.Normalize();
            dir = myCamera.transform.TransformDirection(dir);
            transform.position += (new Vector3(dir.x, 0, dir.z) * Time.deltaTime) * Speed;
            //transform.position += dir * Time.deltaTime * Speed;
            //Vector3 translate = (dir * Time.deltaTime) * Speed;
            //transform.Translate(translate);

            //myCamera.transform.eulerAngles += new Vector3(0,)
            dir = new Vector3(h, -90, v);
            myCharacter.rotation = Quaternion.LookRotation(dir);

            myCamera.transform.eulerAngles += new Vector3(0, h, 0) * rotSpeed * Time.deltaTime;
         }

         else
         {
             transform.position = setPos;
             myCharacter.rotation = setRot;


         }





    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(myCharacter.rotation);
            
        }
        else if (stream.IsReading)
        {
            setPos = (Vector3)stream.ReceiveNext();
            setRot = (Quaternion)stream.ReceiveNext();
            dir_speed = (float)stream.ReceiveNext();
        }

    }
}