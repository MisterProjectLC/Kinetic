Shader "Amazing Assets/Lowpoly Shader/Simple Unlit"
{
    Properties
    {
        [NoScaleOffset]_MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 color : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;

            [maxvertexcount(3)]
            void geom(triangle v2f input[3], inout TriangleStream<v2f> triStream)
            {
                fixed4 lowpolyColor = (input[0].color + input[1].color + input[2].color) / 3.0;
	
	            input[0].color = lowpolyColor;
	            input[1].color = lowpolyColor;
	            input[2].color = lowpolyColor;


                triStream.Append( input[0] );
	            triStream.Append( input[1] );
                triStream.Append( input[2] );

	            triStream.RestartStrip();
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = tex2Dlod(_MainTex, float4(v.uv, 0, 0));
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = i.color;

                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
