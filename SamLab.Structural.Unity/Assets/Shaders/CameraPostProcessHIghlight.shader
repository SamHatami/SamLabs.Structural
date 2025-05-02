Shader "Camera/ScreenSpaceOutlineRoundedCamera"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} // Not directly used, but good practice
        _OutlineColor ("Outline Color", Color) = (1, 0.5, 0, 1)
        _OutlineThickness ("Outline Thickness", Range(0, 10)) = 1
        _OutlineThreshold ("Outline Threshold", Range(0, 1)) = 0.1
        _OutlineSmoothness ("Outline Smoothness", Range(0, 1)) = 0.1
        _OutlineDepthSensitivity ("Depth Sensitivity", Range(0, 10)) = 1
        _OutlineNormalSensitivity ("Normal Sensitivity", Range(0, 10)) = 1
    }
    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        // Pass 1: Edge Detection (Outputs a grayscale edge mask)
        Pass
        {
            Name "EdgeDetection"

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _CameraDepthNormalsTexture; // Depth and Normals
            float4 _MainTex_TexelSize;
            float _OutlineThickness;
            float _OutlineThreshold;
            float _OutlineSmoothness;
            float _OutlineDepthSensitivity;
            float _OutlineNormalSensitivity;

            void GetDepthAndNormal(float2 uv, out float depth, out float3 normal)
            {
                float4 cdn = tex2D(_CameraDepthNormalsTexture, uv);
                depth = DecodeFloatRG(cdn.zw);
                normal = DecodeViewNormalStereo(cdn);
            }

            float SobelSample(float2 uv)
            {
                float2 delta = _MainTex_TexelSize.xy * _OutlineThickness;

                float depthCenter, depthTop, depthBottom, depthLeft, depthRight;
                float3 normalCenter, normalTop, normalBottom, normalLeft, normalRight;

                GetDepthAndNormal(uv, depthCenter, normalCenter);
                GetDepthAndNormal(uv + float2(0, delta.y), depthTop, normalTop);
                GetDepthAndNormal(uv + float2(0, -delta.y), depthBottom, normalBottom);
                GetDepthAndNormal(uv + float2(-delta.x, 0), depthLeft, normalLeft);
                GetDepthAndNormal(uv + float2(delta.x, 0), depthRight, normalRight);

                float depthDifference = 0;
                depthDifference += abs(depthCenter - depthTop) * _OutlineDepthSensitivity;
                depthDifference += abs(depthCenter - depthBottom) * _OutlineDepthSensitivity;
                depthDifference += abs(depthCenter - depthLeft) * _OutlineDepthSensitivity;
                depthDifference += abs(depthCenter - depthRight) * _OutlineDepthSensitivity;

                float normalDifference = 0;
                normalDifference += length(normalCenter - normalTop) * _OutlineNormalSensitivity;
                normalDifference += length(normalCenter - normalBottom) * _OutlineNormalSensitivity;
                normalDifference += length(normalCenter - normalLeft) * _OutlineNormalSensitivity;
                normalDifference += length(normalCenter - normalRight) * _OutlineNormalSensitivity;

                return (depthDifference + normalDifference);
            }


            fixed4 frag(v2f i) : SV_Target
            {
                float edge = SobelSample(i.uv);
                edge = smoothstep(_OutlineThreshold - _OutlineSmoothness * 0.5,
                                _OutlineThreshold + _OutlineSmoothness * 0.5, edge);
                return fixed4(edge, edge, edge, 1); // Grayscale edge mask
            }
            ENDCG
        }

        // Pass 2: Blur and Apply Outline
        Pass
        {
            Name "BlurAndApply"

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex; // This will be the temporary RT from Pass 1
            sampler2D _SourceTex; // This will be the original scene render
            float4 _MainTex_TexelSize; // Texel size of the edge mask
            float4 _OutlineColor;
            float _OutlineThickness; // You might want separate thickness control here

            fixed4 frag(v2f i) : SV_Target
            {
                // Simple box blur (3x3) -  you can replace with a better blur
                float edge = 0;
                float2 delta = _MainTex_TexelSize.xy * _OutlineThickness; // Use _MainTex_TexelSize (edge mask size)

                // This is more accurate than a 3x3, but it's a 5x5 blur.
                float weights[5] = {0.06136, 0.24477, 0.38774, 0.24477, 0.06136}; // Gaussian weights, normalized
                edge += tex2D(_MainTex, i.uv + float2(-2.0 * delta.x, -2.0 * delta.y)).r * weights[0] * weights[0];
                edge += tex2D(_MainTex, i.uv + float2(-1.0 * delta.x, -2.0 * delta.y)).r * weights[1] * weights[0];
                edge += tex2D(_MainTex, i.uv + float2(0.0, -2.0 * delta.y)).r * weights[2] * weights[0];
                edge += tex2D(_MainTex, i.uv + float2(1.0 * delta.x, -2.0 * delta.y)).r * weights[3] * weights[0];
                edge += tex2D(_MainTex, i.uv + float2(2.0 * delta.x, -2.0 * delta.y)).r * weights[4] * weights[0];

                edge += tex2D(_MainTex, i.uv + float2(-2.0 * delta.x, -1.0 * delta.y)).r * weights[0] * weights[1];
                edge += tex2D(_MainTex, i.uv + float2(-1.0 * delta.x, -1.0 * delta.y)).r * weights[1] * weights[1];
                edge += tex2D(_MainTex, i.uv + float2(0.0, -1.0 * delta.y)).r * weights[2] * weights[1];
                edge += tex2D(_MainTex, i.uv + float2(1.0 * delta.x, -1.0 * delta.y)).r * weights[3] * weights[1];
                edge += tex2D(_MainTex, i.uv + float2(2.0 * delta.x, -1.0 * delta.y)).r * weights[4] * weights[1];

                edge += tex2D(_MainTex, i.uv + float2(-2.0 * delta.x, 0.0)).r * weights[0] * weights[2];
                edge += tex2D(_MainTex, i.uv + float2(-1.0 * delta.x, 0.0)).r * weights[1] * weights[2];
                edge += tex2D(_MainTex, i.uv + float2(0.0, 0.0)).r * weights[2] * weights[2];
                edge += tex2D(_MainTex, i.uv + float2(1.0 * delta.x, 0.0)).r * weights[3] * weights[2];
                edge += tex2D(_MainTex, i.uv + float2(2.0 * delta.x, 0.0)).r * weights[4] * weights[2];

                edge += tex2D(_MainTex, i.uv + float2(-2.0 * delta.x, 1.0 * delta.y)).r * weights[0] * weights[3];
                edge += tex2D(_MainTex, i.uv + float2(-1.0 * delta.x, 1.0 * delta.y)).r * weights[1] * weights[3];
                edge += tex2D(_MainTex, i.uv + float2(0.0, 1.0 * delta.y)).r * weights[2] * weights[3];
                edge += tex2D(_MainTex, i.uv + float2(1.0 * delta.x, 1.0 * delta.y)).r * weights[3] * weights[3];
                edge += tex2D(_MainTex, i.uv + float2(2.0 * delta.x, 1.0 * delta.y)).r * weights[4] * weights[3];

                edge += tex2D(_MainTex, i.uv + float2(-2.0 * delta.x, 2.0 * delta.y)).r * weights[0] * weights[4];
                edge += tex2D(_MainTex, i.uv + float2(-1.0 * delta.x, 2.0 * delta.y)).r * weights[1] * weights[4];
                edge += tex2D(_MainTex, i.uv + float2(0.0, 2.0 * delta.y)).r * weights[2] * weights[4];
                edge += tex2D(_MainTex, i.uv + float2(1.0 * delta.x, 2.0 * delta.y)).r * weights[3] * weights[4];
                edge += tex2D(_MainTex, i.uv + float2(2.0 * delta.x, 2.0 * delta.y)).r * weights[4] * weights[4];


                fixed4 originalColor = tex2D(_SourceTex, i.uv); // Sample the original scene color
                return lerp(originalColor, _OutlineColor, edge);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}