// Upgrade NOTE: replaced 'defined POINT' with 'defined (POINT)'

#if !defined (MY_LIGHTING)
#define MY_LIGHTING

#include "AutoLight.cginc"
#include "UnityPBSLighting.cginc"

			float4 _Tint;
			sampler2D _MainTex;
			float4 _MainTex_ST;

			float _Metallic;
			float _Smoothness;

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

				#if defined (VERTEXLIGHT_ON)
				float3 vertexLight : TEXCOORD3;
				#endif
			};
			
			void ComputerVertexLightColor (Interpolators i) {
				#if defined (VERTEXLIGHT_ON)
				// float3 lightPos = float3 (unity_4LightPosX0.x, unity_4LightPosY0.x, unity_4LightPosZ0.x);
				// float3 lightDir = lightPos - i.worldPos;
				// float ndotl = DotClamped (i.normal, lightDir);
				// float attenuation = 1 / (1 + dot(lightDir, lightDir));
				// i.vertexLight = unity_LightColor[0].rgb * ndotl * attenuation;
				i.vertexLight = Shader4PointLights (
					unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
					unity_LightColor[0].rgb,
					unity_LightColor[1].rgb,
					unity_LightColor[2].rgb,
					unity_LightColor[3].rgb,
					unity_4LightAtten0, i.worldPos, i.normal
					);
				#endif
			}
			Interpolators MyVertexProgram (VertexData v) {
				Interpolators i;
				i.position = UnityObjectToClipPos(v.position);
				i.worldPos = mul(unity_ObjectToWorld, v.position);
				i.normal = UnityObjectToWorldNormal(v.normal);
				i.uv = TRANSFORM_TEX(v.uv, _MainTex);
				ComputerVertexLightColor(i);//calculate vertex light
				return i;
			}

			UnityIndirect CreateIndirectLight (Interpolators i) {
				UnityIndirect indirectLight;
				indirectLight.diffuse = 0;
				indirectLight.specular = 0;

				#if defined (VERTEXLIGHT_ON)
				indirectLight.diffuse = i.vertexLight;
				#endif

				return indirectLight;

			}





			UnityLight CreateLight (Interpolators i) {
				 UnityLight light;
				 // float3 lightVec = _WorldSpaceLightPos0.xyz - i.worldPos;
				 // float attenuation = 1 / (1 + lightVec * lightVec);
				 UNITY_LIGHT_ATTENUATION (attenuation, 0, i.worldPos);
				 light.color =  _LightColor0.rgb * attenuation;
				 #if defined (POINT) || defined (SPOT)
				 light.dir = normalize(_WorldSpaceLightPos0.xyz - i.worldPos);
				 #else
				 light.dir = _WorldSpaceLightPos0.xyz;
				 #endif
				 light.ndotl = DotClamped(i.normal, light.dir);
				 return light;
			}

			float4 MyFragmentProgram (Interpolators i) : SV_TARGET {
				i.normal = normalize(i.normal);
				//float3 lightDir = _WorldSpaceLightPos0.xyz;
				float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

				//float3 lightColor = _LightColor0.rgb;
				float3 albedo = tex2D(_MainTex, i.uv).rgb * _Tint.rgb;

				float3 specularTint;
				float oneMinusReflectivity;
				albedo = DiffuseAndSpecularFromMetallic(
					albedo, _Metallic, specularTint, oneMinusReflectivity
				);

				 // UnityLight light;
				 // light.color = lightColor;
				 // light.dir = lightDir;
				 // light.ndotl = DotClamped(i.normal, lightDir);

				UnityIndirect indirectLight;
				indirectLight.diffuse = 0;
				indirectLight.specular = 0;

				return UNITY_BRDF_PBS(
					albedo, specularTint,
					oneMinusReflectivity, _Smoothness,
					i.normal, viewDir,
					CreateLight(i), CreateIndirectLight (i)
				);
			}
			#endif