Shader "Learning/SimpleEmission"
{
    Properties
    {
        _MainTex1 ("Albedo1 (RGB)", 2D) = "white" {}
        _MainTex2 ("Albedo2 (RGB)", 2D) = "white" {}
        _MainTex3 ("Albedo3 (RGB)", 2D) = "white" {}
        _MaskTex ("Mask (RGB)", 2D) = "white" {}
        
        _Color ("Emission color", Color) = (1,1,1,1)
        
//        _Vector ("Vector parameter", Vector) = (1.0, 0.5, 0.2, 0.7)
//        _Int ("Integer parameter", Int) = 2
//        _Float ("Float parameter", Float) = 1.0
//        _Range ("Range parameter", Range (-1, 2)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D
            _MainTex1,
            _MainTex2,
            _MainTex3,
            _MaskTex;

        fixed3 _Color;

        struct Input
        {
            float2 uv_MainTex1;
            float2 uv_MaskTex;
        };

        void surf (Input IN, inout SurfaceOutput outp)
        {
            fixed3 masks = tex2D(_MaskTex, IN.uv_MaskTex);
            fixed3 color = tex2D(_MainTex1, IN.uv_MainTex1) * masks.r;
            color += tex2D(_MainTex2, IN.uv_MainTex1) * masks.g;
            color += tex2D(_MainTex3, IN.uv_MainTex1) * masks.b;
            
            outp.Albedo = color;
            outp.Emission = _Color;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
