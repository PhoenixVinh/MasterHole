Shader "Simple Toon/SToon Outline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        [Header(Colorize)][Space(5)]
        _Color ("Color", COLOR) = (1,1,1,1)
        [HideInInspector] _ColIntense ("Intensity", Range(0,3)) = 1
        [HideInInspector] _ColBright ("Brightness", Range(-1,1)) = 0
        _AmbientCol ("Ambient", Range(0,1)) = 0

        [Header(Detail)][Space(5)]
        [Toggle] _Segmented ("Segmented", Float) = 1
        _Steps ("Steps", Range(1,25)) = 3
        _StpSmooth ("Smoothness", Range(0,1)) = 0
        _Offset ("Lit Offset", Range(-1,1.1)) = 0

        [Header(Light)][Space(5)]
        [Toggle] _Clipped ("Clipped", Float) = 0
        _MinLight ("Min Light", Range(0,1)) = 0
        _MaxLight ("Max Light", Range(0,1)) = 1
        _Lumin ("Luminocity", Range(0,2)) = 0

        [Header(Outline)][Space(5)]
        _OtlColor ("Color", COLOR) = (0,0,0,1)
        _OtlWidth ("Width", Range(0,100)) = 1

        [Header(Shine)][Space(5)]
        [HDR] _ShnColor ("Color", COLOR) = (1,1,0,1)
        [Toggle] _ShnOverlap ("Overlap", Float) = 0
        _ShnIntense ("Intensity", Range(0,1)) = 0
        _ShnRange ("Range", Range(0,1)) = 0.15
        _ShnSmooth ("Smoothness", Range(0,1)) = 0
        
        // Thêm buffer cho instancing
        [PerRendererData] _InstancingColor ("Instancing Color", Color) = (1,1,1,1)
        [PerRendererData] _InstancingOutlineColor ("Instancing Outline Color", Color) = (0,0,0,1)
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "LightMode" = "ForwardBase" }
        Pass
        {
            Name "DirectLight"
            LOD 80

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #pragma multi_compile_instancing // Bật instancing

            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"
            #include "AutoLight.cginc"
            #include "STCore.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID // Thêm ID instance
            };

            struct v2f
            {
                LIGHTING_COORDS(0,1)
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                half3 worldNormal : NORMAL;
                float3 viewDir : TEXCOORD2;
                UNITY_VERTEX_INPUT_INSTANCE_ID // Thêm ID instance vào v2f
            };

            // Khai báo buffer cho instancing
            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(fixed4, _InstancingColor)
            UNITY_INSTANCING_BUFFER_END(Props)

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v); // Thiết lập ID instance
                UNITY_TRANSFER_INSTANCE_ID(v, o); // Chuyển ID sang fragment
                
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = WorldSpaceViewDir(v.vertex);

                TRANSFER_VERTEX_TO_FRAGMENT(o);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i); // Thiết lập ID instance trong fragment
                
                _MaxLight = max(_MinLight, _MaxLight);
                _Steps = _Segmented ? _Steps : 1;
                _StpSmooth = _Segmented ? _StpSmooth : 1;

                _DarkColor = fixed4(0,0,0,1);
                _MaxAtten = 1.0;

                float3 normal = normalize(i.worldNormal);
                float3 light_dir = normalize(_WorldSpaceLightPos0.xyz);
                float3 view_dir = normalize(i.viewDir);
                float3 halfVec = normalize(light_dir + view_dir);
                float3 forward = mul((float3x3)unity_CameraToWorld, float3(0,0,1));

                float NdotL = dot(normal, light_dir);
                float NdotH = dot(normal, halfVec);
                float VdotN = dot(view_dir, normal);
                float FdotV = dot(forward, -view_dir);

                fixed atten = SHADOW_ATTENUATION(i);
                float toon = Toon(NdotL, atten);

                fixed4 shadecol = _DarkColor;
                fixed4 instColor = UNITY_ACCESS_INSTANCED_PROP(Props, _InstancingColor); // Lấy màu instance
                fixed4 litcol = ColorBlend(_Color * instColor, _LightColor0, _AmbientCol); // Áp dụng màu instance
                fixed4 texcol = tex2D(_MainTex, i.uv) * litcol * _ColIntense + _ColBright;

                float4 blendCol = ColorBlend(shadecol, texcol, toon);
                float4 postCol = PostEffects(blendCol, toon, atten, NdotL, NdotH, VdotN, FdotV);

                postCol.a = 1.;
                return _LightColor0.a > 0 ? postCol : 0;
            }
            ENDCG
        }

        Tags { "RenderType" = "Opaque" "LightMode" = "ForwardAdd" }
        Pass
        {
            Name "SpotLight"
            BlendOp Max
            LOD 100

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_instancing // Bật instancing

            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"
            #include "AutoLight.cginc"
            #include "STCore.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID // Thêm ID instance
            };

            struct v2f
            {
                LIGHTING_COORDS(0,1)
                float2 uv : TEXCOORDSS;
                float4 pos : SV_POSITION;
                float3 worldPos : WORLD;
                half3 worldNormal : NORMAL;
                float3 viewDir : TEXCOORD2;
                UNITY_VERTEX_INPUT_INSTANCE_ID // Thêm ID instance vào v2f
            };

            // Khai báo buffer cho instancing
            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(fixed4, _InstancingColor)
            UNITY_INSTANCING_BUFFER_END(Props)

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v); // Thiết lập ID instance
                UNITY_TRANSFER_INSTANCE_ID(v, o); // Chuyển ID sang fragment
                
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.viewDir = WorldSpaceViewDir(v.vertex);

                TRANSFER_VERTEX_TO_FRAGMENT(o);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i); // Thiết lập ID instance trong fragment
                
                _MaxLight = max(_MinLight, _MaxLight);
                _Steps = _Segmented ? _Steps : 1;
                _StpSmooth = _Segmented ? _StpSmooth : 1;

                _DarkColor = fixed4(0,0,0,1);
                _MaxAtten = 1.0;

                float3 normal = normalize(i.worldNormal);
                float3 light_dir = normalize(_WorldSpaceLightPos0.xyz - i.worldPos.xyz);
                float3 view_dir = normalize(i.viewDir);
                float3 halfVec = normalize(light_dir + view_dir);
                float3 forward = mul((float3x3)unity_CameraToWorld, float3(0,0,1));

                float NdotL = dot(normal, light_dir);
                float NdotH = dot(normal, halfVec);
                float VdotN = dot(view_dir, normal);
                float FdotV = dot(forward, -view_dir);

                float atten = LIGHT_ATTENUATION(i);
                float toon = Toon(NdotL, atten);

                fixed4 shadecol = _DarkColor;
                fixed4 instColor = UNITY_ACCESS_INSTANCED_PROP(Props, _InstancingColor); // Lấy màu instance
                fixed4 litcol = ColorBlend(_Color * instColor, _LightColor0, _AmbientCol); // Áp dụng màu instance
                fixed4 texcol = tex2D(_MainTex, i.uv) * litcol * _ColIntense + _ColBright;

                float4 blendCol = ColorBlend(shadecol, texcol, toon);
                float4 postCol = PostEffects(blendCol, toon, atten, NdotL, NdotH, VdotN, FdotV);

                postCol.a = 1.;
                return postCol;
            }
            ENDCG
        }

        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"

        Pass
        {
            Tags { "RenderType" = "Opaque" "LightMode" = "ForwardBase" }
            Blend Off
            Cull Front

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing // Bật instancing

            #include "UnityCG.cginc"
            #include "STCore.cginc"

            float4 _OtlColor;
            float _OtlWidth;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID // Thêm ID instance
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID // Thêm ID instance vào v2f
            };

            // Khai báo buffer cho instancing
            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(fixed4, _InstancingOutlineColor)
            UNITY_INSTANCING_BUFFER_END(Props)

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v); // Thiết lập ID instance
                UNITY_TRANSFER_INSTANCE_ID(v, o); // Chuyển ID sang fragment
                
                o.pos = v.vertex;
                o.pos.xyz += normalize(v.normal.xyz) * _OtlWidth * 0.008;
                o.pos = UnityObjectToClipPos(o.pos);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i); // Thiết lập ID instance trong fragment
                
                clip(-negz(_OtlWidth));
                fixed4 instOutlineColor = UNITY_ACCESS_INSTANCED_PROP(Props, _InstancingOutlineColor); // Lấy màu outline instance
                return instOutlineColor; // Sử dụng màu instance thay vì _OtlColor
            }
            ENDCG
        }
    }
}