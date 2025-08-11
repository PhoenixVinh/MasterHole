Shader "Custom/WaterSurfaceShader"
{
    Properties
    {
        _MainTex ("Water Texture", 2D) = "white" {}
        _WaveStrength ("Wave Strength", Range(0, 0.1)) = 0.05
        _WaveSpeed ("Wave Speed", Float) = 1.0
        _WaveFrequency ("Wave Frequency", Float) = 1.0
        _TintColor ("Tint Color", Color) = (0.2, 0.4, 0.6, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        CGPROGRAM
        #pragma surface surf Standard alpha:fade

        sampler2D _MainTex;

        float _WaveStrength;
        float _WaveSpeed;
        float _WaveFrequency;
        fixed4 _TintColor;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float time = _Time.y * _WaveSpeed;
            float waveX = sin((IN.uv_MainTex.y + time) * _WaveFrequency);
            float waveY = cos((IN.uv_MainTex.x + time) * _WaveFrequency);

            float2 distortedUV = IN.uv_MainTex + float2(waveX, waveY) * _WaveStrength;

            fixed4 tex = tex2D(_MainTex, distortedUV);
            o.Albedo = tex.rgb * _TintColor.rgb;
            o.Alpha = tex.a * _TintColor.a;
        }
        ENDCG
    }
    FallBack "Transparent/Diffuse"
}