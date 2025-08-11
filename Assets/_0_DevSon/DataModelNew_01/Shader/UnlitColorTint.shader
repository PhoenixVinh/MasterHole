Shader "Custom/UnlitColorTint"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1) // Default to white (no tint)
        _Tiling ("Tiling", Vector) = (1,1,0,0) // Added for tiling control
        _Offset ("Offset", Vector) = (0,0,0,0) // Added for offset control
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST; // For tiling and offset
            fixed4 _Color;
            float4 _Tiling; // Custom Tiling
            float4 _Offset; // Custom Offset

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                // Apply custom tiling and offset
                o.uv = v.uv * _Tiling.xy + _Offset.xy;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb *= _Color.rgb; // Apply the tint color
                return col;
            }
            ENDCG
        }
    }
    FallBack "Standard"
}