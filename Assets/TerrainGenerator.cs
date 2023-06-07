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
        float[,] heights = new float[width, height];

        HeightMap(heights, 3F, 0.4F);
        HeightMap(heights, 15F, 0.05F);
        Ridged(heights, 0.20F);

        HeightMap(heights, 50F, 0.007F);
        HeightMap(heights, 100F, 0.003F);
        HeightMap(heights, 300F, 0.004F);

        data.SetHeights(0, 0, heights);
    }

    void Update() {}

    void Ridged(float[,] baseMap, float inversionThreshold) {
        int width = baseMap.GetLength(0);
        int height = baseMap.GetLength(1);
        for (int h = 0; h < height; h++) {
            for (int w = 0; w < width; w++) {
                float baseValue = baseMap[w, h];
                if (baseValue > inversionThreshold) {
                    baseValue -= inversionThreshold;
                    baseValue *= -1.0F;
                    baseValue += inversionThreshold;
                    baseMap[w, h] = baseValue;
                }
            }
        }
    }

    void HeightMap(float[,] baseMap, float planeScale, float heightScale) {
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
    }
}
