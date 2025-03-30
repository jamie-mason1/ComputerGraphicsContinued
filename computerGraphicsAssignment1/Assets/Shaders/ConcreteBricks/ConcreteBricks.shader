Shader "Custom/ConcreteBricks"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _BumpMap ("Normal Map", 2D) = "bump" {}
        _BumpScale ("Bump Scale", Range(0,1)) = 0.5
        _RoughnessMap ("Roughness Map (G)", 2D) = "white" {}
        _HeightMap ("Height Map", 2D) = "white" {}
        _HeightScale ("Height Scale", Range(0, 0.1)) = 0.02
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _TwoSided ("Enable Two-Sided Rendering", Float) = 0
        _ToggleTextures ("Toggle Texture", Range(0,1)) = 0.5
        _GlowIntensity ("Glow Intensity", Range(0,5)) = 1.0
        _GlowColour("Glow Colour", Color) = (1,0.3,0,1)
        _GlowMask ("Glow Mask (Texture)", 2D) = "white" {}
        _VectorBounds ("Glow Bounds Centre", Vector) = (0, 0, 0) // Center and range for glow effect
        _VectorRange("Glow Bounds Range",Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Cull [_TwoSided] // Automatically switch between backface culling and none

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BumpMap;
        sampler2D _RoughnessMap;
        sampler2D _HeightMap;
        sampler2D _GlowMask;

        float4 _Color;
        half _Glossiness;
        half _Metallic;
        float _BumpScale;
        float _HeightScale;
        float _ToggleTextures;
        float _GlowIntensity;
        float4 _GlowColour;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
            float2 uv_RoughnessMap;
            float2 uv_HeightMap;
            float2 uv_GlowMask;
            float3 viewDir; // Required for parallax mapping
        };

        // Function to calculate parallax UV offset
        float2 ParallaxOffset(float2 uv, float height, float3 viewDir)
        {
            float depth = height * _HeightScale;
            float3 viewTangent = normalize(viewDir);
            float parallax = depth * -viewTangent.z; // Only adjust along view direction
            return uv + parallax * viewTangent.xy;
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            // Parallax mapping for height map
            if (_ToggleTextures >= 0.5)
            {
                float height = tex2D(_HeightMap, IN.uv_HeightMap).r;
                float2 parallaxUV = ParallaxOffset(IN.uv_MainTex, height, IN.viewDir);

                // Albedo texture with color tint
                fixed4 c = tex2D(_MainTex, parallaxUV) * _Color;
                o.Albedo = c.rgb;

                // Roughness map modifies smoothness
                float roughness = tex2D(_RoughnessMap, IN.uv_RoughnessMap).g;
                o.Smoothness = lerp(_Glossiness, roughness, roughness);

                // Normal map
                o.Normal = UnpackNormal(tex2D(_BumpMap, parallaxUV)) * _BumpScale;
                o.Alpha = c.a;
            }
            else
            {
                o.Albedo = _Color.rgb;
                o.Alpha = _Color.a;
            }

            // Metallic value
            o.Metallic = _Metallic;
        }
        ENDCG

        // Glow Pass - Additive blending for molten glow effect based on location
        Pass {
            Name "Glow"
            Tags { "LightMode"="Always" }
            Blend One One // Additive blending
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float4 _GlowColour;
            float _GlowIntensity;
            sampler2D _GlowMask;
            float3 _VectorBounds; // Center and Range for glow effect
            float _VectorRange;

            struct Vin
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Vout
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            Vout vert(Vin i)
            {
                Vout o;
                o.pos = UnityObjectToClipPos(i.vertex);
                o.uv = i.uv;
                o.worldPos = mul(unity_ObjectToWorld, i.vertex).xyz;
                return o;
            }

            fixed4 frag(Vout i) : SV_Target
            {
                // Calculate distance from glow center
                float dist = length(i.worldPos - _VectorBounds);

                // Use smoothstep for a smooth falloff
                float glowMask = smoothstep(_VectorRange, _VectorRange * 0.8, dist); // Fade at range edges
                glowMask = 1.0 - glowMask; // Invert for glow falloff from center

                // Sample glow mask texture (adds details like patterns or noise)
                float glowFactor = tex2D(_GlowMask, i.uv).r;

                // Calculate final glow color
                fixed4 glow = _GlowColour * glowFactor * _GlowIntensity * glowMask;
                return glow;
            }
            ENDCG
        }

    }

    FallBack "Diffuse"
}
