Shader "Custom/StarShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} // Main texture of the cube
        _Color ("Color", Color) = (1,1,1,1) // Color of the cube
        _GlowColor ("Glow Color", Color) = (1,0.925,0.6,1) // Yellowish glow color
        _GlowStrength ("Glow Strength", Range(0, 1)) = 0.5 // Strength of the glow effect
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert
        
        sampler2D _MainTex;
        fixed4 _Color;
        fixed4 _GlowColor;
        half _GlowStrength;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Set the base color of the cube
            o.Albedo = _Color.rgb;

            // Add glow effect around the cube
            fixed4 glow = tex2D(_MainTex, IN.uv_MainTex) * _GlowColor;
            o.Emission = glow.rgb * _GlowStrength;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
