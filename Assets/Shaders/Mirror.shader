Shader "Mirror/Lit"
{
    Properties
    {
        [MainTexture]_BaseMap("Render Texture", 2D) = "write" {}
        _OverlayTexture ("Overlay", 2D) = "white" {} 
        _Depth("Depth", float) = 0 
        _EmissionColor("Emission Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "UniversalMaterialType" = "SimpleLit" }

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

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
                half3 lightAmount : TEXCOORD2;
                float4 shadowCoords : TEXCOORD3;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            TEXTURE2D(_OverlayTexture);
            SAMPLER(sampler_OverlayTexture);

            CBUFFER_START(UnityPerMaterial)
                float _Depth;
                float4 _EmissionColor;
                float4 _BaseMap_ST;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.screenPos = ComputeScreenPos(OUT.positionHCS);
                OUT.uv = IN.uv;

                VertexNormalInputs positions = GetVertexNormalInputs(IN.positionOS);

                VertexPositionInputs shadowPositions = GetVertexPositionInputs(IN.positionOS.xyz);
                float4 shadowCoordinates = GetShadowCoord(shadowPositions);

                Light light = GetMainLight(shadowCoordinates);
                OUT.lightAmount = LightingLambert(light.color, light.direction, positions.normalWS.xyz);

                int lightCount = GetAdditionalLightsCount();
                for (int i = 0; i < lightCount; i++)
                {
                    Light light = GetAdditionalLight(i, positions.normalWS.xyz);
                    OUT.lightAmount += LightingLambert(light.color, light.direction, positions.normalWS.xyz);
                }

                OUT.shadowCoords = shadowCoordinates;

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float2 screenSpaceUv = IN.screenPos.xy / IN.screenPos.w;
                half4 color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, float2(1.0 - screenSpaceUv.x, screenSpaceUv.y));
                half4 overlay = SAMPLE_TEXTURE2D(_OverlayTexture, sampler_OverlayTexture, IN.uv);

                float4 emissionColor = _EmissionColor * overlay.a;
                half4 depthColor = float4(0.0, 1.0, 0.0, 1.0);

                half shadowAmount = MainLightRealtimeShadow(IN.shadowCoords);

                return (color * float4(IN.lightAmount, 1) * shadowAmount + overlay * emissionColor) * _Depth;
            }
            ENDHLSL
        }
    }
}


