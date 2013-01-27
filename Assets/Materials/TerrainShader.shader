Shader "Custom/Terrain" 
{
	Properties {
		_GridTex ("Grid (RGB) Trans (A)", 2D) = "white" {}
		_MaskTex ("Mask (RGB) Trans (A)", 2D) = "white" {}
	}

	SubShader {
      Pass {    
         Tags { "RenderType" = "Opaque" } 
 
         CGPROGRAM
 
         #pragma vertex vert  
         #pragma fragment frag 
		 #include "UnityCG.cginc"
 
         uniform sampler2D _GridTex;
         uniform sampler2D _MaskTex;

		 uniform float4 _GridTex_ST; 
		 uniform float4 _MaskTex_ST; 
 
         struct vertexInput {
            float4 vertex : POSITION;
            float2 texCoord : TEXCOORD0;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float2 gridTc : TEXCOORD0;
			float2 maskTc : TEXCOORD1;
         };
 
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
 
            output.pos = mul(UNITY_MATRIX_MVP, input.vertex);

            output.gridTc = _GridTex_ST.xy * input.texCoord.xy + _GridTex_ST.zw;
            output.maskTc = _MaskTex_ST.xy * input.texCoord.xy + _MaskTex_ST.zw;

            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR
         {
            float3 gridColor = tex2D(_GridTex, input.gridTc).rgb;    
			float4 maskColor = tex2D(_MaskTex, input.maskTc);

			return float4(lerp(gridColor + maskColor.rgb * 0.25f, maskColor.rgb, maskColor.a), 1);
         }
 
         ENDCG
      }
   } 
}
