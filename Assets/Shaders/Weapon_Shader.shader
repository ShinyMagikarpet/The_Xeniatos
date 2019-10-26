Shader "Unlit/Weapon_Shader"
{
    Properties
    {
		[Header(Main Textures)]
        _MainTex ("Texture", 2D) = "white" {}
		_SecondTex ("Texture", 2D) = "white" {}
		_Transition("Transition", Range(0, 100)) = 0

		[Header(Scroll Material Speed)]
		_ScrollSpeedX("Scroll x", Range(-10, 10)) = 0
		_ScrollSpeedY("Scroll y", Range(-10, 10)) = 0

		[Header(Distortion)]
		_Displacement("Displacement Texure", 2D) = "white" {}
		_Magnitude("Magnitude", Range(0, 1)) = 0
		_Invert("Invert Color", Range(0, 100)) = 0
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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
			sampler2D _SecondTex;
			sampler2D _Displacement;

			float _ScrollSpeedX;
			float _ScrollSpeedY;
			float _Transition;
			float _Magnitude;
			float _Invert;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float2 disp = tex2D(_Displacement, i.uv).xy;
				disp = ((disp * 2) - 1) * _Magnitude;

				fixed xScroll = _ScrollSpeedX * _Time;
				fixed yScroll = _ScrollSpeedY * _Time;
				fixed2 scrollUV = i.uv + fixed2(xScroll, yScroll);

                fixed4 col = lerp(tex2D(_MainTex, i.uv + disp + scrollUV), tex2D(_SecondTex, i.uv), (_Transition/100));
				col.rgb = abs((_Invert/100) - col.rgb);
                return col;
            }
            ENDCG
        }
    }
}
