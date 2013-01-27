Shader "Custom/Terrain Mask" 
{
	Properties {
		_MaskTex ("Mask (RGB) Trans (A)", 2D) = "white" {}
	}

	SubShader {
		Tags {"Queue"="Transparent-1" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 200
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha 

      Pass {    
		 Fog { Mode Off }
		 	
         CGPROGRAM
 
         #pragma vertex vert  
         #pragma fragment frag 
		 #include "UnityCG.cginc"
 
         uniform sampler2D _MaskTex;

		 uniform float4 _MaskTex_ST; 
 
         struct vertexInput {
            float4 vertex : POSITION;
            float2 texCoord : TEXCOORD0;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
			float2 maskTc : TEXCOORD0;
         };
 
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
 
            output.pos = mul(UNITY_MATRIX_MVP, input.vertex);

            output.maskTc = _MaskTex_ST.xy * input.texCoord.xy + _MaskTex_ST.zw;

            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR
         {
			float4 maskColor = tex2D(_MaskTex, input.maskTc);
			return float4(maskColor.rgb, maskColor.a);
         }
 
         ENDCG
      }
   } 
}
