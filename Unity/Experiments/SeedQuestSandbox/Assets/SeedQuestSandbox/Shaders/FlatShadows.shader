Shader "SeedQuest/FlatShadows" 
{
	Properties 
	{
		_Color("Color", Color) = (0, 0, 0, 1)
		_MainTex("Texture", 2D) = "white" {}
		[HDR] _Emission("Emission", color) = (0,0,0)

		_Ramp("Toon Ramp", 2D) = "white" {}
	}

	SubShader 
	{
		Tags{ "RenderType" = "Opaque" "Queue" = "Geometry" }

		CGPROGRAM

		#pragma surface surf Custom fullforwardShadows
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _Ramp;
		fixed4 _Color;
		half3 _Emission;

		struct Input {
			float2 uv_MainTex;
		};

		float4 LightingCustom(SurfaceOutput s, float3 lightDir, float atten) {

			//how much does the normal point towards the light?
			float towardsLight = dot(s.Normal, lightDir);

			//remap the value from -1 to 1 to between 0 and 1
			towardsLight = towardsLight * 0.5 + 0.5;

			//read from toon ramp
			float3 lightIntensity = tex2D(_Ramp, towardsLight).rgb;

			float4 col;
			
			// Color with Diffuse Ramp Intensity, Diffuse Color, Light Falloff and Shadowcasting, Color of the Light
			//col.rgb = lightIntensity * s.Albedo * atten * _LightColor0.rgb;

			// ColorFlat with Light Falloff, Shadowcasting, and Color of the Light
			col.rgb = s.Albedo * atten * _LightColor0.rgb;

			//in case we want to make the shader transparent in the future - irrelevant right now
			col.a = s.Alpha;

			return col;
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