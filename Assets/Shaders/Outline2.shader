Shader "Unlit/Outline2"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", color) = (1, 1, 1, 1)
		_OutlineColor("Outline Color", color) = (1, 1, 1, 1)
		[PerRendererData]_OutlineWidth("Outline Width", Range(1, 5)) = 1
	}
	
    SubShader
    {
		Tags { "RenderType" = "Transparent" }
		LOD 100
        pass
        {
			zwrite off

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
				float4 pos : POSITION;
				float3 normal : NORMAL;
			};

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float4 _OutlineColor;
			float _OutlineWidth;

            v2f vert (appdata v)
            {
				v.vertex.xyz *= _OutlineWidth;

				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o; 
            }

			fixed4 frag(v2f i) : SV_Target
			{
				return _OutlineColor;
			}
            ENDCG
        }

		pass //Object
		{
			zwrite on

			Material
			{
				Diffuse[_Color]
				Ambient[_Color]
			}

			Lighting On

			SetTexture[_MainTex]
			{
				ConstantColor[_Color]
			}

			SetTexture[_MainTex]
			{
				combine previous * primary DOUBLE
			}
		}
    }
}
