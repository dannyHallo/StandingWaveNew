using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Coloring : MonoBehaviour
{
    public Gradient coloringGradient;

    public Material cubesColoringMat;
    Gen gen;
    Texture2D texture;
    const int textureResolution = 50;


    private void Awake(){
        gen = GetComponent<Gen>();
    }


    void Update()
    {
        UpdateTexture();
        cubesColoringMat.SetFloat("amplitude", gen.amplitude);
        cubesColoringMat.SetTexture("ramp", texture);
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
}
