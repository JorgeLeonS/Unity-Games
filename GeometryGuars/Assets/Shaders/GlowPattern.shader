// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "GlowPattern"
{
	Properties
	{
		[HDR]_BackgroundColor("BackgroundColor", Color) = (0.3018868,0.3018868,0.3018868,1)
		_Pattern("Pattern", 2D) = "white" {}
		[HDR]_PatternColor("PatternColor", Color) = (1,1,1,1)
		[NoScaleOffset]_Multiplier("Multiplier", 2D) = "white" {}
		_Power("Power", Float) = 1
		_PanSpeed("PanSpeed", Vector) = (0.05,0.05,-0.05,-0.05)
		_MultiplierTiling("MultiplierTiling", Vector) = (1,1,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _BackgroundColor;
		uniform float4 _PatternColor;
		uniform sampler2D _Pattern;
		uniform float4 _Pattern_ST;
		uniform sampler2D _Multiplier;
		uniform float4 _PanSpeed;
		uniform float2 _MultiplierTiling;
		uniform float _Power;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_Pattern = i.uv_texcoord * _Pattern_ST.xy + _Pattern_ST.zw;
			float2 appendResult44 = (float2(_PanSpeed.x , _PanSpeed.y));
			float2 uv_TexCoord29 = i.uv_texcoord * _MultiplierTiling;
			float2 panner21 = ( 1.0 * _Time.y * appendResult44 + uv_TexCoord29);
			float4 temp_cast_0 = (_Power).xxxx;
			float2 appendResult45 = (float2(_PanSpeed.z , _PanSpeed.w));
			float2 uv_TexCoord42 = i.uv_texcoord * _MultiplierTiling;
			float2 panner43 = ( 1.0 * _Time.y * appendResult45 + uv_TexCoord42);
			float4 temp_cast_1 = (_Power).xxxx;
			float4 lerpResult16 = lerp( _BackgroundColor , _PatternColor , ( tex2D( _Pattern, uv_Pattern ) * pow( tex2D( _Multiplier, panner21 ) , temp_cast_0 ) * pow( tex2D( _Multiplier, panner43 ) , temp_cast_1 ) ));
			o.Emission = lerpResult16.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
1927;7;1666;985;3202.27;629.3218;2.81778;True;True
Node;AmplifyShaderEditor.Vector4Node;41;-2159.515,318.4809;Float;False;Property;_PanSpeed;PanSpeed;5;0;Create;True;0;0;False;0;0.05,0.05,-0.05,-0.05;0.05,0.05,-0.05,-0.05;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;46;-2177.015,523.7513;Float;False;Property;_MultiplierTiling;MultiplierTiling;6;0;Create;True;0;0;False;0;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.DynamicAppendNode;45;-1793.515,460.481;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;29;-1809.607,173.9985;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;44;-1796.515,308.4809;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;42;-1791.04,625.4889;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;21;-1499.556,236.9828;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;43;-1476.989,477.4731;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;31;-2209.018,82.63702;Float;True;Property;_Multiplier;Multiplier;3;1;[NoScaleOffset];Create;True;0;0;False;0;02223558e546f6a42a1d281233427ec2;02223558e546f6a42a1d281233427ec2;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;32;-1130.217,465.537;Float;True;Property;_TextureSample0;Texture Sample 0;7;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;19;-1222.556,207.9828;Float;True;Property;_Interference;Interference;3;0;Create;True;0;0;False;0;02223558e546f6a42a1d281233427ec2;02223558e546f6a42a1d281233427ec2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;25;-857.7078,356.0985;Float;False;Property;_Power;Power;4;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;40;-591.1094,472.1589;Float;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;18;-878.5386,-33.05037;Float;True;Property;_Pattern;Pattern;1;0;Create;True;0;0;False;0;dab338013e6544b4c842f6be6d46fc88;dab338013e6544b4c842f6be6d46fc88;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;24;-685.7078,215.0985;Float;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-463.762,77.42029;Float;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;14;-802,-416;Float;False;Property;_BackgroundColor;BackgroundColor;0;1;[HDR];Create;True;0;0;False;0;0.3018868,0.3018868,0.3018868,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;15;-793.3176,-217.3008;Float;False;Property;_PatternColor;PatternColor;2;1;[HDR];Create;True;0;0;False;0;1,1,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;16;-250.2311,-148.5756;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;69.22248,-138.6926;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;GlowPattern;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;45;0;41;3
WireConnection;45;1;41;4
WireConnection;29;0;46;0
WireConnection;44;0;41;1
WireConnection;44;1;41;2
WireConnection;42;0;46;0
WireConnection;21;0;29;0
WireConnection;21;2;44;0
WireConnection;43;0;42;0
WireConnection;43;2;45;0
WireConnection;32;0;31;0
WireConnection;32;1;43;0
WireConnection;19;0;31;0
WireConnection;19;1;21;0
WireConnection;40;0;32;0
WireConnection;40;1;25;0
WireConnection;24;0;19;0
WireConnection;24;1;25;0
WireConnection;20;0;18;0
WireConnection;20;1;24;0
WireConnection;20;2;40;0
WireConnection;16;0;14;0
WireConnection;16;1;15;0
WireConnection;16;2;20;0
WireConnection;0;2;16;0
ASEEND*/
//CHKSM=302668ECB7CE2B8B82DE27017474AF88DFD9C1C1