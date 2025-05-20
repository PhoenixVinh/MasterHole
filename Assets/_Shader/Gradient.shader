Shader "Custom/GradientWithOffset"
{
    Properties
    {
        _ColorA("Start Color", Color) = (1,0,0,1)
        _ColorB("End Color", Color) = (0,0,1,1)
        _GradientAxis("Gradient Axis", Vector) = (0,1,0,0)
        _GradientSpread("Gradient Spread", Float) = 1.0
        _GradientOffset("Gradient Offset", Float) = 0.0
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
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 localPos : TEXCOORD0;
            };

            float4 _ColorA;
            float4 _ColorB;
            float3 _GradientAxis;
            float _GradientSpread;
            float _GradientOffset;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.localPos = v.vertex.xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float axisPos = dot(i.localPos, normalize(_GradientAxis));

            // Apply offset here
            float gradientValue = saturate((axisPos - _GradientOffset + (_GradientSpread * 0.5)) / _GradientSpread);

            return lerp(_ColorA, _ColorB, gradientValue);
        }
        ENDCG
    }
    }
}
