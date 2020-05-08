Shader "Unlit/Sharpen"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        texelWidth ("Texel Width", float) = .01
        texelHeight ("Texel Height", float) = .01
        xRadius ("Radius x", int) = 1
        yRadius ("Radius y", int) = 1
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

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float texelWidth;
            float texelHeight;
            float xRadius;
            float yRadius;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 sum = 0;
                float count = 0;
                for (int x = 0; x < xRadius; x++) {
                    sum -= tex2D(_MainTex, i.uv + x*float2(texelWidth, 0));
                    sum -= tex2D(_MainTex, i.uv - x*float2(texelWidth, 0));
                    count += 2;
                }
                for (int y = 0; y < yRadius; y++) {
                    sum -= tex2D(_MainTex, i.uv + y*float2(0, texelHeight));
                    sum -= tex2D(_MainTex, i.uv - y*float2(0, texelHeight));
                    count += 2;
                }
                
                return sum + (count+1)*tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}
