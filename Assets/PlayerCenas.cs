using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCenas : MonoBehaviour
{   

    public GameObject lantern;

    public void EnableLantern(bool enable) {
        if (enable) {
            lantern.SetActive(true);
        }
        else{
            lantern.SetActive(false);
        }
    }

}
