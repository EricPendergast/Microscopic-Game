// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Background Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members vertex)
#pragma exclude_renderers d3d11
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            //#pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                //UNITY_FOG_COORDS(1)
                float4 screen_vertex : SV_POSITION;
                float4 world_position : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.screen_vertex = UnityObjectToClipPos(v.vertex);
                //o.screen_vertex = v.vertex;
                o.world_position = mul(UNITY_MATRIX_M, float4(v.vertex));
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }



            float fract(float f) {
                return f - floor(f);
            }

            float rand3D(float3 co){
                return fract(sin(dot(co.xyz, float3(12.9898,78.233,144.7272))) * 43758.5453);
            }

            float rand2D(float2 co){
                return fract(sin(dot(co.xy, float2(12.9898,78.233))) * 43758.5453);
            }

            float getCircleNoise(float2 pos) {
                float2 middleCell = floor(pos);
                float ringsSum = 0;

                for (int x = -1; x <= 1; x++) {
                    for (int y = -1; y <= 1; y++) {

                        float2 cell = middleCell + float2(x, y);
                        float2 circleCenter = float2(rand3D(float3(cell, 1)), rand3D(float3(cell, 2)));

                        float dist = distance(pos, cell + circleCenter);

                        ringsSum += clamp(-pow(50*dist-20, 2)+1, 0, 1);
                    }
                }

                return ringsSum;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return getCircleNoise(i.world_position.xy/10) * float4(.25, .25, .25, 1);
                // return float4(abs(i.world_position.x)%1,1,0,1);
            }


            float interpolate(float a, float w, float b, float p){
	            float t = p/w;
	            return (b-a)*t*t*t*(6*t*t-15*t+10) + a;
            }

            ENDCG
        }
    }
}
