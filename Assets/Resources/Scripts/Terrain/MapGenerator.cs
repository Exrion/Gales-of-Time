using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}

public class MapGenerator : MonoBehaviour
{

    public enum DrawMode
    {
        NoiseMap,
        ColourMap,
        Mesh
    }

    public DrawMode drawMode;

    public const int mapChunkSize = 241;
    [Range(0,6)]
    public int levelOfDetail;
    public float noiseScale;

    public int octaves;
    [Range(0, 1)]
    public float persistence;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    public bool autoUpdate;

    public TerrainType[] regions;

    /// <summary>
    /// Method <c>GenerateMap</c> Generates a prodcedural map using Pearlin Noise.
    /// <br/>
    /// This requires inclusion of <see cref="MapDisplay"/> in the same game object.
    /// </summary>
    public void GenerateMap()
    {
        float[,] noiseMap = GenerateNoiseMap();
        Color[] colourMap = GenerateColourMap(noiseMap);

        MapDisplay display = FindAnyObjectByType<MapDisplay>();

        if (drawMode == DrawMode.NoiseMap)
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        else if (drawMode == DrawMode.ColourMap)
            display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
        else if (drawMode == DrawMode.Mesh)
            display.DrawMesh(
                MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail),
                TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
    }

    /// <summary>
    /// Method <c>RandomiseSeed</c> Generates a psuedo random seed.
    /// </summary>
    public void RandomiseSeed()
    {
        System.Random random = new System.Random((int)DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        seed = random.Next();
    }

    private float[,] GenerateNoiseMap()
    {
        return Noise.GenerateNoiseMap(
            mapChunkSize,
            mapChunkSize,
            noiseScale,
            seed,
            octaves,
            persistence,
            lacunarity,
            offset);
    }

    private Color[] GenerateColourMap(float[,] noiseMap)
    {
        Color[] colourMap = new Color[mapChunkSize * mapChunkSize];

        for (int y = 0; y < mapChunkSize; y++)
            for (int x = 0; x < mapChunkSize; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                    if (currentHeight <= regions[i].height)
                    {
                        colourMap[y * mapChunkSize + x] = regions[i].color;
                        break;
                    }
            }

        return colourMap;
    }

    private void OnValidate()
    {
        if (lacunarity < 1)
            lacunarity = 1;
        if (octaves < 1)
            octaves = 1;
    }
}
