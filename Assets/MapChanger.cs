using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class MapChanger : MonoBehaviour
{   

    private int currentMap = 1; 

    public Sprite imageComponentArea1;
    public Sprite imageComponentArea2;
    public Sprite imageComponentArea3;

    [SerializeField] Image currentImageMap ;

    // Start is called before the first frame update
    void Start()
    {   

    }

    public void switchToMap(int map)
    {
        currentMap = map;
        switch(map)
        {
            case 1:
                currentImageMap.sprite = imageComponentArea1;
                break;
            case 2:
                currentImageMap.sprite = imageComponentArea2;
                break;
            case 3:
                currentImageMap.sprite = imageComponentArea3;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
