using System;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

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
    public int editorPreviewLOD;
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

    Queue<MapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
    Queue<MapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();

    /// <summary>
    /// Method <c>DrawMapInEditor</c> Draws the map based on the selected type (<seealso cref="DrawMode"/>).
    /// </summary>
    public void DrawMapInEditor()
    {
        MapData mapData = GenerateMapData(Vector2.zero);

        MapDisplay display = FindAnyObjectByType<MapDisplay>();

        if (drawMode == DrawMode.NoiseMap)
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.heightMap));
        else if (drawMode == DrawMode.ColourMap)
            display.DrawTexture(TextureGenerator.TextureFromColourMap(mapData.colourMap, mapChunkSize, mapChunkSize));
        else if (drawMode == DrawMode.Mesh)
            display.DrawMesh(
                MeshGenerator.GenerateTerrainMesh(mapData.heightMap, meshHeightMultiplier, meshHeightCurve, editorPreviewLOD),
                TextureGenerator.TextureFromColourMap(mapData.colourMap, mapChunkSize, mapChunkSize));
    }

    public void RequestMapData(Vector2 centre, Action<MapData> callback)
    {
        ThreadStart threadStart = delegate
        {
            MapDataThread(centre, callback);
        };

        new Thread(threadStart).Start();
    }

    void MapDataThread(Vector2 centre, Action<MapData> callback)
    {
        MapData mapData = GenerateMapData(centre);
        lock (mapDataThreadInfoQueue)
        {
            mapDataThreadInfoQueue.Enqueue(new MapThreadInfo<MapData>(callback, mapData));
        }
    }

    public void RequestMeshData(MapData mapData, int lod, Action<MeshData> callback)
    {
        ThreadStart threadStart = delegate
        {
            MeshDataThread(mapData, lod, callback);
        };

        new Thread(threadStart).Start();
    }

    void MeshDataThread(MapData mapData, int lod, Action<MeshData> callback)
    {
        MeshData meshData = MeshGenerator.GenerateTerrainMesh(
            mapData.heightMap,
            meshHeightMultiplier,
            meshHeightCurve,
            lod);
        lock (meshDataThreadInfoQueue)
        {
            meshDataThreadInfoQueue.Enqueue(new MapThreadInfo<MeshData>(callback, meshData));
        }
    }

    private void Update()
    {
        if (mapDataThreadInfoQueue.Count > 0)
            for (int i = 0; i < mapDataThreadInfoQueue.Count; i++)
            {
                MapThreadInfo<MapData> threadInfo = mapDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }

        if (meshDataThreadInfoQueue.Count > 0)
            for (int i = 0;i < meshDataThreadInfoQueue.Count; i++)
            {
                MapThreadInfo<MeshData> threadInfo = meshDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
    }

    /// <summary>
    /// Method <c>GenerateMap</c> Generates a prodcedural map using Perlin Noise.
    /// <br/>
    /// This requires inclusion of <see cref="MapDisplay"/> in the same game object.
    /// <br/>
    /// <returns>Return: <c><see cref="MapData"/></c></returns>
    /// </summary>
    private MapData GenerateMapData(Vector2 centre)
    {
        float[,] noiseMap = GenerateNoiseMap(centre);
        Color[] colourMap = GenerateColourMap(noiseMap);

        return new MapData(noiseMap, colourMap);
    }

    /// <summary>
    /// Method <c>RandomiseSeed</c> Generates a psuedo random seed.
    /// </summary>
    public void RandomiseSeed()
    {
        System.Random random = new System.Random((int)DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        seed = random.Next();
    }

    private float[,] GenerateNoiseMap(Vector2 centre)
    {
        return Noise.GenerateNoiseMap(
            mapChunkSize,
            mapChunkSize,
            noiseScale,
            seed,
            octaves,
            persistence,
            lacunarity,
            centre + offset);
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

    struct MapThreadInfo<T>
    {
        public readonly Action<T> callback;
        public readonly T parameter;

        public MapThreadInfo(Action<T> callback, T parameter)
        {
            this.callback = callback;
            this.parameter = parameter;
        }
    }
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}

public struct MapData
{
    public readonly float[,] heightMap;
    public readonly Color[] colourMap;

    public MapData(float[,] heightMap, Color[] colorMap)
    {
        this.heightMap = heightMap;
        this.colourMap = colorMap;
    }
}