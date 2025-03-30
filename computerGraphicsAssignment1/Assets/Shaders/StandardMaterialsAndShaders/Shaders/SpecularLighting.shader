Shader "Custom/SpecularLighting"
{
    Properties
    {
        _Color("Color", Color) = (1, 1, 1, 1) // Base color of the object (for ambient, if any)
        _SpecColor("Specular Color", Color) = (1, 1, 1, 1) // Specular color
        _Shininess("Shininess", Range(1, 100)) = 10 // Shininess factor for specular highlights
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" } // Set the rendering type to opaque

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // Properties
            uniform float4 _Color; // Base color of the object
            uniform float4 _SpecColor; // Specular color
            uniform float _Shininess; // Shininess factor

            // Vertex input structure
            struct Vin
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            // Vertex output structure
            struct Vout
            {
                float4 pos : SV_POSITION;
                float3 normal : TEXCOORD1;
                float3 lightDir : TEXCOORD2;
                float3 viewDir : TEXCOORD3;
            };

            // Vertex shader
            Vout vert(Vin v)
            {
                Vout o;
                o.pos = UnityObjectToClipPos(v.vertex); // Transform vertex position to clip space

                // Transform normal to world space
                o.normal = normalize(mul(v.normal, (float3x3)unity_WorldToObject));
                
                // Get light direction
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz; // Transform vertex to world space
                float3 lightPos = _WorldSpaceLightPos0.xyz; // Get light position
                o.lightDir = normalize(lightPos - worldPos); // Calculate light direction
                
                // Calculate view direction
                float3 cameraPos = _WorldSpaceCameraPos.xyz; // Get camera position
                o.viewDir = normalize(cameraPos - worldPos); // Calculate view direction
                
                return o; // Return output to fragment shader
            }

            // Fragment shader
            float4 frag(Vout i) : SV_Target
            {
                // Calculate specular reflection
                float3 reflectDir = reflect(-i.lightDir, i.normal); // Calculate reflection direction
                float spec = pow(max(dot(reflectDir, i.viewDir), 0.0), _Shininess); // Calculate specular factor

                // Final color is based on specular color and intensity
                float4 specularColor = _SpecColor * spec; // Combine specular color with specular factor
                
                return float4(specularColor.rgb, 1.0); // Return only specular color (with alpha set to 1.0)
            }
            ENDCG
        }
    }
    FallBack "Diffuse" // Fallback to default diffuse shader if needed
}
