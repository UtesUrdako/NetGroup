Shader "Learning/MaskBlinnPhong"
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
        
        _SpecColor ("Specular color", Color) = (1.0, 1.0, 1.0, 1.0) //В коде объявлять не надо.
        
        _Shiness1 ("Shiness 1", Range(0.005, 5)) = 0.07
        _Shiness2 ("Shiness 2", Range(0.005, 5)) = 0.07
        _Shiness3 ("Shiness 3", Range(0.005, 5)) = 0.07
        
        _Specularity1 ("Specularity 1", Range(0, 1)) = 0.5
        _Specularity2 ("Specularity 2", Range(0, 1)) = 0.5
        _Specularity3 ("Specularity 3", Range(0, 1)) = 0.5
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
        fixed
            _EmissionAppearance,
            _Specularity1,
            _Specularity2,
            _Specularity3
        ;
        float _StrongNormal;
        half
            _Shiness1,
            _Shiness2,
            _Shiness3
        ;

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
            outp.Specular =
                _Shiness1 * masks.r +
                _Shiness2 * masks.g +
                _Shiness3 * masks.b;
            outp.Gloss =
                _Specularity1 * masks.r +
                _Specularity2 * masks.g +
                _Specularity3 * masks.b;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
