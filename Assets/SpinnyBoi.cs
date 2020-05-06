using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnyBoi : MonoBehaviour {
    List<Rod> rods = new List<Rod>();
    int numberRods = 40;

    void Start() {
        for (int i = 0; i < numberRods; i++) {
            Rod rod = new Rod();
            rod.gameObject.transform.parent = transform;
            rod.gameObject.transform.position = transform.position;
            rods.Add(rod);
        }
    }

    void Update() {
        foreach (Rod rod in rods) {
            rod.Update();
        }
    }
}

public class Rod {
    public GameObject gameObject;
    Vector3 rotation;

    public Rod() {
        Vector3 shape = new Vector3(1F, 0.07F, 0.07F);
        gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gameObject.transform.localScale = shape;
        rotation = VectorUtil.Random();
    }

    public void Update() {
        float baseSpeed = 350;
        float range = 100;
        float speed = UnityEngine.Random.Range(baseSpeed, baseSpeed + range);
        gameObject.transform.Rotate(rotation * speed * Time.deltaTime);
    }
}

public class VectorUtil {
    public static Vector3 Random(float min = -1.0F, float max = 1.0F) {
        return new Vector3(UnityEngine.Random.Range(min, max), UnityEngine.Random.Range(min, max), UnityEngine.Random.Range(min, max));
    }
}
