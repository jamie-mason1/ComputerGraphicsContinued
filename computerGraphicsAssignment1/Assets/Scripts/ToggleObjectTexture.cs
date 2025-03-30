using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToggleObjectTexture : MonoBehaviour
{
    KeyCode toggleTextures = KeyCode.T;
    bool texturesOn;
    [SerializeField] private List<Material> materials;
    [SerializeField] private TextMeshProUGUI textureOnIndicator;

    void Start(){
        texturesOn = true;
        SetTextures(true);
        setTextureLabel(true);
    }
    // Update is called once per frame

    void Update()
    {
        if(Input.GetKeyDown(toggleTextures)){
            texturesOn = !texturesOn;
            SetTextures();
            setTextureLabel();
        }
    }

    void SetTextures(){
        foreach(Material mat in materials){
            if(mat.HasProperty("_ToggleTextures")){
                if(texturesOn){
                    mat.SetFloat("_ToggleTextures", 1f);
                }
                else
                {
                    mat.SetFloat("_ToggleTextures", 0f);
                }
            }
        }
    }
    void SetTextures(bool toggle){
        foreach(Material mat in materials){
            if(mat.HasProperty("_ToggleTextures")){
                if(toggle){
                    mat.SetFloat("_ToggleTextures", 1f);
                }
                else
                {
                    mat.SetFloat("_ToggleTextures", 0f);
                }
            }
        }
    }
    void setTextureLabel(){
        if(texturesOn){
            textureOnIndicator.text = "Texturing Textures ON.";
                
        }
        else{
            textureOnIndicator.text = "Texturing Textures OFF.";
                
        }
    }
    
    void setTextureLabel(bool toggle){
        if(toggle){
            textureOnIndicator.text = "Texturing Textures ON.";
                
        }
        else{
            textureOnIndicator.text = "Texturing Textures OFF.";
                
        }
    }

    void OnDestroy(){
        setTextureLabel(true);
        SetTextures(true);
    }
}

