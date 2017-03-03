Shader "UI/Masks/Soft Mask"
{
	Properties
	{
		[PerRenderData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)

		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255

		_ColorMask("Color Mask", Float) = 15

		_Min("Min", Vector) = (0,0,0,0)
		_Max("Max", Vector) = (1,1,0,0)
		_AlphaMask("Alpha Mask - Must be wrapped", 2D) = "white" {}
		_AlphaUV("Alpha UV", Vector) = (1,1,0,0)
		_AlphaRangeMode("Alpha Range Mode", Int) = 0
		_AlphaRange("Alpha Range", Float) = 1
		_AlphaCurve("Alpha Curve", Float) = 1
		_CutOff("Cutoff", Float) = 0
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CauseSpriteAtlas" = "True"
		}

		Stencil
		{
			Ref[_Stencil]
			Comp[_StencilComp]
			Pass[_StencilOP]
			ReadMask[_StencilReadMask]
			WriteMask[_StencilWriteMask]
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest[unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask[_ColorMask]

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			struct appdata_t
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				half2 texcoord : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
			};

			fixed4 _Color;
			fixed4 _TextureSampleAdd;

			bool _UseClipRect;
			float4 _ClipRect;

			bool _UseAlphaClip;

			float4 _ProgressColor;
			float _Value;
			int _LeftToRight;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.worldPosition = IN.vertex;
				OUT.vertex = mul(UNITY_MATRIX_MVP, OUT.worldPosition);

				OUT.texcoord = IN.texcoord;

				#ifdef UNITY_HALF_TEXEL_OFFSET
					OUT.vertex.xy += (_ScreenParams.zw - 1.0) * float2(-1, 1);
				#endif

				OUT.color = IN.color * _Color;
				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaMask;

			float2 _AlphaUV;

			float _AlphaRangeMode;
			float _AlphaRange;
			float _AlphaCurve;

			float2 _Min;
			float2 _Max;

			float _CutOff;

			fixed4 frag(v2f IN) : SV_Target
			{
				half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;

				if (IN.texcoord.x < _Min.x || IN.texcoord.x > _Max.x || IN.texcoord.y < _Min.y || IN.texcoord.y > _Max.y)
				{
					color.a = color.a * 0;
				}
				else
				{
					float a = tex2D(_AlphaMask, (IN.texcoord - _Min) / _AlphaUV).a;
					if (a <= _CutOff)
						a = 0;

					color.a = color.a * a;
				}

				if (_UseClipRect)
				{
					color *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
				}

				if (_UseAlphaClip)
					clip(color.a - 0.001f);

				if (_AlphaRangeMode == 1)
					color.a = (int)(color.a * (255 * _AlphaRange));
				else if (_AlphaRangeMode == 2)
					color.a = (int)(color.a * (255 * _AlphaRange)) / (255 * _AlphaRange);
				else if (_AlphaRangeMode == 3)
					color.a = floor(color.a * _AlphaRange) / _AlphaRange;
				else if (_AlphaRangeMode == 4)
					color.a = floor(pow(color.a, _AlphaCurve) * _AlphaRange) / _AlphaRange;

				return color;
			}
			ENDCG
		}
	}
}
