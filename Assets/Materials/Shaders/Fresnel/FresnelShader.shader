Shader "Unlit/FresnelShader"
{
    Properties
    {
        _Color ("Fresnel Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { 
            "RenderType"="Transparent" // tag to inform the render pipeline of what type this is
//            "Queue"="Transparent" // changes the render order
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

            float4 _Color;

            struct MeshData
            {
                float4 vertex : POSITION;
                // float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct Interpolators
            {
                float4 vertex : POSITION;
                // float2 uv : TEXCOORD0;
                // float3 normal : TEXCOORD0;
                float4 color : TEXCOORD1;
            };


            Interpolators vert ( MeshData v )
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos( v.vertex );
                // o.normal = UnityObjectToWorldNormal( v.normal );
                // o.uv = TRANSFORM_TEX( v.uv, _MainTex );

                
                float3 fresnel = pow( 1 - dot( normalize( ObjSpaceViewDir( v.vertex ) ), v.normal ), 2);
                o.color = float4( fresnel, 1 );
                return o;
            }

            fixed4 frag ( Interpolators i ) : SV_Target
            {
                return i.color * _Color;
            }
            ENDCG
        }
    }
}
