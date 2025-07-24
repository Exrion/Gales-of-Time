using UnityEngine;

public static class Noise
{
    /// <summary>
    /// Method <c>GenerateNoiseMap</c> creates a noise map given inputs.
    /// <br/><br/>
    /// <param name="mapWidth">int <c>mapWidth</c> Width of the Noise Map.</param>
    /// <br/>
    /// <param name="mapHeight">int <c>mapHeight</c> Height of the Noise Map.</param>
    /// <br/>
    /// <param name="scale">float <c>scale</c> Defines the scaling factor of the Noise Map.</param>
    /// <br/>
    /// <param name="seed">int <c>seed</c> An integer to aid random generation through psuedo number generation.</param>
    /// <br/>
    /// <param name="octaves">int <c>seed</c> Count of layers of noise to combine.</param>
    /// <br/>
    /// <param name="persistence">float <c>persistence</c>Controls decrease in amplitude of octaves.</param>
    /// <br/>
    /// <param name="lacunarity">float <c>lacunarity</c> Controls increase in frequency of octaves.</param>
    /// <br/><br/>
    /// <returns>Return: <c>float[<paramref name="mapWidth"/>, <paramref name="mapHeight"/>]</c>.</returns>
    /// </summary>
    public static float[,] GenerateNoiseMap(
        int mapWidth,
        int mapHeight,
        float scale,
        int seed,
        int octaves,
        float persistence,
        float lacunarity,
        Vector2 offset)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        // Seeding Map
        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0) scale = 0.0001f;

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        // Scale Centering
        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        // Generate Noise Map
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    // Position (adjusted to centre) / Scale * Frequency * Seed Offset
                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;

                    noiseHeight += perlinValue * amplitude;

                    // Decrease amplitude per octave
                    amplitude *= persistence;

                    // Increases frequency per octave
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                    maxNoiseHeight = noiseHeight;
                else if (noiseHeight < minNoiseHeight)
                    minNoiseHeight = noiseHeight;

                noiseMap[x, y] = noiseHeight;
            }
        }

        // Normalise Noise Map
        for (int y = 0; y < mapHeight; y++)
            for (int x = 0; x < mapWidth; x++)
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);

        return noiseMap;
    }
}
