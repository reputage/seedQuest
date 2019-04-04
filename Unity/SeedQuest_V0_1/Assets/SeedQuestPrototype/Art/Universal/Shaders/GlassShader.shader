// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Custom/Glass" {
	Properties{
		_ColorMult("Luminocity", Range(-1,2)) = 1.1
		_Color("Color", Color) = (1,1,1,0.37)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	_Glossiness("Smoothness", Range(0,1)) = 0.973
		_Metallic("Metallic", Range(0,1)) = 0.916
		_Reflection("Reflection Intencity", Range(0.1,10)) = 5.2
		_Saturation("Reflection Saturation", Range(0.6,2.2)) = 1.2
		_DotProduct("Rim effect", Range(-1,1)) = 0.044
		_Fresnel("Fresnel Coefficient", Range(-1,10)) = 5.0
		_Refraction("Refration Index", float) = 0.9
		_Reflectance("Reflectance", Range(-1,1)) = 1.0
	}
		SubShader{
		Tags{
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
	}
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		// take out "fullforwardshadows" and add "alpha:fade"
#pragma surface surf Standard fullforwardshadows alpha:fade

		// Use shader model 3.0 target, to get nicer looking lighting
#pragma target 3.0

		sampler2D _MainTex;
	float _ColorMult;
	float _Reflection;
	float _DotProduct;
	float _Saturation;
	float _Fresnel;
	float _Refraction;
	float _Reflectance;

	struct Input {
		float2 uv_MainTex;
		float3 worldNormal;
		float3 viewDir;
	};

	half _Glossiness;
	half _Metallic;
	fixed4 _Color;
	samplerCUBE _Cube;

	// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
	// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
	// #pragma instancing_options assumeuniformscaling
	UNITY_INSTANCING_BUFFER_START(Props)
		// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf(Input IN, inout SurfaceOutputStandard o) {
		// Albedo comes from a texture tinted by color
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb * _ColorMult;
		// Metallic and smoothness come from slider variables
		o.Metallic = _Metallic * _Saturation;
		o.Smoothness = _Glossiness;

		// Add transparency in the center (if more perpendicular)
		// And more reflections in the side (if more angle).
		float border = 1 - (abs(dot(IN.viewDir,IN.worldNormal)));
		float alpha = (border * (1 - _DotProduct) + _DotProduct);
		o.Alpha = ((c.a * _Reflection)  * alpha);
		o.Emission = c.a;
	}
	ENDCG
	}

		// Fallback to your shader: with a given name
		//Fallback "SimpleTransparent";
}