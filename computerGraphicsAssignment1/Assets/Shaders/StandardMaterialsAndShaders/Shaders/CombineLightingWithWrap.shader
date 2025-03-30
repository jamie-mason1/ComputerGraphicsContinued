Shader "Custom/CombineLightingWithWrap"
{
    Properties
    {
        _Color("Color", Color) = (1, 1, 1, 1)
        _SpecColor("Specular Color", Color) = (1, 1, 1, 1)
        _Shininess("Shininess", Range(1, 100)) = 10
        _ToggleAmbient("Toggle Ambient", Range(0, 1)) = 1
        _ToggleSpecular("Toggle Specular", Range(0, 1)) = 1
        _ToggleDiffuse("Toggle Diffuse", Range(0, 1)) = 1
        _ToggleDiffuseWrap("Toggle Diffuse Wrap", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        Pass
        {
            Tags { "LightMode" = "Always" }
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            
            sampler2D _MainTex;
            float4 _Color;
            float _Shininess;
            float _ToggleAmbient;
            float _ToggleSpecular;
            float _ToggleDiffuse;
            float _ToggleDiffuseWrap;
    struct appdata_t
    {
        float4 vertex : POSITION;   // Vertex position in object space
        float3 normal : NORMAL;     // Normal direction in object space
        float2 uv : TEXCOORD0;      // UV coordinates for the main texture
    };
            struct Vout
            {
                float4 pos : SV_POSITION;
                float4 color : COLOR;
            };

            Vout vert(appdata_t v)
            {
                Vout o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            float4 frag(Vout i) : SV_Target
            {
                float4 finalColor = tex2D(_MainTex, i.pos.xy); // Sample the screen texture

                // Apply ambient lighting if toggled
                if (_ToggleAmbient > 0.5)
                {
                    finalColor.rgb += _Color.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb;
                }

                // Apply diffuse lighting if toggled
                if (_ToggleDiffuse > 0.5)
                {
                    finalColor.rgb *= _Color.rgb;
                }

                // Apply diffuse wrap effect
                if (_ToggleDiffuseWrap > 0.5)
                {
                    finalColor.rgb = (finalColor.rgb + 0.5) / (1.0 + 0.5);
                }

                // Apply specular lighting if toggled
                if (_ToggleSpecular > 0.5)
                {
                    float3 specular = float3(1, 1, 1); // For simplicity, set specular color
                    finalColor.rgb += specular * pow(max(dot(i.pos.xyz, normalize(i.pos.xyz)), 0.0), _Shininess);
                }

                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
