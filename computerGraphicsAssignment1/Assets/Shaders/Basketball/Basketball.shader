Shader "Custom/Basketball"
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
        _Glossiness ("Glossiness", Range(0,1)) = 0.5
        _Specular ("Specular Color", Color) = (1,1,1,1)
        _SpecularIntensity ("Specular Intensity", Range(0,1)) = 0.5

        _TwoSided ("Enable Two-Sided Rendering", Float) = 0
        _ToggleTextures ("Toggle Texture", Range(0,1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Cull [_TwoSided] // Automatically switch between backface culling and none

        CGPROGRAM
        #pragma surface surf StandardSpecular fullforwardshadows

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
        fixed4 _Specular;
        half _Glossiness;
        float _BumpScale;
        float _HeightScale;
        float _ToggleTextures;
        float _SpecularIntensity;

        // Function to calculate parallax UV offset
        float2 ParallaxOffset(float2 uv, float height, float3 viewDir)
        {
            float depth = height * _HeightScale;
            float3 viewTangent = normalize(viewDir);
            float parallax = depth * -viewTangent.z; // Only adjust along view direction
            return uv + parallax * viewTangent.xy;
        }

        void surf(Input IN, inout SurfaceOutputStandardSpecular o)
        {
            // Parallax mapping for height map
            if (_ToggleTextures >= 0.5)
            {
                float height = tex2D(_HeightMap, IN.uv_HeightMap).r;
                float2 parallaxUV = ParallaxOffset(IN.uv_MainTex, height, IN.viewDir);

                // Albedo texture with color tint
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
                o.Albedo = c.rgb;

                // Roughness map modifies glossiness
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

                // Default specular when textures are disabled
                o.Smoothness = _Glossiness;
            }
            o.Specular = _Specular.rgb * _SpecularIntensity;

        }
        ENDCG
    }

    FallBack "Diffuse"
}
