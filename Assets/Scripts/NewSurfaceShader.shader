Shader "Custom/NewSurfaceShader" {
	Properties {
		//_Color ("Color", Color) = (1,1,1,1)
		//_MainTex ("Albedo (RGB)", 2D) = "white" {}
		//_Glossiness ("Smoothness", Range(0,1)) = 0.5
		//_Metallic ("Metallic", Range(0,1)) = 0.0
		_MainTex("Texture", 2D) = "white"{}
		_RampTex("Ramp", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_LightCutoff("Maximum distance", Float) = 5.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		//LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf WrapLambert 

		uniform float _LightCutoff;
		sampler2D _RampTex;

		half4 LightingWrapLambert (SurfaceOutput s, half3 lightDir, half atten){
			half NdotL = dot(s.Normal, lightDir);
			atten = atten * NdotL;

			float2 lookUpPos = (saturate(atten), saturate(atten));

			atten = tex2D(_RampTex, lookUpPos);

			float lowVal = tex2D(_RampTex, float2(0,0));

			if(atten < lowVal)
			{
				atten = 0;
			}

			half vMax = (max(max(s.Albedo.r, s.Albedo.g), s.Albedo.b));
			half3 colorAdjust = vMax > 0 ? s.Albedo / vMax : 1;
			half4 c;
			c.rgb = _LightColor0.rgb * atten * colorAdjust;
			c.a = s.Alpha;
			return c;
		}

		struct Input {
			float2 uv_MainTex;
			float3 viewDir;
		};


		sampler2D _MainTex;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			o.Albedo = tex2D (_MainTex, IN.uv_MainTex) * _Color;
		}
		ENDCG
	}
	FallBack "Diffuse"

}

