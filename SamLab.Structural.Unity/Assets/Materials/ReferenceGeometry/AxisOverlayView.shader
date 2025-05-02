Shader "Unlit/AxisOverlayView"
{
    Properties
    {
        _Color ("Color", Color) = (0,0,0,1) // Changed _Name to _Color
    }
    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Tags
        {
            "Queue" = "Overlay"
        } // Changed "Overlay" = "4000" to "Queue" = "Overlay"

        LOD 100
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
                fixed4 color : COLOR;
            };

            fixed4 _Color; // Declare the color property

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); // Transform vertex
                o.uv = v.uv;
                o.color = _Color; // Pass color to fragment shader
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return i.color; // Output the color
            }
            ENDCG
        }
    }
}