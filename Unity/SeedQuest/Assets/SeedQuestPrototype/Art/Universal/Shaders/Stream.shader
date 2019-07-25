Shader "FX/Waterfall" {
	Properties{
		[Space]
	[Header(Water)]
	_TColor("Top Water Tint", Color) = (0,1,1,1)
		_WaterColor("Side Water Tint", Color) = (0,0.6,1,1)
		_BrightNess("Water Brightness", Range(0.5,2)) = 1.2
		[Space]
	[Header(Surface Noise and Movement)]
	_SideNoiseTex("Side Water Texture", 2D) = "white" {}
	_TopNoiseTex("Top Water Texture", 2D) = "white" {}
	_HorSpeed("Horizontal Flow Speed", Range(-4,4)) = 0.14
		_VertSpeed("Vertical Flow Speed", Range(0,10)) = 6.8
		_TopScale("Top Noise Scale", Range(0,1)) = 0.4
		_NoiseScale("Side Noise Scale", Range(0,1)) = 0.04
		[Toggle(VERTEX)] _VERTEX("Use Vertex Colors", Float) = 0

		[Space]
	[Header(Foam)]
	_FoamColor("Foam Tint", Color) = (1,1,1,1)
		_Foam("Edgefoam Width", Range(1,10)) = 2.35
		_TopSpread("Foam Position", Range(0,6)) = 0.05
		_Softness("Foam Softness", Range(0,0.5)) = 0.1
		_EdgeWidth("Foam Width", Range(0,2)) = 0.4

		[Space]
	[Header(Rim Light)]
	[Toggle(RIM)] _RIM("Hard Rim", Float) = 0
		_RimPower("Rim Power", Range(1,20)) = 18
		_RimColor("Rim Color", Color) = (0,0.5,0.25,1)

	}
		SubShader{
		Tags{ "Queue" = "Transparent" }
		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
#pragma surface surf Standard vertex:vert fullforwardshadows keepalpha

		// Use shader model 3.0 target, to get nicer looking lighting
#pragma target 3.0     
#pragma shader_feature VERTEX
#pragma shader_feature RIM

		sampler2D _SideNoiseTex, _TopNoiseTex;

	uniform sampler2D _CameraDepthTexture; //Depth Texture

	struct Input {
		float3 worldNormal; INTERNAL_DATA// world normal built-in value
			float3 worldPos; // world position built-in value
		float3 viewDir;// view direction for rim
		float4 color : COLOR; // vertex colors
		float4 screenPos; // screen position for edgefoam
		float eyeDepth;// depth for edgefoam
	};

	void vert(inout appdata_full v, out Input o)
	{
		UNITY_INITIALIZE_OUTPUT(Input, o);
		COMPUTE_EYEDEPTH(o.eyeDepth);
	}

	fixed4 _FoamColor, _WaterColor, _RimColor,  _TColor;
	fixed _HorSpeed, _TopScale, _TopSpread, _EdgeWidth, _RimPower,_NoiseScale , _VertSpeed;
	float _BrightNess, _Foam, _Softness;

	void surf(Input IN, inout SurfaceOutputStandard o) {

		// get the world normal
		float3 worldNormal = WorldNormalVector(IN, o.Normal);
		// grab the vertex colors from the model
		float3 vertexColors = IN.color.rgb;
		// normal for triplanar mapping
		float3 blendNormal = saturate(pow(worldNormal * 1.4,4));


#if VERTEX // use vertex colors for flow
		float3 flowDir = (vertexColors * 2.0f) - 1.0f;
#else // or world normal
		float3 flowDir = -(worldNormal * 2.0f) - 1.0f;
#endif
		// horizontal flow speed
		flowDir *= _HorSpeed;

		// flowmap blend timings
		float timing = frac(_Time.y * 0.5f + 0.5f);
		float timing2 = frac(_Time.y* 0.5f);
		float timingLerp = abs((0.5f - timing) / 0.5f);

		// move 2 textures at slight different speeds fased on the flowdirection
		half3 topTex1 = tex2D(_TopNoiseTex, IN.worldPos.xz * _TopScale + (flowDir.xz * timing));
		half3 topTex2 = tex2D(_TopNoiseTex, IN.worldPos.xz * _TopScale + (flowDir.xz * timing2));

		// vertical flow speed
		float vertFlow = _Time.y * _VertSpeed;

		// noise sides
		float3 TopFoamNoise = lerp(topTex1, topTex2, timingLerp);
		float3 SideFoamNoiseZ = tex2D(_SideNoiseTex, float2(IN.worldPos.z * 10, IN.worldPos.y + vertFlow) * _NoiseScale);
		float3 SideFoamNoiseX = tex2D(_SideNoiseTex, float2(IN.worldPos.x * 10, IN.worldPos.y + vertFlow) * _NoiseScale);

		// lerped together all sides for noise texture
		float3 noisetexture = SideFoamNoiseX;
		noisetexture = lerp(noisetexture, SideFoamNoiseZ, blendNormal.x);
		noisetexture = lerp(noisetexture, TopFoamNoise, blendNormal.y);

		// add noise to normal
		o.Normal *= noisetexture;

		// edge foam calculation
		half depth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture , UNITY_PROJ_COORD(IN.screenPos))); // depth
		half4 foamLine = 1 - saturate(_Foam * float4(noisetexture,1) * (depth - IN.screenPos.w));// foam line by comparing depth and screenposition

																								 // rimline
#if RIM
		int rim = 1.0 - saturate(dot(normalize(IN.viewDir) , o.Normal));
#else
		half rim = 1.0 - saturate(dot(normalize(IN.viewDir) , o.Normal));
#endif
		float3 colorRim = _RimColor.rgb * pow(rim, _RimPower);

		// Normalbased Foam
		float worldNormalDotNoise = dot(o.Normal , worldNormal.y);
		float3 foam = (smoothstep(_TopSpread, _TopSpread + _Softness, worldNormalDotNoise) * smoothstep(worldNormalDotNoise,worldNormalDotNoise + _Softness, _TopSpread + _EdgeWidth));

		// combine depth foam and foam + add color
		float3 combinedFoam = (foam + foamLine.rgb) * _FoamColor;

		// colors lerped over blendnormal
		float4 color = lerp(_WaterColor, _TColor, blendNormal.y) * _BrightNess;
		o.Albedo = color;

		// glowing combined foam and colored rim
		o.Emission = combinedFoam + colorRim;

		// clamped alpha
		o.Alpha = clamp(color.a + combinedFoam + foamLine.a, 0, 1);


	}
	ENDCG
	}
		FallBack "Diffuse"
}