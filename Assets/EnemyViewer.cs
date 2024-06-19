using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyViewer : MonoBehaviour
{

    private float targetTime;
    private bool scanned;

    public GameObject circlePrefab;
    public GameObject map;

    private int currentMap;

    private List<GameObject> currentEnemies;
    private List<GameObject> circles;

    void Start() {
        circles = new List<GameObject>();
        currentEnemies = new List<GameObject>();
    }

    public void EnemyToMiniMap(List<GameObject> enemies, int map){
        circles = new List<GameObject>();
        currentEnemies = enemies;
        currentMap = map;
        foreach (GameObject enemy in currentEnemies) {   
            Vector3 mapPos = convertWorldPosToMapPos(enemy.transform.position, currentMap);
            drawCircle(mapPos);
        }
        scanned = true;
        targetTime = 10.0f;
    }

    void drawCircle(Vector3 position){
        GameObject circle = Instantiate(circlePrefab, map.transform);
        circles.Add(circle);
        circle.GetComponent<RectTransform>().anchoredPosition = position;
    }

    public void hideCircles(){
        foreach (GameObject circle in circles) {
            Destroy(circle);
        }
    }

    Vector3 convertWorldPosToMapPos(Vector3 position, int currentMap) {
        Vector3 newPos = new Vector3(0,0,0);
        if (currentMap == 1) {
            //newPos = new Vector3((position.z + 225) * 100 / 220 - 50, (position.x + 85) * 100 / 220 - 50, 0);
            newPos = new Vector3((position.x + 85) * 200 / 220 - 100, (position.z + 225) * 200 / 220 - 100, 0);
        }
        else if (currentMap == 2) {
            //newPos = new Vector3((position.z - 47) * 100 / 279 - 50, (position.x + 130) * 100 / 279 - 50, 0);
            newPos = new Vector3((position.x - 47) * 200 / 280 - 100, (position.z + 150) * 200 / 280 - 100, 0);
        }
        else if (currentMap == 3) {
            //newPos = new Vector3((position.z - 333) * 100 / 79 - 50, (position.x - 10) * 100 / 79 - 50, 0);
            newPos = new Vector3((position.x - 333) * 200 / 80 - 100, (position.z - 10) * 200 / 80 - 100, 0);
        }
        Debug.Log(newPos);
        return newPos;
    }

    void Update(){
        if (scanned) {
            targetTime -= Time.deltaTime;
            if (targetTime <= 0){ 
                hideCircles();
                setScanFalse();
            }
            else {
                for (int i = 0; i < currentEnemies.Count; i++) {   
                    Vector3 mapPos = convertWorldPosToMapPos(currentEnemies[i].transform.position, currentMap);
                    circles[i].GetComponent<RectTransform>().anchoredPosition = mapPos;
                }
            }

        }
    }

    public void setScanFalse() {
        scanned = false;
    }

}
