Shader "Custom/InvisibleStencilMask"
{
    SubShader
    {
        // Render early so that the stencil is set before other objects.
        Tags { "Queue"="Geometry-10" "RenderType"="Transparent" }
        // We want the mask to be invisible.
        ZWrite On
        ColorMask 0

        Stencil
        {
            Ref 1         // The reference value that will be used by other shaders.
            Comp Always   // Always pass the stencil test.
            Pass Replace  // Replace the existing stencil value with Ref.
        }

        Pass { }
    }
}
