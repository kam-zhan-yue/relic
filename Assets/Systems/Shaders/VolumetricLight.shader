Shader "Custom/VolumetricLightCone"
{
    
    Properties
    {
        _Color ("Light Color", Color) = (1, 1, 1, 1)
        _Intensity ("Intensity", Float) = 1
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _NoiseScale ("Noise Scale", Float) = 1
    }
    SubShader
    {
        Tags {"Queue"="Transparent"  "RenderType"="Transparent" "IgnoreProjector" = "true"}
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
                float4 tangent : TANGENT;
            };

            struct Interpolators
            {
                float4 pos : POSITION;
                float3 worldPos : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
                float3 normal : TEXCOORD2;
            };

            sampler2D _NoiseTex;
            float _NoiseScale;
            float4 _Color;
            float _Intensity;

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.viewDir = normalize(_WorldSpaceCameraPos - o.worldPos);
                o.normal = mul((float3x3)unity_ObjectToWorld, v.normal);
                return o;
            }

            half4 frag (Interpolators i) : SV_Target
            {
                // Calculate depth-based effect
                const float depth = length(i.worldPos - _WorldSpaceCameraPos);
                
                // Apply noise texture
                const float noise = tex2D(_NoiseTex, i.worldPos.xy * _NoiseScale).r;

                // Calculate color intensity
                const float intensity = saturate(_Intensity / depth);
                half4 col = _Color * intensity * (1.0 + noise * 0.1);
                
                // Ensure the effect is visible
                col.a = saturate(intensity);

                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
