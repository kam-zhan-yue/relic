Shader "Custom/GhostWall"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1) // Default white color
        _MainTex ("Albedo (RGB)", 2D) = "white" {} // Default texture
        _Transparency ("Transparency", Range(0, 1)) = 0.5 // Default transparency
        _NoiseTex ("Noise", 2D) = "white" {} // Noise Texture
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 100

        Pass
        {
            // Enable blending
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct vertIn
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct vertOut
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _Color;
            float _Transparency;
			uniform sampler2D _NoiseTex;
			float2 offset1 = float2(0.1,0);
			float2 offset2 = float2(0.2,0);

            vertOut vert (vertIn v)
            {
                vertOut o;
                o.vertex = v.vertex;
                // Offset vertex x (back and forth) using sin offsets of x and y
                o.vertex.x += sin(o.vertex.y + (_Time * 60.0)) * 0.05;
                o.vertex.x += cos(o.vertex.z + (_Time * 60.0)) * 0.05;
                // Convert to clip pos
                o.vertex = UnityObjectToClipPos(o.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag (vertOut i) : SV_Target
            {
				// add transparency and texture
                float offset = sin(i.uv.y + _Time * 0.05);
				float noise_color = tex2D(_NoiseTex, (i.uv + offset));
                half4 texColor = tex2D(_MainTex, i.uv);
				float4 finalColor = texColor * _Color * half4(1, 1, 1, _Transparency + noise_color);
				return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}