Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        //        _MainTex ("Texture", 2D) = "white" {}
        _LeftGradColor ("Color", Color) = (0, 0, 0, 1)
        _RightGradColor ("Color", Color) = (1, 1, 1, 1)
        _GradStart ("Gradient start", Range(0, 1)) = 0
        _GradEnd("Gradient end", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Transparent" // tag to inform the render pipeline of what type this is
            "Queue"="Transparent" // changes the render order
        }

        Pass
        {
            Cull Off
            Blend One One // additive blending
            ZWrite Off
            ZTest LEqual

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            // sampler2D _MainTex;
            // float4 _MainTex_ST;
            float4 _Color;
            float4 _LeftGradColor;
            float4 _RightGradColor;
            float _GradStart;
            float _GradEnd;

            struct MeshData
            {
                float3 vertex : POSITION;
                float4 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;
                float2 uv : TEXCOORD0;
            };

            float4 InverseLerp(float a, float b, float v)
            {
                return (v - a) / (b - a);
            }

            Interpolators vert(MeshData v)
            {
                Interpolators i;

                i.vertex = UnityObjectToClipPos(v.vertex);
                i.normal = UnityObjectToWorldNormal(v.normal);
                i.uv = v.uv;

                return i;
            }

            fixed4 frag(Interpolators i) : SV_Target
            {
                float waves = 5;
                float timeOffset = sin(_Time.z) * 0.1;
                float yOffset = sin((i.uv.x + timeOffset) * UNITY_TWO_PI * 10) * 0.02;
                float topNBottomRemoval = abs(i.normal.y) < 0.999;
                float fadeout = 1 - pow(i.uv.y, 1);
                fixed4 color = abs(cos((i.uv.y + yOffset + _Time.x * -5) * waves * UNITY_PI))
                    * fadeout * topNBottomRemoval
                    * lerp(_RightGradColor, _LeftGradColor, i.uv.y);

                return color;
            }
            ENDCG
        }
    }
}