Shader "Unlit/CellPart"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        radius ("Radiuses", Float) = 1
        thickness ("Outer edge thicknesses", Float) = .1
        cellSize ("Square size", Float) = 1
        innerColor ("Inner color", Color) = (1,1,1,1)
        outerColor ("Outer color", Color) = (.5,.5,.5,1)
        perlinOffset ("Perlin offset", Vector) = (0,0,0)
        perlinScale ("Perlin scale", Range(.01, 3)) = 1
        perlinIntensity ("Perlin intensity", Range(0, 5)) = 1
        blur ("Blur distance", Float) = .01
        overallScale ("Overall scale", Float) = 1
        time ("Time", Float) = 0
        animationSpeed ("Animation speed", Float) = .1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Assets/GLSL/helpers.glsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 screen_vertex : SV_POSITION;
                float4 world_position : TEXCOORD1;
                float4 local_position : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float radius;
            float thickness;
            float cellSize;
            float perlinScale;
            float4 innerColor;
            float4 outerColor;
            float4 perlinOffset;
            float perlinIntensity;
            float blur;
            float overallScale;
            float time;
            float animationSpeed;

            struct CircleParameters {
                float radius;
                float thickness;
                float blur;
            };


            v2f vert (appdata v)
            {
                v2f o;
                o.screen_vertex = UnityObjectToClipPos(v.vertex);
                //o.screen_vertex = v.vertex;
                o.world_position = mul(UNITY_MATRIX_M, float4(v.vertex));
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.local_position = float4(o.uv + rand2D(o.world_position.z, 1), 0, 0);
                /*o.local_position = v.vertex;*/
                return o;
            }

            float4 getCircleNoise(float2 pos, float blur) {
                float2 cell = floor(pos/cellSize)*cellSize;
                float min = radius;
                float max = cellSize-radius;

                float2 circleCenter = clamp(float2(rand(cell, 1), rand(cell, 2))*cellSize, min, max) + cell;
                float3 c = circleFunc(pos - circleCenter, radius, blur, thickness);
                //float2 circleCenter = float2(max, max);
                return innerColor*c[0] + outerColor*c[1] + float4(outerColor.xyz, 0)*c[2];
            }

            float4 wavyCircleNoise(float2 pos, float blur) {
                pos *= overallScale;
                return getCircleNoise(pos + perlinIntensity*naturalPerlin2D(pos*perlinScale + perlinOffset + float2(time, time)*animationSpeed), blur);
            }

            fixed4 frag (v2f i) : SV_Target {
                //TODO: See if model.position works? Or model scale?
                /*i.world_position = float4(i.uv, i.world_position.zw);*/
                float4 circNoise1 = wavyCircleNoise(i.local_position, blur);
                float4 circNoise2 = wavyCircleNoise(i.local_position + float2(500.5, 500.5), blur/2);

                float dist = length(i.uv - float2(.5,.5));
                if (dist > .5) {
                    return fixed4(1,0,0,0);
                } else if (dist > .45) {
                    return outerColor;
                } else {
                    return blend(circNoise1, circNoise2);
                }
                //return (perlin(i.world_position.xy) + perlin(rotate(i.world_position.xy, 3.141/3)))/2 > .5 ? 1 : 0;
                /*return circNoise1;*/
            }
            ENDCG
        }
    }
}
