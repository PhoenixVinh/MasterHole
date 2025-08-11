

//
//Shader "Unlit/ZwriteOff"
//{
//    
// 
//   SubShader
//   {
//       Pass {
//        ZWrite Off
//        CGPROGRAM
//        #pragma vertex vert
//        #pragma fragment frag
//        #include "UnityCG.cginc"
//
//        struct appdata {
//            float4 vertex : POSITION;
//        };
//
//        struct v2f {
//            float4 vertex : SV_POSITION;
//        };
//
//        float4 _Color;
//
//        v2f vert (appdata v) {
//            v2f o;
//            o.vertex = UnityObjectToClipPos(v.vertex);
//            return o;
//        }
//
//        fixed4 frag (v2f i) : SV_Target {
//            return _Color; // Apply the color with alpha
//        }
//        ENDCG
//    }
//}
//   }





Shader "Unlit/ZwriteOff"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1) // Màu của vật phẩm
        _MainTex ("Albedo (RGB)", 2D) = "white" {} // Texture chính
        _Alpha ("Transparency", Range(0,1)) = 0.5 // Độ trong suốt
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        // Tắt ZWrite để các đối tượng phía sau vẫn hiển thị
        ZWrite Off
        // Sử dụng Alpha Blending để làm trong suốt
        Blend SrcAlpha OneMinusSrcAlpha

        CGPROGRAM
        // Physically based Standard lighting model, enable shadows on all light types
        #pragma surface surf Standard alpha:fade

        // Use shader model 3.0 target for better lighting
        #pragma target 3.0

        sampler2D _MainTex;
        fixed4 _Color;
        float _Alpha;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Lấy màu từ texture và nhân với _Color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Độ trong suốt được điều khiển bởi _Alpha
            o.Alpha = _Alpha;
        }
        ENDCG
    }
    FallBack "Diffuse"
}


//
//Shader "Unlit/ZWriteOff"
//{
//    Properties
//    {
//        _HoleCenter ("Hole Center (XZ)", Vector) = (0,0,0,0)
//        _HoleRadius ("Hole Radius", Float) = 1.0
//        _HoleColor ("Hole Color", Color) = (0,0,0,1)
//        _MainTex ("Texture", 2D) = "white" {}
//        _MainColor ("Main Color", Color) = (1,1,1,1)
//    }
//    SubShader
//    {
//        Tags { "RenderType"="Opaque" }
//        LOD 100
//
//        Pass
//        {
//            CGPROGRAM
//            #pragma vertex vert
//            #pragma fragment frag
//
//            #include "UnityCG.cginc"
//
//            struct appdata
//            {
//                float4 vertex : POSITION;
//                float2 uv : TEXCOORD0;
//                float3 normal : NORMAL;
//            };
//
//            struct v2f
//            {
//                float2 uv : TEXCOORD0;
//                float4 vertex : SV_POSITION;
//                float3 worldPos : TEXCOORD1;
//            };
//
//            float2 _HoleCenter;
//            float _HoleRadius;
//            fixed4 _HoleColor;
//            sampler2D _MainTex;
//            float4 _MainTex_ST;
//            fixed4 _MainColor;
//
//            v2f vert (appdata v)
//            {
//                v2f o;
//                o.vertex = UnityObjectToClipPos(v.vertex);
//                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
//                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
//                return o;
//            }
//
//            fixed4 frag (v2f i) : SV_Target
//            {
//                // Calculate the distance in the XZ plane from the world position to the hole center
//                float distanceToHole = distance(float2(i.worldPos.x, i.worldPos.z), _HoleCenter);
//
//                // If the distance is within the hole radius, output the hole color
//                if (distanceToHole < _HoleRadius)
//                {
//                    return _HoleColor;
//                }
//                else
//                {
//                    // Otherwise, sample the texture and apply the main color
//                    fixed4 col = tex2D(_MainTex, i.uv) * _MainColor;
//                    return col;
//                }
//            }
//            ENDCG
//        }
//    }
//}

//
//Shader "Unlit/HoleWithTexture"
//{
//    Properties
//    {
//        _MainTex ("Texture", 2D) = "white" {}
//    }
//    SubShader
//    {
//        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
//        ZWrite Off
//        Blend SrcAlpha OneMinusSrcAlpha
//
//        Pass
//        {
//            CGPROGRAM
//            #pragma vertex vert
//            #pragma fragment frag
//            #include "UnityCG.cginc"
//
//            struct appdata
//            {
//                float4 vertex : POSITION;
//                float2 uv : TEXCOORD0;
//            };
//
//            struct v2f
//            {
//                float4 vertex : SV_POSITION;
//                float2 uv : TEXCOORD0;
//            };
//
//            sampler2D _MainTex;
//            float4 _MainTex_ST;
//
//            v2f vert (appdata v)
//            {
//                v2f o;
//                o.vertex = UnityObjectToClipPos(v.vertex);
//                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
//                return o;
//            }
//
//            fixed4 frag (v2f i) : SV_Target
//            {
//                return fixed4(0, 0, 0, tex2D(_MainTex, i.uv).a);
//            }
//            ENDCG
//        }
//    }
//}