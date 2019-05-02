// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Amplify/WorldSpace"
{
	Properties
	{
		_TopColor("TopColor", Color) = (0,0,0,0)
		_Top_Diffuse("Top_Diffuse", 2D) = "white" {}
		_TopNormalScale("TopNormalScale", Float) = 0
		[Normal]_Top_Normal("Top_Normal", 2D) = "bump" {}
		_Top_Spec_Color("Top_Spec_Color", Color) = (0,0,0,0)
		_Top_Spec("Top_Spec", 2D) = "white" {}
		_NormalPower("NormalPower", Range( 0 , 2)) = 0.13
		_Base_Color("Base_Color", Color) = (0,0,0,0)
		_Base_Diffuse("Base_Diffuse", 2D) = "white" {}
		_BaseNormalScale("BaseNormalScale", Float) = 0
		[Normal]_Base_Normal("Base_Normal", 2D) = "bump" {}
		_Base_Spec_Color("Base_Spec_Color", Color) = (0,0,0,0)
		_Base_Spec("Base_Spec", 2D) = "white" {}
		_FresnelColor("FresnelColor", Color) = (0,0,0,0)
		_Fres_Power("Fres_Power", Float) = 0
		_Fres_Scale_Power("Fres_Scale_Power", Float) = 0
		_Fres_Bias_Power("Fres_Bias_Power", Float) = 0
		_Emissions("Emissions", 2D) = "white" {}
		_EmissionPower("EmissionPower", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		ZTest LEqual
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldNormal;
			INTERNAL_DATA
			float3 worldPos;
		};

		uniform float _BaseNormalScale;
		uniform sampler2D _Base_Normal;
		uniform float4 _Base_Normal_ST;
		uniform float _TopNormalScale;
		uniform sampler2D _Top_Normal;
		uniform float4 _Top_Normal_ST;
		uniform float _NormalPower;
		uniform sampler2D _Base_Diffuse;
		uniform float4 _Base_Diffuse_ST;
		uniform float4 _Base_Color;
		uniform sampler2D _Top_Diffuse;
		uniform float4 _Top_Diffuse_ST;
		uniform float4 _TopColor;
		uniform float4 _FresnelColor;
		uniform float _Fres_Bias_Power;
		uniform float _Fres_Scale_Power;
		uniform float _Fres_Power;
		uniform sampler2D _Emissions;
		uniform float4 _Emissions_ST;
		uniform float _EmissionPower;
		uniform sampler2D _Base_Spec;
		uniform float4 _Base_Spec_ST;
		uniform float4 _Base_Spec_Color;
		uniform sampler2D _Top_Spec;
		uniform float4 _Top_Spec_ST;
		uniform float4 _Top_Spec_Color;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Base_Normal = i.uv_texcoord * _Base_Normal_ST.xy + _Base_Normal_ST.zw;
			float2 uv_Top_Normal = i.uv_texcoord * _Top_Normal_ST.xy + _Top_Normal_ST.zw;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_normWorldNormal = normalize( ase_worldNormal );
			float blendOpSrc70 = ( ( ase_normWorldNormal.y * _NormalPower ) * _NormalPower );
			float blendOpDest70 = ( ase_normWorldNormal.x * ase_normWorldNormal.z );
			float3 lerpResult15 = lerp( UnpackScaleNormal( tex2D( _Base_Normal, uv_Base_Normal ), _BaseNormalScale ) , UnpackScaleNormal( tex2D( _Top_Normal, uv_Top_Normal ), _TopNormalScale ) , (( blendOpSrc70 > 0.5 ) ? max( blendOpDest70, 2.0 * ( blendOpSrc70 - 0.5 ) ) : min( blendOpDest70, 2.0 * blendOpSrc70 ) ));
			o.Normal = lerpResult15;
			float2 uv_Base_Diffuse = i.uv_texcoord * _Base_Diffuse_ST.xy + _Base_Diffuse_ST.zw;
			float2 uv_Top_Diffuse = i.uv_texcoord * _Top_Diffuse_ST.xy + _Top_Diffuse_ST.zw;
			float3 newWorldNormal19 = (WorldNormalVector( i , lerpResult15 ));
			float4 lerpResult10 = lerp( ( tex2D( _Base_Diffuse, uv_Base_Diffuse ) * _Base_Color ) , ( tex2D( _Top_Diffuse, uv_Top_Diffuse ) * _TopColor ) , ( newWorldNormal19.y * _NormalPower ));
			o.Albedo = lerpResult10.rgb;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float fresnelNdotV47 = dot( newWorldNormal19, ase_worldViewDir );
			float fresnelNode47 = ( _Fres_Bias_Power + _Fres_Scale_Power * pow( 1.0 - fresnelNdotV47, _Fres_Power ) );
			float2 uv_Emissions = i.uv_texcoord * _Emissions_ST.xy + _Emissions_ST.zw;
			float4 temp_cast_1 = (_EmissionPower).xxxx;
			float4 lerpResult72 = lerp( tex2D( _Emissions, uv_Emissions ) , temp_cast_1 , newWorldNormal19.y);
			o.Emission = ( ( _FresnelColor * fresnelNode47 ) * lerpResult72 ).rgb;
			float2 uv_Base_Spec = i.uv_texcoord * _Base_Spec_ST.xy + _Base_Spec_ST.zw;
			float2 uv_Top_Spec = i.uv_texcoord * _Top_Spec_ST.xy + _Top_Spec_ST.zw;
			float4 lerpResult17 = lerp( ( tex2D( _Base_Spec, uv_Base_Spec ) * _Base_Spec_Color ) , ( tex2D( _Top_Spec, uv_Top_Spec ) * _Top_Spec_Color ) , newWorldNormal19.y);
			o.Smoothness = lerpResult17.r;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16500
132.5714;-1411;1790;958;3680.531;127.8622;1.695819;True;True
Node;AmplifyShaderEditor.CommentaryNode;56;-2351.613,-143.124;Float;False;1225.563;1119.413;Normals;11;14;15;4;70;69;71;21;20;12;76;77;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-2293.085,163.5315;Float;False;Property;_NormalPower;NormalPower;6;0;Create;True;0;0;False;0;0.13;0.82;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;20;-2285.612,-84.12383;Float;False;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-1977.211,-105.2237;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;69;-1817.954,-36.08936;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;-1818.954,63.91064;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;76;-2174.643,847.2331;Float;False;Property;_TopNormalScale;TopNormalScale;2;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;77;-2166.164,487.72;Float;False;Property;_BaseNormalScale;BaseNormalScale;9;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-1890.634,350.0538;Float;True;Property;_Base_Normal;Base_Normal;10;1;[Normal];Create;True;0;0;False;0;None;57a872298e9438a43a4d8d34e3dfc0b8;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendOpsNode;70;-1567.206,4.218208;Float;True;PinLight;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;14;-1911.618,613.2117;Float;True;Property;_Top_Normal;Top_Normal;3;1;[Normal];Create;True;0;0;False;0;None;57a872298e9438a43a4d8d34e3dfc0b8;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;54;-75.03416,-1128.731;Float;False;1041.828;784.4135;Fresnle;9;62;58;51;50;47;52;48;53;72;;1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;15;-1337.924,340.5024;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;57;-1638.714,-1195.245;Float;False;603.8954;938.1384;Comment;6;28;33;35;27;34;36;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-15.70566,-774.6268;Float;False;Property;_Fres_Bias_Power;Fres_Bias_Power;16;0;Create;True;0;0;False;0;0;0.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-22.00563,-680.5269;Float;False;Property;_Fres_Scale_Power;Fres_Scale_Power;15;0;Create;True;0;0;False;0;0;0.55;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;55;-685.8342,381.6859;Float;False;877.5544;899.9091;Roughness;7;37;29;38;30;39;40;17;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;48;24.86219,-593.148;Float;False;Property;_Fres_Power;Fres_Power;14;0;Create;True;0;0;False;0;0;6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;19;-1054.797,-117.4306;Float;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;62;381.1696,-446.5258;Float;False;Property;_EmissionPower;EmissionPower;18;0;Create;True;0;0;False;0;0;1.92;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;50;274.5028,-1051.331;Float;False;Property;_FresnelColor;FresnelColor;13;0;Create;True;0;0;False;0;0,0,0,0;0.8490566,0.7648734,0.5807227,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;28;-1491.441,-923.6116;Float;False;Property;_Base_Color;Base_Color;7;0;Create;True;0;0;False;0;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;58;296.9256,-640.1081;Float;True;Property;_Emissions;Emissions;17;0;Create;True;0;0;False;0;None;2945dcc69dfc1f64299a550d11d6dfd4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;30;-557.4515,642.6074;Float;False;Property;_Base_Spec_Color;Base_Spec_Color;11;0;Create;True;0;0;False;0;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;33;-1573.38,-1145.245;Float;True;Property;_Base_Diffuse;Base_Diffuse;8;0;Create;True;0;0;False;0;None;48db6bd2b1275e74c873996e0d582e0d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;38;-634.3004,839.5824;Float;True;Property;_Top_Spec;Top_Spec;5;0;Create;True;0;0;False;0;None;f24c1d2e69a782a489ce6bdfa0dfb2d8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;47;305.442,-826.0262;Float;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;27;-1498.331,-464.1061;Float;False;Property;_TopColor;TopColor;0;0;Create;True;0;0;False;0;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;29;-562.3416,1074.596;Float;False;Property;_Top_Spec_Color;Top_Spec_Color;4;0;Create;True;0;0;False;0;0,0,0,0;0.7264151,0.7264151,0.7264151,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;35;-1563.714,-690.4111;Float;True;Property;_Top_Diffuse;Top_Diffuse;1;0;Create;True;0;0;False;0;None;48db6bd2b1275e74c873996e0d582e0d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;37;-635.8342,431.6859;Float;True;Property;_Base_Spec;Base_Spec;12;0;Create;True;0;0;False;0;None;f24c1d2e69a782a489ce6bdfa0dfb2d8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-278.5401,477.6889;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-1226.82,-582.469;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-232.5365,922.3892;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;598.3898,-1036.021;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-1203.818,-1113.996;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;-723.2554,-93.99866;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;72;669.0459,-581.0894;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;849.2544,-790.6754;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;10;-529.8495,-606.5659;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;17;7.720413,463.0822;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1074.25,-490.0427;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Amplify/WorldSpace;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;3;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;1;False;-1;1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;21;0;20;2
WireConnection;21;1;12;0
WireConnection;69;0;21;0
WireConnection;69;1;12;0
WireConnection;71;0;20;1
WireConnection;71;1;20;3
WireConnection;4;5;77;0
WireConnection;70;0;69;0
WireConnection;70;1;71;0
WireConnection;14;5;76;0
WireConnection;15;0;4;0
WireConnection;15;1;14;0
WireConnection;15;2;70;0
WireConnection;19;0;15;0
WireConnection;47;0;19;0
WireConnection;47;1;52;0
WireConnection;47;2;53;0
WireConnection;47;3;48;0
WireConnection;39;0;37;0
WireConnection;39;1;30;0
WireConnection;36;0;35;0
WireConnection;36;1;27;0
WireConnection;40;0;38;0
WireConnection;40;1;29;0
WireConnection;51;0;50;0
WireConnection;51;1;47;0
WireConnection;34;0;33;0
WireConnection;34;1;28;0
WireConnection;75;0;19;2
WireConnection;75;1;12;0
WireConnection;72;0;58;0
WireConnection;72;1;62;0
WireConnection;72;2;19;2
WireConnection;73;0;51;0
WireConnection;73;1;72;0
WireConnection;10;0;34;0
WireConnection;10;1;36;0
WireConnection;10;2;75;0
WireConnection;17;0;39;0
WireConnection;17;1;40;0
WireConnection;17;2;19;2
WireConnection;0;0;10;0
WireConnection;0;1;15;0
WireConnection;0;2;73;0
WireConnection;0;4;17;0
ASEEND*/
//CHKSM=3257F51D01D18C23B9D9742B7AE869ADE94838EB