Shader "Custom/ShadowShader"
{
    Properties
    {
        _Color ("Color", Color) = (0, 0, 0, 1)
        _Color2 ("Color2", Color) = (1, 1, 1, 1)
        _NoiseTex ("NoiseTex", 2D) = "white" {}

        _OutlineColor ("OutlineColor", Color) = (1,1,1,1)
        _OutlineWidth ("OutlineWidth", Range(0, 1.0)) = 0.05
        _OutlineNoiseTex ("OutlineNoise", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue" = "Transparent+1" "RenderType" = "Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        Pass // Initial Pass for Color
        {
            Name "FirstPass"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            float4 _Color;
            float4 _Color2;
            uniform sampler2D _NoiseTex;

            // Spotlight properties
            float3 _SpotlightPosition;
            float3 _SpotlightDirection;
            float _SpotlightRange;
            float _SpotlightIntensity;
            float _SpotlightAngle;  // Spotlight cone angle (degrees)

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal: normal;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 worldPos: TEXCOORD1;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                // Make shadows very slightly smaller to prevent render collision with ethereal block
                v.vertex.xyz -= v.normal * 0.01;

                // Wavy positioning for a mirage-like effect on shadows
                // Only x required because that's the perspective the player views it from
                // Calculate distance from the spotlight position
                float3 difference = o.worldPos - _SpotlightPosition;

                // Calculate angle of the fragment relative to the spotlight direction
                float fragmentAngle = acos(dot(normalize(difference), normalize(_SpotlightDirection)));
                float spotlightAngle = radians(_SpotlightAngle) * 0.5;
                // If vertex is inside spotlight increase the waviness effect
                if (!(length(difference) > _SpotlightRange || fragmentAngle > spotlightAngle))
                {
                    v.vertex.x += sin(v.vertex.y + _Time * 50) * 0.5;
                } else{
                    v.vertex.x += sin(v.vertex.y + _Time * 20) * 0.03;
                }

                // Convert to clip pos and return
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float blend = tex2D(_NoiseTex, i.uv + _Time * 0.1).r;
                fixed4 col = lerp(_Color, _Color2, blend);
                return col;
            }
            ENDCG
        }

        // Second pass for outline
        Pass {
            Name "OutlinePass"
            Cull Front
            CGPROGRAM
            # pragma vertex vert
            # pragma fragment frag
            #include "UnityCG.cginc"

            float _OutlineWidth;
            float4 _OutlineColor;
            uniform sampler2D _OutlineNoiseTex;

            // Spotlight properties
            float3 _SpotlightPosition;
            float3 _SpotlightDirection;
            float _SpotlightRange;
            float _SpotlightIntensity;
            float _SpotlightAngle;  // Spotlight cone angle (degrees)

            struct MeshData 
            {
                float4 vertex: POSITION;
                float2 uv: TEXCOORD0;
                float3 normal: NORMAL;
                float3 smoothNormal: TEXCOORD1;
            };

            struct Interpolators
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Interpolators vert(MeshData v) {
                Interpolators o;
                v.vertex.xyz += v.smoothNormal * _OutlineWidth;


                // Replicating wavy effect on the outline to match the shadow
                // Calculate distance from the spotlight position
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                float3 difference = worldPos - _SpotlightPosition;

                // Calculate angle of the fragment relative to the spotlight direction
                float fragmentAngle = acos(dot(normalize(difference), normalize(_SpotlightDirection)));
                float spotlightAngle = radians(_SpotlightAngle) * 0.5;
                // If vertex is inside spotlight increase the waviness effect
                if (!(length(difference) > _SpotlightRange || fragmentAngle > spotlightAngle))
                {
                    v.vertex.x += sin(v.vertex.y + _Time * 50) * 0.5;
                } else{
                    v.vertex.x += sin(v.vertex.y + _Time * 20) * 0.03;
                }

                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag(Interpolators i): SV_Target {
                float offsety = _Time * 5;
                float alphaNoise = tex2D(_OutlineNoiseTex, i.uv * 4 + float2(0, offsety)).r;
                return float4(_OutlineColor.rgb, alphaNoise);
            }
            ENDCG
        }
        
    }
    FallBack "Diffuse"
}