using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class EnemySpawn : MonoBehaviour
{   

    public GameObject prefab; // The prefab you want to spawn
    public Transform parentTransform; //EnemySpawner transform
    private List<Vector3> spawnLocations = new List<Vector3>(); // List of spawn locations
    private List<GameObject> map1Prefabs = new List<GameObject>(); // List for map 1 prefabs
    private List<GameObject> map2Prefabs = new List<GameObject>(); // List for map 2 prefabs
    private List<GameObject> map3Prefabs = new List<GameObject>(); // List for map 3 prefabs

    PhotonView pv;

    // Start is called before the first frame update
    void Start()
    {   
        pv = GetComponent<PhotonView>();
        //MAP 1
        spawnLocations.Add(new Vector3(-10f,1f,-27f));
        spawnLocations.Add(new Vector3(4f, 1f, -154f));
        spawnLocations.Add(new Vector3(-24f, 1f, -110f));
        //spawnLocations.Add(new Vector3(13f, 1f, -85f));
        spawnLocations.Add(new Vector3(-5f, 1f, -62f));
        //spawnLocations.Add(new Vector3(30f, 1f, -42f));
        spawnLocations.Add(new Vector3(-19f, 1f, -34f));
        spawnLocations.Add(new Vector3(10f,1f,-112f));

        // MAP 2
        spawnLocations.Add(new Vector3(97f, 1f, -19f));
        spawnLocations.Add(new Vector3(81f, 1f, 79f));
        //spawnLocations.Add(new Vector3(135f, 1f, 77f));
        spawnLocations.Add(new Vector3(165f, 1f, 95f));
        spawnLocations.Add(new Vector3(146f, 1f, 79f));
        spawnLocations.Add(new Vector3(117f, 1f, -3f));
        spawnLocations.Add(new Vector3(138f, 1f, -44f));
        spawnLocations.Add(new Vector3(182f, 1f, 5f));
        spawnLocations.Add(new Vector3(228f, 1f, 5f));
        spawnLocations.Add(new Vector3(252f, 1f, 37f));
        spawnLocations.Add(new Vector3(297f, 1f, 69f));
        spawnLocations.Add(new Vector3(297f, 1f, 76f));

        // MAP 3
        spawnLocations.Add(new Vector3(395f,-8f,44f));
        spawnLocations.Add(new Vector3(363f,-8f,45f));
        spawnLocations.Add(new Vector3(389f,-8f,44f));
        spawnLocations.Add(new Vector3(391f,-8f,72f));

        Invoke("SpawnPrefabs", 3);
    }

    // Update is called once per frame
    void Update()
    {   

    }

    void SpawnPrefabs()
    {   
        //if (Application.platform == RuntimePlatform.WindowsPlayer)
        if (PhotonNetwork.LocalPlayer == PhotonNetwork.MasterClient)
        {
            foreach (Vector3 location in spawnLocations)
            {
                GameObject spawnedPrefab = PhotonNetwork.Instantiate(prefab.name, location, Quaternion.identity);
                //spawnedPrefab.transform.SetParent(parentTransform, false);
                AssignPrefabToMap(spawnedPrefab);
            }
        }

        else{
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            RequestOwnership(enemies);
        }
        // else if (Application.platform == RuntimePlatform.WindowsEditor) {
        //     pv.RPC("GiveMeMyEnemies", RpcTarget.Others);
        // }
    }

    // [PunRPC]
    // void GiveMeMyEnemies(PhotonMessageInfo info) 
    // {   
    //     pv.RPC("HereAreYourEnemies", RpcTarget.Others, map1Prefabs, map2Prefabs, map3Prefabs);
    // }

    // [PunRPC]
    // void HereAreYourEnemies(PhotonMessageInfo info, 
    //     List<GameObject> _map1Prefabs, 
    //     List<GameObject> _map2Prefabs, 
    //     List<GameObject> _map3Prefabs) 
    // {   
    //     map1Prefabs = _map1Prefabs;
    //     map2Prefabs = _map2Prefabs;
    //     map3Prefabs = _map3Prefabs;
    // }

    void AssignPrefabToMap(GameObject prefab)
    {
        float xPos = prefab.transform.position.x;

        if (xPos >= -25 && xPos <= 40)
        {
            map1Prefabs.Add(prefab);
        }
        else if (xPos >= 80 && xPos <= 300)
        {
            map2Prefabs.Add(prefab);
        }
        else if (xPos >= 350 && xPos <= 410)
        {
            map3Prefabs.Add(prefab);
        }
    }

    public List<GameObject> GetCurrentEnemies(int mapNumber)
    {
        switch (mapNumber)
        {
            case 1:
                return map1Prefabs;
            case 2:
                return map2Prefabs;
            case 3:
                return map3Prefabs;
            default:
                return map1Prefabs;
        }
    }
    
    public void RequestOwnership(GameObject[] enemies){
        Debug.Log("Lista de enemies: " + enemies[2]);

        foreach (GameObject enemy in enemies)
        {   
            PhotonView enemyPV = enemy.GetComponent<PhotonView>();
            enemyPV.TransferOwnership(PhotonNetwork.LocalPlayer);
        }
    }
}
