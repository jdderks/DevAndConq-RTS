Shader "Custom/GroundHighlightShader" {
    Properties {
        _XWidth ("X Width", Float) = 1.0
        _ZDepth ("Z Depth", Float) = 1.0
    }
    SubShader {
        Tags { "RenderType"="Opaque" }

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            struct appdata {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 pos : SV_POSITION;
            };

            float _XWidth;
            float _ZDepth;

            v2f vert(appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                float2 uv = i.pos.xy / i.pos.w;
                float4 pos = float4(uv.x * _XWidth - _XWidth * 0.5, 0.0, uv.y * _ZDepth - _ZDepth * 0.5, 1.0);
                return fixed4(1, 1, 1, 1); // White color
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
