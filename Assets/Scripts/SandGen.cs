using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandGen : MonoBehaviour
{
    public GameObject sand;
    public float genHeight = 10f;
    public float sandDensity;
    float spacing = 2;
    Gen gen;

    private void Awake()
    {
        gen = GetComponent<Gen>();
    }
    // Start is called before the first frame update
    void Start()
    {
        for (float y = 0; y < gen.numCubesPerAxis * spacing; y += spacing / sandDensity)
        {
            for (float x = 0; x < gen.numCubesPerAxis * spacing; x += spacing / sandDensity)
            {
                GameObject.Instantiate(sand, new Vector3(x, genHeight, y), Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
