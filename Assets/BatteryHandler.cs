using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class BatteryHandler : MonoBehaviour
{   
    public GameObject batteryPrefab;
    private PlayerCenas playerCenas;
    private float battery;
    private bool disabled;

    private PhotonView pv;
    private GameObject player2;
    private Text batteryText;

    private bool canStartLanterning;

    private List<GameObject> batteries;

    // Start is called before the first frame update
    void Start()
    {
        battery = 60f;
        pv = GetComponent<PhotonView>();

        if (PhotonNetwork.LocalPlayer != PhotonNetwork.MasterClient){
            Invoke("GetRefToP1", 3);
            pv.RPC("CanStartLanterning",RpcTarget.Others);
        }
        else{ 
            GetRefToBatteryText();
        }

        batteries = new List<GameObject>();

        foreach(Transform battery in transform){
            batteries.Add(battery.gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {   
        if (canStartLanterning && PhotonNetwork.LocalPlayer == PhotonNetwork.MasterClient)
        {
            battery = Mathf.Clamp(battery - Time.deltaTime, 0, 100);
            if (!disabled && battery <= 0)
            {
                pv.RPC("EnableLantern", RpcTarget.Others, false);
                disabled = true;
            }

            else if (disabled && battery > 0)
            {
                pv.RPC("EnableLantern", RpcTarget.Others, true);
                disabled = false;
            }

            batteryText.text = Mathf.Round(battery) + " %";
        }

    }

    public void GetRefToP1(){
        playerCenas = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCenas>();
    }

    public void GetRefToBatteryText(){
        batteryText = GameObject.FindGameObjectWithTag("BatteryText").GetComponent<Text>();
    }

    public void PickUpBattery(){
        pv.RPC("PickUpBattery",RpcTarget.Others);
    }

    public void Reset(){
        
        foreach(GameObject battery in batteries){
            battery.SetActive(true);
        }

        pv.RPC("ResetBattery",RpcTarget.Others);
    }

    [PunRPC]
    void ResetBattery(PhotonMessageInfo info){
        battery = 60f;
    }

    [PunRPC]
    void PickUpBattery(PhotonMessageInfo info) 
    {       
        Debug.Log("Picked Up Battery");
        battery = Mathf.Clamp(battery + 80f,0,100);
    }

    [PunRPC]
    void EnableLantern(bool enable, PhotonMessageInfo info) 
    {       
        playerCenas.EnableLantern(enable);
    }

    [PunRPC]
    void CanStartLanterning(PhotonMessageInfo info) 
    {       
        canStartLanterning = true;
    }

}

