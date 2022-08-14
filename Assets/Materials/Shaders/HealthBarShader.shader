Shader "Unlit/HealthBarShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LowHealthColor ("LowHealthColor", Color) = ( 1, 0, 0, 1 )
        _MaxHealthColor ("MaxHealthColor", Color) = ( 0, 1, 0, 1 )
        _Health ("Health", Range( 0, 1 )) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha // Alpha blending
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _Health;
            float4 _LowHealthColor;
            float4 _MaxHealthColor;

            float InverseLerp ( float a, float b, float v )
            {
                return (v - a) / (b - a);
            }
            
            Interpolators vert ( MeshData v )
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos( v.vertex );
                o.uv = v.uv;
                return o;
            }

            fixed4 frag ( Interpolators i ) : SV_Target
            {
                
                fixed healthBarMask = _Health > i.uv.x;
                fixed4 color = tex2D( _MainTex, float2( InverseLerp( -0.1, 0.95, _Health ), i.uv.y ) );
                
                return fixed4( color.rgb, healthBarMask );
            }
            ENDCG
        }
    }
}
