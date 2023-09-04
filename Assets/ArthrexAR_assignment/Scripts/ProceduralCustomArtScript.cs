using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class ProceduralCustomArtScript : MonoBehaviour
{
    public Material arthrexMaterial;
    public Texture ImageTexture;

    private int gridSize=100;

    //Initialize vertex grid in range (-0.5, 0.5) for both x and y
    void InitVerticesInMesh(ref Mesh mesh, int size)
    {
        float sizeOfEachGridCell = 1.0f / size;

        Vector3[] vertices = new Vector3[size * size * 4];
        for (int row = 0; row < size; row++)
        {
            for (int column = 0; column < size; column++)
            {
                vertices[4 * (row * size + column) + 0].x = (column / (float)size) - 0.5f;
                vertices[4 * (row * size + column) + 0].y = 0.0f;
                vertices[4 * (row * size + column) + 0].z = (row / (float)size) - 0.5f;
                vertices[4 * (row * size + column) + 1].x = (column / (float)size) - 0.5f + sizeOfEachGridCell;
                vertices[4 * (row * size + column) + 1].y = 0.0f;
                vertices[4 * (row * size + column) + 1].z = (row / (float)size) - 0.5f;
                vertices[4 * (row * size + column) + 2].x = (column / (float)size) - 0.5f;
                vertices[4 * (row * size + column) + 2].y = 0.0f;
                vertices[4 * (row * size + column) + 2].z = (row / (float)size) - 0.5f + sizeOfEachGridCell;
                vertices[4 * (row * size + column) + 3].x = (column / (float)size) - 0.5f + sizeOfEachGridCell;
                vertices[4 * (row * size + column) + 3].y = 0.0f;
                vertices[4 * (row * size + column) + 3].z = (row / (float)size) - 0.5f + sizeOfEachGridCell;

            }
        }
        int[] indices = new int[size * size * 6];
        for (int row = 0; row < size; row++)
        {
            for (int column = 0; column < size; column++)
            {
                int firstVertIndex = 4 * (row * size + column);
                int secondVertIndex = 4 * (row * size + column) + 1;
                int thirdVertIndex = 4 * (row * size + column) + 2;
                int fourthVertIndex = 4 * (row * size + column) + 3;


                indices[6 * (row * (size) + column) + 0] = firstVertIndex;
                indices[6 * (row * (size) + column) + 1] = thirdVertIndex;
                indices[6 * (row * (size) + column) + 2] = secondVertIndex;
                indices[6 * (row * (size) + column) + 3] = thirdVertIndex;
                indices[6 * (row * (size) + column) + 4] = fourthVertIndex;
                indices[6 * (row * (size) + column) + 5] = secondVertIndex;

            }
        }

        //Center position of each Particle
        Vector4[] tangent = new Vector4[size * size * 4];
        for (int row = 0; row < size; row++)
        {
            for (int column = 0; column < size; column++)
            {
                tangent[4 * (row * size + column) + 0] = new Vector4((column / (float)size) - 0.5f + sizeOfEachGridCell / 2.0f, (row / (float)size) - 0.5f + sizeOfEachGridCell / 2.0f, -1.0f, -1.0f);
                tangent[4 * (row * size + column) + 1] = new Vector4((column / (float)size) - 0.5f + sizeOfEachGridCell / 2.0f, (row / (float)size) - 0.5f + sizeOfEachGridCell / 2.0f, 1.0f, -1.0f);
                tangent[4 * (row * size + column) + 2] = new Vector4((column / (float)size) - 0.5f + sizeOfEachGridCell / 2.0f, (row / (float)size) - 0.5f + sizeOfEachGridCell / 2.0f, -1.0f, 1.0f);
                tangent[4 * (row * size + column) + 3] = new Vector4((column / (float)size) - 0.5f + sizeOfEachGridCell / 2.0f, (row / (float)size) - 0.5f + sizeOfEachGridCell / 2.0f, 1.0f, 1.0f);
            }
        }

        Vector3[] normals = new Vector3[size * size * 4];
        for (int row = 0; row < size; row++)
        {
            for (int column = 0; column < size; column++)
            {
                normals[4 * (row * size + column) + 0] = Vector3.up;
                normals[4 * (row * size + column) + 1] = Vector3.up;
                normals[4 * (row * size + column) + 2] = Vector3.up;
                normals[4 * (row * size + column) + 3] = Vector3.up;
            }
        }

        Vector2[] uvs = new Vector2[size * size * 4];
        for (int row = 0; row < size; row++)
        {
            for (int column = 0; column < size; column++)
            {
                uvs[4 * (row * size + column) + 0] = new Vector2(0.0f, 0.0f);
                uvs[4 * (row * size + column) + 1] = new Vector2(1.0f, 0.0f);
                uvs[4 * (row * size + column) + 2] = new Vector2(0.0f, 1.0f);
                uvs[4 * (row * size + column) + 3] = new Vector2(1.0f, 1.0f);
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = indices;
        mesh.tangents = tangent;
        mesh.normals = normals;
        mesh.uv = uvs;
    }


    void Start()
    {
        //Initialize Plane Mesh to render
        Mesh proceduralPlaneMesh = new Mesh();
        InitVerticesInMesh(ref proceduralPlaneMesh, gridSize);

        //Assign Mesh specific components
        MeshRenderer meshRendererComponent = gameObject.GetComponent<MeshRenderer>();
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
        meshFilter.mesh = proceduralPlaneMesh;
        meshCollider.sharedMesh = proceduralPlaneMesh;
        meshRendererComponent.sharedMaterial = arthrexMaterial;

        //Set the image to sample color
        arthrexMaterial.SetTexture("_ImageTex", ImageTexture);
    }

    public void UpdateInputTexture(ref RenderTexture inputTexture)
    {
        arthrexMaterial.SetTexture("_MotionTex", inputTexture);
    }

}
