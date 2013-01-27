// Unlit alpha-blended shader with tint!
// - no lighting
// - no lightmap support
// - totally coloring support

Shader "Unlit/Transparent (no fog)" 
{
	Properties {
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
			Fog { Mode Off }
			SetTexture [_MainTex] 
			{ 
				Combine texture
			} 
		}
	}
}
