// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Unlit alpha-blended shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Tutorial/Gradient Map Multiply" {
	Properties{
		_MainTex("Base (RGBA)", 2D) = "white" {}
		_Color1("Hi Color", color) = (1.0,1.0,1.0,1.0)
		_Step1("Color Cutoff", range(0.0,2.0)) = 0.5
		_Color2("Lo Color", color) = (1.0,0.95,0.8,1.0)
		//_BackfaceTint("Backface Tint", range(0.0,1.0)) = 0
		_AlphaCutoff("Alpha Cutoff", range(0.0,1.0)) = 0.0
		_AlphaInfluence("Alpha Influence", range(0.0,50.0)) = 1.0

		_Intensity("Intensity", range(-1,10)) = 1

		_MKGlowPower("Emission Power", range(0,10)) = 0

		_MKGlowColor("Emission Color", color) = (1.0,0.95,0.8,1.0)
		_Mask("Mask", 2D) = "white" {}
		_MaskCutoff("Mask Cutoff", range(-0.1,1)) = 0
		_Fog("Fog", range(0.0,1)) = 0
	}

		SubShader{
				//Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
				 Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "RenderType" = "MKGlow" }
				LOD 100

				//Lighting On
			//Cull Back

				//Cull Front
			//ZWrite Off
				//ColorMask 0
					//ZTest LEqual
				Blend One OneMinusSrcAlpha//SrcAlpha OneMinusSrcAlpha
					//AlphaToMask On

				Pass {
				Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "RenderType" = "MKGlow" }
				Blend One OneMinusSrcAlpha
				ZTest Lequal
				Cull Front
				ZWrite Off
						CGPROGRAM
						#pragma vertex vert
						#pragma fragment frag

						#include "UnityCG.cginc"

						struct appdata_t {
							float4 vertex : POSITION;
							float2 texcoord : TEXCOORD0;
							fixed4 color : COLOR;
							float2 maskcoord : TEXCOORD1;
						};

						struct v2f {
							float4 vertex : SV_POSITION;
							half2 texcoord : TEXCOORD0;
							fixed4 color : COLOR;
							half2 maskcoord : TEXCOORD1;
							//fixed4 color : COLOR;
						};

						//sampler2D _MainTex;

						float4 _MainTex_ST;
						float4 _Color1;
						float4 _Color2;
						float _Step1;
						float _AlphaCutoff;
						float _AlphaInfluence;

						float _Intensity;

						sampler2D _Mask;
						float4 _Mask_ST;
						float _MaskCutoff;
						//float _BackfaceTint;
						//float4 unity_FogColor;
						//float4 unity_FogDensity;
						v2f vert(appdata_t v)
						{
							v2f o;
							o.vertex = UnityObjectToClipPos(v.vertex);
							o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
							o.maskcoord = TRANSFORM_TEX(v.maskcoord, _Mask);
							o.color = v.color;

							return o;
						}


						sampler2D _MainTex;



						fixed4 frag(v2f i) : SV_Target
						{
							fixed2 myUVs = i.texcoord;
							myUVs.y = clamp(myUVs.y, 0, 1);
							fixed4 col = tex2D(_MainTex, myUVs);
							fixed4 mask = tex2D(_Mask, i.maskcoord);

							float blendV = col.r;
							float texA = col.a;

							
							//_Step1 += _BackfaceTint;
							
							_Step1 = lerp(_AlphaCutoff,2.0, _Step1);
							col = lerp(_Color2,_Color1,smoothstep(_AlphaCutoff,_Step1,blendV));
							
							//col.rgb = lerp(col.rgb, col.rgb * col.rgb, _BackfaceTint);
							col *= _Intensity;

							if (texA <= _AlphaCutoff) discard;
							texA = lerp(1.0,texA,_AlphaInfluence);

							col.xyz *= texA;
							col.a *= texA;
							col *= i.color;

							if (mask.r <= _MaskCutoff) discard;
							col *= mask.a;
							//col = unity_FogColor;
							return fixed4(col.r, col.g, col.b, col.a);
						}
					ENDCG
				}
					
					Pass{
							Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "RenderType" = "MKGlow" }
							Blend One OneMinusSrcAlpha
							ZTest Lequal
							Cull Back
							ZWrite Off
							CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

						struct appdata_t {
							float4 vertex : POSITION;
							float2 texcoord : TEXCOORD0;
							fixed4 color : COLOR;
							float2 maskcoord : TEXCOORD1;
						};

						struct v2f {
							float4 vertex : SV_POSITION;
							half2 texcoord : TEXCOORD0;
							fixed4 color : COLOR;
							half2 maskcoord : TEXCOORD1;
						};

						sampler2D _MainTex;

						float4 _MainTex_ST;
						float4 _Color1;
						float4 _Color2;
						float _Step1;
						float _AlphaCutoff;
						float _AlphaInfluence;

						float _Intensity;

						sampler2D _Mask;

						float4 _Mask_ST;
						float _MaskCutoff;

						v2f vert(appdata_t v)
						{
							v2f o;
							o.vertex = UnityObjectToClipPos(v.vertex);
							o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
							o.maskcoord = TRANSFORM_TEX(v.maskcoord, _Mask);
							o.color = v.color;
							return o;
						}

						fixed4 frag(v2f i) : SV_Target
						{
							//half2 uv = i.texcoord;
							//uv.x = uv.x<0 ? 0 : uv.x;
							//uv.y = uv.y<0 ? 0 : uv.y;
							//half4 col = tex2D(_MainTex, uv);
							fixed2 myUVs = i.texcoord;
						myUVs.y = clamp(myUVs.y, 0, 1);
						fixed4 col = tex2D(_MainTex, myUVs);
						fixed4 mask = tex2D(_Mask, i.maskcoord);
						float blendV = col.r;
						float texA = col.a;

						_Step1 = lerp(_AlphaCutoff,2.0, _Step1);
						col = lerp(_Color2,_Color1,smoothstep(_AlphaCutoff,_Step1,blendV));
						//col.xyz *= _Intensity;
						col *= _Intensity;

						if (texA <= _AlphaCutoff) discard;
						texA = lerp(1.0,texA,_AlphaInfluence);
						col.xyz *= texA;
						col.a *= texA;
						col *= i.color;


						if (mask.r <= _MaskCutoff) discard;
						col *= mask.a;

						return fixed4(col.r, col.g, col.b, col.a);
						}
							ENDCG
						}

			}

}