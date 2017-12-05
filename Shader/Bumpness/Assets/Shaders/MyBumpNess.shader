Shader "Custom/MyBumpNess" {

	Properties {
		_MainTex("Albedo", 2D) = "White"{}
		[Gamma] _Metallic ("Metallic", Range(0, 1)) = 0
		[NoScaleOffset] _HeightMap("Height Map", 2D) = "gray"{}
		_Smoothness ("Smoothness", Range(0, 1)) = 0.1
	}

	SubShader {
		Pass{
			Tags{
				"LightMode" = "ForwardBase"
			}

			CGPROGRAM

			#pragma target 3.0
			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram

			#include "MyBumpNessCg.cginc"

			ENDCG

		}
	}


}