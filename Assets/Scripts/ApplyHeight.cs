using Unity.Mathematics;
using UnityEngine;

public class ApplyHeight : MonoBehaviour
{
    [SerializeField] Terrain ourTerrain;
    [SerializeField] Texture2D heightMapTex;
    [SerializeField] float heightScale = 1.0f;

    void Start()
    {
        ApplyHeights();   
    }
    void ApplyHeights()
    {
        var data = ourTerrain.terrainData;
        int texWidth = heightMapTex.width;
        int texHeight = heightMapTex.height;

        int resolution = Mathf.Min(texWidth, texHeight);

        if(data.heightmapResolution != resolution)
        {
            data.heightmapResolution = resolution;
        }

        float[,] heights = new float [resolution, resolution];

        for(int y = 0; y < resolution; y++)
        {
            for(int x = 0; x < resolution; x++)
            {
                float u = (float)x / (resolution - 1);
                float v = (float)y / (resolution - 1);

                Color c = heightMapTex.GetPixelBilinear(u, v);
                float gray = c.r;

                heights[y,x] = gray * heightScale;
            }
        }
        data.SetHeights(0, 0, heights);
    }
}
