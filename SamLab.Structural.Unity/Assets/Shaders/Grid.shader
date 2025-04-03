Shader "Custom/CameraBasedGridURP"
{
    Properties
    {
        _GridColorThin ("Thin Grid Color", Color) = (0.5, 0.5, 0.5, 1)
        _GridColorThick ("Thick Grid Color", Color) = (0, 0, 0, 1)
        _GridCellSize ("Grid Cell Size", Float) = 0.025
        _OpacityFalloff ("Opacity Falloff", Float) = 100
    }

    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha // ✅ Enables transparency
            ZWrite Off // ✅ Prevents depth write (important for transparency)

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct VertexInput {
                float3 position : POSITION;
            };

            struct VertexOutput {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            float4 _GridColorThin;
            float4 _GridColorThick;
            float _GridCellSize;
            float _OpacityFalloff;
            float4 _CameraPosition; // 💡 We use this to move the grid

            VertexOutput vert(VertexInput v)
            {
                VertexOutput o;
                o.worldPos = TransformObjectToWorld(v.position);
                o.pos = TransformObjectToHClip(v.position);
                return o;
            }

            float4 frag(VertexOutput IN) : SV_Target
            {
                float2 worldGridPos = (IN.worldPos.xz - _CameraPosition.xz); // Move with camera
                float cellSize = _GridCellSize * _CameraPosition.w; // Scale with zoom (w = camera size)

                float2 gridUV = abs(frac(worldGridPos / cellSize) - 0.5) * 2.0;
                float gridFactor = max(gridUV.x, gridUV.y); // Grid line strength

                float4 color = lerp(_GridColorThin, _GridColorThick, step(0.9, gridFactor));
                color.a *= smoothstep(0.1, 0.2, gridFactor); // Smooth grid edges

                float distanceFade = saturate(1.0 - length(worldGridPos) / _OpacityFalloff);
                color.a *= distanceFade; // Grid fades out with distance

                return color;
            }
            ENDHLSL
        }
    }
}
