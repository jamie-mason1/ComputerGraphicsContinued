Shader "Custom/SimpleDiffuseLightingShader"
{
    Properties
    {
        _Color("Colour", Color) = (1.0, 1.0, 1.0, 1.0) // Color property for the shader (added alpha channel)
        _ToggleDiffuse("Toggle Diffuse", Float) = 1.0 // Toggle for ambient lighting

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }          // Tag for rendering opaque objects
        Tags { "LightMode"="ForwardBase" }      // Tag for forward rendering mode

        Pass
        {
            CGPROGRAM
            #pragma vertex vert                     // Vertex shader entry point
            #pragma fragment frag                   // Fragment shader entry point

            // Declare uniform variables for the shader properties
            uniform float4 _Color;                         // Color property passed from the material
            uniform float4 _LightColor0;                   // Color of the main light source
            uniform float _ToggleDiffuse;
            
            struct Vin                             // Input structure for the vertex shader
            {
                float4 vertex : POSITION;          // Vertex position
                float3 normal : NORMAL;            // Vertex normal
                float2 uv : TEXCOORD0;

            };

            struct Vout                            // Output structure for the vertex shader
            {
                float4 pos : SV_POSITION;           // Position in clip space
                float4 col : COLOR;                 // Color output for the fragment shader
                float2 uv : TEXCOORD0;

            };

            Vout vert(Vin v)                       // Vertex shader function
            {
                Vout o;
                float3 normalDirection = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz); // Transform normal to world space
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz); // Get main light direction
                float atten = 1.0;                   // Light attenuation factor
                float3 diffuseReflection = atten * _LightColor0.xyz * _Color.rgb * max(0.0, dot(normalDirection, lightDirection)); // Calculate diffuse reflection
                if(_ToggleDiffuse > 0.5){
                    o.col = float4(diffuseReflection, 1.0); // Set final color output with full alpha
                }
                else{
                    o.col = float4(0, 0, 0, 1);
                }
                o.pos = UnityObjectToClipPos(v.vertex); // Transform vertex position to clip space
                o.uv = v.uv;
                return o;                               // Return output to fragment shader
            }

            float4 frag(Vout i) : SV_Target          // Fragment shader function
            {
            
                return i.col;                        // Output final color
            }
            ENDCG
        }
    }
    FallBack "Diffuse"                          // Fallback to default diffuse shader if needed
}
