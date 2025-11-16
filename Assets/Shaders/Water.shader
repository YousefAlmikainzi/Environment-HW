Shader "Unlit/Water"
{
    Properties
    {
        _DepthMaxDistance ("Max Depth", Float) = 1
        _Color1 ("Color 1", Color) = (0,0,0,0)
        _Color2 ("Color 2", Color) = (1,1,1,1)
        _FoamMaxDistance ("Foam Max Distance", Float) = 1
        _FoamMinDistance ("Foam Min Distance", Float) = 0.4
        _NoiseScale ("Noise Scale", Float) = 5
        _FoamCutOff ("Foam Cutoff", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #define RANDOM1 12.9898
            #define RANDOM2 78.233
            #define RANDOM3 43758.54531213
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 screenPosition : TEXCOORD2;
                float3 viewNormal : NORMAL;
            };

            sampler2D _CameraDepthTexture, _CameraNormalsTexture;
            float4 _Color1, _Color2;
            float _FoamMaxDistance, _FoamMinDistance, _NoiseScale, _FoamCutOff, _DepthMaxDistance;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.viewNormal = COMPUTE_VIEW_NORMAL;
                o.screenPosition = ComputeScreenPos(o.vertex);
                return o;
            }

            float random(float2 st)
            {
                float randomVal = frac(sin(dot(st.xy, float2(RANDOM1, RANDOM2))) * RANDOM3);
                return  randomVal;
            }

            float simple_Noise(float2 st)
            {
                float2 i = floor(st);
                float2 f = frac(st);

                float a = random(i);
                float b = random(i + float2(1.0, 0.0));
                float c = random(i + float2(0.0, 1.0));
                float d = random(i + float2(1.0, 1.0));

                float2 u = f * f * (3.0 - 2.0 * f);
                return lerp(lerp(a, b, u.x), lerp(c, d, u.x), u.y);
            }

            float4 frag (v2f i) : SV_Target
            {
                float2 screenUV = i.screenPosition.xy / i.screenPosition.w;
                float ed = tex2D(_CameraDepthTexture, UNITY_PROJ_COORD(screenUV));
                float edl = LinearEyeDepth(ed);
                float depthDifference = edl - i.screenPosition.w;

                float waterDepthDFifference = saturate(depthDifference/_DepthMaxDistance);
                float4 water = lerp(_Color1, _Color2, waterDepthDFifference);

                float noise = simple_Noise(screenUV * _NoiseScale);
                float3 existingNormal = tex2Dproj(_CameraNormalsTexture, UNITY_PROJ_COORD(i.screenPosition));
                float3 normalDot = saturate(dot(existingNormal, i.viewNormal));
                float foamDistance = lerp(_FoamMaxDistance, _FoamMinDistance, normalDot);
                float foamDepthDiff = saturate(depthDifference/foamDistance);
                float surfaceNoiseCutoff = foamDepthDiff * _FoamCutOff;
                float foamStep = step(surfaceNoiseCutoff, noise);

                float4 final = foamStep + water;
                return final;
            }
            ENDCG
        }
    }
    Fallback "Diffuse"
}
