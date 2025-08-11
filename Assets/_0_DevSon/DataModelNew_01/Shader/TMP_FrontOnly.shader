Shader "Custom/TMP_FrontOnly"
{
    Properties
    {
        _FaceTex            ("Face Texture", 2D) = "white" {}
        _FaceColor          ("Face Color", Color) = (1,1,1,1)
        _FaceDilate        ("Face Dilate", Range(-1.0, 1.0)) = 0.0
        _OutlineTex        ("Outline Texture", 2D) = "white" {}
        _OutlineColor      ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth      ("Outline Width", Range(0.0, 0.1)) = 0.0
        _WeightNormal      ("Weight Normal", Float) = 0
        _WeightBold        ("Weight Bold", Float) = 0.5
        _ScaleRatioA       ("Scale Ratio A", Float) = 1
        _ScaleRatioB       ("Scale Ratio B", Float) = 1
        _ScaleRatioC       ("Scale Ratio C", Float) = 1
        _MainTex           ("Font Atlas", 2D) = "white" {}
        _TextureWidth      ("Texture Width", Float) = 512
        _TextureHeight     ("Texture Height", Float) = 512
        _GradientScale     ("Gradient Scale", Float) = 5.0
        _ScaleX            ("Scale X", Float) = 1.0
        _ScaleY            ("Scale Y", Float) = 1.0
        _PerspectiveFilter ("Perspective Correction", Float) = 0.875
        _Sharpness         ("Sharpness", Range(-1.0, 1.0)) = 0
        [HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil ("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
        [HideInInspector] _CullMode ("Cull Mode", Float) = 0
        [HideInInspector] _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Back // Chỉ render mặt trước
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile __ UNITY_UI_CLIP_RECT
            #pragma multi_compile __ UNITY_UI_ALPHACLIP

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"
            //#include "TMPro_Properties.cginc"
            //#include "TMPro.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                float2 uv1      : TEXCOORD1; // For SDF
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                float4 mask     : TEXCOORD1; // For clipping
                float2 uv1      : TEXCOORD2; // For SDF
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _FaceDilate;
            float _OutlineWidth;
            float4 _FaceColor;
            float4 _OutlineColor;
            float _WeightNormal;
            float _WeightBold;
            float _GradientScale;
            float _ScaleX;
            float _ScaleY;
            float _Sharpness;
            float4 _ClipRect;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                OUT.vertex = UnityObjectToClipPos(v.vertex);
                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                OUT.color = v.color * _FaceColor;
                OUT.mask = v.vertex;
                OUT.uv1 = v.uv1;

                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                float2 uv = IN.texcoord;
                fixed4 color = IN.color;

                // Sample SDF texture
                float dist = tex2D(_MainTex, uv).a;
                float scale = _GradientScale * (1.0 + _Sharpness);
                float bias = (0.5 - _WeightNormal) * scale + _FaceDilate;
                float alpha = saturate((dist - bias) / scale);

                // Apply outline
                float outlineAlpha = saturate((dist - (_FaceDilate + _OutlineWidth)) / scale);
                fixed4 finalColor = lerp(_OutlineColor, _FaceColor, outlineAlpha);
                finalColor.a *= alpha;

                // Clip for UI
                #ifdef UNITY_UI_CLIP_RECT
                finalColor.a *= UnityGet2DClipping(IN.mask.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip(finalColor.a - 0.001);
                #endif

                return finalColor;
            }
            ENDCG
        }
    }
    Fallback "TextMeshPro/Mobile/Distance Field"
}