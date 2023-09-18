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

    public static Vector3[] GeneratePointsInLine(Vector3 point1, Vector3 point2, int amount)
    {
        Vector3[] points = new Vector3[amount + 1];
        for (int i = 0; i <= amount; i++)
        {
            float t = i / (float)amount;
            points[i] = Vector3.Lerp(point1, point2, t);
        }
        return points;
    }
}