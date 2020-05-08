using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MaterialEffect : MonoBehaviour {
    public Material material;
    void OnRenderImage (RenderTexture source, RenderTexture destination) {
        //material.SetFloat("texelWidth", 1.0f/source.width);
        //material.SetFloat("texelHeight", 1.0f/source.height);
        Graphics.Blit(source, destination, material);
    }
}
