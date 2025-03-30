Shader "Unlit/FirstColourCorrectionShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LUT("Look Up Table",2D) = "white" {}
        _Contribution("Contribution", Range(0,1)) = 1
        _RedValue("Red", Range(0,1)) = 0
        _GreenValue("Green", Range(0,1)) = 0
        _BlueValue("Blue", Range(0,32)) = 0

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            #define COLORS 32.0

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _LUT;
            float4 _LUT_TexelSize;
            float _Contribution;
            float _RedValue;
            float _GreenValue;
            float _BlueValue;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float maxColor = COLORS - 1.0;
                fixed4 col = saturate(tex2D(_MainTex,i.uv));
                float halfColX = _RedValue / _LUT_TexelSize.z;
                float halfColY = _GreenValue / _LUT_TexelSize.w;
                float threshold = maxColor / COLORS;

                float xOffset = halfColX + col.r * threshold / COLORS;
                float yOffset = halfColY + col.g * threshold;
                float cell = floor(col.b * (maxColor-_BlueValue));
                float2 lutPos = float2(cell/COLORS + xOffset, yOffset);
                float4 gradedCol = tex2D(_LUT, lutPos);
                return lerp(col,gradedCol , _Contribution);
            }
            ENDCG
        }
    }
}
