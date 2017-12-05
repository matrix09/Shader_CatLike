#if !defined (MY_BUMP_NESS_CG)
#define MY_BUMP_NESS_CG

#include "UnityPBSLighting.cginc"
#include "AutoLight.cginc"

sampler2D _MainTex;

float4 _MainTex_ST;

sampler2D _HeightMap;	//高位图
float4 _HeightMap_TexelSize;
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

void InitializeFragmentNormals ( inout Interpolators i) {

	float2 delta = float2(_HeightMap_TexelSize.x * 0.5, 0);
	float h1 = tex2D(_HeightMap, i.uv - delta);
	float h2 = tex2D(_HeightMap, i.uv + delta);

	float2 deltav1 = float2 (0, _HeightMap_TexelSize.y * 0.5);
	float v1 = tex2D(_HeightMap, i.uv - deltav1);
	float v2 = tex2D(_HeightMap, i.uv + deltav1);



	//i.normal = float3(1, h2 - h1, 0);
	i.normal = float3(h1 - h2, 1, v1 - v2);
	
	i.normal = normalize(i.normal);
	// float h = tex2D(_HeightMap, i.uv);
	// i.normal = float3(0, h, 0);
	// i.normal = normalize(i.normal);
}

fixed4 MyFragmentProgram (Interpolators i) : SV_TARGET {
	InitializeFragmentNormals(i);

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

// void InitializeFragmentNormal (inout Interpolators i) {
// 	//-------------------Bump Mapping--------------------------------//
// 		//---- v1
// 		// i.normal = float3(0, 1, 0);
// 		// i.normal = normalize(i.normal);

// 		//---- v2 : 读取高位图
// 		// float h = tex2D(_HeightMap, i.uv);
// 		// i.normal = float3(0, h, 0);
// 		// i.normal = normalize(i.normal);

// 		//---- v3 : 横向读取<相当于对X分量进行了求导 -> 可以理解为去获取切线向量>
// 		// float2 delta = float2(_HeightMap_TexelSize.x, 0);
// 		// float h1 = tex2D(_HeightMap, i.uv);
// 		// float h2 = tex2D(_HeightMap, i.uv + delta);
// 		// i.normal = float3(1, (h2 - h1), 0);
// 		// i.normal = float3(h1 - h2, 1, 0);

// 		//----v4 : 横纵读取
// 		float2 du = float2(_HeightMap_TexelSize.x * 0.5, 0);
// 		float h1 = tex2D(_HeightMap, i.uv - du);
// 		float h2 = tex2D(_HeightMap, i.uv + du);
// 		//i.normal = float3(1, (h2 - h1), 0);
// 		float3 nu = float3(h1 - h2, 1, 0);

// 		du = float2(0, _HeightMap_TexelSize.y * 0.5);
// 		float v1 = tex2D(_HeightMap, i.uv - du);
// 		float v2 = tex2D(_HeightMap, i.uv + du);
// 		float3 nv = float3(0, 1, (v2 - v1));

// 		i.normal = float3(h1 - h2, 1, v1 - v2);


// 	//---------------------------------------------------------------//
	
// 	//-------------------Normal Mapping------------------------------//
// 	// i.normal.xy = tex2D(_NormalMap, i.uv).wy * 2 - 1;
// 	// i.normal.xy *= _BumpScale;
// 	// i.normal.z = sqrt(1 - saturate(dot(i.normal.xy, i.normal.xy)));
// 	// i.normal = i.normal.xzy;
// 	// i.normal = UnpackScaleNormal(tex2D(_NormalMap, i.uv), _BumpScale);
// 	// i.normal = i.normal.xzy;
// 	//--------------------------------------------------------------//
// }