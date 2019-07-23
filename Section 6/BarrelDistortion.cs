using UnityEngine;

public class BarrelDistortion : MonoBehaviour {

    [SerializeField]
    private Material distortionMaterial;

    [SerializeField]
    [Range(0.5f, 1.5f)]
    private float fov = 1f;


    void Start()
    {
        distortionMaterial.SetFloat("_Alpha", 0.5f);
        distortionMaterial.SetFloat("_FOV", fov);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, null, distortionMaterial);
    }

}
