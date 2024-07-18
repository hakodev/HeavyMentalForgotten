Shader "Custom/OutlineShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineThickness ("Outline Thickness", Range(0.0, 5)) = 0.02
    }
    SubShader
    {
        Tags {"RenderType"="Opaque"}
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _OutlineColor;
            float _OutlineThickness;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float2 offset1 = float2(_OutlineThickness, 0);
                float2 offset2 = float2(0, _OutlineThickness);

                float alpha = tex2D(_MainTex, uv).a;
                alpha += tex2D(_MainTex, uv + offset1).a;
                alpha += tex2D(_MainTex, uv - offset1).a;
                alpha += tex2D(_MainTex, uv + offset2).a;
                alpha += tex2D(_MainTex, uv - offset2).a;

                if (alpha > 0.0)
                {
                    return _OutlineColor;
                }
                else
                {
                    return tex2D(_MainTex, uv);
                }
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}