// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Stencil/StencilObject"
{
	Properties
	{
		_Color("Main Color", Color) = (1,1,1,1)

		_StencilReferenceID("Stencil Reference", Float) = 1
		[Enum(UnityEngine.Rendering.CompareFunction)]		_StencilComp("Stencil Comparison", Float) = 8
		[Enum(UnityEngine.Rendering.StencilOp)]				_StencilOp("Stencil Operation", Float) = 2
	}

	SubShader
	{
		Tags
		{ 
			"Queue" = "Transparent+1" 
			"IgnoreProjector" = "True" 
			"RenderType" = "Transparent" 
		}

		ColorMask 0
		ZWrite Off

		Pass
		{
			Stencil
			{
				Ref[_StencilReferenceID]
				Comp[_StencilComp]	// always
				Pass[_StencilOp]	// replace
			}

			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				fixed4 _Color;

				struct appdata
				{
					float4 vertex : POSITION;
				};

				struct v2f 
				{
					float4 vertex : SV_POSITION;
				};

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					return o;
				}

				half4 frag(v2f i) : COLOR
				{
					return _Color;
				}
			ENDCG
		}
	}
}