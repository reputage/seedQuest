// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Amplify/Grass"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_Color0("Color 0", Color) = (1,1,1,1)
		_Diffuse_CutoutA("Diffuse_Cutout(A)", 2D) = "white" {}
		_WindPower("WindPower", Float) = 0
		_Freq_Power("Freq_Power", Float) = 0
		_Ampl_Power("Ampl_Power", Float) = 0
		_Wind_Sample("Wind_Sample", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _WindPower;
		uniform float _Freq_Power;
		uniform sampler2D _Wind_Sample;
		uniform float4 _Wind_Sample_ST;
		uniform float _Ampl_Power;
		uniform float4 _Color0;
		uniform sampler2D _Diffuse_CutoutA;
		uniform float4 _Diffuse_CutoutA_ST;
		uniform float _Cutoff = 0.5;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float4 temp_cast_0 = (_WindPower).xxxx;
			float4 transform8 = mul(unity_WorldToObject,temp_cast_0);
			float3 temp_cast_1 = (transform8.y).xxx;
			float3 break3_g1 = temp_cast_1;
			float2 uv_Wind_Sample = v.texcoord * _Wind_Sample_ST.xy + _Wind_Sample_ST.zw;
			float mulTime4_g1 = _Time.y * 2.0;
			float3 appendResult11_g1 = (float3(break3_g1.x , ( break3_g1.y + ( sin( ( ( break3_g1.x * ( _Freq_Power * tex2Dlod( _Wind_Sample, float4( uv_Wind_Sample, 0, 0.0) ) ).r ) + mulTime4_g1 ) ) * _Ampl_Power ) ) , break3_g1.z));
			v.vertex.xyz += ( appendResult11_g1 * v.color.r );
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_Diffuse_CutoutA = i.uv_texcoord * _Diffuse_CutoutA_ST.xy + _Diffuse_CutoutA_ST.zw;
			float4 tex2DNode2 = tex2D( _Diffuse_CutoutA, uv_Diffuse_CutoutA );
			o.Emission = ( _Color0 * tex2DNode2 ).rgb;
			o.Alpha = 1;
			clip( tex2DNode2.a - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16200
493;-1287;1818;1099;1254.593;350.947;1;True;True
Node;AmplifyShaderEditor.RangedFloatNode;7;-1301.249,318.7943;Float;False;Property;_WindPower;WindPower;3;0;Create;True;0;0;False;0;0;0.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-1082.046,476.2941;Float;False;Property;_Freq_Power;Freq_Power;4;0;Create;True;0;0;False;0;0;80;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;14;-1203.914,592.6062;Float;True;Property;_Wind_Sample;Wind_Sample;6;0;Create;True;0;0;False;0;None;bdbe94d7623ec3940947b62544306f1c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldToObjectTransfNode;8;-979.046,273.0946;Float;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;13;-1104.446,836.694;Float;False;Property;_Ampl_Power;Ampl_Power;5;0;Create;True;0;0;False;0;0;0.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-816.9143,482.6062;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;1;-635.8474,-187.1055;Float;False;Property;_Color0;Color 0;1;0;Create;True;0;0;False;0;1,1,1,1;0.4343661,0.6886792,0.2306424,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;5;-426.346,579.9946;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;4;-477.847,303.8954;Float;True;Waving Vertex;-1;;1;872b3757863bb794c96291ceeebfb188;0;3;1;FLOAT3;0,0,0;False;12;FLOAT;0;False;13;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;2;-677.8455,12.59455;Float;True;Property;_Diffuse_CutoutA;Diffuse_Cutout(A);2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-321.2467,-131.2055;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-134.5928,312.053;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;11;193.7001,-115.7;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Amplify/Grass;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;8;0;7;0
WireConnection;15;0;12;0
WireConnection;15;1;14;0
WireConnection;4;1;8;2
WireConnection;4;12;15;0
WireConnection;4;13;13;0
WireConnection;3;0;1;0
WireConnection;3;1;2;0
WireConnection;18;0;4;0
WireConnection;18;1;5;1
WireConnection;11;2;3;0
WireConnection;11;10;2;4
WireConnection;11;11;18;0
ASEEND*/
//CHKSM=E0E823A6C219042E2486A1520A4C3A270AD453F9