using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class SpinnyBoi : Agent 
{
    List<Rod> rods = new List<Rod>();
    static float size = 4;
    Vector3 scale = new Vector3(size, size, size);
    int numberRods = 40;
    Rigidbody rigidbody;
    public GameObject food;

    void Start() 
    {
        SetupGameObject();
        SetupRods();
        ResetSpinnyBoi();
        ResetFood();
    }

    void SetupGameObject() 
    {
        gameObject.AddComponent<Rigidbody>();
        gameObject.AddComponent<SphereCollider>();
        rigidbody = gameObject.GetComponent<Rigidbody>();
        gameObject.transform.parent = transform;
        gameObject.transform.position = transform.position;
    }

    void SetupRods()
    {
        for (int i = 0; i < numberRods; i++) 
        {
            Rod rod = new Rod(scale);
            rod.gameObject.transform.parent = gameObject.transform;
            rod.gameObject.transform.position = gameObject.transform.position;
            rods.Add(rod);
        }
    }

    void ResetSpinnyBoi() 
    {
        transform.localPosition = VectorUtil.RandomTerrainPosition();
        transform.localScale = scale;
        rigidbody.velocity = Vector3.zero;
    }

    void ResetFood() 
    {
        food = GameObject.Find("Food");
        food.transform.localPosition = VectorUtil.RandomTerrainPosition();
    }

    void Update() 
    {
        foreach (Rod rod in rods) 
        {
            float speed = rigidbody.velocity.magnitude;
            rod.Update(speed);
        }
    }

    void FixedUpdate() 
    {
        // float delay = 4F;
        // if (Time.realtimeSinceStartup < delay) {
        //     return;
        // }
        // Vector3 direction = new Vector3(1, 0, -1);
        // Vector3 velocity = direction * (Time.realtimeSinceStartup - delay) * 3F;
        // rigidbody.AddForce(velocity);
    }

    /////////
    // ACTOR
    /////////

    public override void OnEpisodeBegin()
    {  
        food.transform.localPosition = VectorUtil.RandomTerrainPosition();
    }
      

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(food.transform.localPosition);
        sensor.AddObservation(gameObject.transform.localPosition);
        sensor.AddObservation(rigidbody.velocity.x);
        sensor.AddObservation(rigidbody.velocity.z);
    }

    float currentEpisodeTime = 0F;
    float currentDistanceToTarget = 999999999F;

    public override void OnActionReceived(float[] vectorAction)
    {
        float velocityStrength = 30F;
        currentEpisodeTime += Time.deltaTime;
        Vector3 velocity = Vector3.zero;
        velocity.x = vectorAction[0] * velocityStrength;
        velocity.z = vectorAction[1] * velocityStrength;
        rigidbody.AddForce(velocity);

        float lastDistanceToTarget = currentDistanceToTarget;
        currentDistanceToTarget = Vector3.Distance(gameObject.transform.localPosition, food.transform.localPosition);

        bool isMovingCloser = currentDistanceToTarget < lastDistanceToTarget;
        Debug.Log(isMovingCloser);
        if (isMovingCloser) 
        {
            SetReward(0.01F);
        }
        if (!isMovingCloser) 
        {
            SetReward(-0.01F);
        }

        bool reachedTarget = currentDistanceToTarget < 3.5f;
        if (reachedTarget)
        {
            ResetFood();
            SetReward(1.0f);
            currentEpisodeTime = 0F;
            EndEpisode();
        }

        bool isTooFarAway = currentDistanceToTarget > 50f;
        bool isTimeOver = currentEpisodeTime > 30F;
        if (isTooFarAway || isTimeOver)
        {
            SetFailedEndEpisode();
        }
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");
    }

    private void SetFailedEndEpisode()
    {
        ResetFood();
        ResetSpinnyBoi();
        SetReward(-1.0f);
        currentEpisodeTime = 0F;
        EndEpisode();
    }
}

public class Rod 
{
    public GameObject gameObject;
    Vector3 rotation;

    public Rod(Vector3 baseScale) 
    {
        Vector3 shape = new Vector3(1F, 0.07F, 0.07F);
        gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        UnityEngine.Object.Destroy(gameObject.GetComponent<Collider>());
        gameObject.transform.localScale = Vector3.Scale(shape, baseScale);
        rotation = VectorUtil.Random();
    }

    public void Update(float parentSpeed) 
    {
        // float baseSpeed = (float)Math.Pow(2d, parentSpeed / 1.05F);
        float speed = parentSpeed / 20;
        gameObject.transform.Rotate(rotation * speed);
    }
}

public class VectorUtil 
{
    public static Vector3 Random(float min = -1.0F, float max = 1.0F) 
    {
        return new Vector3(UnityEngine.Random.Range(min, max), UnityEngine.Random.Range(min, max), UnityEngine.Random.Range(min, max));
    }

    public static Vector3 RandomTerrainPosition(float xBase = 980F, float zBase = 1180F)
    {
        float spread = 20F;
        float x = UnityEngine.Random.Range(xBase - spread, xBase + spread);
        float z = UnityEngine.Random.Range(zBase - spread, zBase + spread);
        float y = Terrain.activeTerrain.SampleHeight(new Vector3(x, 0, z));
        return new Vector3(x, y + 3F, z);
    }
}