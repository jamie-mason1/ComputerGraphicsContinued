Shader "Custom/CombinedLighting"
{
     Properties
    {
        _Color("Color", Color) = (1, 1, 1, 1) // Base color of the object
        _SpecColor("Specular Color", Color) = (1, 1, 1, 1) // Specular color
        _Shininess("Shininess", Range(1, 100)) = 10 // Shininess factor for specular highlights
        _ToggleAmbient("Toggle Ambient", Float) = 1 // Toggle for ambient lighting
        _ToggleSpecular("Toggle Specular", Float) = 1 // Toggle for specular lighting
        _ToggleDiffuse("Toggle Diffuse", Float) = 1 // Toggle for diffuse lighting
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" } // Set the rendering type to opaque
        Tags { "LightMode" = "ForwardBase" } // Tag for forward rendering mode

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // Properties
            uniform float4 _Color; // Base color of the object
            uniform float4 _SpecColor; // Specular color
            uniform float _Shininess; // Shininess factor
            uniform float _ToggleAmbient; // Toggle for ambient lighting
            uniform float _ToggleSpecular; // Toggle for specular lighting
            uniform float _ToggleDiffuse; // Toggle for diffuse lighting
            uniform float4 _LightColor0; // Color of the main light source

            // Vertex input structure
            struct Vin
            {
                float4 vertex : POSITION; // Vertex position
                float3 normal : NORMAL;   // Vertex normal
                float2 uv : TEXCOORD0;   // Texture coordinates
            };

            // Vertex output structure
            struct Vout
            {
                float4 pos : SV_POSITION; // Position in clip space
                float3 normal : TEXCOORD1; // Normal direction
                float3 lightDir : TEXCOORD2; // Light direction
                float3 viewDir : TEXCOORD3; // View direction
                float4 col : COLOR; // Color output for the fragment shader
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

                // Initialize color based on toggles
                float3 diffuseReflection;
                if (_ToggleDiffuse > 0.5)
                {
                    float3 normalDirection = normalize(o.normal); // Ensure normal is normalized
                    diffuseReflection = _LightColor0.xyz * _Color.rgb * max(0.0, dot(normalDirection, o.lightDir)); // Calculate diffuse reflection
                }
                else{
                    diffuseReflection = float3(0, 0, 0);
                }

                o.col = float4(diffuseReflection, 1.0); // Initialize color with diffuse reflection
                return o; // Return output to fragment shader
            }

            // Fragment shader
            float4 frag(Vout i) : SV_Target
            {
                float4 finalColor = i.col; // Start with diffuse color

                // Ambient Lighting
                if (_ToggleAmbient > 0.5)
                {
                    finalColor += _Color * UNITY_LIGHTMODEL_AMBIENT; // Add ambient color
                }

                // Specular Lighting
                if (_ToggleSpecular > 0.5)
                {
                    // Calculate specular reflection
                    float3 reflectDir = reflect(-i.lightDir, i.normal); // Calculate reflection direction
                    float spec = pow(max(dot(reflectDir, i.viewDir), 0.0), _Shininess); // Calculate specular factor

                    // Combine specular color with specular factor
                    finalColor += _SpecColor * spec; // Add specular color
                }

                return finalColor; // Return final color
            }
            ENDCG
        }
    }
    FallBack "Diffuse" // Fallback to default diffuse shader if needed
}
