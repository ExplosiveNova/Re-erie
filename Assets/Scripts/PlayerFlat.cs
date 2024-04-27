using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlat : MonoBehaviour
{   
    [SerializeField]
    [Range(1f, 1000f)]
    float launchForce = 500f;
    Ball grabbedBall;
    Camera cam;
    bool hasBall;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            // Interact
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            

        if (Physics.Raycast(ray, out RaycastHit hitInfo)) {
            // Interact
            GameObject hitObj = hitInfo.collider.gameObject;
            if (hitObj.CompareTag("Ball")){
                GrabBall(hitObj);
                }

            }
        }
        else if (hasBall && Input.GetMouseButtonUp(0)){
            LaunchBall();
            }
    }

    void GrabBall(GameObject ballObj) {
    // Position the ball in front of me
        hasBall = true;
        Debug.Log("hasBall: " + hasBall);
        ballObj.transform.position = cam.transform.position + new Vector3(0, -1, 1);
        grabbedBall = ballObj.GetComponent<Ball>();
        grabbedBall.Freeze();
    }

    void LaunchBall() {
    // Throw the ball forward
        hasBall = false;
        Debug.Log("hasBall: " + hasBall);
        grabbedBall.Unfreeze();
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        grabbedBall.LaunchBall(ray.direction, launchForce);
        grabbedBall = null;
    }

}
