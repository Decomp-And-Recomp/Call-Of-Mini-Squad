// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Triniti/Model/CartoonRendering" {
	
	Properties {
		_Color ("Main Color", Color) = (.5,.5,.5,1)
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_Outline ("Outline width", Range (.002, 1)) = 0.1
		_MainTex ("Base (RGB)", 2D) = "white" { }
	}

	SubShader {
		Tags
    {
        "Queue"="Transparent"
        "RenderType"="Transparent"
    }
		
		  
		Pass 
		{
			Cull Front
			ZWrite On
			ColorMask RGBA
			Blend SrcAlpha OneMinusSrcAlpha			
			
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			struct appdata {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 pos : POSITION;
				float4 color : COLOR;
				float2 uv : TEXCOORD0;
			};
			
			uniform float _Outline;
			uniform float4 _OutlineColor;
			sampler2D _MainTex;
  			float4 _MainTex_ST;
		
			v2f vert(appdata v) {
				v2f o;
				appdata v2;
				
				v2 = v;
				v2.vertex.xyz = v.vertex.xyz + v.normal* (_Outline / 250);
				o.pos = UnityObjectToClipPos(v2.vertex);
				o.uv = TRANSFORM_TEX (v2.texcoord, _MainTex);
				o.color = _OutlineColor;
				return o;
			}
			
			half4 frag(v2f i) :COLOR { 
    			return i.color ;				 
    			
    		}

			ENDCG
					


		}

	}
	
	
	

			/*
			SubShader {
			
			    float convertGray(float4 color)
    			{
    			
    				return (color.x+color.y+color.z)*0.33f;
    		
    			}
    		
    			half4 frag(v2f i) : COLOR
				{
				
				  float Threshold = 2;
				  float2 ox = float2(1.0/128.0, 0.0);
				  float2 oy = float2(0.0, 1.0/128.0);
				  float2 PP = i.uv - oy;
				  float g00 = convertGray(tex2D(_MainTex, PP - ox));
				  float g01 = convertGray(tex2D(_MainTex, PP));
				  float g02 = convertGray(tex2D(_MainTex, PP + ox));
				  PP = i.uv;
				  float g10 = convertGray(tex2D(_MainTex, PP - ox));
				  // float g11 = tex2D(_MainTex, PP).x;
				  
				  float g12 = convertGray(tex2D(_MainTex, PP + ox));
				  PP = i.uv + oy;
				  float g20 = convertGray(tex2D(_MainTex, PP - ox));
				  float g21 = convertGray(tex2D(_MainTex, PP));
				  float g22 = convertGray(tex2D(_MainTex, PP + ox));
				  float sx = g20 + g22 - g00 - g02 + 2 * (g21 - g01);
				  float sy = g22 + g02 - g00 - g20 + 2 * (g12 - g10);
				  float dist = (sx * sx + sy * sy);
				  float tSq = Threshold * Threshold; // could be done on CPU
				  
				  half4 result = half4(1,1,1,1);
				  if (dist > tSq) 
				  { 
					result = half4(0,0,0,1); 
				  }
				  return tex2D(_MainTex, i.uv)*result.x;
				}
			}
			*/
			
			
	//FallBack "Diffuse"
}


