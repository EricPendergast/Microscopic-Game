Shader "Unlit/SimpleBlur"
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
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float texelWidth;
            float texelHeight;
            int xRadius;
            int yRadius;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = 0;
                int count = 0;
                for (int x = -xRadius; x <= xRadius; x++) {
                    for (int y = -yRadius; y <= yRadius; y++) {
                        count++;
                        col += fixed4(tex2D(_MainTex, i.uv + float2(x*texelWidth, y*texelHeight)));
                    }
                }
                return col/count;
            }
            ENDCG
        }
    }
}
