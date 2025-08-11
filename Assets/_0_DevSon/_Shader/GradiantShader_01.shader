Shader"Custom/GradiantShader_01"
{
    Properties
    {
        _ColorA ("Start Color", Color) = (0, 0, 0, 1) // Black
        _ColorB ("End Color", Color) = (1, 1, 1, 1) // White
        _Progress ("Transition Progress", Range(0, 1)) = 0
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

            float4 _ColorA;
            float4 _ColorB;
            float _Progress;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Wipe transition from left to right
                if (i.uv.x > _Progress)
                {
                    return _ColorA; // Start color (e.g., black)
                }
                return _ColorB; // End color (e.g., white)
            }
            ENDCG
        }
    }
}