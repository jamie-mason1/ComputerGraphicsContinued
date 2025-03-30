using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LUTchange : MonoBehaviour
{
    protected Material LUTMat; // Material used for LUT
    ColourCorrectionScript colourCorrectionScript; // Reference to Colour Correction Script
    int currentLUT, maxLength; // Current LUT index and max LUT index
    bool hasChanged; // Flag to track if LUT has changed
    [SerializeField] private Texture2D[] LUTTextures; // Array of LUT textures
    [SerializeField] private RawImage LUTImage; // UI element to display LUT
    [SerializeField] private TMP_InputField LUTContributionInputField; // Input field for LUT contribution
    bool isInputFieldSelected; // Flag to track if input field is selected
    float lastValue, value; // Last value of contribution and current value

    void Start()
    {
        // Get the Colour Correction Script and LUT Material from the main camera
        colourCorrectionScript = Camera.main.gameObject.GetComponent<ColourCorrectionScript>();
        LUTMat = colourCorrectionScript.m_renderMaterial;

        // Set initial LUT texture if available
        if (LUTTextures.Length >= 1 && LUTTextures[0] != null)
        {
            LUTMat.SetTexture("_LUT", LUTTextures[0]);
        }

        // Find and assign the RawImage to display the LUT
        if (LUTImage == null)
        {
            LUTImage = GameObject.Find("LUTImage").GetComponent<RawImage>();
        }
        if (LUTImage != null)
        {
            LUTImage.texture = LUTMat.GetTexture("_LUT");
        }
        else
        {
            Debug.LogError("the variable LUTImage has failed to instance an object in the inspector.");
        }

        // Find and assign the TMP_InputField for LUT contribution
        if (LUTContributionInputField == null)
        {
            LUTContributionInputField = GameObject.Find("LUTContribution").GetComponent<TMP_InputField>();
        }

        // Initialize LUT indices and contribution values
        currentLUT = 0;
        maxLength = LUTTextures.Length - 1;
        hasChanged = false;

        // Set up the input field and its listeners
        if (LUTContributionInputField != null)
        {
            LUTContributionInputField.text = "0"; // Set initial contribution value to 0
            value = float.Parse(LUTContributionInputField.text);
            LUTMat.SetFloat("_Contribution", value); // Set contribution in the material

        }
        else
        {
            value = 0;
            Debug.LogError("LUTContributionInputField variable has not been instanced.");
        }

        lastValue = value; // Initialize last value
    }

    void Update()
    {
        // Update current contribution value from input field
        if (LUTContributionInputField != null)
        {
            //check if value in input field can be converted to a float
            if (!float.TryParse(LUTContributionInputField.text, out value))
            {
                //If parsing fails set the value to the last received value if it does not already equal the last value received. 
                if (value != lastValue)
                {
                    value = lastValue;
                }
            }
        }

        // Check if there are multiple LUT textures to switch between
        if (LUTTextures.Length > 1)
        {
            // Change LUT based on input keys
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (currentLUT > 0)
                {
                    currentLUT--;
                }
                else
                {
                    currentLUT = maxLength; // Loop to last LUT
                }
                hasChanged = true;
            }
            else if (Input.GetKeyDown(KeyCode.O))
            {
                if (currentLUT < maxLength)
                {
                    currentLUT++;
                }
                else
                {
                    currentLUT = 0; // Loop to first LUT
                }
                hasChanged = true;
            }
            else
            {
                hasChanged = false; // No change
            }

            // Update LUT material if it has changed
            if (hasChanged)
            {
                if (LUTTextures[currentLUT] != null)
                {
                    LUTMat.SetTexture("_LUT", LUTTextures[currentLUT]);
                    if (LUTImage != null)
                    {
                        LUTImage.texture = LUTMat.GetTexture("_LUT");
                    }
                    else
                    {
                        Debug.LogError("the variable LUTImage has failed to instance an object in the inspector.");
                    }
                }
                else
                {
                    Debug.LogError($"LUTTextures[{currentLUT}] has not been instanced.");
                }
            }
        }

        // Update LUT contribution if input field is not selected and value has changed
        if (value != lastValue)
        {
            if (LUTContributionInputField != null)
            {
                value = Mathf.Round(value * 100f) / 100f; // Round to two decimal places
                value = Mathf.Clamp01(value); // Clamp value between 0 and 1
                LUTContributionInputField.text = value.ToString(); // Update input field text
                if (LUTMat.HasProperty("_Contribution"))
                {
                    LUTMat.SetFloat("_Contribution", value); // Set contribution in the material
                }
                else
                {
                    Debug.LogError("The property _Contribution does not exist in the given shader.");
                }
            }
        }
        lastValue = value; // Update last value
    }
}
