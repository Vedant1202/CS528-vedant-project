Shader "Custom/GlowyStarCube"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} // Main texture of the cube
        _Color ("Color", Color) = (1,1,1,1) // Color of the cube
        _GlowColor ("Glow Color", Color) = (1,1,0,1) // Glow color
        _GlowStrength ("Glow Strength", Range(0, 1)) = 0.5 // Strength of the glow effect
        _WaveSpeed ("Wave Speed", float) = 1.0 // Speed of the wave animation
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
        float _WaveSpeed;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Set the base color of the cube
            o.Albedo = _Color.rgb;

            // Calculate glow effect based on distance from cube center
            float glowStrength = smoothstep(0.9, 1.0, length(IN.worldPos));

            // Calculate wave distortion
            float waveDistortion = sin(_WaveSpeed * _Time.y + IN.worldPos.x + IN.worldPos.y + IN.worldPos.z);

            // Apply glow and wave effects
            fixed4 glowColor = _GlowColor * _GlowStrength * glowStrength;
            o.Emission = glowColor.rgb;
            o.Normal += waveDistortion * 0.1; // Adjust the strength of the wave effect

            // Sample the main texture
            fixed4 mainTexColor = tex2D(_MainTex, IN.uv_MainTex);

            // Combine the main texture color with the glow effect
            o.Emission += mainTexColor.rgb * glowColor.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
