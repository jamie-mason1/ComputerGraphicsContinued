Shader "Custom/EnhancedTrailEffect"
{
    Properties
    {
        _MainTex ("Trail Texture", 2D) = "white" {}
        _Color ("Trail Color", Color) = (1,1,1,1)
        _FadeStart ("Fade Start", Range(0,1)) = 0.5
        _FadeEnd ("Fade End", Range(0,1)) = 1.0
        _Width ("Trail Width", Float) = 1.0
        _Speed ("Texture Scroll Speed", Float) = 1.0
        _Emission ("Emission Intensity", Float) = 1.0
        _ToggleTextures("Toggle Texture", Range(0,1)) = 1.0
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float dist : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _FadeStart;
            float _FadeEnd;
            float _Width;
            float _Speed;
            float _Emission;
            float _ToggleTextures;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                o.dist = saturate((o.uv.x - _FadeStart) / (_FadeEnd - _FadeStart));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the texture and apply scrolling
                float2 uv = i.uv;
                uv.x += _Speed * _Time.y; // Scrolling effect over time
                fixed4 texColor = tex2D(_MainTex, uv);
                fixed4 color;
                // Apply color gradient and fade
                if(_ToggleTextures >= 0.5){
                    color = lerp(texColor * _Color, float4(0, 0, 0, 0), i.dist);
                }
                else{
                    color = lerp(_Color, float4(0, 0, 0, 0), i.dist);
                }

                // Add glow (emission effect)
                color.rgb += color.rgb * _Emission;

                // Output final color
                return color;
            }
            ENDCG
        }
    }
    FallBack "Transparent/VertexLit"
}
