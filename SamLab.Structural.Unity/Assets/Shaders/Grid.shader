Shader "SamLab/CADGrid"
{
   Properties
   {
      _Color("Primary Grid Color", Color) = (0.5, 1.0, 1.0)
      _SecondaryColor("Secondary Grid Color", Color) = (0.0, 0.0, 0.0)
      _BackgroundColor("Background Color", Color) = (0.0, 0.0, 0.0, 0.0)
      _MaskTexture("Mask Texture", 2D) = "white" {}

      [Header(Grid Properties)]
      _Scale("Adaptive Grid Scale", Float) = 1.0
      _BaseGridSize("Base Grid Size", Float) = 1.0
      _SubdivisionFactor("Subdivision Factor", Float) = 10.0
      _Thickness("Lines Thickness", Range(0.0001, 0.01)) = 0.005
      _DistanceToCamera("Distance To Camera", Float) = 10.0
      _FadeStart("Subdivision Fade Start", Range(0.1, 0.9)) = 0.3
      _FadeEnd("Subdivision Fade End", Range(0.1, 0.9)) = 0.7
   }
   SubShader
   {
      Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
      LOD 100

      ZWrite On
      Blend SrcAlpha OneMinusSrcAlpha

      Pass
      {
         CGPROGRAM
         #pragma vertex vert
         #pragma fragment frag
         #pragma multi_compile _ UNITY_SINGLE_PASS_STEREO STEREO_INSTANCING_ON STEREO_MULTIVIEW_ON
         #include "UnityCG.cginc"

         struct appdata
         {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
            float2 uv1 : TEXCOORD1;
            UNITY_VERTEX_INPUT_INSTANCE_ID
         };

         struct v2f
         {
            float2 uv : TEXCOORD0;
            float2 worldPos : TEXCOORD1;
            float2 maskUV : TEXCOORD2;
            float4 vertex : SV_POSITION;
            UNITY_VERTEX_OUTPUT_STEREO
         };

         sampler2D _MaskTexture;
         float4 _MaskTexture_ST;

         float _Scale;
         float _BaseGridSize;
         float _SubdivisionFactor;
         float _Thickness;
         float _DistanceToCamera;
         float _FadeStart;
         float _FadeEnd;

         fixed4 _Color;
         fixed4 _SecondaryColor;
         fixed4 _BackgroundColor;

         v2f vert(appdata v)
         {
            v2f o;
            UNITY_SETUP_INSTANCE_ID(v);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
            o.vertex = UnityObjectToClipPos(v.vertex);
            
            // Convert object space to world space for proper grid coordinates
            float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
            o.worldPos = worldPos.xz; // Use XZ plane for grid
            
            // Regular UVs
            o.uv = v.uv - 0.5f;
            
            // Mask texture UVs
            o.maskUV = TRANSFORM_TEX(v.uv1, _MaskTexture);
            return o;
         }

         // Remap value from one range to another
         float remap(float value, float from1, float to1, float from2, float to2) {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
         }

         // Check if position is on a grid line
         bool isGridLine(float position, float cellSize, float thickness) {
            float normalizedPos = position / cellSize;
            float distToLine = min(frac(normalizedPos), 1.0 - frac(normalizedPos));
            float halfThick = thickness * 0.5;
            return distToLine < halfThick / cellSize;
         }

         fixed4 frag(v2f i) : SV_Target
         {
            fixed4 col = _BackgroundColor;
            fixed4 maskCol = tex2D(_MaskTexture, i.maskUV);
            
            // Calculate grid sizes based on adaptive scale
            float primaryGridSize = _Scale;
            float secondaryGridSize = primaryGridSize / _SubdivisionFactor;
            
            // Calculate fade factor for secondary grid
            // We want secondary grid to smoothly appear as we get closer
            float distanceFactor = 1.0 - saturate((_DistanceToCamera - _FadeStart) / (_FadeEnd - _FadeStart));
            
            // Calculate effective thickness based on distance
            float primaryThickness = _Thickness * primaryGridSize;
            float secondaryThickness = _Thickness * secondaryGridSize * distanceFactor;
            
            // Check if we're on primary grid lines
            if (isGridLine(i.worldPos.x, primaryGridSize, primaryThickness) || 
                isGridLine(i.worldPos.y, primaryGridSize, primaryThickness)) {
                col = _Color;
            }
            // Check if we're on secondary grid lines
            else if (isGridLine(i.worldPos.x, secondaryGridSize, secondaryThickness) || 
                     isGridLine(i.worldPos.y, secondaryGridSize, secondaryThickness)) {
                col = _SecondaryColor;
                col.a *= distanceFactor; // Fade secondary grid based on distance
            }
            
            // Apply mask
            col.a *= maskCol.a;
            return col;
         }
         ENDCG
      }
   }
}