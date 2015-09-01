Shader "Blend/HSBConversion" {
	Properties {
		_MainTex ("_MainTex", 2D) = "white" {}
		_HSB ("HSB Values", Vector) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		Pass {
			Lighting Off Fog { Mode Off } ZTest Always Cull Off ZWrite Off
			CGPROGRAM
			//#include "Assets/Shaders/_Includes/Blends.cginc"
			#pragma vertex vert_uv0
			#pragma fragment frag

			sampler2D _MainTex;
			float4 _HSB;
			
			//r=h g=s b=b
			struct a2f_uv0 
			{
			    float4 vertex : POSITION;
			    float4 texcoord : TEXCOORD0;
			};
			struct v2f_uv0 
			{
			    float4 pos : SV_POSITION;
			    float2 uv : TEXCOORD0;
			};
			
			v2f_uv0 vert_uv0(a2f_uv0 v) 
			{
			    v2f_uv0 o;
			    o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			    o.uv = v.texcoord.xy;
			    return o;
			}
			
			float4 frag( v2f_uv0 i ) : COLOR 
			{
				float4 In = tex2D(_MainTex, i.uv);
				
				float4 ret = float4(0,0,0,In.a);
				
				float r = In.r;
				float g = In.g;
				float b = In.b;
				
				float maxHSB = max(r, max(g, b));
				
				if (maxHSB > 0)
				{
					float minHSB = min(r, min(g, b));
					float dif = maxHSB - minHSB;
					
					if (maxHSB > minHSB)
					{
						if (g == maxHSB)
						{
							ret.r = (b - r) / dif * 60 + 120;
						}
						else if (b == maxHSB)
						{
							ret.r = (r - g) / dif * 60 + 240;
						}
						else if (b > g)
						{
							ret.r = (g - b) / dif * 60 + 360;
						}
						else
						{
							ret.r = (g - b) / dif * 60;
						}
						if (ret.r < 0)
						{
							ret.r = ret.r + 360;
						}
					}
					else
					{
						ret.r = 0;
					}
					
					ret.r = ret.r * (1.0 / 360.0);
					ret.g = (dif / maxHSB) * 1;
					ret.b = maxHSB;
				}
				
				
				ret.r = ret.r + _HSB.r;
				ret.g = ret.g * _HSB.g;
				ret.b = ret.b * _HSB.b;
				ret.a = ret.a * _HSB.a;
				
				r = ret.b;
				g = ret.b;
				b = ret.b;
				if (ret.g != 0) 
				{
					float maxHSB = ret.b;
					float dif = ret.b * ret.g;
					float minHSB = ret.b - dif;
					
					//return float4(minHSB, minHSB, minHSB, ret.a);
					//float h = ret.h * 360f;
					float h = (ret.r * 360) % 360;
					if (h < 60) 
					{
						r = maxHSB;
						g = h * dif / 60 + minHSB;
						b = minHSB;
					} 
					else if (h < 120) 
					{
						r = (-(h - 120)) * dif / 60 + minHSB;
						g = maxHSB;
						b = minHSB;
					} 
					else if (h < 180) 
					{
						r = minHSB;
						g = maxHSB;
						b = (h - 120) * dif / 60 + minHSB;
					} 
					else if (h < 240) 
					{
						r = minHSB;
						g = (-(h - 240)) * dif / 60 + minHSB;
						b = maxHSB;
					} 
					else if (h < 300) 
					{
						r = (h - 240) * dif / 60 + minHSB;
						g = minHSB;
						b = maxHSB;
					} 
					else if (h <= 360) 
					{
						r = maxHSB;
						g = minHSB;
						b = (-(h - 360)) * dif / 60 + minHSB;
					} 
					else 
					{
						r = 0;
						g = 0;
						b = 0;
					}
				} 
				
				return float4(r,g,b,ret.a);
			}
			ENDCG
		}
	}
}