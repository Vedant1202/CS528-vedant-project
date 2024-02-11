//using System.Collections.Generic;
//using System.IO;
//using UnityEngine;

//public class LoadStars : MonoBehaviour
//{
//    // Path to the directory containing assets
//    public string assetDirectoryPath = "Assets/Stars";

//    // Parameter names for x, y, and z coordinates in asset metadata
//    public string xParameterName = "X_zero";
//    public string yParameterName = "Y_zero";
//    public string zParameterName = "Z_zero";

//    void Start()
//    {
//        // Load all files in the directory
//        // Get all files in the directory
//        string[] filePaths = Directory.GetFiles(assetDirectoryPath, "*.asset");

//        foreach (string filePath in filePaths)
//        {
//            // Get asset name
//            string assetName = Path.GetFileNameWithoutExtension(filePath);

//            // Load the asset GameObject
//            GameObject assetObject = (GameObject)Resources.Load(assetName);

//            // Check if asset was loaded
//            if (assetObject == null)
//            {
//                Debug.LogError($"Failed to load asset: {assetName}");
//                continue;
//            }

//            // Read metadata from the asset (replace with your logic)
//            //Dictionary<string, string> assetMetadata = ReadAssetMetadata(assetObject);

//            //// Check if required parameters exist
//            //if (!assetMetadata.ContainsKey(xParameterName) ||
//            //    !assetMetadata.ContainsKey(yParameterName) ||
//            //    !assetMetadata.ContainsKey(zParameterName))
//            //{
//            //    Debug.LogError($"Asset {assetName} is missing required parameters!");
//            //    continue;
//            //}

//            // Parse coordinates from metadata
//            float x = float.Parse(assetMetadata[xParameterName]);
//            float y = float.Parse(assetMetadata[yParameterName]);
//            float z = float.Parse(assetMetadata[zParameterName]);

//            // Set transform position
//            assetObject.transform.position = new Vector3(x, y, z);

//            // Optionally, set other properties based on metadata

//            // Instantiate the GameObject in the scene
//            Instantiate(assetObject);
//        }
//    }

//    // Replace this with your actual logic for reading metadata from the asset
//    //private Dictionary<string, string> ReadAssetMetadata(GameObject assetObject)
//    //{
//    //    // Access your custom script or component that stores metadata within the asset
//    //    // Example: assuming a script named "AssetMetadata" attached to the GameObject
//    //    AssetMetadata metadataScript = assetObject.GetComponent<AssetMetadata>();
//    //    if (metadataScript != null)
//    //    {
//    //        return metadataScript.GetMetadata();
//    //    }
//    //    else
//    //    {
//    //        Debug.LogError($"Asset {assetObject.name} has no 'AssetMetadata' script!");
//    //        return new Dictionary<string, string>();
//    //    }
//    //}
//}

