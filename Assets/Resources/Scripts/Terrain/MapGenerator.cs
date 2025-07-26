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

    public int mapWidth;
    public int mapHeight;
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
            display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, mapWidth, mapHeight));
        else if (drawMode == DrawMode.Mesh)
            display.DrawMesh(
                MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve),
                TextureGenerator.TextureFromColourMap(colourMap, mapWidth, mapHeight));
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
            mapWidth,
            mapHeight,
            noiseScale,
            seed,
            octaves,
            persistence,
            lacunarity,
            offset);
    }

    private Color[] GenerateColourMap(float[,] noiseMap)
    {
        Color[] colourMap = new Color[mapWidth * mapHeight];

        for (int y = 0; y < mapHeight; y++)
            for (int x = 0; x < mapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                    if (currentHeight <= regions[i].height)
                    {
                        colourMap[y * mapWidth + x] = regions[i].color;
                        break;
                    }
            }

        return colourMap;
    }

    private void OnValidate()
    {
        if (mapWidth < 1)
            mapWidth = 1;
        if (mapHeight < 1)
            mapHeight = 1;
        if (lacunarity < 1)
            lacunarity = 1;
        if (octaves < 1)
            octaves = 1;
    }
}
