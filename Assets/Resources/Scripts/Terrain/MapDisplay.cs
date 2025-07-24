using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer texturerRenderer;

    /// <summary>
    /// Method <c>DrawNoiseMap</c> Takes in a noise map and applies the output to a <see cref="Renderer"/>.
    /// <br/><br/>
    /// <param name="noiseMap">float[,] <c>noiseMap</c> A noise map for generating the texture.</param>
    /// <br/><br/>
    /// <seealso cref="Noise.GenerateNoiseMap(int, int, float)"/>
    /// </summary>
    public void DrawNoiseMap(float[,] noiseMap)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);
        Texture2D texture = new Texture2D(width, height);

        Color[] colourMap = new Color[width * height];
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);

        texture.SetPixels(colourMap);
        texture.Apply();

        texturerRenderer.sharedMaterial.mainTexture = texture;
        texturerRenderer.transform.localScale = new Vector3(width, 1, height);
    }
}
