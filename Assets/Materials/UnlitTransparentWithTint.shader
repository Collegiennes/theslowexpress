// Unlit alpha-blended shader with tint!
// - no lighting
// - no lightmap support
// - totally coloring support

Shader "Unlit/Transparent (with tint)" 
{
	Properties {
		_Color ("Color Tint", Color) = (1,1,1,1)   
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	}

	SubShader 
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 100
	
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha 

		Pass 
		{
			Lighting Off
			SetTexture [_MainTex] 
			{ 
				ConstantColor [_Color]
				Combine texture * constant
			} 
		}
	}
}