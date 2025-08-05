using System;
using System.Xml.Schema;
using TMPro;
using UnityEngine;
using static UnityEngine.Mesh;

public class MeshData
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;

    int triangleIndex = 0;
    int vertexIndex = 0;

    public MeshData(int radius)
    {
        // @TODO: Optimize Later ;)
        vertices = new Vector3[(radius * 2 + 1) * (radius * 2 + 1)];
        //vertices = new Vector3[CalculateVerticeCount(radius) + GetHexGridCellCount(radius)];
        triangles = new int[6 * GetHexGridCellCount(radius) * 3];
        uvs = new Vector2[(radius * 2 + 1) * (radius * 2 + 1)];
    }

    private static int CalculateVerticeCount(int radius)
    {
        if (radius <= 0) return 0;

        return 6 * (1 + radius * (radius + 2));
    }

    public static int GetHexGridCellCount(int radius)
    {
        if (radius <= 0) return 0;

        int diamater = radius * 2 + 1;
        int total = diamater;
        for (int i = 0; i < radius; i++)
            total += --diamater * 2;
        return total;
    }

    public void AddHexagon(Vector2 centre, Vector2 offset)
    {
        Vector2[] cellVertices = HexCellData.DeriveCorners(centre);
        int[] vertexIndexMap = new int[cellVertices.Length];

        // Centre
        vertices[vertexIndex] = centre;
        vertexIndexMap[0] = vertexIndex;
        vertexIndex++;

        // Corners
        for (int i = 1; i < cellVertices.Length; i++) // Array out of index due to repeated corner vertices
        {
            vertices[vertexIndex] = cellVertices[i] + offset;
            vertexIndexMap[i] = vertexIndex;
            vertexIndex++;
        }

        for (int i = 1; i < 7; i++)
            AddTriangle(
                vertexIndexMap[i], 
                vertexIndexMap[i == 6 ? 1 : i + 1], 
                vertexIndexMap[0]);
    }

    void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;

        triangleIndex += 3;
    }

    private Vector3 SurfaceNormalFromIndices(int a, int b, int c)
    {
        Vector3 A = vertices[a];
        Vector3 B = vertices[b];
        Vector3 C = vertices[c];

        Vector3 AB = B - A;
        Vector3 AC = C - A;

        return Vector3.Cross(AB, AC).normalized;
    }

    private Vector3[] CalculateNormals()
    {
        Vector3[] vertexNormals = new Vector3[vertices.Length];
        int triangleCount = triangles.Length / 3;

        for (int i = 0; i < triangleCount; i++)
        {
            int normalTriangleIndex = i * 3;
            int A = triangles[normalTriangleIndex];
            int B = triangles[normalTriangleIndex + 1];
            int C = triangles[normalTriangleIndex + 2];

            Vector3 triangleNormal = SurfaceNormalFromIndices(A, B, C);
            vertexNormals[A] = triangleNormal;
            vertexNormals[B] = triangleNormal;
            vertexNormals[C] = triangleNormal;
        }

        for (int i = 0; i < vertexNormals.Length; i++)
            vertexNormals[i].Normalize();

        return vertexNormals;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.normals = CalculateNormals();
        return mesh;
    }
}

public static class MeshGenerator
{
    public static MeshData GenerateTerrainMesh(
        float[,] heightMap,
        int radius,
        Vector2 offset,
        AnimationCurve _heightcurve)
    {
        AnimationCurve heightCurve = new AnimationCurve(_heightcurve.keys);
        //int meshSimplificationIncrement = levelOfDetail == 0 ? 1 : levelOfDetail * 2;
        int borderedSize = heightMap.GetLength(0);

        MeshData meshData = new MeshData(radius);

        for (int y = 0; y < borderedSize; y++)
        {
            for (int x = 0; x < borderedSize; x++)
            {
                meshData.AddHexagon(new Vector2(x, y), offset);
            }
        }

        return meshData;
    }
}
