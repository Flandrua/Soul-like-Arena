// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "@Xxuebi/JiG_Add"
{
	Properties
	{
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		[HDR]_Color0("Color 0", Color) = (1,1,1,0)
		_Uv_Speed("Uv_Speed", Vector) = (0,0,0,0)
		[Toggle]_Fresnel_OnOff("Fresnel_On/Off", Float) = 1
		_Fresnel_Bias("Fresnel_Bias", Float) = 0
		_Fresnel_Scale("Fresnel_Scale", Float) = 0
		_Fresnel_Power("Fresnel_Power", Float) = 0
		_Vertex_Offset("Vertex_Offset", Float) = 0
		[Toggle(_VERTEX_OFFSET_ON_OFF_ON)] _Vertex_Offset_On_Off("Vertex_Offset_On_Off", Float) = 0
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma shader_feature _VERTEX_OFFSET_ON_OFF_ON
		#pragma surface surf Unlit alpha:fade keepalpha noshadow vertex:vertexDataFunc 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float4 uv_tex4coord;
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
			float3 worldPos;
			float3 worldNormal;
		};

		uniform sampler2D _TextureSample0;
		uniform float2 _Uv_Speed;
		uniform float4 _TextureSample0_ST;
		uniform float _Vertex_Offset;
		uniform float4 _Color0;
		uniform float _Fresnel_OnOff;
		uniform float _Fresnel_Bias;
		uniform float _Fresnel_Scale;
		uniform float _Fresnel_Power;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 uv0_TextureSample0 = v.texcoord.xy * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float2 panner6 = ( 1.0 * _Time.y * _Uv_Speed + uv0_TextureSample0);
			float4 tex2DNode2 = tex2Dlod( _TextureSample0, float4( panner6, 0, 0.0) );
			float3 ase_vertexNormal = v.normal.xyz;
			#ifdef _VERTEX_OFFSET_ON_OFF_ON
				float3 staticSwitch26 = ( tex2DNode2.r * ase_vertexNormal * _Vertex_Offset );
			#else
				float3 staticSwitch26 = float3( 0,0,0 );
			#endif
			v.vertex.xyz += staticSwitch26;
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv0_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float2 panner6 = ( 1.0 * _Time.y * _Uv_Speed + uv0_TextureSample0);
			float4 tex2DNode2 = tex2D( _TextureSample0, panner6 );
			o.Emission = ( ( i.uv_tex4coord.z + 1.0 ) * ( _Color0 * tex2DNode2.r * i.vertexColor ) ).rgb;
			float temp_output_9_0 = ( tex2DNode2.r * i.vertexColor.a );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV13 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode13 = ( _Fresnel_Bias + _Fresnel_Scale * pow( 1.0 - fresnelNdotV13, _Fresnel_Power ) );
			float clampResult21 = clamp( fresnelNode13 , 0.0 , 1.0 );
			o.Alpha = (( _Fresnel_OnOff )?( ( temp_output_9_0 * clampResult21 ) ):( temp_output_9_0 ));
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17700
352;187;1207;831;484.6502;-86.47392;1.343548;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;8;-1077.901,-170.8834;Inherit;False;0;2;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;7;-993.9006,2.116608;Inherit;False;Property;_Uv_Speed;Uv_Speed;2;0;Create;True;0;0;False;0;0,0;0,-2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;6;-781.1518,-167.5984;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-863.9341,408.7514;Inherit;False;Property;_Fresnel_Scale;Fresnel_Scale;5;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-858.6564,332.2222;Inherit;False;Property;_Fresnel_Bias;Fresnel_Bias;4;0;Create;True;0;0;False;0;0;0.11;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-861.2952,486.6001;Inherit;False;Property;_Fresnel_Power;Fresnel_Power;6;0;Create;True;0;0;False;0;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-570.7,-197.6;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;-1;None;b0730eaea09fe1041af4274f09337ce8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;5;-474.4072,9.441514;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;13;-662.3162,315.1616;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-201.7898,39.55629;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-499.5539,-610.7854;Inherit;False;0;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalVertexDataNode;22;180.4635,441.7883;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;24;108.013,610.6869;Inherit;False;Property;_Vertex_Offset;Vertex_Offset;7;0;Create;True;0;0;False;0;0;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;3;-487.4072,-366.2585;Inherit;False;Property;_Color0;Color 0;1;1;[HDR];Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;21;-254.3078,306.1127;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;44.41129,173.3361;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;11;-195.1928,-457.1048;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-201.4072,-199.8585;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;593.9779,364.4707;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;358.1309,614.6696;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;29;347.3274,858.2069;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;19;181.8371,-2.127116;Inherit;True;Property;_Fresnel_OnOff;Fresnel_On/Off;3;0;Create;True;0;0;False;0;1;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;3.918608,-260.2842;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;26;792.4507,171.1947;Inherit;True;Property;_Vertex_Offset_On_Off;Vertex_Offset_On_Off;8;0;Create;True;0;0;False;0;0;0;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT3;0,0,0;False;0;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;30;-55.63971,812.9834;Inherit;False;0;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1518.977,-279.0178;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;@Xxuebi/JiG_Add;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;6;0;8;0
WireConnection;6;2;7;0
WireConnection;2;1;6;0
WireConnection;13;1;14;0
WireConnection;13;2;15;0
WireConnection;13;3;16;0
WireConnection;9;0;2;1
WireConnection;9;1;5;4
WireConnection;21;0;13;0
WireConnection;20;0;9;0
WireConnection;20;1;21;0
WireConnection;11;0;10;3
WireConnection;4;0;3;0
WireConnection;4;1;2;1
WireConnection;4;2;5;0
WireConnection;23;0;2;1
WireConnection;23;1;22;0
WireConnection;23;2;24;0
WireConnection;27;0;24;0
WireConnection;27;1;30;4
WireConnection;29;0;30;4
WireConnection;19;0;9;0
WireConnection;19;1;20;0
WireConnection;12;0;11;0
WireConnection;12;1;4;0
WireConnection;26;0;23;0
WireConnection;0;2;12;0
WireConnection;0;9;19;0
WireConnection;0;11;26;0
ASEEND*/
//CHKSM=F3777D01E1A3DDF300FDD7197F0F9D26F9F8814A