using UnityEngine;

public static class TextureGenerator
{
    /// <summary>
    /// Method <c>TextureFromColourMap</c> Generates a texture given a colour map.
    /// <br/>
    /// <param name="colourMap">Color[] <c>colourMap</c> The colour map to create a texture from.</param>
    /// <br/>
    /// <param name="width">int <c>width</c> Width of the colour map.</param>
    /// <br/>
    /// <param name="height">int <c>height</c> Height of the colour map.</param>
    /// <br/>
    /// <returns>Return: <c><see cref="Texture2D"/></c></returns>
    /// </summary>
    public static Texture2D TextureFromColourMap(Color[] colourMap, int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);

        // Prevent Bi-linear
        texture.filterMode = FilterMode.Point;
        // Prevent Repeat
        texture.wrapMode = TextureWrapMode.Clamp;

        texture.SetPixels(colourMap);
        texture.Apply();
        return texture;
    }

    /// <summary>
    /// Method <c>TextureFromHeightMap</c> Generates a texture given a height map.
    /// <br/>
    /// <param name="heightMap"> float[,] <c>heightMap</c> The height map to create a texture from.</param>
    /// <br/>
    /// <returns>Return: <c><see cref="Texture2D"/></c></returns>
    /// </summary>
    public static Texture2D TextureFromHeightMap(float[,] heightMap)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        Texture2D texture = new Texture2D(width, height);

        Color[] colourMap = new Color[width * height];
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);

        return TextureFromColourMap(colourMap, width, height);
    }
}
