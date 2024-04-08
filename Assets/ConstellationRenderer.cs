using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ConstellationRenderer : MonoBehaviour
{
    public Transform userOrigin; // Reference to the user's position
    public GameObject dynamicRenderingObject; // Reference to the GameObject containing DynamicRendering script
    public Material lineMaterial; // Material for constellation lines

    public ConstellationDatasetManager ConstellationDatasetManager;

    private Dictionary<string, string> constellationNameMap = new Dictionary<string, string>(); // Dictionary to store constellation ID and names
    private Dictionary<string, GameObject> loadedStars = new Dictionary<string, GameObject>(); // Dictionary to store loaded stars with hip id as key
    private Dictionary<string, List<GameObject>> constellationLines = new Dictionary<string, List<GameObject>>(); // Dictionary to store constellation lines with constellation id as key

    public List<GameObject> RenderedConstellationCollection = new List<GameObject>();
    public float updateInterval = 5f; // Time interval between position updates
    public float lineWidth = 0.01f;

    public float thickLineWidth = 0.02f; // Public variable for thick line width
    public Color lineColor = Color.blue; // Public variable for line color
    public float starScaleMultiplier = 1.5f; // Public variable for star scale multiplier
    public Shader starHighlightShader; // Public variable for star shader
    private Dictionary<string, (float, float, Color)> originalValues = new Dictionary<string, (float, float, Color)>(); // Store original values

    void Start()
    {
    }

    // Update constellation positions based on user's origin position
    public void UpdateConstellations()
    {
        // Clear existing constellations
        ClearConstellations();

        FetchLoadedStars(); // fetch updated loaded stars

        // Re-render constellations
        LoadConstellations();
    }

    void FetchLoadedStars()
    {
        // Clear existing constellations
        ClearConstellations();

        if (dynamicRenderingObject != null)
        {
            // Access the DynamicRendering script from the specified GameObject
            DynamicRendering dynamicRendering = dynamicRenderingObject.GetComponent<DynamicRendering>();

            if (dynamicRendering != null)
            {
                // Clear existing loaded stars
                loadedStars.Clear();

                // Fetch the updated loaded stars from the DynamicRendering script
                foreach (var pair in dynamicRendering.loadedStars)
                {
                    // Convert the key from int to string
                    string hipId = pair.Key.ToString();

                    // Add to the loadedStars dictionary
                    loadedStars.Add(hipId, pair.Value);
                }
            }
            else
            {
                Debug.LogError("DynamicRendering script not found on the specified GameObject.");
            }
        }
        else
        {
            Debug.LogError("DynamicRendering GameObject reference is not set.");
        }
    }

    // Clear existing constellations
    void ClearConstellations()
    {
        foreach (var pair in constellationLines)
        {
            foreach (GameObject lineObj in pair.Value)
            {
                Destroy(lineObj); // Destroy each line GameObject
            }
        }
        foreach (GameObject constellationObj in RenderedConstellationCollection)
        {
            Destroy(constellationObj);
        }
        constellationLines.Clear(); // Clear the dictionary
    }

    void LoadConstellations()
    {
        LoadConstellationNames();

        //constellationCsvFile = ConstellationDatasetManager.GetConstellationFile();
        int lineNumber = 0;
        //string[] starDataRows = constellationCsvFile.text.Split('\n');
        string[] starDataRows = ConstellationDatasetManager.GetConstellationFile();
        bool isFirstLine = true;

        // Read the rest of the lines
        foreach (string row in starDataRows)
        {
            if (isFirstLine)
            {
                isFirstLine = false;
                continue;
            }

            lineNumber++;
            string line = row;
            string[] values = line.Split(new string[] { "  ", " " }, System.StringSplitOptions.RemoveEmptyEntries);
            if (values.Length >= 3)
            {
                string constellationId = values[0].Trim();
                if (constellationNameMap.ContainsKey(constellationId))
                {
                    constellationId = constellationNameMap[constellationId];
                } else
                {
                    //Debug.Log(constellationId); // constellation name not found
                }
                int numLines;

                if (int.TryParse(values[1], out numLines)) // Check if numLines is parseable
                {
                    // Create a new list to store constellation lines
                    List<GameObject> lines = new List<GameObject>();

                    // Create a new gameobject for the constellation
                    GameObject constellation = new GameObject("Constellation_" + constellationId); // Instantiate constellation object
                    constellation.transform.SetParent(transform); // Set the constellation as a child of this GameObject

                    // Add stars and lines as children to the constellation
                    for (int i = 0; i < numLines; i++)
                    {
                        int star1;
                        int star2;
                        if (int.TryParse(values[i * 2 + 2], out star1) && int.TryParse(values[i * 2 + 3], out star2))
                        {
                            // Find the star gameobjects with the given IDs
                            GameObject star1Obj = FindStar(star1.ToString());
                            GameObject star2Obj = FindStar(star2.ToString());

                            if (star1Obj != null && star2Obj != null)
                            {
                                // Create a new GameObject for the line
                                GameObject lineObj = new GameObject("Line_" + i);
                                lineObj.transform.SetParent(constellation.transform); // Set the line as a child of the constellation

                                // Add Line Renderer component
                                LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();
                                lineRenderer.positionCount = 2; // Two points define a line
                                lineRenderer.material = lineMaterial; // Assign line material

                                // Set line width
                                lineRenderer.startWidth = lineWidth;
                                lineRenderer.endWidth = lineWidth;

                                // Set initial positions of the line endpoints
                                lineRenderer.SetPositions(new Vector3[] { star1Obj.transform.position, star2Obj.transform.position });

                                // Attach LineFollower component to the line GameObject
                                LineFollower lineFollower = lineObj.AddComponent<LineFollower>();
                                lineFollower.startPoint = star1Obj.transform;
                                lineFollower.endPoint = star2Obj.transform;

                                // Add line object to the list
                                lines.Add(lineObj);
                            }
                        }
                        else
                        {
                            Debug.LogWarning("Failed to parse star IDs at line " + lineNumber);
                        }
                    }

                    // Add constellation lines to the dictionary
                    constellationLines.Add(constellationId, lines);
                    RenderedConstellationCollection.Add(constellation);
                }
                else
                {
                    Debug.LogWarning("Failed to parse numLines at line " + lineNumber);
                }
            }
        }
    }

    GameObject FindStar(string hipId)
    {
        GameObject star;
        loadedStars.TryGetValue(hipId, out star);
        return star;
    }

    void LoadConstellationNames()
    {
        constellationNameMap.Clear();
        string[] lines = ConstellationDatasetManager.GetConstellationNamesFile();

        foreach (string line in lines)
        {
            string[] fields = line.Split('\t');

            if (fields.Length >= 3)
            {
                string constellationID = fields[0];
                string constellationName = ExtractName(fields[2]);

                if (!string.IsNullOrEmpty(constellationID) && !string.IsNullOrEmpty(constellationName))
                {
                    Debug.Log(constellationID + " " + constellationName);
                    constellationNameMap.Add(constellationID, constellationName);
                }
            }
        }
    }

    // Function to extract the name from the field containing the name in parentheses
    private string ExtractName(string field)
    {
        int startIndex = field.IndexOf('"') + 1;
        int endIndex = field.LastIndexOf('"');

        if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
        {
            return field.Substring(startIndex, endIndex - startIndex);
        }

        return null;
    }

    private void StoreOriginalValues(GameObject lineObj)
    {
        LineRenderer lineRenderer = lineObj.GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            originalValues[lineObj.name] = (lineRenderer.startWidth, lineRenderer.endWidth, lineRenderer.material.color);
        }
    }

    public void ResetConstellation(string constellationName)
    {
        if (constellationLines.ContainsKey(constellationName))
        {
            List<GameObject> lines = constellationLines[constellationName];

            foreach (GameObject lineObj in lines)
            {
                if (originalValues.ContainsKey(lineObj.name))
                {
                    // Reset to original values
                    (float originalStartWidth, float originalEndWidth, Color originalColor) = originalValues[lineObj.name];

                    LineRenderer lineRenderer = lineObj.GetComponent<LineRenderer>();
                    if (lineRenderer != null)
                    {
                        lineRenderer.startWidth = originalStartWidth;
                        lineRenderer.endWidth = originalEndWidth;
                        lineRenderer.material.color = originalColor;
                    }
                }
            }

            // remove the entries from originalValues dictionary
            originalValues.Remove(constellationName);
        }
        else
        {
            Debug.LogWarning("Constellation not found: " + constellationName);
        }
    }

    public void ModifyConstellation(string constellationName)
    {
        if (constellationLines.ContainsKey(constellationName))
        {
            List<GameObject> lines = constellationLines[constellationName];

            foreach (GameObject lineObj in lines)
            {
                // Store original values
                StoreOriginalValues(lineObj);
                // Adjust line width
                LineRenderer lineRenderer = lineObj.GetComponent<LineRenderer>();
                if (lineRenderer != null)
                {
                    lineRenderer.startWidth = thickLineWidth;
                    lineRenderer.endWidth = thickLineWidth;
                    lineRenderer.material.color = lineColor; // Apply line color
                }

                // Adjust start and end points
                LineFollower lineFollower = lineObj.GetComponent<LineFollower>();
                if (lineFollower != null)
                {
                    if (lineFollower.startPoint != null)
                    {
                        lineFollower.startPoint.localScale *= starScaleMultiplier; // Scale start point
                        ApplyStarShader(lineFollower.startPoint.gameObject); // Apply star shader
                    }
                    if (lineFollower.endPoint != null)
                    {
                        lineFollower.endPoint.localScale *= starScaleMultiplier; // Scale end point
                        ApplyStarShader(lineFollower.endPoint.gameObject); // Apply star shader
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("Constellation not found: " + constellationName);
        }
    }

    private void ApplyStarShader(GameObject starObject)
    {
        Renderer starRenderer = starObject.GetComponent<Renderer>();
        if (starRenderer != null)
        {
            starRenderer.material.shader = starHighlightShader; // Assign star shader
        }
        else
        {
            Debug.LogWarning("Renderer component not found on star GameObject: " + starObject.name);
        }
    }
}

public class LineFollower : MonoBehaviour
{
    public Transform startPoint; // The start point of the line
    public Transform endPoint; // The end point of the line

    void Update()
    {
        // Update the line positions based on the positions of the start and end points
        if (startPoint != null && endPoint != null)
        {
            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            if (lineRenderer != null)
            {
                lineRenderer.SetPositions(new Vector3[] { startPoint.position, endPoint.position });
            }
        }
    }
}
