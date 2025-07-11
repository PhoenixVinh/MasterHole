Shader "Custom/MobileEnhanced" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1) // Đã đổi thành Color để hiển thị tốt hơn trong trình chỉnh sửa
        _MainTex ("Albedo", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0, 1)) = 0.5
        _BumpMap ("Normal Map", 2D) = "bump" {}
        _ColorEnhance ("Color Enhancement (Sat,Bright,Contrast)", Vector) = (1.2,1.1,1.1,0)
    }
    SubShader{
        Tags { "RenderType" = "Opaque" "Queue" = "Geometry+10" }
        LOD 200

        Pass
        {

            


            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase // Dành cho ánh sáng base pass forward (directional, ambient, lightmaps)

            #include "UnityCG.cginc" // Dành cho các hàm và macro Unity thông dụng
            #include "UnityLightingCommon.cginc" // Dành cho các hàm chiếu sáng

            struct Vertex_Stage_Input
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0; // Thêm tọa độ UV
                float3 normal : NORMAL; // Thêm normal để chiếu sáng
                float4 tangent : TANGENT; // Thêm tangent cho normal mapping
            };

            struct Vertex_Stage_Output
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0; // Truyền tọa độ UV
                float3 worldNormal : TEXCOORD1; // Truyền normal thế giới
                float3 worldPos : TEXCOORD2; // Truyền vị trí thế giới để chiếu sáng
                float3 worldTangent : TEXCOORD3; // Truyền tangent thế giới
                float3 worldBitangent : TEXCOORD4; // Truyền bitangent thế giới
            };

            float4 _Color;
            sampler2D _MainTex;
            sampler2D _BumpMap;
            float _Glossiness;
            float4 _ColorEnhance; // x=Saturation, y=Brightness, z=Contrast

            Vertex_Stage_Output vert(Vertex_Stage_Input input)
            {
                Vertex_Stage_Output output;
                output.pos = UnityObjectToClipPos(input.pos); // Sử dụng UnityObjectToClipPos cho tiện lợi
                output.uv = input.uv;
                output.worldNormal = UnityObjectToWorldNormal(input.normal); // Biến đổi normal sang không gian thế giới
                output.worldPos = mul(unity_ObjectToWorld, input.pos).xyz; // Biến đổi vị trí sang không gian thế giới

                // Tính toán world tangent và bitangent cho normal mapping
                output.worldTangent = UnityObjectToWorldDir(input.tangent.xyz);
                output.worldBitangent = cross(output.worldNormal, output.worldTangent) * input.tangent.w;

                return output;
            }

            // Hàm để điều chỉnh độ bão hòa
            float3 AdjustSaturation(float3 color, float saturation)
            {
                float luma = dot(color, float3(0.2126, 0.7152, 0.0722)); // Luma (độ sáng)
                return lerp(float3(luma, luma, luma), color, saturation);
            }

            float4 frag(Vertex_Stage_Output input) : SV_TARGET
            {
                // Chuẩn hóa các vector để tính toán chiếu sáng
                float3 worldNormal = normalize(input.worldNormal);
                float3 worldTangent = normalize(input.worldTangent);
                float3 worldBitangent = normalize(input.worldBitangent);

                // Tạo ma trận TBN
                float3x3 tbn = float3x3(worldTangent, worldBitangent, worldNormal);

                // Lấy mẫu texture albedo
                float4 albedo = tex2D(_MainTex, input.uv) * _Color;

                // Lấy mẫu normal map và biến đổi sang không gian thế giới
                float3 normalMap = UnpackNormal(tex2D(_BumpMap, input.uv));
                float3 finalNormal = mul(normalMap, tbn); // Biến đổi normal từ không gian tiếp tuyến sang không gian thế giới
                finalNormal = normalize(finalNormal);

                // Hướng ánh sáng
                float3 lightDir = normalize(UnityWorldSpaceLightDir(input.worldPos));
                float3 viewDir = normalize(UnityWorldSpaceViewDir(input.worldPos));

                // Tính toán ánh sáng khuếch tán (Diffuse)
                float NdotL = saturate(dot(finalNormal, lightDir));
                float3 diffuse = NdotL * _LightColor0.rgb; // _LightColor0 là màu của ánh sáng chính

                // Tính toán ánh sáng phản xạ (Specular) (Phong model đơn giản)
                float3 halfVector = normalize(lightDir + viewDir);
                float NdotH = saturate(dot(finalNormal, halfVector));
                float specular = pow(NdotH, _Glossiness * 128.0) * _Glossiness; // Sử dụng _Glossiness
                float3 finalSpecular = specular * _LightColor0.rgb;

                // Ánh sáng môi trường và ánh sáng gián tiếp
                float3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz; // Ánh sáng môi trường
                float3 lightmapColor = ShadeSH9(float4(finalNormal, 1.0)); // Ánh sáng SH (nếu có lightmap hoặc probe)

                float3 finalColor = albedo.rgb * (diffuse + ambient + lightmapColor) + finalSpecular;

                // Áp dụng tăng cường màu sắc
                // Độ sáng (Brightness)
                finalColor *= _ColorEnhance.y;
                // Độ tương phản (Contrast)
                finalColor = (finalColor - 0.5) * _ColorEnhance.z + 0.5;
                // Độ bão hòa (Saturation)
                finalColor = AdjustSaturation(finalColor, _ColorEnhance.x);

                return float4(finalColor, albedo.a);
            }

            ENDHLSL
        }
    }
    Fallback "Mobile/Diffuse"
}