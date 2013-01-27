Shader "Custom/Invertable Opaque With Fog" 
{
	Properties 
	{
		_InvertFactor ("Invert Factor", Range (0, 1)) = 0
		_Saturation ("Saturation", Range (0, 1)) = 0
		_Color ("Color Tint", Color) = (1,1,1,1)   
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	}

	SubShader 
	{
		Tags { "RenderType" = "Opaque" }
	
		Pass 
		{
			CGPROGRAM
 
			#pragma vertex vert  
			#pragma fragment frag 
			#include "UnityCG.cginc"
 
			uniform sampler2D _MainTex;
			uniform float4 _Color;
			uniform float4 _MainTex_ST; 
			uniform float _InvertFactor;
			uniform float _Saturation;

			const float3 LuminanceTransform = float3(0.2126f, 0.7152f, 0.0722f);
 
			struct vertexInput 
			{
				float4 vertex : POSITION;
				float2 texCoord : TEXCOORD0;
			};
			struct vertexOutput 
			{
				float4 pos : SV_POSITION;
				float2 texCoord : TEXCOORD0;
			};
 
			vertexOutput vert(vertexInput input) 
			{
				vertexOutput output;
 
				output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
				output.texCoord = _MainTex_ST.xy * input.texCoord.xy + _MainTex_ST.zw;

				return output;
			}
 
			float4 frag(vertexOutput input) : COLOR
			{
				float4 sample = tex2D(_MainTex, input.texCoord);
				float3 color = sample.rgb;

				float3 inverted = 1 - color;
				color = lerp(color, inverted, _InvertFactor);

				float luminance = dot(color, LuminanceTransform);
				color = lerp(color, luminance.xxx, _Saturation);

				return float4(color, sample.a) * _Color;
			}
 
			ENDCG
		}
	}
}
