Shader "Custom/Mirror"
{
    Properties
    {
        // [MainColor]_BaseColor("Base Color", Color) = (1, 1, 1, 1)
        [MainTexture]_RenderTexture("Render Texture", 2D) = "write" {}
        _OverlayTexture ("Overlay", 2D) = "white" {} 
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
        Cull Back

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float4 screenPos : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };

            TEXTURE2D(_RenderTexture);
            SAMPLER(sampler_RenderTexture);

            TEXTURE2D(_OverlayTexture);
            SAMPLER(sampler_OverlayTexture);

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                float4 _RenderTexture_ST;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.screenPos = ComputeScreenPos(OUT.positionHCS);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float2 screenSpaceUv = IN.screenPos.xy / IN.screenPos.w;
                half4 color = SAMPLE_TEXTURE2D(_RenderTexture, sampler_RenderTexture, float2(1.0 - screenSpaceUv.x, screenSpaceUv.y));
                half4 overlay = SAMPLE_TEXTURE2D(_OverlayTexture, sampler_OverlayTexture, IN.uv);

                float4 emissionColor = float4(1, 1, 1, 1) * overlay.a;

                return color + overlay * emissionColor;
            }
            ENDHLSL
        }
    }

    Fallback "Standard" // for shadows
}
