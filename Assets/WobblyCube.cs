using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WobblyCube : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
    
        // Add a force to the Rigidbody.
        rb.AddForce(Vector3.up * 500f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
