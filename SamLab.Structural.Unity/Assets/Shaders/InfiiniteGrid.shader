Shader "Custom/InfiniteGrid"
{
    //SOURCE: https://github.com/emeiri/ogldev/tree/master/DemoLITION/Framework/Shaders/GL
    //Reworked with Claude Sonnet 3.7 to work with Unity.

    Properties
    {
        _GridSize ("Grid Size", Float) = 100.0
        _GridMinPixelsBetweenCells ("Grid Min Pixels Between Cells", Float) = 2.0
        _GridCellSize ("Grid Cell Size", Float) = 0.025
        _GridColorThin ("Grid Color Thin", Color) = (0.5, 0.5, 0.5, 1.0)
        _GridColorThick ("Grid Color Thick", Color) = (0.0, 0.0, 0.0, 1.0)
        _OriginColor ("Origin Axes Color", Color) = (1.0, 0.0, 0.0, 1.0)
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent" "RenderType"="Transparent"
        }
        LOD 100

        ZWrite On
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float4 lightSpacePos : TEXCOORD1;
                UNITY_FOG_COORDS(2)
                UNITY_VERTEX_OUTPUT_STEREO
            };

            float _GridSize;
            float _GridMinPixelsBetweenCells;
            float _GridCellSize;
            float4 _GridColorThin;
            float4 _GridColorThick;
            float _ShadowsEnabled;
            float4 _OriginColor;
            sampler2D _ShadowMap;

            // Unity automatically provides _WorldSpaceCameraPos

            v2f vert(appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                // Create a grid centered at camera xz position
                // Use a fixed grid size that doesn't change with camera properties
                float3 worldPos = v.vertex.xyz * (_GridSize + abs(_WorldSpaceCameraPos.y) * 100);
                worldPos.x += _WorldSpaceCameraPos.x;
                worldPos.z += _WorldSpaceCameraPos.z;
                worldPos.y = 0; // Keep it flat on y=0

                o.pos = UnityWorldToClipPos(worldPos);
                o.worldPos = worldPos;

                UNITY_TRANSFER_FOG(o, o.pos);
                return o;
            }

            float log10(float x)
            {
                return log(x) / log(10.0);
            }

            float satf(float x)
            {
                return clamp(x, 0.0, 1.0);
            }

            float2 satv(float2 x)
            {
                return clamp(x, float2(0.0, 0.0), float2(1.0, 1.0));
            }

            float max2(float2 v)
            {
                return max(v.x, v.y);
            }

            bool IsNearOriginAxes(float2 worldPos, float2 dudv)
            {
                // Detect proximity to X and Z axes through origin
                // Use a tighter threshold for the origin lines
                float thresholdX = dudv.x * 1.1;
                float thresholdZ = dudv.y * 1.1;

                // Check if we're near the X axis (z ≈ 0)
                bool nearXAxis = abs(worldPos.y) < thresholdZ;

                // Check if we're near the Z axis (x ≈ 0)
                bool nearZAxis = abs(worldPos.x) < thresholdX;

                return nearXAxis || nearZAxis;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Calculate derivatives for LOD calculations
                float2 dvx = float2(ddx(i.worldPos.x), ddy(i.worldPos.x));
                float2 dvy = float2(ddx(i.worldPos.z), ddy(i.worldPos.z));

                float lx = length(dvx);
                float ly = length(dvy);

                float2 dudv = float2(lx, ly);
                float l = length(dudv);

                // Calculate LOD based on screen-space derivatives, with a minimum to prevent disappearing when close
                float LOD = max(0.0, log10(max(l, 0.001) * _GridMinPixelsBetweenCells / _GridCellSize) + 1.0);

                float gridCellSizeLod0 = _GridCellSize * pow(10.0, floor(LOD));
                float gridCellSizeLod1 = gridCellSizeLod0 * 10.0;
                float gridCellSizeLod2 = gridCellSizeLod1 * 10.0;

                dudv *= 2.0;

                // Calculate grid lines for different LOD levels - using abs(fmod) to handle negative coordinates
                // For negative coordinates, we need to handle the modulo differently
                float2 world_pos_mod0 = i.worldPos.xz;
                float2 world_pos_mod1 = i.worldPos.xz;
                float2 world_pos_mod2 = i.worldPos.xz;

                // Custom modulo that works correctly with negative numbers
                world_pos_mod0 = world_pos_mod0 - gridCellSizeLod0 * floor(world_pos_mod0 / gridCellSizeLod0);
                world_pos_mod1 = world_pos_mod1 - gridCellSizeLod1 * floor(world_pos_mod1 / gridCellSizeLod1);
                world_pos_mod2 = world_pos_mod2 - gridCellSizeLod2 * floor(world_pos_mod2 / gridCellSizeLod2);

                float2 mod_div_dudv = world_pos_mod0 / dudv;
                float Lod0a = max2(float2(1.0, 1.0) - abs(satv(mod_div_dudv) * 2.0 - float2(1.0, 1.0)));

                mod_div_dudv = world_pos_mod1 / dudv;
                float Lod1a = max2(float2(1.0, 1.0) - abs(satv(mod_div_dudv) * 2.0 - float2(1.0, 1.0)));

                mod_div_dudv = world_pos_mod2 / dudv;
                float Lod2a = max2(float2(1.0, 1.0) - abs(satv(mod_div_dudv) * 2.0 - float2(1.0, 1.0)));

                float LOD_fade = frac(LOD);
                float4 color;

                bool nearOrigin = IsNearOriginAxes(i.worldPos.xz, dudv);

                // Choose grid line color based on LOD level and origin proximity
                if (nearOrigin)
                {
                    // Use origin color for the center axes
                    _GridColorThick = _OriginColor;
                }

                if (Lod2a > 0.0)
                {
                    color = _GridColorThick;
                    color.a *= Lod2a;
                }
                else
                {
                    if (Lod1a > 0.0)
                    {
                        color = lerp(_GridColorThick, _GridColorThin, LOD_fade);
                        color.a *= Lod1a;
                    }
                    else
                    {
                        color = _GridColorThin;
                        color.a *= (Lod0a * (1.0 - LOD_fade));
                    }
                }


                // Apply opacity falloff based on distance from camera, with a minimum opacity for close distances
                float distanceToCamera = length(i.worldPos.xyz - _WorldSpaceCameraPos.xyz);
                float opacityFalloff = (1.0 - satf(distanceToCamera / _GridSize));

                // Ensure minimum opacity when very close to prevent disappearing
                float minOpacity = 0.3;
                float closeDistance = _GridSize * 0.01; // 1% of grid size is considered "close"
                float closeOpacityFactor = 1.0;

                if (distanceToCamera < closeDistance)
                {
                    // Smoothly blend opacity as we get closer
                    closeOpacityFactor = lerp(minOpacity, 1.0, distanceToCamera / closeDistance);
                }

                color.a *= max(opacityFalloff, closeOpacityFactor);


                // Apply fog
                UNITY_APPLY_FOG(i.fogCoord, color);

                return color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}