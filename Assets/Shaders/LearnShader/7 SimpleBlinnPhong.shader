Shader "Learning/SimpleBlinnPhong"
{
    Properties
    {
        _MainTex1 ("Albedo1 (RGB)", 2D) = "white" {}
        _MainTex2 ("Albedo2 (RGB)", 2D) = "white" {}
        _MainTex3 ("Albedo3 (RGB)", 2D) = "white" {}
        
        _MaskTex ("Mask (RGB)", 2D) = "white" {}
        
        _EmissionMskTex ("Emission mask", 2D) = "black" {}
        _EmissionColor ("Emission color", Color) = (1,1,1,1)
        
        _EmissionAppearance ("Emission appearance", Range(0, 1)) = 1
        
        _BumpMap ("Normal map", 2D) = "bump" {}
        _StrongNormal ("_Strong normal", Range (0.0, 10)) = 1.0
        
        _SpecColor ("Specular color", Color) = (0.5, 0.5, 0.5, 1.0) //В коде объявлять не надо.
        _Specular ("Specular", Range(0.005, 5)) = 0.07
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf BlinnPhong

        sampler2D
            _MainTex1,
            _MainTex2,
            _MainTex3,
            _MaskTex,
            _EmissionMskTex,
            _BumpMap
        ;

        fixed3 _EmissionColor;
        fixed _EmissionAppearance;
        float _StrongNormal;
        half _Specular;

        struct Input
        {
            float2 uv_MainTex1;
            float2 uv_MaskTex;
            float2 uv_BumpMap;
            float2 uv_EmissionMskTex;
        };

        void surf (Input IN, inout SurfaceOutput outp)
        {
            fixed3 masks = tex2D(_MaskTex, IN.uv_MaskTex).rgb;
            fixed3 color = tex2D(_MainTex1, IN.uv_MainTex1).rgb * masks.r;
            color += tex2D(_MainTex2, IN.uv_MainTex1) * masks.g;
            color += tex2D(_MainTex3, IN.uv_MainTex1) * masks.b;
            
            outp.Albedo = color;

            fixed3 emTex = tex2D(_EmissionMskTex, IN.uv_MaskTex).rgb;

            half appearMask = emTex.b;
            appearMask = smoothstep(
                _EmissionAppearance * 1.2,
                _EmissionAppearance * 1.2 - 0.2,
                appearMask);
            
            outp.Emission = appearMask * emTex.r * _EmissionColor;
            float4 normal = tex2D(_BumpMap, IN.uv_BumpMap);
            //outp.Normal = UnpackNormal(normal);
            outp.Normal = UnpackScaleNormal(normal, _StrongNormal);
            outp.Specular = _Specular;
            outp.Gloss = 1.0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
