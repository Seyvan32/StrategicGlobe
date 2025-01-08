using UnityEngine;
using System.Collections.Generic;

public class BorderRenderer : MonoBehaviour
{
    public GameObject linePrefab; // Prefab with LineRenderer
    public TextAsset geoJsonFile;

    void Start()
    {
        GeoJsonParser parser = new GeoJsonParser { geoJsonFile = geoJsonFile };
        List<List<Vector3>> borders = parser.ParseGeoJson();

        foreach (List<Vector3> border in borders)
        {
            DrawBorder(border);
        }
    }

    void DrawBorder(List<Vector3> borderPoints)
    {
        GameObject lineObj = Instantiate(linePrefab, transform);
        LineRenderer lineRenderer = lineObj.GetComponent<LineRenderer>();
        lineRenderer.positionCount = borderPoints.Count;
        lineRenderer.SetPositions(borderPoints.ToArray());
    }
}