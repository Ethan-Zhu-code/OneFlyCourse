// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/BrickSlice" {
	Properties
	{
		_MainTex("Main Tex", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,2)) = 1
		//_MainTex("Main Tex", Color) = (0.1, 1.0, 1.0, 1.0)
		_Opacity("Opacity", Range(0, 1)) = 1
		_BumpTex("Bump Tex", 2D) = "bump" {}
		_Ambient("Ambient Color", Color) = (0.6, 0.6, 0.6, 1.0)
		_Diffuse("Diffuse Color", Color) = (0.7, 0.7, 0.8, 1.0)
		_Specular("Specular Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_UseSceneLight("Use Scene Light", Float) = 1.0
		_LightDir("Light Dir", Vector) = (0.0, -1.0, 1.0, 0.0)

		_A("A", Vector) = (0.0,0,0, 0.0)
		_B("B", Vector) = (0.0,0,0, 0.0)
		_C("C", Vector) = (0.0,0,0, 0.0)
		_Threshold("Threshold",Float)=0.001
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Back

		Pass
		{
			//ZWrite Off
			CGPROGRAM
			#pragma vertex   vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f
			{
				float4 pos      : SV_POSITION;
				float2 uv       : TEXCOORD0;
				float3 viewDir  : TEXCOORD1;
				float3 lightDir : TEXCOORD2;
				float3 wPos		: TEXCOORD3;
				float4 localPos	: TEXCOORD4;
			};

			sampler2D _MainTex;
			float _Glossiness;
			float	  _Opacity;
			sampler2D _BumpTex;
			float4    _Ambient;
			float4    _Diffuse;
			float4    _Specular;
			float     _UseSceneLight;
			float4    _LightDir;
			float4 _A;
			float4 _B;
			float4 _C;
			float _Threshold;

			v2f vert(appdata_full v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.wPos = mul(unity_ObjectToWorld,v.vertex);
				o.uv = v.texcoord;
				o.localPos=v.vertex;

				float3 tangent = normalize(v.tangent);
				float3 normal = normalize(v.normal);
				float3 binormal = normalize(cross(normal, v.tangent.xyz) * v.tangent.w);

				float3x3 TBN;
				TBN[0] = tangent;
				TBN[1] = binormal;
				TBN[2] = normal;

				float3 viewDir = ObjSpaceViewDir(v.vertex);
				float3 lightDir = float3(0.0, 0.0, 0.0);
				if (_UseSceneLight > 0.0)
				{
					lightDir = ObjSpaceLightDir(v.vertex);
				}
				else
				{
					lightDir = -mul(unity_WorldToObject, _LightDir);
				}

				o.viewDir = mul(TBN, ObjSpaceViewDir(v.vertex));
				o.lightDir = mul(TBN, lightDir);

				return o;
			}

			float4 frag(v2f i) : COLOR0
			{
				float3 vB= _B - i.wPos;

				float3 vAB = _B - _A;
				float3 vCA = _B - _C;
				if(dot(vB, cross(vAB, vCA)) < 0)
					discard;
	
				float3 viewDir = normalize(i.viewDir);
				float3 lightDir = normalize(i.lightDir);

				float4 normalMap = tex2D(_BumpTex, i.uv);
				float3 normalDir = normalize(UnpackNormal(normalMap));

				float  s = max(0, dot(lightDir, normalDir));
				fixed3 h = normalize(viewDir + lightDir);
				float  r = max(0, dot(h, normalDir));
				float  spec = pow(r, 48.0);
				float4 clr = tex2D(_MainTex, i.uv);

				float4 c;
				c.rgb = ((_Ambient + _Diffuse * s) * clr.rgb + spec * _Specular.rgb * clr.a * 1.5) * _Glossiness;
				c.a = _Opacity;

				return c;
			}

			ENDCG
		}
	}
	Fallback "Diffuse"
}
