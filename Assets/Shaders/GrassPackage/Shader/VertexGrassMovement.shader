Shader "Unlit/VertexGrassMovement"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _HeightModifier ("Height Modifier", Float) = 1
        _WindSpeed ("Wind Speed", Float) = 1
        _WindStrength ("Wind Strength", Float) = 1
        _UVScale ("UV Scale", Float) = 0
        _UVScale2 ("UV Scale2", Float) = 1
    }
    SubShader
    {
        Tags{ "RenderType"="opaque"}
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityPBSLighting.cginc"
            #include "AutoLight.cginc"

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
            float4 _MainTex_ST;
            float4 _Color;
            float _UVScale, _UVScale2, _HeightModifier, _WindSpeed, _WindStrength;

            v2f vert (appdata v)
            {
                v2f o;
                float3 startingPosition = v.vertex.xyz;
                float height = v.uv.y * _HeightModifier;
                float grassMovement = sin(_Time.y * _WindSpeed) * _WindStrength * height;
                startingPosition.x += grassMovement;
                o.vertex = UnityObjectToClipPos(float4(startingPosition, 1.0));
                o.uv = v.uv;
                return o;
            }
            
            float4 frag (v2f i) : SV_Target
            {
                float3 tex = tex2D(_MainTex, i.uv);
                float3 fixingUVY = i.uv.y * _UVScale2;
                float3 fixedColor = _Color * (fixingUVY + _UVScale);
                float3 albedo = tex * fixedColor;
                float luminance = dot(tex, float3(0.299, 0.587, 0.114));
                clip(luminance - 0.01); 
                return float4(albedo, 1);
            }
            ENDCG
        }
    }
}
