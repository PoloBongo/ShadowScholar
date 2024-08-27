Shader "Custom/UIBlurWithBackground"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurSize ("Blur Size", Range(0, 10)) = 1.0
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" }

        GrabPass { "_GrabTexture" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 screenPos : TEXCOORD0;  // Coordonnées d'écran
            };

            sampler2D _GrabTexture;
            float _BlurSize;
            float4 _GrabTexture_TexelSize;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Calculer les coordonnées d'écran
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                // Convertir les coordonnées d'écran en coordonnées UV
                float2 uv = i.screenPos.xy / i.screenPos.w;

                // Inverser l'axe Y des UV si nécessaire (cela dépend de la plateforme)
                uv.y = 1.0 - uv.y;

                half4 color = half4(0, 0, 0, 0);

                // Appliquer un effet de flou en échantillonnant autour du pixel capturé
                color += tex2D(_GrabTexture, uv + _GrabTexture_TexelSize.xy * _BlurSize);
                color += tex2D(_GrabTexture, uv - _GrabTexture_TexelSize.xy * _BlurSize);
                color += tex2D(_GrabTexture, uv + float2(_GrabTexture_TexelSize.x * _BlurSize, -_GrabTexture_TexelSize.y * _BlurSize));
                color += tex2D(_GrabTexture, uv + float2(-_GrabTexture_TexelSize.x * _BlurSize, _GrabTexture_TexelSize.y * _BlurSize));

                // Moyenne des échantillons pour créer l'effet de flou
                return color * 0.25;
            }
            ENDCG
        }
    }
    FallBack "Transparent"
}
