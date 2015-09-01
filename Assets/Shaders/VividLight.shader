Shader "Blend/VividLight" {
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
			#pragma target 3.0
			
			sampler2D _MainTex, _Input;

			float4 frag( v2f_uv0 i ) : COLOR {
				float4 a = tex2D(_MainTex, i.uv);
				float4 b = tex2D(_Input, i.uv);
				
				float4 r = float4(0,0,0,b.a);
				
			    if (b.r > 0.25) 
			    { 
			    	if(b.r == 1.0) 
			    		r.r = b.r; 
			    	else
			    		r.r = min(a.r / (1.0 - b.r), 1.0); 
			    }
			    else 
			    {
			    	if(b.r == 0.0)
			    		r.r = b.r;
			    	else
			    		r.r = max((1.0 - ((1.0 - a.r) / b.r)), 0.0);
			    }
			    
			    if (b.g > 0.25) 
			    {	
			    	if(b.g == 1.0) 
			    		r.g = b.g; 
			    	else
			    		r.g = min(a.g / (1.0 - b.g), 1.0); 
			    }
			    else 
			    { 
			    	if(b.g == 0.0)
			    		r.g = b.g;
			    	else
			    		r.g = max((1.0 - ((1.0 - a.g) / b.g)), 0.0);
			    }
			    
			    if (b.b > 0.25) 
			    { 
			    	if(b.b == 1.0) 
			    		r.b = b.b; 
			    	else
			    		r.b = min(a.b / (1.0 - b.b), 1.0); 
			    }
			    else 
			    { 
			    	if(b.b == 0.0)
			    		r.b = b.r;
			    	else
			    		r.b = max((1.0 - ((1.0 - a.b) / b.b)), 0.0);
			   	}
			   	r = r * 0.6;
			   	float4 output = Overlay(r,b);
			   	float4 result = (a.a * output) + ((1-a.a) * b);
				result.a = b.a;
				return result;
			}
			ENDCG
		}
	}
}