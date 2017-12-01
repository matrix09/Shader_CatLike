﻿#if !defined (MY_BUMP_NESS_CG)
#define MY_BUMP_NESS_CG

#include "UnityPBSLighting.cginc"
#include "AutoLight.cginc"

sampler2D _MainTex;

float4 _MainTex_ST;

float _Metallic;

float _Smoothness;

sampler2D _NormalMap;

float _BumpScale;

struct VertexData {
	float4 position : POSITION;
	float3 normal : NORMAL;
	float2 uv : TEXCOORD0;
};

struct Interpolators {
	float4 position : SV_POSITION;
	float2 uv : TEXCOORD0;
	float3 normal : TEXCOORD1;
	float3 worldPos : TEXCOORD2;

};

Interpolators MyVertexProgram(VertexData v) {

	Interpolators i;
	i.normal = UnityObjectToWorldNormal(v.normal);
	i.position = UnityObjectToClipPos(v.position);
	i.worldPos = mul(unity_ObjectToWorld, v.position);
	i.uv = TRANSFORM_TEX(v.uv, _MainTex);
	return i;

}

void InitializeFragmentNormal (inout Interpolators i) {


	

	//-------------------Normal Mapping------------------------------//
	// i.normal.xy = tex2D(_NormalMap, i.uv).wy * 2 - 1;
	// i.normal.xy *= _BumpScale;
	// i.normal.z = sqrt(1 - saturate(dot(i.normal.xy, i.normal.xy)));
	// i.normal = i.normal.xzy;
	i.normal = UnpackScaleNormal(tex2D(_NormalMap, i.uv), _BumpScale);
	i.normal = i.normal.xzy;
	//--------------------------------------------------------------//

}

UnityLight CreateLight (Interpolators i) {
	UnityLight light;

	#if defined(POINT) || defined(POINT_COOKIE) || defined(SPOT)
		light.dir = normalize(_WorldSpaceLightPos0.xyz - i.worldPos);
	#else
		light.dir = _WorldSpaceLightPos0.xyz;
	#endif
	
	UNITY_LIGHT_ATTENUATION(attenuation, 0, i.worldPos);
	light.color = _LightColor0.rgb * attenuation;
	light.ndotl = DotClamped(i.normal, light.dir);
	return light;
}

UnityIndirect CreateIndirectLight (Interpolators i) {
	UnityIndirect indirectLight;
	indirectLight.diffuse = 0;
	indirectLight.specular = 0;

	return indirectLight;
}

fixed4 MyFragmentProgram (Interpolators i) : SV_TARGET {
	
	InitializeFragmentNormal(i);

	float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

	float3 albedo = tex2D (_MainTex, i.uv).rgb;

	float3 specularTint;
	float oneMinusReflectivity;
	albedo = DiffuseAndSpecularFromMetallic(
		albedo, _Metallic, specularTint, oneMinusReflectivity
	);

	return UNITY_BRDF_PBS(
		albedo, specularTint,
		oneMinusReflectivity, _Smoothness,
		i.normal, viewDir,
		CreateLight(i), CreateIndirectLight(i)
	);
	
}

#endif