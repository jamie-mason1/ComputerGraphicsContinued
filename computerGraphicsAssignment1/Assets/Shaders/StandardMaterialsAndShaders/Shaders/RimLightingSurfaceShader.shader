Shader "Custom/RimLightingSurfaceShader"
{
    Properties
    {
        _RimColour ("Rim Colour", Color) = (1, 0, 0, 1) 
        _RimPower ("Rim Power", Range(0.1, 10.0)) = 2.0
        _ToggleRimLighting("Rim Toggle", Range (0, 1)) = 1
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 200
        Blend SrcAlpha One

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Lambert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0


        struct Input
        {
            float3 viewDir;
        };

        float _RimPower;
        float4 _RimColour;
        float _ToggleRimLighting;

        void surf (Input IN, inout SurfaceOutput o)
        {
            if(_ToggleRimLighting>=0.5){
                float rimFactor = 1.0 - saturate(dot(normalize(IN.viewDir),o.Normal));
                o.Emission = _RimColour.rgb * pow(rimFactor,_RimPower);
            }
        }
        ENDCG
    }
    FallBack "Diffuse"
}
