using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using System.Linq;


public class IlluminationCorrectionScript : MonoBehaviour
{
    public Shader illuminationShader; // The post-processing shader
    public Material illuminationMaterial;

    [SerializeField] private int[] sceneLightingToggles; // Array to hold lighting toggle values
    [SerializeField] private List<int[]> sceneLightingTogglesCombinations = new List<int[]>(); // List of all possible toggle combinations

    private string[] ToggleProperties = new string[4]; // Shader properties to be toggled

    [SerializeField] private TextMeshProUGUI illumnationDispay; // UI text to display current illumination type

    bool hasChanged; // Flag to track changes in lighting
    int maxLength; // Maximum length of the lighting combinations
    int currentLight; // Currently selected lighting combination

    private void Awake()
    {
        // Initialize toggle property names
        ToggleProperties[0] = "_ToggleAmbient";
        ToggleProperties[1] = "_ToggleDiffuse";
        ToggleProperties[2] = "_ToggleSpecular";
        ToggleProperties[3] = "_ToggleDiffuseWrap";

        int numOfToggles = ToggleProperties.Length; // Number of toggle properties
        int numOfCombinations = (int)Math.Pow(2, numOfToggles); // Calculate number of combinations
        sceneLightingToggles = new int[numOfCombinations]; // Initialize the array

        GenerateCombinations(numOfToggles); // Generate all toggle combinations
    }
    private void GenerateCombinations(int numOfToggles)
    {
        // Generate all possible combinations of toggle states (0 or 1)
        for (int i = 0; i < (1 << numOfToggles); i++)
        {
            int[] combination = new int[numOfToggles];
            for (int j = 0; j < numOfToggles; j++)
            {
                combination[j] = (i & (1 << j)) != 0 ? 1 : 0; // Set toggle states
            }
            sceneLightingTogglesCombinations.Add(combination); // Add combination to the list
        }
    }

    void Start()
    {
        
        // Ensure the shader is assigned
        if (illuminationShader == null)
        {
            Debug.LogError("Illumination shader not assigned!");
            return;
        }
        for (int i = 0; i < sceneLightingToggles.Length; i++)
        {
            sceneLightingToggles[i] = i + 1;
        }

        // Create material for post-processing
        if(illuminationMaterial == null){
            illuminationMaterial = new Material(illuminationShader);
        }

        hasChanged = false; // Initialize change flag
        maxLength = sceneLightingToggles.Length - 1; // Set maximum length
        currentLight = maxLength; // Start with the last lighting configuration
        illumnationDispay.text = toggleGUI(currentLight); // Update display with current lighting
        UpdateMaterials(maxLength); // Apply the initial lighting configuration to materials
    }

    string ExtractAfterToggle(string property)
    {
        if (property.Contains("Toggle") && property != null)
        {
            return property.Substring(property.IndexOf("Toggle") + "Toggle".Length); // Extract the property name after "Toggle"
        }
        return property;
    }

    string InsertSpaces(string input)
    {
        return Regex.Replace(input, "(\\B[A-Z])", " $1"); // Add spaces before capital letters in the string
    }


    string toggleGUI(int currentLight)
    {
        string guiText;
        int onesCount = 0; // Count of active toggle states
        List<int> activeBits = new List<int>(); // List to store indices of active bits

        // Count active bits and store their indices
        for (int i = 0; i < sceneLightingTogglesCombinations[currentLight].Length; i++)
        {
            if (sceneLightingTogglesCombinations[currentLight][i] == 1)
            {
                onesCount++;
                activeBits.Add(i);
            }
        }

        // Generate GUI text based on active toggles
        if (onesCount == 1)
        {
            guiText = InsertSpaces(ExtractAfterToggle(ToggleProperties[activeBits[0]])) + " illumination only";
        }
        else if (onesCount > 1)
        {
            guiText = string.Join(" + ", activeBits.Select(index => InsertSpaces(ExtractAfterToggle(ToggleProperties[index]))));
        }
        else
        {
            guiText = "No Lighting"; // No active lighting toggles
        }

        return guiText; // Return the GUI text
    }
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (illuminationMaterial != null)
        {
            // Apply the illumination shader using the material
            Graphics.Blit(source, destination, illuminationMaterial);
        }
        else
        {
            // If no material, just copy the source to destination
            Graphics.Blit(source, destination);
        }
    }
void UpdateMaterials(int currentLight)
    {
        
            for (int i = 0; i < ToggleProperties.Length; i++)
            {
                if (illuminationMaterial.HasProperty(ToggleProperties[i]))
                {
                    illuminationMaterial.SetFloat(ToggleProperties[i], sceneLightingTogglesCombinations[currentLight][i]); // Set the toggle property
                }
            }
        
    }

    private void Update()
    {
        // Handle input for toggling lights
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (currentLight > 0)
            {
                currentLight--; // Move to the previous lighting configuration
            }
            else
            {
                currentLight = maxLength; // Wrap around to the last configuration
            }
            hasChanged = true;
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            if (currentLight < maxLength)
            {
                currentLight++; // Move to the next lighting configuration
            }
            else
            {
                currentLight = 0; // Wrap around to the first configuration
            }
            hasChanged = true;
        }
        else
        {
            hasChanged = false; // No changes made
        }

        UpdateMaterials(currentLight); // Update material properties based on current lighting configuration
        if (hasChanged && illumnationDispay != null)
        {
            illumnationDispay.text = toggleGUI(currentLight); // Update UI text if there were changes
        }
    }
    private void OnDestroy()
    {
        UpdateMaterials(maxLength); // Reset materials when disabled
    }
}

