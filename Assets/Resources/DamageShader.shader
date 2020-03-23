Shader "Custom/DamageShader"
{
	Properties
	{
		_Tint ("Tint", Color) = (1,1,1,1)
	}

	SubShader
	{
		Pass
		{
			CGPROGRAM //Unity shading language

			#pragma vertex vert   //Vertex Program
			#pragma fragment frag //Fragment Program

			#include "UnityCG.cginc"

			float4 _Tint; //accessing properties

			float4 vert(float4 position : POSITION) : SV_POSITION //float4 - a collection of four floating point numbers
			{									   				  //SV_POSITION - System value (SV), POSITION - Final Vertex Position
				return UnityObjectToClipPos(position); //correct to project display, can be moved, rotate, scale
			}

			float4 frag(float4 position : SV_POSITION) : SV_TARGET//SV TARGET - frame buffer
			{
				return _Tint; //float4(1.0f,0.0f,0.0f,0.0f)  //Color - ;
			}

			ENDCG
		}
	}
}
