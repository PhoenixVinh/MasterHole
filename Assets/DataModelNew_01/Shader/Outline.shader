Shader "UI/OutlinePrecise"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1, 1, 1, 1) // Màu trắng mặc định
        _OutlineWidth ("Outline Width", Range(0.0, 0.1)) = 0.02 // Độ dày outline
        _OutlineSmoothness ("Outline Smoothness", Range(0.0, 1.0)) = 0.7 // Độ mượt
        [HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil ("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
        [HideInInspector] _ColorMask ("Color Mask", Float) = 15
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
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
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _OutlineColor;
            float _OutlineWidth;
            float _OutlineSmoothness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Lấy mẫu texture chính
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;

                // Kiểm tra alpha của pixel hiện tại
                if (col.a < 0.1)
                {
                    // Lấy mẫu 12 hướng xung quanh với offset nhỏ hơn để sát biên
                    float2 offsets[12] = {
                        float2(-_OutlineWidth, 0),           // trái
                        float2(_OutlineWidth, 0),            // phải
                        float2(0, -_OutlineWidth),           // dưới
                        float2(0, _OutlineWidth),            // trên
                        float2(-_OutlineWidth * 0.7, -_OutlineWidth * 0.7), // trái trên
                        float2(_OutlineWidth * 0.7, -_OutlineWidth * 0.7),  // phải trên
                        float2(-_OutlineWidth * 0.7, _OutlineWidth * 0.7),  // trái dưới
                        float2(_OutlineWidth * 0.7, _OutlineWidth * 0.7),   // phải dưới
                        float2(-_OutlineWidth * 0.5, 0),     // trái nhỏ
                        float2(_OutlineWidth * 0.5, 0),      // phải nhỏ
                        float2(0, -_OutlineWidth * 0.5),     // dưới nhỏ
                        float2(0, _OutlineWidth * 0.5)       // trên nhỏ
                    };

                    float maxAlpha = 0.0;
                    // Tính gradient alpha để xác định biên sát
                    for (int j = 0; j < 12; j++)
                    {
                        fixed4 neighbor = tex2D(_MainTex, i.uv + offsets[j] * 0.5); // Giảm offset để sát hơn
                        maxAlpha = max(maxAlpha, neighbor.a);
                    }

                    // Tính toán độ mượt và alpha dựa trên biên
                    float edgeAlpha = smoothstep(0.0, _OutlineSmoothness, maxAlpha - col.a);
                    if (edgeAlpha > 0.1)
                    {
                        return fixed4(_OutlineColor.rgb, edgeAlpha * _OutlineColor.a);
                    }
                }

                return col; // Trả về màu gốc nếu không phải outline
            }
            ENDCG
        }
    }
}