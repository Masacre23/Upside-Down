﻿Shader "Custom/CellShadingForward" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BumpMap("Bumpmap", 2D) = "bump" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		//#pragma surface surf Standard fullforwardshadows
		#pragma surface surf Standard CelShadingForward

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		half4 LightingCelShadingForward(SurfaceOutput s, half3 lightDir, half atten) {
			half NdotL = dot(s.Normal, lightDir);
			if (NdotL <= 0.0) NdotL = 0;
			else NdotL = 1;
			half4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten * 2);
			c.a = s.Alpha;
			return c;
		}

		sampler2D _MainTex;
		sampler2D _BumpMap;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		//void surf (Input IN, inout SurfaceOutputStandard o) {
		void surf(Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}