Shader "Unlit/Shader1"
{
    Properties
    {
        _Colour("Colour", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            float4 _Colour;

            struct MeshData // per-vertex mesh data
            {
                float4 vertex : POSITION; // vertex position
                float2 uv : TEXCOORD0; // uv coordinates - used to map textures to object
                float3 normals: NORMAL;
            };

            struct Interpolators
            {
                float4 vertex : SV_POSITION; // clip space position
                float3 normal: TEXCOORD0;
                float2 uv : TEXTCOORD1;
            };

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normals); // just pass data to the fragment shader through the vertex shader
                o.uv = v.uv;
                return o;
            }

            float4 frag (Interpolators i) : SV_Target
            {
                return float4(i.uv, 0, 1);
            }
            ENDCG
        }
    }
}
