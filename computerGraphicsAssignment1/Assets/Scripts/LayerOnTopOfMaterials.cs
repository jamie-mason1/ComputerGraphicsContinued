using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerOnTopOfMaterials : MonoBehaviour
{
   public Shader customLightingShader; 

    void Start()
    {
        // Find all renderers in the scene
        Renderer[] renderers = FindObjectsOfType<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            if (renderer != null && renderer.material != null)
            {
                // Get the existing material
                Material existingMaterial = renderer.material;

                // Change the shader to custom lighting shader
                existingMaterial.shader = customLightingShader;

                // Preserve the original texture if there is one
                existingMaterial.mainTexture = renderer.material.mainTexture;

                //set the custom shader properties as needed
                existingMaterial.SetColor("_Color", existingMaterial.GetColor("_Color")); // Preserve original base color
                existingMaterial.SetColor("_SpecColor", existingMaterial.GetColor("_SpecColor")); // Preserve original specular color
                existingMaterial.SetFloat("_Shininess", existingMaterial.GetFloat("_Shininess")); 

               
            }
        }
    }

}
