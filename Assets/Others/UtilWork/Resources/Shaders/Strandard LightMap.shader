Shader "MyFlying/Strandard Lightmap" {
	Properties {
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo", 2D) = "white" {}
		_Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
		_Glossiness("Smoothness", Range(0.0, 1.0)) = 0.5
		_SpecColor("Specular", Color) = (0.2,0.2,0.2)
		_SpecGlossMap("Specular", 2D) = "white" {}
		_BumpScale("Scale", Float) = 1.0
		_BumpMap("Normal Map", 2D) = "bump" {}
		_Fresnel("Fresnel", Range(0.001,1)) = 0.001
		_WaveSpeed("Wave speed (map1 x,y; map2 x,y)", Vector) = (0.2,0.2,-0.2,-0.2)
		
		_LightMap("Light Map", 2D) = "white" {}
		_IntensityLM("Intensity (LM)", range(0,2)) = 1

		[HideInInspector] _AddDepth("__adddepth", Float) = 0.0
		[HideInInspector] _Mode("__mode", Float) = 0.0
		[HideInInspector] _RefMode("__refmode", Float) = 0.0
		[HideInInspector] _SrcBlend("__src", Float) = 1.0
		[HideInInspector] _DstBlend("__dst", Float) = 0.0
		[HideInInspector] _ZWrite("__zw", Float) = 1.0
	}
	SubShader{
		LOD 200
		Blend[_SrcBlend][_DstBlend]
		ZWrite[_ZWrite]
		CGPROGRAM
		#pragma surface surf StandardSpecular nolightmap keepalpha
		#pragma target 3.0
		#pragma shader_feature _NORMALMAP
		#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
		#pragma shader_feature _WATER
		#pragma shader_feature _WATER_ALPHA
		#pragma shader_feature _REFLECTION_REAL

		half4 _Color;
		sampler2D _MainTex;
		half _Cutoff;
		half _Glossiness;
		sampler2D _SpecGlossMap;
		half _BumpScale;
		sampler2D _BumpMap;
		half _Fresnel;
		sampler2D _ReflectionTex;
		half4 _LightMapColor;
		half4 _WaveSpeed;
		sampler2D _LightMap;
		half _IntensityLM;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float2 uv2_LightMap;
			float3 viewDir;
			float4 screenPos;
		};
		float2 MoveTex(float2 uv, float2 pan) {
			return pan*_Time + uv;
		}
		half4 ClampColor (half4 cc) {
			return half4(clamp(cc.r,0,1),clamp(cc.g,0,1),clamp(cc.b,0,1),clamp(cc.a,0,1));
		}
		void surf (Input IN, inout SurfaceOutputStandardSpecular o) {
			half4 lm = tex2D(_LightMap, IN.uv2_LightMap) * _IntensityLM;
#ifdef _NORMALMAP
#ifdef _WATER
			half4 bump0 = tex2D(_BumpMap, MoveTex(IN.uv_BumpMap, _WaveSpeed.xy));
			half4 bump1 = tex2D(_BumpMap, MoveTex(IN.uv_BumpMap, _WaveSpeed.zw));
			o.Normal = UnpackScaleNormal((bump0 + bump1) * 0.5, _BumpScale);
#else
			o.Normal = UnpackScaleNormal(tex2D(_BumpMap, IN.uv_BumpMap), _BumpScale);
#endif
#endif
			float fresnel = pow(abs(1.0 - dot(normalize(IN.viewDir.xyz), normalize(o.Normal))), _Fresnel);
			half4 tex = tex2D(_MainTex, IN.uv_MainTex);
			half4 glossMap = tex2D(_SpecGlossMap, IN.uv_MainTex);
			o.Specular = _SpecColor * fresnel * glossMap.rgb;
			o.Smoothness = _Glossiness * glossMap.a;
			half4 difCol = tex * ClampColor(_Color - half4(o.Specular, 0));
			
#ifdef _REFLECTION_REAL
			float2 screenPos = IN.screenPos.xy / (IN.screenPos.w + 0.000001);
			float2 reflectionUV = screenPos + o.Normal.xy * 0.02 * o.Smoothness;
			half4 reflcol = _SpecColor * tex2D(_ReflectionTex, reflectionUV);
			o.Emission = reflcol.rgb * fresnel + difCol.rgb * lm;
#else
			o.Emission = difCol.rgb * lm;
#endif
			
#ifdef _WATER
#ifdef _WATER_ALPHA
			o.Alpha = difCol.a * fresnel;
#else
			o.Alpha = difCol.a;
#endif
#else
			o.Alpha = difCol.a * fresnel;
#endif

#ifdef _ALPHATEST_ON
			clip(difCol.a - _Cutoff);
#endif
		}
		ENDCG
	} 
	FallBack "VertexLit"
	CustomEditor "MFStandardShaderGUI"
}
