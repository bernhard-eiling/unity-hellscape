using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    GameObject spinnyBoi;

    void Start()
    {
        spinnyBoi = GameObject.Find("SpinnyBoi");
    }

    void Update()
    {
        transform.LookAt(spinnyBoi.transform);
    }
}
