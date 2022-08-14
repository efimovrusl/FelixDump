Shader "Unlit/UnlitTexture"
{
    Properties
    {
        _MainTex ( "Texture", 2D ) = "white" {}
        _Brightness ( "Brightness", Range( 0, 1 ) ) = 1
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct MeshData
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD1;
                float modifier : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Brightness;

            Interpolators vert( MeshData v )
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos( v.vertex );
                o.uv = TRANSFORM_TEX( v.uv, _MainTex );
                o.modifier = 0.1 + 0.9 * dot( UnityObjectToWorldNormal( v.normal ), float3( 0, 1, 0 ) );
                return o;
            }

            fixed4 frag( Interpolators i ) : SV_Target
            {
                return tex2D( _MainTex, i.uv ) * _Brightness * i.modifier;
            }
            ENDCG
        }
    }
}