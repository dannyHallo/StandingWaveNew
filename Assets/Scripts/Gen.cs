using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using UnityEngine.Profiling;

[ExecuteInEditMode]
public class Gen : MonoBehaviour
{
    public GameObject cube;
    public Material testMaterial;
    // public ComputeShader computeShader;
    public Gradient coloringGradient;

    [Range(10, 100)]
    public int numCubesPerAxis = 50;
    public float c = 1;
    public float amplitude = 1;

    [Range(1, 10)]
    public int m = 2;
    public Vector2Int meshSize;

    [Range(1, 10)]
    public int n = 2;
    int x, y;
    // public Material cubesColoringMat;
    bool settingsUpdated = false;
    List<GameObject> existingCubes = new List<GameObject>();
    float spacing = 2f;

    ComputeBuffer computeBuffer;

    Texture2D texture;
    const int textureResolution = 50;

    // struct float3
    // {
    //     public float x;
    //     public float y;
    //     public float z;
    // }

    // float3[] cubeData;

    struct myJob : IJob
    {
        public void Execute()
        {

        }
    }

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


            // t += Time.deltaTime;
            // cubeData = new float3[existingCubes.Count];
            // for (int i = 0; i < existingCubes.Count; i++)
            // {
            //     float posZ = 0;
            //     x = i % numCubesPerAxis;
            //     y = i / numCubesPerAxis;

            //     existingCubes[i].transform.position = new Vector3(
            //         x * spacing,
            //         amplitude * posZ,
            //         y * spacing
            //     );
            // }

            // int Lx = numCubesPerAxis - 1;
            // int Ly = numCubesPerAxis - 1;

            // computeBuffer = new ComputeBuffer(existingCubes.Count, sizeof(float) * 3);
            // computeBuffer.SetData(cubeData);
            // computeShader.SetBuffer(0, "cubeData", computeBuffer);
            // computeShader.SetInt("numCubesPerAxis", numCubesPerAxis);
            // computeShader.SetInt("m", m);
            // computeShader.SetInt("n", n);
            // computeShader.SetFloat("t", t);
            // computeShader.SetFloat("c", c);
            // computeShader.SetFloat("lxReciprocal", 1 / (float)Lx);
            // computeShader.SetFloat("lyReciprocal", 1 / (float)Ly);

            // computeShader.Dispatch(0,
            // Mathf.CeilToInt(numCubesPerAxis / 10f),
            // Mathf.CeilToInt(numCubesPerAxis / 10f),
            // 1);

            // computeBuffer.GetData(cubeData);
            // computeBuffer.Release();
            // // cubeData
            // for (int i = 0; i < existingCubes.Count; i++)
            // {
            //     x = i % numCubesPerAxis;
            //     y = i / numCubesPerAxis;
            //     existingCubes[i].transform.position = new Vector3(
            //         x * spacing,
            //         amplitude * cubeData[i].z,
            //         y * spacing
            //     );
            // }

            // for (int i = 0; i < existingCubes.Count; i++)
            // {
            //     float posZ = 0;
            //     x = i % numCubesPerAxis;
            //     y = i / numCubesPerAxis;

            //     existingCubes[i].transform.position = new Vector3(
            //         x * spacing,
            //         amplitude * posZ,
            //         y * spacing
            //     );
            // }
            UpdateTexture();

            int Lx = meshSize.x - 1;
            int Ly = meshSize.y - 1;

            // testMaterial.SetInt("numCubesPerAxis", numCubesPerAxis);
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

    void UpdateCubes()
    {
        List<GameObject> cubesToDestroy = new List<GameObject>();
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Cube"))
        {
            cubesToDestroy.Add(g);
        }

        foreach (GameObject g in cubesToDestroy)
            DestroyImmediate(g);

        existingCubes.Clear();

        for (int y = 0; y < numCubesPerAxis; y++)
        {
            for (int x = 0; x < numCubesPerAxis; x++)
            {
                GameObject thisCube = GameObject.Instantiate(cube, new Vector3(x * spacing, 0, y * spacing), Quaternion.identity);
                thisCube.GetComponent<MeshRenderer>().material = testMaterial;
                existingCubes.Add(thisCube);
            }
        }
        settingsUpdated = false;
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
