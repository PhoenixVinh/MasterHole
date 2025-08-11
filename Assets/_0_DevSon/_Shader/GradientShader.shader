Shader "Custom/GradientShader"
{

    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color1 ("Color 1", Color) = (0, 1, 0, 1) // Green
        _Color2 ("Color 2", Color) = (1, 0, 0, 1) // Red
        _Color3 ("Color 3", Color) = (0, 0, 1, 1) // Blue
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        fixed4 _Color1;
        fixed4 _Color2;
        fixed4 _Color3;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Calculate a gradient based on world position Y (vertical gradient)
            float gradient = IN.worldPos.y * 0.5 + 0.5; // Normalize to 0-1 range

            // Interpolate between colors based on gradient
            fixed4 color = lerp(_Color1, _Color2, gradient);
            color = lerp(color, _Color3, saturate(gradient - 0.5) * 2);

            // Apply texture if needed
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * color;

            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
