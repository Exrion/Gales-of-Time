using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int mapWidth;
    public int mapHeight;
    public float noiseScale;

    public int octaves;
    [Range(0, 1)]
    public float persistence;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public bool autoUpdate;

    /// <summary>
    /// Method <c>GenerateMap</c> Generates a prodcedural map using Pearlin Noise.
    /// <br/>
    /// This requires inclusion of <see cref="MapDisplay"/> in the same game object.
    /// </summary>
    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(
            mapWidth, 
            mapHeight, 
            noiseScale, 
            seed,
            octaves, 
            persistence, 
            lacunarity,
            offset);
        MapDisplay display = FindAnyObjectByType<MapDisplay>();
        display.DrawNoiseMap(noiseMap);
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
