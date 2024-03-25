using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseValues
{
    //put in a struct
    public int resolution;
    public float noiseScale;

    public int octaves;
    [Range(0f, 1f)] public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public NoiseValues Clone()
    {
        NoiseValues clone = new NoiseValues();

        clone.resolution = this.resolution;
        clone.noiseScale = this.noiseScale;
        clone.octaves = this.octaves;
        clone.persistance = this.persistance;
        clone.lacunarity = this.lacunarity;
        clone.seed = this.seed;
        clone.offset = this.offset;

        return clone;
    }
}