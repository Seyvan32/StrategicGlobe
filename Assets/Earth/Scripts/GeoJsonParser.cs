using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class GeoJsonParser : MonoBehaviour
{
    public TextAsset geoJsonFile; // Assign GeoJSON file in the Inspector

    public List<List<Vector3>> ParseGeoJson()
{
    List<List<Vector3>> borders = new List<List<Vector3>>();

    // Parse GeoJSON
    JObject geoJson = JObject.Parse(geoJsonFile.text);
    JArray features = (JArray)geoJson["features"];

    foreach (JObject feature in features)
    {
        JObject geometry = (JObject)feature["geometry"];
        string type = geometry["type"].ToString();
        JToken coordinates = geometry["coordinates"];

        Debug.Log($"Processing Geometry Type: {type}");

        if (type == "Polygon")
        {
            ParsePolygon(coordinates, borders);
        }
        else if (type == "MultiPolygon")
        {
            foreach (var polygon in coordinates)
            {
                ParsePolygon(polygon, borders);
            }
        }
        else
        {
            Debug.LogWarning($"Unsupported geometry type: {type}");
        }
    }

    return borders;
}

void ParsePolygon(JToken coordinates, List<List<Vector3>> borders)
{
    foreach (var ring in coordinates)
    {
        List<Vector3> border = new List<Vector3>();

        foreach (var point in ring)
        {
            if (point is JArray array && array.Count < 2) continue; // Ensure valid lon/lat pair

            float lon = (float)point[0];
            float lat = (float)point[1];
            border.Add(LatLonToUnityCoord(lat, lon));
        }

        borders.Add(border);
    }
}

    // Convert lat/lon to Unity 3D coordinates on a sphere
    private Vector3 LatLonToUnityCoord(float lat, float lon, float radius = 620f)
    {
        float radLat = Mathf.Deg2Rad * lat;
        float radLon = Mathf.Deg2Rad * lon;

        float x = radius * Mathf.Cos(radLat) * Mathf.Cos(radLon);
        float y = radius * Mathf.Cos(radLat) * Mathf.Sin(radLon);
        float z = radius * Mathf.Sin(radLat);

        return new Vector3(x, z, y); // Unity's y-axis is "up"
    }
}