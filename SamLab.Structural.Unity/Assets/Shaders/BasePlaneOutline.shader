Shader "Custom/TwoSidedTransparentBorder"
{
    Properties
    {
        _MainColor ("Center Color", Color) = (0.5, 0.5, 0.5, 0.2)
        _BorderColor ("Border Color", Color) = (1, 1, 1, 1)
        _BorderSize ("Border Size", Range(0, 0.5)) = 0.05
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True"
        }
        LOD 100

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off // This disables backface culling, making the shader two-sided

        Pass
        {
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

            float4 _MainColor;
            float4 _BorderColor;
            float _BorderSize;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Convert UV to distance from edge
                float2 centered = abs(i.uv - 0.5) * 2; // Convert 0-1 to distance from center
                float maxDist = max(centered.x, centered.y);

                // If near edge, use border color
                if (maxDist > (1.0 - _BorderSize * 2))
                {
                    return _BorderColor;
                }
                else
                {
                    return _MainColor;
                }
            }
            ENDCG
        }
    }
}