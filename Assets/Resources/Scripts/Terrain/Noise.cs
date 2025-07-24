using UnityEngine;

public static class Noise
{
    /// <summary>
    /// Method <c>GenerateNoiseMap</c> creates a noise map given inputs.
    /// <br/><br/>
    /// <param name="mapWidth">int <c>mapWidth</c> Width of the Noise Map</param>
    /// <br/>
    /// <param name="mapHeight">int <c>mapHeight</c> Height of the Noise Map</param>
    /// <br/>
    /// <param name="scale">float <c>scale</c> Defines the scaling factor of the Noise Map</param>
    /// <br/><br/>
    /// <returns>Return: <c>float[<paramref name="mapWidth"/>, <paramref name="mapHeight"/>]</c></returns>
    /// </summary>
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];
        if (scale <= 0) scale = 0.0001f;
        for (int y = 0; y < mapHeight; y++) 
            for (int x = 0; x < mapWidth; x++)
            {
                float sampleX = x / scale;
                float sampleY = y / scale;
                float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
                noiseMap[x, y] = perlinValue;
            }
        return noiseMap;
    }
}
