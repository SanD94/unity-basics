Shader "Graph/Point Surface"
{
    SubShader
    {
        Tags 
        { 
            "RenderPipeline" = "UniversalPipeline"
        }
        
        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        ENDHLSL
        
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float3 worldPos;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            o.Smoothness = 0.5;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
