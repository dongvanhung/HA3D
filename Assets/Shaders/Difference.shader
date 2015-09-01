Shader "Blend/Difference" {
	Properties {
		_MainTex ("_MainTex", 2D) = "white" {}
		_Input ("_Input", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		Pass {
			Lighting Off Fog { Mode Off } ZTest Always Cull Off ZWrite Off
			CGPROGRAM
			#include "Assets/Shaders/_Includes/Blends.cginc"
			#pragma vertex vert_uv0
			#pragma fragment frag

			sampler2D _MainTex, _Input;

			float4 frag( v2f_uv0 i ) : COLOR {
				float4 a = tex2D(_MainTex, i.uv);
				float4 b = tex2D(_Input, i.uv);
				float4 output = Difference(a, b);
				float4 result = (a.a * output) + ((1-a.a) * b);
				result.a = b.a;
				return result;
			}
			ENDCG
		}
	}
}