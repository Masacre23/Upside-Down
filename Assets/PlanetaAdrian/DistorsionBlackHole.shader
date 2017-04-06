Shader "FX/BlackRender" {

	CGINCLUDE
#pragma fragmentoption ARB_precision_hint_fastest
#pragma fragmentoption ARB_fog_exp2
#include "UnityCG.cginc"

		sampler2D _GrabTexture : register(s0);
	float4 _GrabTexture_TexelSize;
	sampler2D _BumpMap : register(s1);
	sampler2D _MainTex : register(s2);

	struct v2f {
		float4 vertex : POSITION;
		float4 uvgrab : TEXCOORD0;
		float2 uvbump : TEXCOORD1;
		float2 uvmain : TEXCOORD2;
	};

	uniform float2 _Position;
	uniform float _Distance;

	half4 frag(v2f i) : COLOR
	{
		float2 offset = i.uvgrab - _Position; // We shift our pixel to the desired position
		float2 ratio = { 1,1 }; // determines the aspect ratio
		float rad = length(offset / ratio); // the distance from the conventional "center" of the screen.
		float deformation = 1 / pow(rad*pow(_Distance,0.5),2)*0.1 * 2;

		offset = offset*(1 - deformation);

		//if (rad*_Distance<_Rad){res.r=0;res.g=0;res.b=0;} // check radius BH

		offset = offset * _GrabTexture_TexelSize.xy;
		offset += _Position;
		i.uvgrab.xy = offset * i.uvgrab.z + i.uvgrab.xy;

		half4 res = tex2D(_GrabTexture, i.uvgrab.xy);
		//if (rad*1<pow(2*0.1/1,0.5)*1) {res.g = 0; res.b = 0; res.r = 0;} // verification of compliance with the Einstein radius
		return res;
	}
		ENDCG

		Category {

		// We must be transparent, so other objects are drawn before this one.
		Tags{ "Queue" = "Transparent+100" "RenderType" = "Opaque" }


			SubShader{

			// This pass grabs the screen behind the object into a texture.
			// We can access the result in the next pass as _GrabTexture
			GrabPass{
			Name "BASE"
			Tags{ "LightMode" = "Always" }
		}

			// Main pass: Take the texture grabbed above and use the bumpmap to perturb it
			// on to the screen
			Pass{
			Name "BASE"
			Tags{ "LightMode" = "Always" }

			CGPROGRAM
#pragma vertex vert
#pragma fragment frag

			struct appdata_t {
			float4 vertex : POSITION;
			float2 texcoord: TEXCOORD0;
		};

		v2f vert(appdata_t v)
		{
			v2f o;
			o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
#else
			float scale = 1.0;
#endif
			o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;
			o.uvgrab.zw = o.vertex.zw;
			o.uvbump = MultiplyUV(UNITY_MATRIX_TEXTURE1, v.texcoord);
			o.uvmain = MultiplyUV(UNITY_MATRIX_TEXTURE2, v.texcoord);
			return o;
		}
		ENDCG
		}
		}

			// ------------------------------------------------------------------
			// Fallback for older cards and Unity non-Pro

			SubShader{
			Blend DstColor Zero
			Pass{
			Name "BASE"
			SetTexture[_MainTex]{ combine texture }
		}
		}
	}
	Fallback "FX/Glass/Stained BumpDistort"
}
