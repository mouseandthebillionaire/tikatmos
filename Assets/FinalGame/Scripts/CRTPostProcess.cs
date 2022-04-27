using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRTPostProcess : MonoBehaviour
{
    public Shader shader;
    public float bend = 2f;
    public float scanlineSize1 = 200;
    public float scanlineSpeed1 = -10;
    public float scanlineSize2 = 20;
    public float scanlineSpeed2 = -3;
    public float scanlineAmount = 0.05f;
    public float vignetteSize = 1.9f;
    public float vignetteSmoothness = 0.6f;
    public float vignetteEdgeRound = 8f;
    public float noiseSize = 75f;
    public float noiseAmount = 0.05f;

    // Chromatic aberration amounts
    public Vector2 redOffset = new Vector2(0, -0.01f);
    public Vector2 blueOffset = Vector2.zero;
    public Vector2 greenOffset = new Vector2(0, 0.01f);

    private Material material;

    // Use this for initialization
    void Start()
    {
        material = new Material(shader);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        material.SetFloat("u_time", Time.fixedTime);
        material.SetFloat("u_bend", bend);
        material.SetFloat("u_scanline_size_1", scanlineSize1);
        material.SetFloat("u_scanline_speed_1", scanlineSpeed1);
        material.SetFloat("u_scanline_size_2", scanlineSize2);
        material.SetFloat("u_scanline_speed_2", scanlineSpeed2);
        material.SetFloat("u_scanline_amount", scanlineAmount);
        material.SetFloat("u_vignette_size", vignetteSize);
        material.SetFloat("u_vignette_smoothness", vignetteSmoothness);
        material.SetFloat("u_vignette_edge_round", vignetteEdgeRound);
        material.SetFloat("u_noise_size", noiseSize);
        material.SetFloat("u_noise_amount", noiseAmount);
        material.SetVector("u_red_offset", redOffset);
        material.SetVector("u_blue_offset", blueOffset);
        material.SetVector("u_green_offset", greenOffset);
        Graphics.Blit(source, destination, material);
    }
}
