Shader "Custom/RepeatByObjectSize"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TileSizeX ("Tile Size X", Float) = 1.0
        _TileSizeY ("Tile Size Y", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        float _TileSizeX;
        float _TileSizeY;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            float3 objectScale = float3(1.0, 1.0, 1.0);
            #ifdef UNITY_MATRIX_M
            objectScale = float3(
                length(mul(unity_ObjectToWorld[0], unity_ObjectToWorld[0])),
                length(mul(unity_ObjectToWorld[1], unity_ObjectToWorld[1])),
                length(mul(unity_ObjectToWorld[2], unity_ObjectToWorld[2]))
            );
            #endif

            float2 tiledUV = IN.uv_MainTex;
            tiledUV.x *= objectScale.x / _TileSizeX;
            tiledUV.y *= objectScale.y / _TileSizeY;

            o.Albedo = tex2D(_MainTex, tiledUV).rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}

