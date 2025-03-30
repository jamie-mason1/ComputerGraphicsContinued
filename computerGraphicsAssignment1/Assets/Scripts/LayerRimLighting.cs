using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerRimLighting : MonoBehaviour
{
    [SerializeField] private Shader rimLighting;
    [SerializeField] private Material rimMaterial;
    private Material materialOnRenderer; // List to store modified materials

    Renderer rend;

    // Update is called once per frame
    void Start(){
        rend = GetComponent<Renderer>();
        if(rend!=null){
            if(rend.material != null){
                materialOnRenderer = rend.material;
            }
        }
        UpdateMaterial();

    }
    void Update()
    {
        
    }
    void UpdateMaterial(){
        
    }
}
