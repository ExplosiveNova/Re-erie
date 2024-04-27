using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{   

    // Start is called before the first frame update
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LaunchBall(Vector3 direction, float force){
        rb.AddForce(direction.normalized * force);
    }

    public void Freeze(){
        rb.isKinematic = true;
    }
    public void Unfreeze(){
        rb.isKinematic = false;
    }
}
