using System.Collections.Generic;
using UnityEngine;

public static class PointGenerator
{
    public static List<Vector3> GeneratePointsInCircle(Vector3 center, int numPoints, float radius)
    {
        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < numPoints; i++)
        {
            float angle = i * (360f / numPoints) * Mathf.Deg2Rad;
            float x = center.x + radius * Mathf.Cos(angle);
            float z = center.z + radius * Mathf.Sin(angle);

            Vector3 point = new Vector3(x, center.y, z);
            points.Add(point);
        }

        return points;
    }

    public static List<Vector3> GenerateSunflowerPoints(Vector3 position, int numberOfPoints, float radius)
    {
        List<Vector3> points = new List<Vector3>();

        float phi = (1 + Mathf.Sqrt(5)) / 2 - 1; // Golden ratio

        for (int i = 0; i < numberOfPoints; i++)
        {
            float angle = 2 * Mathf.PI * i * phi; // Fibonacci angle formula

            float distance = Mathf.Sqrt(i) / Mathf.Sqrt(numberOfPoints) * radius; // Fibonacci distance formula

            float x = position.x + distance * Mathf.Cos(angle);
            float z = position.z + distance * Mathf.Sin(angle);

            Vector3 point = new Vector3(x, position.y, z);
            points.Add(point);
        }

        return points;
    }

    public static List<Vector3> GenerateExperimentalPoints(Vector3 position, int numberOfPoints, float radius)
    {
        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < numberOfPoints; i++)
        {
            float angle = Random.Range(0f, 2f * Mathf.PI); // Random angle
            float randomRadius = Mathf.Sqrt(Random.Range(0f, 1f)) * radius; // Random radius within the circle

            float x = position.x + randomRadius * Mathf.Cos(angle);
            float z = position.z + randomRadius * Mathf.Sin(angle);

            Vector3 point = new Vector3(x, position.y, z);
            points.Add(point);
        }

        return points;
    }
}