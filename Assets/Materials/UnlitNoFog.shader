// Unlit alpha-blended shader with tint!
// - no lighting
// - no lightmap support
// - totally coloring support

Shader "Unlit/Texture (no fog)" 
{
	Properties {
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	}

	SubShader 
	{
		Tags {"RenderType"="Opaque"}
	
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
