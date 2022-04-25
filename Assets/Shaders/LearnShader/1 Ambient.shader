Shader "Learning/Ambient"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
 
        [Header(Ambient)]
        _Ambient ("Intensity", Range(0., 1.)) = 0.1
        _AmbColor ("Color", color) = (1., 1., 1., 1.)
    }
 
    SubShader
    {
        Pass
        {
            Tags {
                "RenderType"="Transparent"
                "Queue"="Geometry"
                "LightMode"="ForwardBase"
            }
 
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
  
            #include "UnityCG.cginc"
 
            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                fixed4 light : COLOR0;
            };
 
            fixed _Ambient;
            fixed4 _AmbColor;
 
            v2f vert(appdata_base v)
            {
                v2f o;
 
                // Clip position
                o.pos = UnityObjectToClipPos(v.vertex);
 
                // Compute ambient lighting
                fixed4 amb = _Ambient * _AmbColor;
 
                o.light = amb;
               
                o.uv = v.texcoord;
                return o;
            }
 
            sampler2D _MainTex;

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 c = tex2D(_MainTex, i.uv);
                c.rgb *= i.light;
                return c;
            }
 
            ENDCG
        }
    }
    FallBack "Diffuse"
}