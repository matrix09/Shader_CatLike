Shader "Custom/MyBumpNess" {

	Properties {
		_MainTex("Albedo", 2D) = "White"{}
		[NoScaleOffset] _NormalMap ("Normals", 2D) = "gray"{}
		_BumpScale("Bump Scale", Float) = 1
		[Gamma] _Metallic ("Metallic", Range(0, 1)) = 0
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