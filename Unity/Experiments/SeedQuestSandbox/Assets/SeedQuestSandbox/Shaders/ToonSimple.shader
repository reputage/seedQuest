Shader "SeedQuest/ToonSimple"
{
	Properties
	{
		[Header(Base Parameters)]
		_Color("Tint", Color) = (0, 0, 0, 1)
		_MainTex("Texture", 2D) = "white" {}
		[HDR] _Emission("Emission", color) = (0,0,0)

		[Header(Lighting Parameters)]
		_ShadowTint ("Shadow Color", Color) = (0, 0, 0, 1)
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque" "Queue" = "Geometry" }

		CGPROGRAM

		#pragma surface surf SimpleToon fullforwardShadows
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _Ramp;
		fixed4 _Color;
		half3 _Emission;

		float3 _ShadowTint;

		struct Input {
			float2 uv_MainTex;
		};
		
		float4 LightingSimpleToon(SurfaceOutput s, float3 lightDir, half3 viewDir, float shadowAttenuation) {

			//how much does the normal point towards the light?
			float towardsLight = dot(s.Normal, lightDir);
			float towardsLightChange = fwidth(towardsLight);
			float lightIntensity = smoothstep(0, towardsLightChange, towardsLight);

			// For hard shadows
			#ifdef USING_DIRECTIONAL_LIGHT
				float attenuationChange = fwidth(shadowAttenuation) * 0.5;
				float shadow = smoothstep(0.5 - attenuationChange, 0.5 + attenuationChange, shadowAttenuation);
			#else // For point lights
				float attenuationChange = fwidth(shadowAttenuation);
				float shadow = smoothstep(0, attenuationChange, shadowAttenuation);
			#endif
			
			lightIntensity = lightIntensity * shadow;

			//calculate shadow color and mix light and shadow based on the light. Then taint it based on the light color
			float3 shadowColor = s.Albedo * _ShadowTint;
			float4 color;
			color.rgb = lerp(shadowColor, s.Albedo, lightIntensity) * _LightColor0.rgb;
			color.a = s.Alpha;
			return color;
		}

		void surf(Input i, inout SurfaceOutput o) {
			fixed4 col = tex2D(_MainTex, i.uv_MainTex);
			col *= _Color;
			o.Albedo = col.rgb;

			o.Emission = _Emission;
		}

		ENDCG
	}

	FallBack "Standard"
}