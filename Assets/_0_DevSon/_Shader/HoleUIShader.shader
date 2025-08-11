Shader "Custom/HoleUIShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1)
        _Cutoff ("Alpha Cutoff", Range(0, 1)) = 0.5
    }
    
    SubShader
    {
        Tags 
        { 
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "IgnoreProjector" = "True"
            "PreviewType" = "Plane"
        }
        
        // Tắt ZWrite để không ghi vào depth buffer
        // ZWrite Off
        // Sử dụng alpha blending
        Blend SrcAlpha OneMinusSrcAlpha
        // Không cần culling vì UI thường cần hiển thị cả hai mặt
        Cull Off
        // Tắt ZTest để luôn hiển thị ở trên
        ZTest Always
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _Cutoff;
            
            v2f vert (appdata v)
            {
                v2f o;
                // Chuyển đổi tọa độ từ object space sang clip space
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // Sample texture
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                
                // Alpha test
                clip(col.a - _Cutoff);
                
                return col;
            }
            ENDCG
        }
    }
    FallBack "UI/Default"
}