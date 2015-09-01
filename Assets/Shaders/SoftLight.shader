Shader "Blend/SoftLight" {
	Properties {
		_MainTex ("_MainTex", 2D) = "white" {}
		_Input ("_Input", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Transparent" }
		Pass {
			Lighting Off Fog { Mode Off } ZTest Always Cull Off ZWrite Off
			CGPROGRAM
			#include "Assets/Shaders/_Includes/Blends.cginc"
			#pragma vertex vert_uv0
			#pragma fragment frag
			
			
			sampler2D _MainTex, _Input;

			float4 frag( v2f_uv0 i ) : COLOR 
			{
				float4 a = tex2D(_MainTex, i.uv);
				float4 b = tex2D(_Input, i.uv);
				
				float4 r = float4(0,0,0,b.a);
				
			    if (b.r > 0.5) 
			    { 
			    	//r.r = (sqrt(a.r) * (2.0 * b.r - 1.0) + 2.0 * a.r * (1.0 - b.r));
			    	r.r = a.r*(1-(1-a.r)*(1-4*(b.r))) * 0.75; 
			    }
			    else 
			    {
			    	r.r = (2.0 * a.r * b.r + a.r * a.r * (1.0 - 2.0 * b.r));
			    	//r.r = 1-(1-a.r)*(1-(a.r*(2*b.r))); 
			    }
			    
			    if (b.g > 0.5) 
			    {	
			    	//r.g = (sqrt(a.g) * (2.0 * b.g - 1.0) + 2.0 * a.g * (1.0 - b.g));
			    	r.g = a.g*(1-(1-a.g)*(1-4*(b.g))) * 0.75; 
			    }
			    else 
			    { 
			    	r.g = (2.0 * a.g * b.g + a.g * a.g * (1.0 - 2.0 * b.g));
			    	//r.g = 1-(1-a.g)*(1-(a.g*(2*b.g))); 
			    }
			    
			    if (b.b > 0.5) 
			    { 
			    	//r.b = (sqrt(a.b) * (2.0 * b.b - 1.0) + 2.0 * a.b * (1.0 - b.b));
			    	r.b = a.b*(1-(1-a.b)*(1-4*(b.b))) * 0.75; 
			    }
			    else 
			    { 
			    	r.b = (2.0 * a.b * b.b + a.b * a.b * (1.0 - 2.0 * b.b));
			    	//r.b = 1-(1-a.b)*(1-(a.b*(2*b.b))); 
			   	}
    			
    			float4 result = (a.a * r) + ((1-a.a) * b);
				result.a = b.a;
				return result;
			}
			ENDCG
		}
	}
}