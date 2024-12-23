Shader "Custom/StencilShader"
{
    Properties
    {
        [IntRange] _StencilID ("Stencil ID", Range(0, 255)) = 0
    }
    SubShader
    {
        Tags 
        { 
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Geometry"
        }
        Pass
        {
            // for the colour of this pixel, take 0% of the colour output of this shader and 100% of the colour already rendered at this pixel
            Blend Zero One
            // prevents the shader from writing to the depth buffer, which would screw the rendering of the object
            ZWrite Off
            
            Stencil
            {
                Ref [_StencilID]
                // the stencil pass always passes                
                Comp Always
                Pass Replace
                Fail Keep
            }
        }
    }
    FallBack "Diffuse"
}
