using UnityEngine;
using UnityEditor;
using System.IO;

public class CSVtoSO
{
    private static string starCSVPath = "/Editor/CSVs/processed_star_near_data.csv";

    [MenuItem("Utilities/Generate Star")]

    public static void GenerateStar()
    {
        Debug.Log("I'm here");
        string[] allLines = File.ReadAllLines(Application.dataPath + starCSVPath);
        int i = 0;

        foreach (string s in allLines)
        {
            if (i == 0) // Skip first line containing headers
            {
                i++; 
                
            } else
            {
                string[] splitData = s.Split(',');

                if (splitData.Length != 11)
                {
                    Debug.Log(s + "Incorrect data entered");
                    return;
                }

                Debug.Log(s);
                Debug.Log(splitData[0]);

                Star star = ScriptableObject.CreateInstance<Star>();

                star.hip = int.Parse(splitData[0]);
                star.dist = float.Parse(splitData[1]);
                star.x_zero = float.Parse(splitData[2]);
                star.y_zero = float.Parse(splitData[3]);
                star.z_zero = float.Parse(splitData[4]);
                star.mag = float.Parse(splitData[5]);
                star.absmag = float.Parse(splitData[6]);
                star.vx = float.Parse(splitData[7]);
                star.vy = float.Parse(splitData[8]);
                star.vz = float.Parse(splitData[9]);
                star.spect = splitData[10];

                AssetDatabase.CreateAsset(star, $"Assets/Stars/star-{star.hip}.asset");
            }
        }

        AssetDatabase.SaveAssets();
    }
}
