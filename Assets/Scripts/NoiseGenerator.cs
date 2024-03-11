using UnityEngine;

public class NoiseGenerator
{
    public static Texture2D GenerateNoiseTexture(NoiseValues values)
    {
        Texture2D mapTexture = new Texture2D(values.resolution, values.resolution);
        float[,] noiseMap = new float[values.resolution, values.resolution];

        float halfWidth = values.resolution / 2f;
        float halfHeight = values.resolution / 2f;

        for (int y = 0; y < values.resolution; y++)
        {
            for (int x = 0; x < values.resolution; x++)
            {
                float sampleX = (x - halfWidth + values.offset.x) / values.noiseScale;
                float sampleY = (y - halfHeight + values.offset.y) / values.noiseScale;

                float noiseValue = 0;
                float amplitude = 1;
                float frequency = 1;

                for (int i = 0; i < values.octaves; i++)
                {
                    float perlinValue = Mathf.PerlinNoise(sampleX * frequency + values.seed, sampleY * frequency + values.seed) * 2 - 1;
                    noiseValue += perlinValue * amplitude;

                    //amplitude *= values.persistence;
                    frequency *= values.lacunarity;
                }

                noiseMap[x, y] = noiseValue;
            }
        }

        Color[] colorMap = new Color[values.resolution * values.resolution];
        for (int y = 0; y < values.resolution; y++)
        {
            for (int x = 0; x < values.resolution; x++)
            {
                colorMap[y * values.resolution + x] = Color.Lerp(Color.black, Color.white, Mathf.InverseLerp(-1f, 1f, noiseMap[x, y]));
            }
        }

        mapTexture.SetPixels(colorMap);
        mapTexture.Apply();

        return mapTexture;
    }


}
