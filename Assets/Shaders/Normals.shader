Shader "Unlit/Normals"
{

    SubShader
    {
        Tags { "RenderType"="Normal" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv.xyz = UnityObjectToWorldNormal(v.normal);
                return o;
            }

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col;
				col.rgb = i.uv.xyz * 0.5 + 0.5;
				return col;
            }
            ENDCG
        }
    }
}
