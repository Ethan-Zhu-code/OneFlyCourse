// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ClipObj"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	//裁切的范围(根据实际物体大小而定，通过C#赋值)
	_DiscardFactor("DiscardFactor",Range(-0.51,0.55)) = 0.0
		//切口光的颜色
		_LightColor("LightColor",Color) = (1,1,1,1)
		//光的宽度
		_LightWidth("LightWidth",Range(0.0,0.1)) = 0.05
	}
		SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		//不关闭背面剔除的话看不到物体内侧
		Cull off
		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag	
#include "UnityCG.cginc"
		struct v2f
	{
		float2 uv : TEXCOORD0;
		float4 vertex : SV_POSITION;
		float3 worldPos:TEXCOORD1;
	};

	sampler2D _MainTex;
	float _YFactor;
	float _DiscardFactor;
	float4 _LightColor;
	float _LightWidth;

	v2f vert(appdata_base v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
		o.uv = v.texcoord;
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		float factor = _YFactor - i.worldPos.y + _DiscardFactor;
	//_YFactor-i.worldPos.y=0时像素为物体中心，大于0像素在下面，小于0像素在上面，_DiscardFactor越小，factor越小，下面保留的就越少
	if (factor < 0)
	{
		discard;
	}
	float4 color = tex2D(_MainTex, i.uv);
	//factor<0的部分已经被裁减，剩下的部分在加个边界做切面的描边
	if (factor < _LightWidth)
	{
		return _LightColor;
	}
	return color;
	//优化了上面if的写法，等价，但是看起来比较绕
	//float lerpFactor=saturate(sign(_LightWidth -factor));
	//return color * (1 - lerpFactor) + lerpFactor * _LightColor;
	}
		ENDCG
	}
	}
}
