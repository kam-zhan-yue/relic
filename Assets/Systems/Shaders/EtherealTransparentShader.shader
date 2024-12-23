Shader "Custom/EtherealTransparentShader"
{
    Properties
    {
        _Color ("Color", Color) = (0, 0, 0, 1)
        _Color2 ("Color2", Color) = (1, 1, 1, 1)
        _NoiseTex ("NoiseTex", 2D) = "white" {}
        _Transparency ("Transparency", Range(0, 1)) = 0.7 

        _SymbolTex("SymbolTex", 2D) = "black" {}  // Main texture
        _SymbolColour("SymbolColour", Color) = (0,0,0,1)
    }
    SubShader
    {
        Tags {"RenderType" = "Transparent"  "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass // Simple Transparent Shader
        {
            Name "FirstPass"
            Offset 1, 1
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            float4 _Color;
            float4 _Color2;
            uniform sampler2D _NoiseTex;
            float _Transparency;

            float4 _SymbolColour;
            uniform sampler2D _SymbolTex;

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
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {

                // Calculate distance from the spotlight position
                float3 difference = i.worldPos - _SpotlightPosition;

                // Calculate angle of the fragment relative to the spotlight direction
                float fragmentAngle = acos(dot(normalize(difference), normalize(_SpotlightDirection)));
                float spotlightAngle = radians(_SpotlightAngle) * 0.5;
                
                // If INSIDE spotlight range or angle discard the fragment pixel and exit
                if (!(length(difference) > _SpotlightRange || fragmentAngle > spotlightAngle))
                {
                    discard;
                }
                
                // If OUTSIDE spotlight range, continue
                // Sample Symbol texture at UV Coordinates
                fixed4 col = tex2D(_SymbolTex, i.uv);
                // If the texture color is white, replace with symbol colour, otherwise just use transparency with noise;
                if (col.r >= 0.90 && col.g >= 0.9 && col.b >= 0.9)
                {
                    col = _SymbolColour;
                    col.a = _Transparency;
                } else {
                    // Get blend between two colors and return
                    float blend = tex2D(_NoiseTex, i.uv + _Time * 0.1).r;
                    col = lerp(_Color, _Color2, blend);
                    // Set Transparency
                    col.a = _Transparency;
                }
                return col;
            }
            ENDCG
        }
        
    }
    FallBack "Diffuse"
}