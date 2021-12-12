using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

[ExecuteInEditMode]
public class Gen : MonoBehaviour
{
    public Material testMaterial;
    public Gradient coloringGradient;
    [Range(1, 15)] public int m = 2;
    [Range(1, 15)] public int n = 2;
    [Range(1, 20)] public float c = 1;
    [Range(1, 20)] public float amplitude = 1;
    public Vector2Int meshSize;
    bool settingsUpdated = false;
    Texture2D texture;
    const int textureResolution = 50;

    private void Awake()
    {
        CreateTerrain();
    }

    // Update is called once per frame
    void Update()
    {
        if (settingsUpdated)
        {
            CreateTerrain();
            settingsUpdated = false;
        }

        //if (Application.isPlaying)
        {
            Profiler.BeginSample("SampleCode");
            UpdateTexture();

            int Lx = meshSize.x - 1;
            int Ly = meshSize.y - 1;

            testMaterial.SetInt("m", m);
            testMaterial.SetInt("n", n);
            testMaterial.SetFloat("c", c);
            testMaterial.SetFloat("amplitude", amplitude);
            testMaterial.SetFloat("lxReciprocal", 1 / (float)Lx);
            testMaterial.SetFloat("lyReciprocal", 1 / (float)Ly);
            testMaterial.SetTexture("ramp", texture);
            Profiler.EndSample();
        }
    }


    private void OnValidate()
    {
        settingsUpdated = true;

    }

    void UpdateTexture()
    {
        if (texture == null || texture.width != textureResolution)
        {
            texture = new Texture2D(textureResolution, 1, TextureFormat.RGBA32, false);
        }

        if (coloringGradient != null)
        {
            Color[] colours = new Color[texture.width];
            for (int i = 0; i < textureResolution; i++)
            {
                Color gradientCol = coloringGradient.Evaluate(i / (textureResolution - 1f));
                colours[i] = gradientCol;
            }

            texture.SetPixels(colours);
            texture.Apply();
        }
    }

    private void CreateTerrain()
    {
        DestroyImmediate(GameObject.FindGameObjectWithTag("Mesh"));
        
        GameObject procedualMesh = new GameObject("procedualMesh");
        MeshFilter _meshFilter = procedualMesh.AddComponent<MeshFilter>();
        MeshRenderer _meshRenderer = procedualMesh.AddComponent<MeshRenderer>();
        procedualMesh.GetComponent<MeshRenderer>().material = testMaterial;
        procedualMesh.tag = "Mesh";

        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        Vector3[] face = new Vector3[] {
             new Vector3(0, 0, 0),
             new Vector3(1, 0, 0),
             new Vector3(1, 1, 0),
             new Vector3(0, 1, 0)
         };
        int vertexIndex = 0;

        for (uint x = 0; x < meshSize.x; x++)
        {
            for (uint y = 0; y < meshSize.y; y++)
            {
                Vector3[] currentFace = new Vector3[] {
                      new Vector3(0 + x, 0 + y, 0),
                      new Vector3(1 + x, 0 + y, 0),
                      new Vector3(1 + x, 1 + y, 0),
                      new Vector3(0 + x, 1 + y, 0)
                  };
  
                  vertices.Add(currentFace[0]);
                  vertices.Add(currentFace[1]);
                  vertices.Add(currentFace[2]);
                  vertices.Add(currentFace[3]);
  
                  triangles.Add(0 + vertexIndex);
                  triangles.Add(1 + vertexIndex);
                  triangles.Add(2 + vertexIndex);
                  triangles.Add(0 + vertexIndex);
                  triangles.Add(2 + vertexIndex);
                  triangles.Add(3 + vertexIndex);
 
                 vertexIndex += 4;
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        _meshFilter.mesh = mesh;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        procedualMesh.transform.Rotate(new Vector3(-90,0,0), Space.Self);
    }
}
