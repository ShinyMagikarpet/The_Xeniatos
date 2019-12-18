// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/XRay"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", color) = (1, 1, 1, 1)
	}

	SubShader
	{
		Tags {	"RenderType" = "Transparent"
				"Queue" = "Transparent"
				"Xray" = "ColoredOutline"
		}
		ZTest Always
		Zwrite off
		Blend One One

		Pass
		{
			Stencil{
				Ref 0
				Comp NotEqual
				Pass Keep
			}
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 viewDir : TANGENT;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float3 viewDir : TANGENT;
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.viewDir = normalize(_WorldSpaceCameraPos.xyz - mul(unity_ObjectToWorld, v.vertex).xyz);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float ndotv = 1 - dot(i.normal, i.viewDir) * 2;
				ndotv *= 2;
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 col2 = float4(ndotv, ndotv, ndotv, 0);
				fixed4 col3 = fixed4(0, 0, 1, 0);
				return col * col2 + col3 + _Color;// *col2* col3;
			}
			ENDCG
		}

	}

}
