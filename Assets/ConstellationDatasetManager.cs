using System.Collections.Generic;
using UnityEngine;

public class ConstellationDatasetManager : MonoBehaviour
{
    public enum ConstellationDataset
    {
        Modern,
        Aztec,
        Norse,
        Egyptian,
        Chinese,
        Maya
    }

    public TextAsset modernConstellationFile;
    public TextAsset modernConstellationNamesFile;

    public TextAsset aztecConstellationFile;
    public TextAsset aztecConstellationNamesFile;

    public TextAsset norseConstellationFile;
    public TextAsset norseConstellationNamesFile;

    public TextAsset egyptianConstellationFile;
    public TextAsset egyptianConstellationNamesFile;

    public TextAsset chineseConstellationFile;
    public TextAsset chineseConstellationNamesFile;

    public TextAsset mayaConstellationFile;
    public TextAsset mayaConstellationNamesFile;

    private Dictionary<ConstellationDataset, (TextAsset, TextAsset)> datasetMap = new Dictionary<ConstellationDataset, (TextAsset, TextAsset)>();

    public ConstellationDataset SelectedConstellationDataset { get; private set; }

    public ConstellationRenderer ConstellationRenderer;

    void Start()
    {
        // Populate the dataset map
        datasetMap.Add(ConstellationDataset.Modern, (modernConstellationFile, modernConstellationNamesFile));
        datasetMap.Add(ConstellationDataset.Aztec, (aztecConstellationFile, aztecConstellationNamesFile));
        datasetMap.Add(ConstellationDataset.Norse, (norseConstellationFile, norseConstellationNamesFile));
        datasetMap.Add(ConstellationDataset.Egyptian, (egyptianConstellationFile, egyptianConstellationNamesFile));
        datasetMap.Add(ConstellationDataset.Chinese, (chineseConstellationFile, chineseConstellationNamesFile));
        datasetMap.Add(ConstellationDataset.Maya, (mayaConstellationFile, mayaConstellationNamesFile));

        // Set default dataset
        SelectedConstellationDataset = ConstellationDataset.Modern;
    }

    public void SetSelectedConstellationDataset(string datasetName)
    {
        // Convert the dataset name string to uppercase to match enum names
        datasetName = datasetName.ToLower();
        datasetName = char.ToUpper(datasetName[0]) + datasetName.Substring(1);

        // Parse the dataset name string to the ConstellationDataset enum
        ConstellationDataset datasetEnum;
        if (System.Enum.TryParse(datasetName, out datasetEnum))
        {
            SelectedConstellationDataset = datasetEnum;
        }
        else
        {
            Debug.LogError("Invalid dataset name: " + datasetName);
        }

        ConstellationRenderer.SendMessage("UpdateConstellationsFromFile");
    }

    public void ToggleModern(bool isOn)
    {
        if (isOn)
        {
            SetSelectedConstellationDataset("modern");
        }
    }

    public void ToggleAztec(bool isOn)
    {
        if (isOn)
        {
            SetSelectedConstellationDataset("aztec");
        }
    }

    public void ToggleNorse(bool isOn)
    {
        if (isOn)
        {
            SetSelectedConstellationDataset("norse");
        }
    }

    public void ToggleEgyptian(bool isOn)
    {
        if (isOn)
        {
            SetSelectedConstellationDataset("egyptian");
        }
    }

    public void ToggleChinese(bool isOn)
    {
        if (isOn)
        {
            SetSelectedConstellationDataset("chinese");
        }
    }

    public void ToggleMaya(bool isOn)
    {
        if (isOn)
        {
            SetSelectedConstellationDataset("maya");
        }
    }


    public TextAsset GetConstellationFile()
    {
        switch (SelectedConstellationDataset)
        {
            case ConstellationDataset.Modern:
                return modernConstellationFile;
            case ConstellationDataset.Aztec:
                return aztecConstellationFile;
            case ConstellationDataset.Norse:
                return norseConstellationFile;
            case ConstellationDataset.Egyptian:
                return egyptianConstellationFile;
            case ConstellationDataset.Chinese:
                return chineseConstellationFile;
            case ConstellationDataset.Maya:
                return mayaConstellationFile;
            default:
                Debug.LogError("Invalid constellation dataset selected: " + SelectedConstellationDataset);
                return null;
        }
    }

    public TextAsset GetConstellationNamesFile()
    {
        switch (SelectedConstellationDataset)
        {
            case ConstellationDataset.Modern:
                return modernConstellationNamesFile;
            case ConstellationDataset.Aztec:
                return aztecConstellationNamesFile;
            case ConstellationDataset.Norse:
                return norseConstellationNamesFile;
            case ConstellationDataset.Egyptian:
                return egyptianConstellationNamesFile;
            case ConstellationDataset.Chinese:
                return chineseConstellationNamesFile;
            case ConstellationDataset.Maya:
                return mayaConstellationNamesFile;
            default:
                Debug.LogError("Invalid constellation dataset selected: " + SelectedConstellationDataset);
                return null;
        }
    }

}
