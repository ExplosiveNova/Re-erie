using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{   
    public GameObject playerPrefab1;
    public GameObject playerPrefab2;
    
    private Vector3 player2Position;
    private Vector3 player1Position;

    private int currentMap = 1;
    private PhotonView photonView;
    private GameObject player1;
    private GameObject player2;
    private MapChanger mapChanger;
    private EnemyViewer enemyViewer;
    public bool player1Join = false;

    [SerializeField] EnemySpawn enemySpawner;

    // Start is called before the first frame update
    void Start()
    {   
        Debug.developerConsoleVisible = true;
        photonView = GetComponent<PhotonView>();
        //player1Position = new Vector3(-332,-126,-86);
        player1Position = new Vector3(96.0999985f,5.49781704f,-174.199997f);


        //if (Application.platform == RuntimePlatform.WindowsEditor)
        //{
        //  P1stuff();
        //}

        //else if (Application.platform == RuntimePlatform.WindowsPlayer)
        //{
        if (PhotonNetwork.LocalPlayer == PhotonNetwork.MasterClient)
        {
            P2stuff();
        }

        else{
            P1stuff();
        }
        //}

    }

    void Update()
    {
        //if (Application.platform == RuntimePlatform.WindowsEditor)
        //{
        if (PhotonNetwork.LocalPlayer != PhotonNetwork.MasterClient){
            checkPosition();
        }

        //else if (Application.platform == RuntimePlatform.WindowsPlayer)
        else
        {
            if (Input.GetKeyDown("s"))
            {
                Scan();
            }
        }
    }

    public void P1stuff()
    {   
        Debug.Log("Logged as p1");
        player1 = Instantiate(playerPrefab1, player1Position, Quaternion.identity);
        player1Join = true;
    }

    public void Respawn(){
        if (currentMap == 1) player1.transform.position = new Vector3(96.0999985f,5.49781704f,-174.199997f);
        if (currentMap == 2) player1.transform.position = new Vector3(66.4000015f,5.49781704f,-37.5999985f);
        if (currentMap == 3) player1.transform.position = new Vector3(315.100006f,0.5f,34.2000008f);
    }

    public void P2stuff()
    {   
        Debug.Log("Logged as p2");
        player2 = Instantiate(playerPrefab2, player2Position, Quaternion.identity);
        mapChanger = player2.GetComponent<MapChanger>();
        enemyViewer = player2.GetComponent<EnemyViewer>();
    }

    void checkPosition()
    {   
        Vector3 playerPosition = player1.transform.position;
         
        if (currentMap == 1 && playerPosition.x > 45f && playerPosition.z > -130f)
            {   
                photonView.RPC("ChangeMap", RpcTarget.Others , 2);
                currentMap = 2;
            }
        else if (currentMap == 2 && playerPosition.x <= 45f)
            {
                photonView.RPC("ChangeMap", RpcTarget.Others, 1);
                currentMap = 1;
            }
        else if (currentMap == 2 && playerPosition.x >= 330f)
            {
                photonView.RPC("ChangeMap", RpcTarget.Others, 3);
                currentMap = 3;

            }
        else if (currentMap == 3 && playerPosition.x < 330f)
            {
                photonView.RPC("ChangeMap", RpcTarget.Others, 2);
                currentMap = 2;
            }
        
    }



    [PunRPC]
    void ChangeMap(int map,PhotonMessageInfo info) 
    {   
        Debug.Log("Changed to map : " + map);
        mapChanger.switchToMap(map);
        enemyViewer.hideCircles();
        enemyViewer.setScanFalse();
        currentMap = map;
    }

    void Scan(){
        photonView.RPC("Scan", RpcTarget.Others);

        enemyViewer.EnemyToMiniMap(enemySpawner.GetCurrentEnemies(currentMap), currentMap);
        Debug.Log("Getting enemies from map " + currentMap);
    }

    [PunRPC]
    void Scan(PhotonMessageInfo info) 
    {       
        Debug.Log("Scanning : ");
        player1.GetComponent<SonarSoundSpawn>().SpawnSonarSound();
    
    }

}
