Shader "Custom/Outline + Rim" {
	Properties {
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0

		_OutlineColor("Outline Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_OutlineWidth("Outline Width", Range(0, 2.0)) = 0

		_RimColor("Rim Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_RimPower("Rim Power", Range(0.5, 8.0)) = 2.0
	}
	SubShader {
		Tags{ "Queue" = "Transparent" "RenderType" = "Opaque" }
		LOD 200

		Pass{
		ZWrite Off

		CGPROGRAM

		#pragma vertex vert
		#pragma fragment frag

		struct vertInput {
			float4 pos : POSITION;
			float3 normal : NORMAL;
		};

		struct vertOutput {
			float4 pos : SV_POSITION;
		};

		half4 _OutlineColor;
		float _OutlineWidth;

		vertOutput vert(vertInput input) {
			input.pos.xyz *= _OutlineWidth;
			//input.pos.xyz += input.normal * _OutlineWidth;

			vertOutput o;
			o.pos = UnityObjectToClipPos(input.pos);
			return o;
		}

		half4 frag(vertOutput output) : COLOR{
			return _OutlineColor;
		}

		ENDCG
	}


		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float3 viewDir;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float4 _RimColor;
		float _RimPower;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
		// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf(Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;

			// Rim Effect
			half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
			half rimPower = pow(rim, _RimPower) * ((1.0 + sin(3.0 * _Time.y)) * 0.5);
			o.Emission = _RimColor.rgb * rimPower;

			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
