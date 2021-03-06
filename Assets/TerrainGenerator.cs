﻿using System;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    void Start()
    {
        return;

        TerrainData data = Terrain.activeTerrain.terrainData;

        int width = data.heightmapResolution;
        int height = data.heightmapResolution;
        float[,] map1 = HeightMap(new float[width, height], 3.0F, 0.4F);

        float[,] map2 = HeightMap(map1, 15F, 0.05F);
        float[,] ridged1 = Ridged(map2, 0.2F);

        float[,] map3 = HeightMap(ridged1, 50F, 0.007F);
        float[,] map4 = HeightMap(map3, 100F, 0.003F);
        float[,] map5 = HeightMap(map4, 300F, 0.002F);

        data.SetHeights(0, 0, map5);
    }

    void Update()
    {}

    float[,] Ridged(float[,] baseMap, float inversionThreshold) 
    {
        int width = baseMap.GetLength(0);
        int height = baseMap.GetLength(1);
        for (int h = 0; h < height; h++) 
        {
            for (int w = 0; w < width; w++) 
            {
                float baseValue = baseMap[w, h];
                if (baseValue > inversionThreshold) 
                {
                    baseValue -= inversionThreshold;
                    baseValue *= -1.0F;
                    baseValue += inversionThreshold;
                    baseMap[w, h] = baseValue;
                }
            }
        }
        return baseMap;
    }
 
    float[,] HeightMap(float[,] baseMap, float planeScale, float heightScale) 
    {
        int width = baseMap.GetLength(0);
        int height = baseMap.GetLength(1);
        for (int h = 0; h < height; h++) 
        {
            for (int w = 0; w < width; w++) 
            {
                float x = (float)w / width * planeScale;
                float y = (float)h / height * planeScale;
                float randValue = Mathf.PerlinNoise(x, y) * heightScale;
                float baseValue = baseMap[w, h];
                baseMap[w, h] = baseValue + randValue;
            }
        }
        return baseMap;
    }

    float PerlinNoise3D(float x, float y, float z)
    {
        float xy = Mathf.PerlinNoise(x, y);
        float xz = Mathf.PerlinNoise(x, z);
        float yz = Mathf.PerlinNoise(y, z);
        float yx = Mathf.PerlinNoise(y, x);
        float zx = Mathf.PerlinNoise(z, x);
        float zy = Mathf.PerlinNoise(z, y);
 
        return (xy + xz + yz + yx + zx + zy) / 6;
    }
}
