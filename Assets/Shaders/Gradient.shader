// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Gradient"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color("Color", color) = (1, 1, 1, 1)
		_Invert("Invert Color", Range(0, 1)) = 0
		[Header(Scroll Material Speed)]
		_ScrollSpeedX ("Scroll x", Range(-10, 10)) = 0
		_ScrollSpeedY ("Scroll y", Range(-10, 10)) = 0
		[Header (Distortion)]
		_DisTex("Texture", 2D) = "white" {}
		_Magnitude ("Magnitude", Range(0, 10)) = 0
		_SineWaveX ("Wave X", Range(0, 10)) = 0
		_SineWaveY ("Wave Y", Range(0, 10)) = 0
		_ScrollSinX ("Scroll X", Range(-10, 10)) = 0
		_ScrollSinY ("Scroll Y", Range(-10, 10)) = 0
    }
    SubShader
    {
        Tags { 
				//"RenderType"="Opaque" 
				//"Queue" = "Transparent"
		}
        //LOD 100

        Pass
        {
			//Blend SrcAlpha OneMinusSrcAlpha
			//Blend OneMinusDstColor OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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
			float4 _Color;
			float _Invert;
			float _ScrollSpeedX;
			float _ScrollSpeedY;
			int _Rotate;

			float _Magnitude;
			sampler2D _DisTex;
			float _SineWaveX;
			float _SineWaveY;
			float _ScrollSinX;
			float _ScrollSinY;

            v2f vert (appdata v)
            {
                v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;

                return o;
            }

            float4 frag (v2f i) : SV_Target
            {

				fixed xScroll = _ScrollSpeedX * _Time;
				fixed yScroll = _ScrollSpeedY * _Time;
				fixed2 scrollUV = i.uv + fixed2(xScroll, yScroll);
				float2 uv = i.uv;
				uv.x += sin((uv.y + (_ScrollSinX * _Time)) * _SineWaveX) * 0.3;
				uv.y += cos((uv.x + (_ScrollSinY * _Time)) * _SineWaveY) * 0.3;

				float2 disp = tex2D(_DisTex, i.uv).xy;
				disp = ((disp * 2) - 1) * _Magnitude;



				//float4 color = tex2D(_MainTex, i.uv) * _Color; 
				float4 color = tex2D(_MainTex, scrollUV + disp + uv)  * (_Color); 
				//float4 color = float4(i.uv.x, i.uv.y, 1, 1);
				color.rgb = abs(_Invert - color.rgb); //Inverts the color
				return color;
            }
            ENDCG
        }
    }
}
