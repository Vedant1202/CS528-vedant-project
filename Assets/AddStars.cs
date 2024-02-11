using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class AddStars : MonoBehaviour
{
    // Path to the directory containing assets
    //public string assetDirectoryPath = "Assets/MyAssets";

    // Path to CSV file
    //private static string starCSVPath = "/Editor/CSVs/processed_star_data_final.csv";
    private static string starCSVPath = "/Editor/CSVs/star_data_shortened.csv";

    // Define scale for cubes
    private const float scale = 0.1f;

    // Parameter names for x, y, and z coordinates in asset metadata
    //public string xParameterName = "x";
    //public string yParameterName = "y";
    //public string zParameterName = "z";

    void Start()
    {
        string[] allLines = File.ReadAllLines(Application.dataPath + starCSVPath);
        int i = 0;

        foreach (string s in allLines)
        {
            if (i == 0) // Skip first line containing headers
            {
                i++;

            }
            else
            {
                string[] splitData = s.Split(',');

                if (splitData.Length != 11)
                {
                    Debug.Log(s + "Incorrect data entered");
                    return;
                }

                 //Debug.Log(s);
                 //Debug.Log($"hip-{ splitData[0]}");

                //Star star = ScriptableObject.CreateInstance<Star>();

                //star.hip = int.Parse(splitData[0]);
                //star.dist = float.Parse(splitData[1]);
                //star.x_zero = float.Parse(splitData[2]);
                //star.y_zero = float.Parse(splitData[3]);
                //star.z_zero = float.Parse(splitData[4]);
                //star.mag = float.Parse(splitData[5]);
                //star.absmag = float.Parse(splitData[6]);
                //star.vx = float.Parse(splitData[7]);
                //star.vy = float.Parse(splitData[8]);
                //star.vz = float.Parse(splitData[9]);
                //star.spect = splitData[10];

                //AssetDatabase.CreateAsset(star, $"Assets/Stars/star-{star.hip}.asset");

                // Parse coordinates from metadata
                float x = float.Parse(splitData[2]);
                float y = float.Parse(splitData[3]);
                float z = float.Parse(splitData[4]);

                // Load or create the asset GameObject
                GameObject assetObject = Resources.Load<GameObject>($"star-{ float.Parse(splitData[0]) }");
                if (assetObject == null)
                {
                    assetObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                }

                // Set transform position
                assetObject.transform.position = new Vector3(x, y, z);
                assetObject.transform.localScale = Vector3.one * scale;

                // Optionally, set other properties based on metadata

                // Add the GameObject to the scene
                Instantiate(assetObject);
            }
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
}
