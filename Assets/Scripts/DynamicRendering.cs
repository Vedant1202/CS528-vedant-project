using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class DynamicRendering : MonoBehaviour
{
    public Transform userOrigin; // Reference to the user's position
    public float maxDistance = 100f; // Maximum distance to load stars
    private float positionScale = 1.0f; // Scale factor for positions of stars

    public GameObject starPrefab; // Prefab for star cube
    public Button startVelocityButton; // Reference to the button to start velocity
    public Button stopVelocityButton; // Reference to the button to stop velocity
    public float velocityDistanceMultiplier = 0.1f;

    public TextAsset exoCsvFile; // Reference to the InputField for exoplanet CSV file path
    public TextAsset starCsvFile;
    public Slider positionScaleSlider; // Reference to the Slider for position scale


    // List to store star data
    public List<StarData> starDataset = new List<StarData>();
    //private List<StarData> starDataset = new List<StarData>(); // List to store star data
    public Dictionary<int, GameObject> loadedStars = new Dictionary<int, GameObject>(); // Dictionary to store loaded stars with hip as key
    private List<StarData> loadedStarsArray = new List<StarData> { }; // Dictionary to store loaded stars with hip as key

    private bool isStarted = false; // Flag to indicate whether Start function has been called
    private bool isVelocityEnabled = false; // Flag to indicate whether velocity functionality is enabled

    private float lastUpdateTime; // Time of the last update
    public float updateInterval = 5f; // Update interval in seconds
    public float updateVelocityInterval = 2f; // Update interval in seconds
    private float lastVelocityUpdateTime; // Time of the last velocity update
                                          // Path to the exoplanet CSV file
    public ConstellationRenderer ConstellationRenderer;

    public static event Action StarsLoaded; // Event to signal that stars are loaded

    private Vector3 initialPosition; // Initial position of the origin GameObject
    //public float distanceThreshold = 1.0f; // Distance threshold to trigger the function

    public Toggle stellarCheckbox;
    public Toggle knownPlanetsCheckbox;

    private int numVelocityCycles = 0;

    // Start is called before the first frame update
    void Start()
    {
        LoadCSVFile();
        UpdateStarsWithOriginMovement();

        // Store the initial position
        initialPosition = userOrigin.transform.position;

        // Raise the event to signal that stars are loaded
        if (StarsLoaded != null)
        {
            StarsLoaded.Invoke();
        }
        //StartCoroutine(CallFunctionRepeatedly());
        lastUpdateTime = Time.time; // Initialize the last update time
        //ToggleVelocity(true); // toggle
        //UpdateStarsWithOriginMovement();
        // Add listener to the InputField for value changes
        //csvFilePathInput.onValueChanged.AddListener(delegate { LoadCSVFile(); });
         //Add listener to the Slider for position scale changes
        positionScaleSlider.onValueChanged.AddListener(delegate { SetPositionScale(positionScaleSlider.value); });
        // Add onClick listeners to the buttons
        startVelocityButton.onClick.AddListener(StartVelocity);
        stopVelocityButton.onClick.AddListener(StopVelocity);

        // Add listeners to detect changes in checkbox states
        stellarCheckbox.onValueChanged.AddListener(OnStarColorSchemeCheckboxValueChanged);
        knownPlanetsCheckbox.onValueChanged.AddListener(OnStarColorSchemeCheckboxValueChanged);
        OnStarColorSchemeCheckboxValueChanged(true);
    }

    // Function to handle changes in the "Stellar" checkbox
    void OnStarColorSchemeCheckboxValueChanged(bool newValue = true)
    {
        UpdateStarsWithOriginMovement();
    }

    // Function to start velocity
    void StartVelocity()
    {
        Debug.Log("velocity start called");
        ToggleVelocity(true);
    }

    // Function to stop velocity
    void StopVelocity()
    {
        Debug.Log("velocity stop called");
        ToggleVelocity(false);
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateStarsWithOriginMovement(); // Always update stars based on origin movement
        if (isStarted)
        {
            // Check if it's time to update stars with origin movement
            if (Time.time - lastUpdateTime >= updateInterval)
            {
                if (HasMovedBeyondThreshold())
                {
                    UpdateStarsWithOriginMovement();
                    lastUpdateTime = Time.time; // Update the last update time
                }
            }

            if (isVelocityEnabled)
            {
                // Check if it's time to update stars with velocity
                if (Time.time - lastVelocityUpdateTime >= updateVelocityInterval)
                {
                    numVelocityCycles += 1;
                    UpdateStarsWithVelocity(updateVelocityInterval * numVelocityCycles);
                    ConstellationRenderer.SendMessage("UpdateConstellations");
                    lastVelocityUpdateTime = Time.time; // Update the last velocity update time
                }
                //UpdateStarsWithVelocity(Time.deltaTime);
            }
        }
    }


    private bool HasMovedBeyondThreshold()
    {
        // Calculate the distance between the current position and the initial position
        float distance = Vector3.Distance(userOrigin.transform.position, initialPosition);

        // Return true if the distance is greater than or equal to the threshold
        bool shouldUpdate = distance >= maxDistance;
        //Debug.Log("Called moved" + shouldUpdate);
        if (shouldUpdate)
        {
            initialPosition = userOrigin.transform.position;
        }
        return shouldUpdate;
    }

    void UpdateStarsWithOriginMovement()
    {
        // Clear previously loaded stars
        ClearLoadedStars();

        // Load stars within maxDistance
        foreach (var starData in starDataset)
        {
            Vector3 starPosition = new Vector3(starData.x0 * positionScale, starData.y0 * positionScale, starData.z0 * positionScale);
            float distanceToOrigin = Vector3.Distance(userOrigin.position, starPosition);
            //Debug.Log("Called origin distance" + distanceToOrigin + maxDistance + positionScale);


            if (distanceToOrigin <= maxDistance * positionScale)
            {
                //Debug.Log("Called update origin" + starData.hip);
                GameObject newStar = Instantiate(starPrefab, starPosition, Quaternion.identity);
                newStar.name = "star-" + starData.hip.ToString();
                // Set color based on spectral type
                Color starColor = GetStarColor(starData);
                SetStarColor(newStar, starColor);
                loadedStarsArray.Add(starData);
                loadedStars.Add(starData.hip, newStar);
            }
        }

        ConstellationRenderer.SendMessage("UpdateConstellations");
    }

    Color GetColorForSpectType(string spectType)
    {
        // Define a mapping between spectral types and colors
        Dictionary<string, Color> spectColorMapping = new Dictionary<string, Color>()
        {
            {"O", Color.blue},
            {"B", Color.cyan},
            {"A", Color.white},
            {"F", new Color(1, 1, 0.8f)}, // yellow-white
            {"G", Color.yellow},
            {"K", new Color(1, 0.5f, 0)},  // Orange color
            {"M", Color.red}
        };

        // Check if the given spectral type exists in the mapping
        if (spectColorMapping.ContainsKey(spectType))
        {
            return spectColorMapping[spectType];
        }
        else
        {
            // Default color if the spectral type is not found
            return Color.gray;
        }
    }

    Color GetColorForNumExo(int numExo)
    {
        // Define a mapping between numExo values and colors in a descending gradient
        Dictionary<int, Color> numExoColorMapping = new Dictionary<int, Color>()
        {
            {6, Color.red},
            {5, Color.magenta},
            {4, Color.yellow},
            {3, Color.green},
            {2, Color.cyan},
            {1, Color.blue}
        };

        // Check if the given numExo value exists in the mapping
        if (numExoColorMapping.ContainsKey(numExo))
        {
            return numExoColorMapping[numExo];
        }
        else
        {
            // Default color if the numExo value is not found
            return Color.white;
        }
    }

    Color GetColorForCheckboxState(int numExo, string spect, bool stellarCheckboxChecked, bool knownPlanetsCheckboxChecked)
    {
        if (knownPlanetsCheckboxChecked)
        {
            // Color based on numExo
            if (numExo != null)
            {
                return GetColorForNumExo((int)numExo);
            }
            else
            {
                // Default color if numExo is null
                return Color.white;
            }
        }
        else if (stellarCheckboxChecked)
        {
            // Color based on spectral type
            return GetColorForSpectType(spect);
        }
        else
        {
            // Default color based on spectral type
            return GetColorForSpectType(spect);
        }
    }

    // Function to update star colors based on checkbox states
    private Color GetStarColor(StarData starData)
    {
        // Get the current state of the checkboxes
        bool stellarChecked = stellarCheckbox.isOn;
        bool knownPlanetsChecked = knownPlanetsCheckbox.isOn;
        Color color = GetColorForCheckboxState(starData.numExo, starData.spect, stellarChecked, knownPlanetsChecked);
        return color;
    }

    void SetStarColor(GameObject starObject, Color color)
    {
        Renderer renderer = starObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = color;
        }
    }

    void UpdateStarsWithVelocity(float deltaTime)
    {
        // Update positions of loaded stars based on their velocities
        foreach (var starHip in loadedStars.Keys)
        {
            foreach (var starData in loadedStarsArray)
            {
                if (starData.hip == starHip)
                {
                    Vector3 velocity = new Vector3(starData.vx, starData.vy, starData.vz);
                    Vector3 newPosition = new Vector3(
                        starData.x0 + velocity.x * deltaTime,
                        starData.y0 + velocity.y * deltaTime,
                        starData.z0 + velocity.z * deltaTime
                    );

                    // Scale the positions of stars
                    newPosition *= positionScale;

                    // Check if the star is within the visible range
                    float distanceToOrigin = Vector3.Distance(userOrigin.position, newPosition);
                    if (distanceToOrigin <= maxDistance * velocityDistanceMultiplier)
                    {
                        // Update the position of the loaded star
                        loadedStars[starHip].transform.position = newPosition;
                    }
                    break;
                }
            }
        }
    }


    void ClearLoadedStars()
    {
        loadedStarsArray.Clear();
        // Destroy previously loaded stars
        foreach (var star in loadedStars.Values)
        {
            Destroy(star);
        }
        loadedStars.Clear();
    }

    void ParseCSV()
    {
        // Clear existing data
        starDataset.Clear();

        //// Read the CSV file
        string[] starDataRows = starCsvFile.text.Split('\n');

        bool isFirstLine = true;

        // Read the rest of the lines
        foreach (string row in starDataRows)
        {
            if (isFirstLine)
            {
                isFirstLine = false;
                continue;
            }

            // Read the rest of the lines
            //while (!reader.EndOfStream)
        //{
            string[] values = row.Split(',');
            if (values.Length == 11) // Ensure all fields are present
            {
                StarData starData = new StarData();
                starData.hip = int.Parse(values[0]);
                starData.dist = float.Parse(values[1]);
                starData.x0 = float.Parse(values[2]);
                starData.y0 = float.Parse(values[3]);
                starData.z0 = float.Parse(values[4]);
                starData.absmag = float.Parse(values[5]);
                starData.mag = float.Parse(values[6]);
                starData.vx = float.Parse(values[7]);
                starData.vy = float.Parse(values[8]);
                starData.vz = float.Parse(values[9]);
                starData.spect = values[10];
                starDataset.Add(starData);
            }
            else
            {
                Debug.LogError("Invalid data format in CSV " + values);
            }
        }

        // Close the reader
        //reader.Close();
    }

    // Call this method to load the CSV file from the provided path
    public void LoadCSVFile()
    {
        LoadStarCSV();
        LoadExoplanetCSV();
        isStarted = true; // Set flag to indicate that Start function has been called
        Debug.Log("star dataset" + starDataset[0]);

    }

    // Load the star CSV file
    void LoadStarCSV()
    {
        ParseCSV();
    }

    // Load the exoplanet CSV file
    void LoadExoplanetCSV()
    {
       ParseExoplanetCSV();
    }

    // Parse the exoplanet CSV file and update the starDataset with number of exoplanets
    void ParseExoplanetCSV()
    {
        //// Read the CSV file
        string[] exoDataRows = exoCsvFile.text.Split('\n');

        bool isFirstLine = true;

        // Read the rest of the lines
        foreach (string row in exoDataRows)
        {
            if (isFirstLine)
            {
                isFirstLine = false;
                continue;
            }
            string[] values = row.Split(',');
            if (values.Length == 2) // Ensure all fields are present
            {
                string hipIdString = values[0].Trim();
                // Extract the HIP ID from the string format "HIP XXXXX"
                int hipId;
                if (int.TryParse(hipIdString.Split(' ')[1], out hipId))
                {
                    int numExo = int.Parse(values[1]);

                    // Find the star in the star dataset by HIP ID and update the number of exoplanets
                    for (int i = 0; i < starDataset.Count; i++)
                    {
                        if (starDataset[i].hip == hipId)
                        {
                            // Get the reference to the star data
                            StarData star = starDataset[i];
                            // Update the number of exoplanets
                            star.numExo = numExo;
                            // Set the updated star data back to the list
                            starDataset[i] = star;
                            Debug.Log("numExo" + starDataset[i].numExo);
                            break; // Exit loop after updating
                        }
                    }
                }
                else
                {
                    Debug.LogError("Invalid HIP ID format in exoplanet CSV: " + hipIdString);
                }
            }
            else
            {
                Debug.LogError("Invalid data format in exoplanet CSV");
            }
        }

        // Close the reader
        //reader.Close();
    }

    // Function to set position scale factor
    public void SetPositionScale(float scale)
    {
        positionScale = scale;
        UpdateStarsWithOriginMovement();
    }

    // Function to toggle velocity functionality
    public void ToggleVelocity(bool isEnabled)
    {
        isVelocityEnabled = isEnabled;
        Debug.Log("Called toggle velocity" + isVelocityEnabled);
    }

    // Define your star data structure
    public struct StarData
    {
        public int hip;
        public float dist, x0, y0, z0, absmag, mag, vx, vy, vz;
        public string spect;
        public int numExo; // Number of exoplanets
    }
}
