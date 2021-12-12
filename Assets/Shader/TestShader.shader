// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/TestShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 wpos : TEXCOORD1;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            float normalOffsetWeight;
            float amplitude;

            half _Glossiness;
            half _Metallic;
            fixed4 _Color;
            sampler2D ramp;

            static const float pi = 3.14159265f;

            float c;
            float t;
            int m, n;
            float lxReciprocal;
            float lyReciprocal;

            v2f vert (appdata v)
            {
                v2f o;
                float3 worldPos = mul (unity_ObjectToWorld, v.vertex).xyz;
                float w = c * pi * sqrt(pow(n * lxReciprocal, 2) + pow(m * lyReciprocal, 2));
                float posZ = sin((m * pi * worldPos.x) * lxReciprocal) * sin((n * pi * worldPos.z) * lyReciprocal) * sin(w * _Time.y);
                v.vertex.z = posZ * amplitude * 1;
                worldPos = mul (unity_ObjectToWorld, v.vertex).xyz;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.wpos = worldPos;
                return o;
            }

            float3 frag (v2f i) : SV_Target
            {
                // float offset = 1;
                float gradientScale = 1.8;
                float h = smoothstep(-amplitude * gradientScale, amplitude * gradientScale, i.wpos.y);
                float3 tex = tex2D(ramp, float2(h,.5));
                return tex;
            }
            ENDCG
        }
    }
}
