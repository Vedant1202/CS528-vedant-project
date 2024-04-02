Shader "Custom/StarShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _Threshold ("Threshold", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        
        CGPROGRAM
        #pragma surface surf Lambert
        
        struct Input
        {
            float2 uv_MainTex;
        };
        
        sampler2D _MainTex;
        fixed4 _Color;
        float _Threshold;
        
        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 texColor = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            if (texColor.a < _Threshold)
                discard;
            o.Albedo = texColor.rgb;
            o.Alpha = texColor.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
