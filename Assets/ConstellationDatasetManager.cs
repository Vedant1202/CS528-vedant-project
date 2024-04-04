using System;
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

    private Dictionary<ConstellationDataset, (string[], string[])> datasetMap = new Dictionary<ConstellationDataset, (string[], string[])>();

    public ConstellationDataset SelectedConstellationDataset { get; private set; }

    public ConstellationRenderer ConstellationRenderer;

    void Start()
    {
        // Populate the dataset map
        datasetMap.Add(ConstellationDataset.Modern, (GetLines(modernConstellationFile), GetLines(modernConstellationNamesFile)));
        datasetMap.Add(ConstellationDataset.Aztec, (GetLines(aztecConstellationFile), GetLines(aztecConstellationNamesFile)));
        datasetMap.Add(ConstellationDataset.Norse, (GetLines(norseConstellationFile), GetLines(norseConstellationNamesFile)));
        datasetMap.Add(ConstellationDataset.Egyptian, (GetLines(egyptianConstellationFile), GetLines(egyptianConstellationNamesFile)));
        datasetMap.Add(ConstellationDataset.Chinese, (GetLines(chineseConstellationFile), GetLines(chineseConstellationNamesFile)));
        datasetMap.Add(ConstellationDataset.Maya, (GetLines(mayaConstellationFile), GetLines(mayaConstellationNamesFile)));

        // Set default dataset
        //Debug.Log(ConstellationDataset.Keys());
        ToggleModern(true);
        // Iterate over each enum value and print its name
        //foreach (ConstellationDataset dataset in Enum.GetValues(typeof(ConstellationDataset)))
        //{
        //    Debug.Log(dataset);
        //}
        //SelectedConstellationDataset = ConstellationDataset.Modern;

        //Invoke("ToggleAztec", 5f);
    }

    string[] GetLines(TextAsset textAsset)
    {
        if (textAsset != null)
        {
            string[] lines = textAsset.text.Split('\n');
            Debug.Log(lines[0]);
            return lines;
        }
        else
        {
            Debug.LogError("TextAsset is null.");
            return new string[0];
        }
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

        ConstellationRenderer.SendMessage("UpdateConstellations");
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
        Debug.Log("called aztec");
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

    public string[] GetConstellationFile()
    {
        (string[], string[]) dataset;
        if (datasetMap.TryGetValue(SelectedConstellationDataset, out dataset))
        {
            return dataset.Item1;
        }
        else
        {
            Debug.LogError("Selected constellation dataset not found in dataset map: " + SelectedConstellationDataset);
            return null;
        }
    }

    public string[] GetConstellationNamesFile()
    {
        (string[], string[]) dataset;
        if (datasetMap.TryGetValue(SelectedConstellationDataset, out dataset))
        {
            return dataset.Item2;
        }
        else
        {
            Debug.LogError("Selected constellation dataset not found in dataset map: " + SelectedConstellationDataset);
            return null;
        }
    }
}
