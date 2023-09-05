using UnityEngine;

[System.Serializable]
public class Octave
{
    public float frequency = 1f;
    public float amplitude = 1f;
}

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Terrain : MonoBehaviour
{
    public int resolution = 50; // Number of vertices per side of the mesh
    public float scale = 1f; // Scale of the Perlin noise
    public float heightMultiplier = 10f; // Multiplier to adjust the height of the terrain

    public Octave[] octaves; // List of octaves for Perlin noise

    public Material terrainMaterial; // Reference to the material you want to apply

    private Mesh mesh;
    private Vector3[] vertices;

    private void Start()
    {
        GenerateFlatMesh();
        ApplyMaterial();
    }

    private void GenerateFlatMesh()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        vertices = new Vector3[resolution * resolution];

        for (int x = 0; x < resolution; x++)
        {
            for (int z = 0; z < resolution; z++)
            {
                int index = x * resolution + z;
                vertices[index] = new Vector3(x, 0f, z);
            }
        }

        mesh.vertices = vertices;

        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        int triangleIndex = 0;
        for (int x = 0; x < resolution - 1; x++)
        {
            for (int z = 0; z < resolution - 1; z++)
            {
                int topLeft = x * resolution + z;
                int topRight = topLeft + 1;
                int bottomLeft = (x + 1) * resolution + z;
                int bottomRight = bottomLeft + 1;

                triangles[triangleIndex] = topLeft;
                triangles[triangleIndex + 1] = topRight;
                triangles[triangleIndex + 2] = bottomLeft;

                triangles[triangleIndex + 3] = topRight;
                triangles[triangleIndex + 4] = bottomRight;
                triangles[triangleIndex + 5] = bottomLeft;

                triangleIndex += 6;
            }
        }

        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    private void ApplyMaterial()
    {
        if (terrainMaterial != null)
        {
            GetComponent<MeshRenderer>().material = terrainMaterial;
        }
    }

    private void Update()
    {
        ApplyPerlinNoise();
    }

    private void ApplyPerlinNoise()
    {
        if (octaves == null || octaves.Length == 0)
        {
            Debug.LogWarning("No octaves defined for Perlin noise!");
            return;
        }

        for (int x = 0; x < resolution; x++)
        {
            for (int z = 0; z < resolution; z++)
            {
                int index = x * resolution + z;
                float perlinValue = 0f;

                foreach (Octave octave in octaves)
                {
                    perlinValue += Mathf.PerlinNoise((x + transform.position.x) * scale * octave.frequency, (z + transform.position.z) * scale * octave.frequency) * octave.amplitude;
                }

                vertices[index].y = perlinValue * heightMultiplier;
            }
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }
}
