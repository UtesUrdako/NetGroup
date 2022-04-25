Shader "Learning/MaskLambert"
{
    Properties
    {
        _MainTex1 ("Albedo1 (RGB)", 2D) = "white" {}
        _MainTex2 ("Albedo2 (RGB)", 2D) = "white" {}
        _MainTex3 ("Albedo3 (RGB)", 2D) = "white" {}
        _MaskTex ("Mask (RGB)", 2D) = "white" {}
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

        struct Input
        {
            float2 uv_MainTex1;
            float2 uv_MainTex2;
            float2 uv_MainTex3;
            float2 uv_MaskTex;
        };

        void surf (Input IN, inout SurfaceOutput outp)
        {
            fixed3 masks = tex2D(_MaskTex, IN.uv_MaskTex);
            fixed3 color = tex2D(_MainTex1, IN.uv_MainTex1) * masks.r;
            color += tex2D(_MainTex2, IN.uv_MainTex2) * masks.g;
            color += tex2D(_MainTex3, IN.uv_MainTex3) * masks.b;
            
            outp.Albedo = color;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
