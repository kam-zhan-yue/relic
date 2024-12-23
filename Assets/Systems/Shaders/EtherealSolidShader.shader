Shader "Custom/EtherealSolidShader"
{
    Properties
    {
        _MainTex("Albedo", 2D) = "white" {}  // Main texture
        _EtherealColor ("Ethereal Color", Color) = (0, 0, 0, 1) // Transparency control
        _SolidColor ("Solid Color", Color) = (0, 0, 0, 1)
        _SymbolColour ("Symbol Color", Color) = (1, 0, 0, 1)
        _NoiseTex ("NoiseTex", 2D) = "white" {}  // Noise texture

        _OutlineColor ("OutlineColor", Color) = (1,1,1,1)  // Outline color (if needed)
        _OutlineWidth ("OutlineWidth", Range(0, 0.5)) = 0.05  // Outline width (if needed)
        _OutlineNoiseTex ("OutlineNoise", 2D) = "white" {}  // Noise texture for outline
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows alpha:keepalpha
        #include "UnityCG.cginc"

        sampler2D _MainTex;  // Main texture
        sampler2D _NoiseTex;  // Noise texture for additional effects
        float4 _SymbolColour;  // Solid color for tint
        float4 _EtherealColor;  // Ethereal Color
        float4 _SolidColor;

        // Spotlight properties
        float3 _SpotlightPosition;
        float3 _SpotlightDirection;
        float _SpotlightRange;
        float _SpotlightIntensity;
        float _SpotlightAngle;  // Spotlight cone angle (degrees)

        struct Input
        {
            float2 uv_MainTex;  // UV coordinates for texture sampling
            float3 worldPos;    // World position of the fragment
        };

        // Surface function to calculate lighting and transparency
        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            // Sample the main texture and set base color
            fixed4 col = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = col.rgb;
            
            // If the color is white, replace with symbol colour and add emissive property;
            // If color is black, replace with solid material color.
            float luminance = dot(col.rgb, float3(0.299, 0.587, 0.114));
            if (luminance >= 0.9)
            {
                o.Albedo = _SymbolColour;
                o.Emission = _SymbolColour;
            } else if (luminance <= 0.01) {
                o.Albedo = _SolidColor;
            }

            // Calculate distance from the spotlight position
            float3 difference = IN.worldPos - _SpotlightPosition;

            // Calculate angle of the fragment relative to the spotlight direction
            float fragmentAngle = acos(dot(normalize(difference), normalize(_SpotlightDirection)));
            float spotlightAngle = radians(_SpotlightAngle) * 0.5;
            
            // If OUTSIDE spotlight range or angle discard fragment pixel
            if (length(difference) > _SpotlightRange || fragmentAngle > spotlightAngle)
            {
                o.Albedo = _EtherealColor.rgb;
                discard;
                // return;
            }
        }
        ENDCG
    }

    FallBack "Diffuse"
}