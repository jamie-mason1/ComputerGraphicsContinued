Shader "Custom/DiffuseWrap"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {} // Base texture of the object
        _ToggleDiffuseWrap("Toggle Diffuse Wrap", Float) = 1.0 // Toggle for diffuse wrap effect
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex; // The main texture
            uniform float _ToggleDiffuseWrap; // Toggle for diffuse wrap effect

            struct Vin
            {
                float4 vertex : POSITION; // Vertex position
                float3 normal : NORMAL; // Vertex normal
                float2 uv : TEXCOORD0; // UV coordinates
            };

            struct Vout
            {
                float4 pos : SV_POSITION; // Position in clip space
                float2 uv : TEXCOORD0; // UV coordinates passed to fragment shader
                float3 worldNormal : TEXCOORD1; // World normal for lighting calculations
            };

            Vout vert(Vin v)
            {
                Vout o;
                o.pos = UnityObjectToClipPos(v.vertex); // Transform vertex position to clip space
                o.uv = v.uv; // Pass UV coordinates
                o.worldNormal = normalize(mul(v.normal, (float3x3)unity_WorldToObject)); // Transform normal to world space
                return o; // Return output to fragment shader
            }

            float4 frag(Vout i) : SV_Target
            {
                // Direction to the light source
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz); 
                
                // Calculate diffuse lighting
                float diff = max(0.0, dot(i.worldNormal, lightDir)); 

                // Sample the texture
                float4 texColor = tex2D(_MainTex, i.uv); 

                // Calculate luminance and create a grayscale color
                float grayValue = dot(texColor.rgb, float3(0.299, 0.587, 0.114)); 
                float4 desaturatedColor = float4(grayValue, grayValue, grayValue, texColor.a); 

                // Final color based on the toggle
                float4 finalColor;
                if (_ToggleDiffuseWrap < 0.5)
                {
                    // Use desaturated color when toggled on
                    finalColor = diff; 
                }
                else
                {
                    // Use the original texture color when toggled off
                    finalColor = desaturatedColor * diff; 
                }

                return finalColor; // Return final color
            }

            ENDCG
        }
    }
    FallBack "Diffuse"
}
