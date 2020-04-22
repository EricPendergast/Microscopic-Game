// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Background Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        diameter ("Ring diameters", Float) = 1
        thickness ("Ring thicknesses", Float) = .1
        scale ("Overall scale", Float) = 1
        layerTwoSpeed ("Layer Two Speed", float) = 1.5
        checkDistance ("Check distance", Int) = 2
        ringColor ("Color", Color) = (1,1,1,1)
        cameraPosition ("Camera Position", Vector) = (0,0,0,0)
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
            float diameter;
            float thickness;
            int checkDistance;
            float scale;
            float4 ringColor;
            float sh;
            float4 cameraPosition;
            float layerTwoSpeed;

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

            float ringFcn1(float dist) {
                return clamp(-pow(1/thickness*dist-diameter/thickness, 2)+1, 0, 1);
            }
            
            float ringFcn2(float dist) {
                dist -= diameter;
                dist *= 3;
                return clamp(1.5*pow(dist/thickness,4) - pow(dist/thickness, 6) + .5, 0, 1);
            }

            float getCircleNoise(float2 pos) {
                float2 middleCell = floor(pos);
                float ringsSum = 0;

                //int checkDistance = 2;
                for (int x = -checkDistance; x <= checkDistance; x++) {
                    for (int y = -checkDistance; y <= checkDistance; y++) {

                        float2 cell = middleCell + float2(x, y);
                        float2 circleCenter = float2(rand3D(float3(cell, 1)), rand3D(float3(cell, 2)));
                        float dist = distance(pos, cell + circleCenter) - clamp(rand3D(float3(cell, 3)), .1, 1);

                        //float thickness = .4;
                        //float diameter = 1;
                        //float sharpness = 4;
                        ringsSum += pow(ringFcn2(dist), 2);
                    }
                }

                return sqrt(ringsSum);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float circNoise1 = getCircleNoise(i.world_position.xy/scale);
                float circNoise2 = getCircleNoise((i.world_position.xy + cameraPosition*layerTwoSpeed)/(2*scale));
                return sqrt(circNoise1*circNoise1 + circNoise2*circNoise2) * ringColor;
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
