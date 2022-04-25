Shader "Learning/Diffuse"
    {
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
 
        [Header(Ambient)]
        _Ambient ("Intensity", Range(0., 1.)) = 0.1
        _AmbColor ("Color", color) = (1., 1., 1., 1.)
 
        [Header(Diffuse)]
        _Diffuse ("Val", Range(0., 1.)) = 1.
        _DifColor ("Color", color) = (1., 1., 1., 1.)
     }
 
    SubShader
    {
        Pass
        {
            Tags { "RenderType"="Transparent" "Queue"="Geometry" "LightMode"="ForwardBase" }
 
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
  
            #include "UnityCG.cginc"
 
            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                fixed4 light : COLOR0;
            };
 
            fixed4 _LightColor0;
            fixed _Diffuse;
            fixed4 _DifColor;
            fixed _Ambient;
            fixed4 _AmbColor;
 
            v2f vert(appdata_base v)
            {
                v2f o;
 
                // Clip position
                o.pos = UnityObjectToClipPos(v.vertex);
 
                // Light direction
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
 
                // Normal in WorldSpace
                float3 worldNormal = UnityObjectToWorldNormal(v.normal.xyz);
 
                 // World position
                //float4 worldPos = mul(unity_ObjectToWorld, v.vertex);

                // Camera direction
                //float3 viewDir = normalize(UnityWorldSpaceViewDir(worldPos.xyz));
 
                // Compute ambient lighting
                fixed4 amb = _Ambient * _AmbColor;
 
                // Compute the diffuse lighting
                fixed4 lightTemp = max(0., dot(worldNormal, lightDir) * _LightColor0);
                fixed4 diffuse = lightTemp * _Diffuse * _LightColor0 * _DifColor;
 
                o.light = diffuse + amb;
               
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
}