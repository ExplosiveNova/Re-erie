using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;


public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public InputField createInput;
    public InputField joinInput;


    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createInput.text);
        Debug.Log("Room name : " + createInput.text);
    }

    public void JoinRoom()
    {   
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {   
        Debug.Log("Joined Room");
        PhotonNetwork.LoadLevel("Main"); 
    }


    void Start()
    {   
        if (Application.platform == RuntimePlatform.WindowsEditor)
        { 
        PhotonNetwork.CreateRoom("a");
        }


        else{
            PhotonNetwork.JoinRoom("a");
        }


        //PhotonNetwork.CreateRoom("a");
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
