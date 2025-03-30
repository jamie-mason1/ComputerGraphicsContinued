Shader "Custom/AmbientLighting"
{
    Properties
    {
        _Color("Colour", Color) = (1.0, 1.0, 1.0, 1.0) // Color property for the shader (added alpha channel)
        _ToggleAmbient("Toggle Ambient", Float) = 1.0 // Toggle for ambient lighting
    }
    SubShader
    {
        Tags { "LightMode"="ForwardBase" } // Tag for forward rendering mode

        Pass
        {
            CGPROGRAM
            #pragma vertex vert // Vertex shader entry point
            #pragma fragment frag // Fragment shader entry point

            // Properties
            uniform float4 _Color; // Color property passed from the material
            uniform float _ToggleAmbient; // Toggle for ambient lighting

            struct Vin // Input structure for the vertex shader
            {
                float4 vertex : POSITION; // Vertex position
                float3 normal : NORMAL; // Vertex normal
                float2 uv : TEXCOORD0;
            };

            struct Vout // Output structure for the vertex shader
            {
                float4 pos : SV_POSITION; // Position in clip space
                float4 col : COLOR; // Color output for the fragment shader
                float2 uv : TEXCOORD0;
            };

            Vout vert(Vin v) // Vertex shader function
            {
                Vout o;

                // Calculate ambient color
                float3 ambientColor = _Color.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb;

                // Using if statement to toggle ambient lighting
                if (_ToggleAmbient > 0.5) // Check if ambient lighting is enabled
                {
                    o.col = float4(ambientColor, _Color.a); // Apply ambient color
                }
                else // If ambient lighting is not enabled
                {
                    o.col = float4(0.0, 0.0, 0.0, _Color.a); // Set color to black
                }

                o.pos = UnityObjectToClipPos(v.vertex); // Transform vertex position to clip space
                o.uv = v.uv;

                return o; // Return output to fragment shader
            }

            float4 frag(Vout i) : SV_Target // Fragment shader function
            {
                return i.col; // Output final color
            }
            ENDCG
        }
    }
    FallBack "Diffuse" // Fallback to default diffuse shader if needed
}
