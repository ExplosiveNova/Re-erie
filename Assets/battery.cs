using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battery : MonoBehaviour
{   

    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player")){
            GetComponentInParent<BatteryHandler>().PickUpBattery();
            gameObject.SetActive(false);
            other.gameObject.GetComponent<PlayerAudioManager>().PlayBatterySound();
        }
    }
}
