Shader "Custom/GlobalIlluminationShader"
{
    Properties
    {
        _Color("Color", Color) = (1, 1, 1, 1) // Base color
        _SpecColor("Specular Color", Color) = (1, 1, 1, 1) // Specular color
        _Shininess("Shininess", Range(1, 100)) = 10 // Shininess factor
        _MainTex("Main Texture", 2D) = "white" {} // Base texture
        _BumpMap("Bump Texture", 2D) = "bump" {} // Normal map
        _BumpScale("Bump Scale", Float) = 1.0 // Scale for normal map
        _AmbientLightColor("Ambient Light Color", Color) = (1, 1, 1, 1) // Ambient light color
        _ToggleAmbient("Toggle Ambient", Range(0, 1)) = 1
        _ToggleSpecular("Toggle Specular", Range(0, 1)) = 1
        _ToggleDiffuse("Toggle Diffuse", Range(0, 1)) = 1
        _ToggleDiffuseWrap("Toggle Diffuse Wrap", Range(0, 1)) = 1
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            Tags { "LightMode"="ForwardBase" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _BumpMap;
            float _BumpScale;
            float4 _Color;
            float4 _SpecColor;
            float _Shininess;
            float4 _AmbientLightColor;
            float _ToggleAmbient;
            float _ToggleSpecular;
            float _ToggleDiffuse;
            float _ToggleDiffuseWrap;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float3 lightDir : TEXCOORD2;
                float3 viewDir : TEXCOORD3;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                // Transform normal to world space
                o.normal = normalize(mul(v.normal, (float3x3)unity_ObjectToWorld));

                // Calculate light direction (world space)
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.lightDir = normalize(_WorldSpaceLightPos0.xyz - worldPos);
                o.viewDir = normalize(_WorldSpaceCameraPos.xyz - worldPos);

                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                // Sample main texture
                half4 texColor = tex2D(_MainTex, i.uv) * _Color;

                // Apply normal map
                half3 normal = i.normal;
                if (_BumpScale > 0.0)
                {
                    half4 normalMapSample = tex2D(_BumpMap, i.uv) * 2.0 - 1.0;
                    normalMapSample.xy *= _BumpScale;
                    normal = normalize(normal + float3(normalMapSample.xy, normalMapSample.z));
                }

                // Initialize lighting components
                half4 ambientLight = 0;
                half diff = 0.0;
                half spec = 0.0;

                // Ambient lighting
                if (_ToggleAmbient > 0.5)
                {
                    ambientLight = texColor * _AmbientLightColor;
                }

                // Diffuse lighting
                if (_ToggleDiffuse > 0.5)
                {
                    diff = max(0.0, dot(-normal, normalize(i.lightDir)));
                }

                // Diffuse wrap effect
                if (_ToggleDiffuseWrap > 0.5)
                {
                    float wrapFactor = 0.5; // Wrapping intensity
                    diff = (diff + wrapFactor) / (1.0 + wrapFactor);
                }

                // Specular lighting (Phong model)
                if (_ToggleSpecular > 0.5)
                {
                    half3 reflectDir = reflect(i.lightDir, normal);
                    spec = pow(max(dot(reflectDir, i.viewDir), 0.0), _Shininess);
                }

                // Combine lighting components
                half4 finalColor = ambientLight + texColor * diff + _SpecColor * spec;

                return finalColor;
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}
