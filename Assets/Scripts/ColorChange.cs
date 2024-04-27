using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    MeshRenderer mesh;
    Material mat;
    Color color;

    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        mat = mesh.material;
        color = mat.color;
    }

    // Update is called once per frame
    void Update()
    {
        color.g = (color.g + 0.001f) % 1f;
        mat.color = color;

    }
}
