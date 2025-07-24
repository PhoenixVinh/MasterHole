Shader "Custom/CircleTransition"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1) // Đổi tên để rõ ràng hơn
        _Radius ("Circle Radius", Range(0.0, 1.1)) = 0
        _CenterX ("Center X (0-1)", Range(0.0, 1.0)) = 0.5
        _CenterY ("Center Y (0-1)", Range(0.0, 1.0)) = 0.5
        _AspectRatio ("Aspect Ratio (Width/Height)", Float) = 1.0 // Thêm thuộc tính tỷ lệ khung hình
        _InvertTransition ("Invert Transition", Float) = 0 // 0 = reveal, 1 = hide
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Transparent" "Queue" = "Transparent"
        }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex; // Khai báo sampler cho Texture
            fixed4 _Color;
            float _Radius;
            float _CenterX;
            float _CenterY;
            float _AspectRatio; // Biến cho tỷ lệ khung hình
            float _InvertTransition;

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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // Đã sửa hàm drawCircle:
            // - Sửa lỗi so sánh sqrDistance với radius (thay vì sqrRadius)
            // - Thêm biến 'output' vào tham số để tránh lỗi biên dịch trên một số platform (tốt hơn nên trả về giá trị)
            float calculateCircleAlpha(in float2 uv_normalized, in float2 center_normalized, in float radius_normalized, in float smoothValue)
            {
                // Điều chỉnh UV theo tỷ lệ khung hình
                float2 adjusted_uv = uv_normalized;
                float2 adjusted_center = center_normalized;

                // Nếu chiều rộng lớn hơn chiều cao, chúng ta cần thu nhỏ UV theo chiều X
                // Hoặc chiều ngược lại, nếu chiều cao lớn hơn chiều rộng, chúng ta cần thu nhỏ UV theo chiều Y
                // Để đảm bảo hình tròn luôn tròn, chúng ta sẽ scale một chiều để nó khớp với chiều nhỏ hơn
                if (_AspectRatio > 1.0) // Nếu rộng hơn cao
                {
                    adjusted_uv.x = (uv_normalized.x - 0.5) * _AspectRatio + 0.5;
                    adjusted_center.x = (center_normalized.x - 0.5) * _AspectRatio + 0.5;
                }
                else if (_AspectRatio < 1.0) // Nếu cao hơn rộng
                {
                    adjusted_uv.y = (uv_normalized.y - 0.5) / _AspectRatio + 0.5;
                    adjusted_center.y = (center_normalized.y - 0.5) / _AspectRatio + 0.5;
                }

                float distance_from_center = distance(adjusted_uv, adjusted_center);
                
                // Sử dụng smoothstep để tạo hiệu ứng chuyển tiếp mượt mà
                // Bên trong bán kính, alpha = 0 (trong suốt)
                // Bên ngoài bán kính, alpha = 1 (hiển thị)
                float circle_mask = smoothstep(radius_normalized - smoothValue, radius_normalized, distance_from_center);
                
                return circle_mask;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 center = float2(_CenterX, _CenterY);
                float smoothValue = 0.01; // Độ rộng của vùng chuyển tiếp mượt mà

                // Tính toán mặt nạ hình tròn
                float circle_mask_alpha = calculateCircleAlpha(i.uv, center, _Radius, smoothValue);
                
                // Lấy màu từ texture chính
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;

                // Áp dụng mặt nạ:
                // Nếu _InvertTransition = 0 (reveal): alpha = circle_mask_alpha (bên trong trong suốt, bên ngoài hiển thị)
                // Nếu _InvertTransition = 1 (hide): alpha = 1 - circle_mask_alpha (bên trong hiển thị, bên ngoài trong suốt)
                float final_alpha = lerp(circle_mask_alpha, 1.0 - circle_mask_alpha, _InvertTransition);

                // Gán alpha cuối cùng cho màu pixel
                col.a *= final_alpha; 
                
                return col;
            }
            ENDCG
        }
    }
}