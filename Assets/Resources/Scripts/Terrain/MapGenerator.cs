using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class MapGenerator : MonoBehaviour
{
    public int hexChunkRadius;
    public Vector2 hexChunkOffset;

    public AnimationCurve heightCurve;

    public bool autoUpdate;

    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    public void GenerateMap()
    {
        //Mesh mesh = new Mesh();
        //Vector3 value1 = new Vector3( 0f, 1f, 0f );
        //Vector3 value2 = new Vector3( 1f, 1f, 0f);
        //Vector3 value3 = new Vector3( 1f, 0f, 0f);
        //mesh.vertices = new Vector3[] { value1, value2, value3 };
        //mesh.triangles = new int[] { 0, 1, 2 };
        //meshFilter.sharedMesh = mesh;

        MeshData meshData = MeshGenerator.GenerateTerrainMesh(TempMap(hexChunkRadius), hexChunkRadius, hexChunkOffset, heightCurve);
        meshFilter.sharedMesh = meshData.CreateMesh();
    }

    float[,] TempMap(int radius)
    {
        float[,] map = new float[radius * 2 + 1, radius * 2 + 1];
        return map;
    }

    private void OnValidate()
    {
        if (hexChunkRadius <= 0) hexChunkRadius = 1;
    }
}
