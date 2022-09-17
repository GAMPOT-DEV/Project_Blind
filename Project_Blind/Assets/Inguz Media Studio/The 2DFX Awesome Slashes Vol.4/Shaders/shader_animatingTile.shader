// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "WooArt/Animating Texture by Age"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Repeat("Repeat Time", Range(1, 10)) = 1.0
        _Emissive("Emissive", Range(1,5)) = 1.0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType"="Opaque" }
        LOD 100

        Blend One One
        ZWrite Off
        Cull Off
        Lighting Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float3 uv : TEXCOORD0;
            };

            struct v2f
            {
                float3 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Repeat;
            float _Emissive;

            v2f vert (appdata v)
            {
                v2f o;

                float agePercent = v.uv.z;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex) - float2(0, agePercent * _Repeat);

                o.color = v.color;
                o.uv.z = v.uv.z;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
            
                return col * _Emissive * col.a;
            }
            ENDCG
        }
    }
}
