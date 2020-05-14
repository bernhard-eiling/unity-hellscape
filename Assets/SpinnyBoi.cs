using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnyBoi : MonoBehaviour {
    List<Rod> rods = new List<Rod>();
    static float size = 4;
    Vector3 scale = new Vector3(size, size, size);
    int numberRods = 40;
    Rigidbody rigidbody;

    void Start() {
        transform.localScale = scale;
        
        SetupGameObject();
        SetupRods();
    }

    void SetupGameObject() {
        gameObject.AddComponent<Rigidbody>();
        rigidbody = gameObject.GetComponent<Rigidbody>();
        gameObject.transform.parent = transform;
        gameObject.transform.position = transform.position;
    }

    void SetupRods() {
        for (int i = 0; i < numberRods; i++) {
            Rod rod = new Rod(scale);
            rod.gameObject.transform.parent = gameObject.transform;
            rod.gameObject.transform.position = gameObject.transform.position;
            rods.Add(rod);
        }
    }

    void Update() {
        foreach (Rod rod in rods) {
            float speed = rigidbody.velocity.magnitude;
            rod.Update(speed);
        }
    }

    void FixedUpdate() {
        // float delay = 4F;
        // if (Time.realtimeSinceStartup < delay) {
        //     return;
        // }
        // Vector3 direction = new Vector3(1, 0, -1);
        // Vector3 velocity = direction * (Time.realtimeSinceStartup - delay) * 3F;
        // rigidbody.AddForce(velocity);
    }
}

public class Rod {
    public GameObject gameObject;
    Vector3 rotation;

    public Rod(Vector3 baseScale) {
        Vector3 shape = new Vector3(1F, 0.07F, 0.07F);
        gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gameObject.transform.localScale = Vector3.Scale(shape, baseScale);
        rotation = VectorUtil.Random();
    }

    public void Update(float parentSpeed) {
        // float baseSpeed = (float)Math.Pow(2d, parentSpeed / 1.05F);
        float speed = parentSpeed / 20;
        gameObject.transform.Rotate(rotation * speed);
    }
}

public class VectorUtil {
    public static Vector3 Random(float min = -1.0F, float max = 1.0F) {
        return new Vector3(UnityEngine.Random.Range(min, max), UnityEngine.Random.Range(min, max), UnityEngine.Random.Range(min, max));
    }
}
