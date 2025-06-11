Shader "Custom/GradientWithOffsetAndTexture"
{
    Properties
    {
        _ColorA("Start Color", Color) = (1,0,0,1)
        _ColorB("End Color", Color) = (0,0,1,1)
        _MainTex("Texture", 2D) = "white" {} // Added texture property
        _GradientAxis("Gradient Axis", Vector) = (0,1,0,0)
        _GradientSpread("Gradient Spread", Float) = 1.0
        _GradientOffset("Gradient Offset", Float) = 0.0
        _TextureBlend("Texture Blend", Range(0,1)) = 0.5 // Control texture influence
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
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
                float2 uv : TEXCOORD0; // Added UV coordinates
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 localPos : TEXCOORD0;
                float2 uv : TEXCOORD1; // Added UV coordinates
            };

            float4 _ColorA;
            float4 _ColorB;
            sampler2D _MainTex;
            float4 _MainTex_ST; // Texture scale and offset
            float3 _GradientAxis;
            float _GradientSpread;
            float _GradientOffset;
            float _TextureBlend;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.localPos = v.vertex.xyz;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex); // Transform UVs
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float axisPos = dot(i.localPos, normalize(_GradientAxis));
                float gradientValue = saturate((axisPos - _GradientOffset + (_GradientSpread * 0.5)) / _GradientSpread);
                
                // Sample texture
                fixed4 texColor = tex2D(_MainTex, i.uv);
                
                // Blend gradient with texture
                fixed4 gradientColor = lerp(_ColorA, _ColorB, gradientValue);
                return lerp(gradientColor, texColor * gradientColor, _TextureBlend);
            }
            ENDCG
        }
    }
}