using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer texturerRenderer;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    /// <summary>
    /// Method <c>DrawTexture</c> Takes in a <see cref="Texture2D"/> and applies the output to a <see cref="Renderer"/>.
    /// <br/><br/>
    /// <param name="noiseMap">float[,] <c>noiseMap</c> A noise map for generating the texture.</param>
    /// <br/><br/>
    /// <seealso cref="Noise.GenerateNoiseMap(int, int, float)"/>
    /// </summary>
    public void DrawTexture(Texture2D texture)
    {
        texturerRenderer.sharedMaterial.mainTexture = texture;
        texturerRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public void DrawMesh(MeshData meshData, Texture2D texture)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();
        meshRenderer.sharedMaterial.mainTexture = texture;
    }
}
