using System;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    private TerrainData data;

    void Start()
    {
        data = GetComponent<Terrain>().terrainData;
        int width = data.heightmapResolution;
        int height = data.heightmapResolution;
        float[,] map1 = HeightMap(new float[width, height], 5F, 0.4F);
        float[,] ridged = Ridged(map1, 0.15F);
        // float[,] map2 = HeightMap(map1, 15F, 0.05F);
        // float[,] map3 = HeightMap(map2, 50F, 0.007F);
        // float[,] map4 = HeightMap(map3, 100F, 0.004F);
        data.SetHeights(0, 0, ridged);
    }

    void Update()
    {}

    float[,] Ridged(float[,] baseMap, float inversionThreshold) {
        int width = baseMap.GetLength(0);
        int height = baseMap.GetLength(1);
        for (int h = 0; h < height; h++) {
            for (int w = 0; w < width; w++) {
                float baseValue = baseMap[w, h];
                Debug.Log(baseValue);
                if (baseValue > inversionThreshold) {
                    baseMap[w, h] = 1.0F - baseValue;
                }
            }
        }
        return baseMap;
    }
 
    float[,] HeightMap(float[,] baseMap, float planeScale, float heightScale) {
        int width = baseMap.GetLength(0);
        int height = baseMap.GetLength(1);
        for (int h = 0; h < height; h++) {
            for (int w = 0; w < width; w++) {
                float x = (float)w / width * planeScale;
                float y = (float)h / height * planeScale;
                float randValue = Mathf.PerlinNoise(x, y) * heightScale;
                float baseValue = baseMap[w, h];
                baseMap[w, h] = baseValue + randValue;
            }
        }
        return baseMap;
    }
}
