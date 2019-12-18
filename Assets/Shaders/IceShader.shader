Shader "Unlit/IceShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (0, 0, 1, 1)
	}

    SubShader
    {
		
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		Blend SrcAlpha One
		Zwrite On
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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
				float3 normal : NORMAL;
            };

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.vertex.y += 1;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.normal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

			fixed4 frag(v2f i) : SV_Target
			{
				float ndotl = dot(i.normal, _WorldSpaceLightPos0);
				fixed4 col = tex2D(_MainTex, i.uv);
                return col  * fixed4(0.13, 0.745283, 1, 1);
            }
            ENDCG
        }
		
    }
}
