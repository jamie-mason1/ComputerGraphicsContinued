Shader "Unlit/EMberShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _Color ("Particle Color", Color) = (1, 0.5, 0, 1) // Default ember color (orange)
        _GlowColor ("Glow Color", Color) = (1, 1, 0, 1) // Yellow glow
        _GlowIntensity ("Glow Intensity", Range(0, 10)) = 2.0
        _TimeScale ("Time Scale", Range(0, 5)) = 1.0
        _Transparency ("Transparency", Range(0, 1)) = 0.5
        _FlickerAmount ("Flicker Amount", Range(0, 1)) = 0.2
        _Speed ("Particle Speed", Range(0, 5)) = 2.0
        _ToggleTextures("Toggle Texture", Range(0,1)) = 1.0
    }
    
    SubShader
    {
        Tags { "RenderType"="Fade" }
        Tags { "Queue" = "Overlay" }
        
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha  // Normal alpha blending
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // Texture and properties
            sampler2D _MainTex;
            float4 _Color;
            float4 _GlowColor;
            float _GlowIntensity;
            float _TimeScale;
            float _Transparency;
            float _FlickerAmount;
            float _Speed;
            float _ToggleTextures;


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                // Sample the base texture
                half4 texColor = tex2D(_MainTex, i.uv);
                
                // Apply the particle color
                texColor *= _Color;
                if(_ToggleTextures<0.5){
                    //if textures are disabled set the texcolor to _Color
                    texColor = _Color;
                }

                

                // Calculate the flicker effect by adding a sine wave modulation
                float flicker = sin(_TimeScale * _Time + i.uv.x * _Speed) * 0.5 + 0.5;
                texColor.rgb += flicker * _FlickerAmount;

                // Apply glowing effect
                float glow = sin(_TimeScale * _Time) * 0.5 + 0.5;
                texColor.rgb += glow * _GlowColor.rgb * _GlowIntensity;

                // Apply transparency (dark pixels should be more transparent)
                texColor.a *= _Transparency;

                return texColor;
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}
