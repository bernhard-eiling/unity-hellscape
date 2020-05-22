using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    GameObject spinnyBoi;

    void Start()
    {
        spinnyBoi = GameObject.Find("SpinnyBoi");
        Vector3 localPosition = spinnyBoi.transform.localPosition;
        transform.localPosition = new Vector3(localPosition.x + 20F, localPosition.y + 20F, localPosition.z + 20F);
    }

    void Update()
    {   
        transform.LookAt(spinnyBoi.transform);
        Vector3 currentPositionBoi = spinnyBoi.transform.localPosition;
        transform.localPosition = new Vector3(transform.localPosition.x, currentPositionBoi.y + 20F, transform.localPosition.z);
    }
}
