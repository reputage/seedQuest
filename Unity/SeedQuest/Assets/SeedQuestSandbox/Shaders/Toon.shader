Shader "SeedQuest/Toon"
{
	Properties{
		[Header(Base Parameters)]
		_Color("Color", Color) = (1, 1, 1, 1)
		_MainTex("Texture", 2D) = "white" {}
		_Specular("Specular Color", Color) = (1,1,1,1)
		[HDR] _Emission("Emission", color) = (0 ,0 ,0 , 1)

		[Header(Lighting Parameters)]
		_ShadowTint("Shadow Color", Color) = (0.5, 0.5, 0.5, 1)
		[IntRange]_StepAmount("CelShading Steps", Range(1, 16)) = 2
		_StepWidth("CelShading Step Size", Range(0, 1)) = 0.25
		_SpecularSize("Specular Size", Range(0, 1)) = 0.1
		_SpecularFalloff("Specular Falloff", Range(0, 2)) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque" "Queue" = "Geometry" }

		CGPROGRAM

		#pragma surface surf Toon fullforwardshadows
		#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		half3 _Emission;
		fixed4 _Specular;

		float3 _ShadowTint;
		float _StepWidth;
		float _StepAmount;
		float _SpecularSize;
		float _SpecularFalloff;

		struct ToonSurfaceOutput {
			fixed3 Albedo;
			half3 Emission;
			fixed3 Specular;
			fixed Alpha;
			fixed3 Normal;
		};

		float4 LightingToon(ToonSurfaceOutput s, float3 lightDir, half3 viewDir, float shadowAttenuation) {
			
			// how much does the normal point towards the light?
			float towardsLight = dot(s.Normal, lightDir);

			// stretch values so each whole value is one step
			towardsLight = towardsLight / _StepWidth;
			//make steps harder
			float lightIntensity = floor(towardsLight);

			// smooth the cell shading steps
			float change = fwidth(towardsLight);
			float smoothing = smoothstep(0, change, frac(towardsLight));
			lightIntensity = lightIntensity + smoothing;

			// clamp light intensity range
			lightIntensity = lightIntensity / _StepAmount;
			lightIntensity = saturate(lightIntensity);

			#ifdef USING_DIRECTIONAL_LIGHT
				//for directional lights
				float attenuationChange = fwidth(shadowAttenuation) * 0.5;
				float shadow = smoothstep(0.5 - attenuationChange, 0.5 + attenuationChange, shadowAttenuation);
			#else
				//for other light types (point, spot)
				float attenuationChange = fwidth(shadowAttenuation);
				float shadow = smoothstep(0, attenuationChange, shadowAttenuation);
			#endif

			lightIntensity = lightIntensity * shadow;

			// calculate how much the surface points points towards the reflection direction
			float3 reflectionDirection = reflect(lightDir, s.Normal);
			float towardsReflection = dot(viewDir, -reflectionDirection);

			// make specular highlight all off towards outside of model
			float specularFalloff = dot(viewDir, s.Normal);
			specularFalloff = pow(specularFalloff, _SpecularFalloff);
			towardsReflection = towardsReflection * specularFalloff;

			// make specular intensity with a hard corner
			float specularChange = fwidth(towardsReflection);
			float specularIntensity = smoothstep(1 - _SpecularSize, 1 - _SpecularSize + specularChange, towardsReflection);
			
			// factor inshadows
			specularIntensity = specularIntensity * shadow;

			float4 color;
			//calculate final color
			color.rgb = s.Albedo * lightIntensity * _LightColor0.rgb;
			color.rgb = lerp(color.rgb, s.Specular * _LightColor0.rgb, saturate(specularIntensity));

			color.a = s.Alpha;
			return color;
		}

		struct Input {
			float2 uv_MainTex;
		};

		void surf(Input i, inout ToonSurfaceOutput o) {

			fixed4 col = tex2D(_MainTex, i.uv_MainTex);
			col *= _Color;
			o.Albedo = col.rgb;

			o.Specular = _Specular;

			float3 shadowColor = col.rgb * _ShadowTint;
			o.Emission = _Emission + shadowColor;
		}

		ENDCG
	}

	FallBack "Standard"
}