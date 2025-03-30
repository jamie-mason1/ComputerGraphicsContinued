Shader "Custom/VolcanicRock"
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

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
            float2 uv_RoughnessMap;
            float2 uv_HeightMap;
            float3 viewDir; // Required for parallax mapping
        };

        fixed4 _Color;
        half _Glossiness;
        half _Metallic;
        float _BumpScale;
        float _HeightScale;
        float _ToggleTextures;

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

            // Alpha channel
        }
        ENDCG
    }

    FallBack "Diffuse"
}
