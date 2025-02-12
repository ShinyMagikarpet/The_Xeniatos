﻿Shader "Unlit/Hologram"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_TintCol("Tint Color", Color) = (1,1,1,1)
		_Transparency("Transparency", range(0.0, 1.0)) = 1.0
		_CutoutThresh("Cutout Threshold", range(0.0,1.0)) = 0.2
		_Distance("Distance", float) = 1.0
		_Amplitude("Amplitude", float) = 1.0
		_Speed("Speed", float) = 1.0
		_Amount("Amount", range(0.0,1.0)) = 1.0
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType"="Transparent" }
        LOD 100
		
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

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
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float4 _TintCol;
			float _Transparency;
			float _CutoutThresh;

			float _Distance;
			float _Amplitude;
			float _Speed;
			float _Amount;

            v2f vert (appdata v)
            {
                v2f o;
				v.vertex.x += sin(_Time.x * _Speed * v.vertex.z * _Amplitude) * _Distance * _Amount;
				o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) * _TintCol;
				col.a = _Transparency;
				clip(col.g - _CutoutThresh);
                return col;
            }
            ENDCG
        }
    }
}

