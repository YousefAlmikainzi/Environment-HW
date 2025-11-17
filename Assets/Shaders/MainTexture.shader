Shader "Unlit/MainTexture"
{
    Properties
    {
        _MainTex    ("Texture",    2D)    = "white" {}
        _Color      ("Color",      Color) = (1,1,1,1)
        _HeightMap  ("Height Map", 2D)    = "gray" {}
        _HeightScale("Height Scale", Float) = 0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex   vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv      : TEXCOORD0;
                float4 vertex  : SV_POSITION;
                float3 normal  : TEXCOORD1;
            };

            sampler2D _MainTex;
            sampler2D _HeightMap;

            float4 _MainTex_ST;
            float4 _Color;
            float  _HeightScale;

            v2f vert (appdata v)
            {
                v2f o;

                float height = tex2Dlod(_HeightMap, float4(v.uv, 0, 0)).r;
                v.vertex.xyz += v.normal * (height * _HeightScale);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv     = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = UnityObjectToWorldNormal(v.normal);

                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                i.normal = normalize(i.normal);

                float3 diffuse = max(dot(float3(0, 1, 0), i.normal), 0);
                float3 col     = tex2D(_MainTex, i.uv).rgb;
                float3 albedo  = col * _Color;
                float3 final   = albedo * diffuse;

                return float4(final, 1);
            }
            ENDCG
        }
    }

    Fallback "Diffuse"
}
