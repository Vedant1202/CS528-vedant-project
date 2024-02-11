using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AddConstellation : MonoBehaviour
{
    // Path to the directory containing assets
    //public string assetDirectoryPath = "Assets/MyAssets";

    // Path to CSV file
    private static string constellationFilePath = "/Editor/CSVs/constellation1.csv";

    // Define scale for cubes
    private const float scale = 0.01f;
    public Color startColor = Color.green;
    public Color endColor = Color.red;
    //public Color c1 = new Color(1, 0, 0, 1);
    //public Color c2 = new Color(1, 0, 0, 1);

    // Parameter names for x, y, and z coordinates in asset metadata
    //public string xParameterName = "x";
    //public string yParameterName = "y";
    //public string zParameterName = "z";

    void Start()
    {
        string[] allLines = File.ReadAllLines(Application.dataPath + constellationFilePath);
        for (int i = 1; i < allLines.Length - 1; i = i + 2)
        {
            Debug.Log($"x-{allLines[i]}");
            string s = allLines[i];
            string s1 = allLines[i + 1];
            string[] splitData = s.Split(',');
            string[] splitData1 = s1.Split(',');

            GameObject lineObject = new GameObject("Line");
            LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();

            lineRenderer.startWidth = scale;
            lineRenderer.endWidth = scale;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, new Vector3(float.Parse(splitData[0]), float.Parse(splitData[1]), float.Parse(splitData[2])));
            lineRenderer.SetPosition(1, new Vector3(float.Parse(splitData1[0]), float.Parse(splitData1[1]), float.Parse(splitData1[2])));

            //lineRenderer.width = 0.1f;
            //lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
            //lineRenderer.startColor = Color.red;
            //lineRenderer.endColor = Color.blue;
            //lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
            //lineRenderer.SetColors(Color.red, Color.red);
            //lineRenderer.sharedMaterial.SetColor("_Color", Color.blue);
            // A simple 2 color gradient with a fixed alpha of 1.0f.
            float alpha = 1.0f;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(startColor, 0.0f), new GradientColorKey(endColor, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
            lineRenderer.colorGradient = gradient;
        }
    }

    //// Replace this with your actual logic for loading asset metadata from file
    //private Dictionary<string, string> LoadAssetMetadata(string filePath)
    //{
    //    // Parse metadata from file format (e.g., JSON, CSV)
    //    // Example: read key-value pairs from a text file
    //    Dictionary<string, string> metadata = new Dictionary<string, string>();
    //    foreach (string line in File.ReadAllLines(filePath))
    //    {
    //        string[] parts = line.Split('=');
    //        if (parts.Length == 2)
    //        {
    //            metadata[parts[0].Trim()] = parts[1].Trim();
    //        }
    //    }
    //    return metadata;
    //}

    // Update is called once per frame
    void Update()
    {
        
    }
}

