using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ShaderEffect : MonoBehaviour {
    public int xRadius;
    public int yRadius;

    public Shader shader;
    [SerializeField]
    private Material material;

    // Postprocess the image
    void OnRenderImage (RenderTexture source, RenderTexture destination) {
        if (shader == null) {
            Graphics.Blit(source, destination);
            return;
        }
        if (material == null || material.shader != shader) {
            material = new Material(shader);
        }
        material.SetFloat("texelWidth", 1.0f/source.width);
        material.SetFloat("texelHeight", 1.0f/source.height);
        material.SetInt("xRadius", xRadius);
        material.SetInt("yRadius", yRadius);
        Graphics.Blit(source, destination, material);
    }
}
