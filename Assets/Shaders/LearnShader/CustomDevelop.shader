Shader "Learning/CustomDevelop"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Amplitude ("Amplitude", float) = 200.0
        _Frequence ("Frequence", float) = 150.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma Standart

            #include "UnityCG.cginc"

            struct vertexdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                //UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Amplitude;
            float _Frequence;

            v2f vert (vertexdata IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.vertex.y += sin(OUT.vertex.x * _Frequence + _Time.w) / _Amplitude;
                //OUT.vertex.x += sin(OUT.vertex.y * _Frequence + _Time.w) / _Amplitude;
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                //UNITY_TRANSFER_FOG(o,o.vertex);
                return OUT;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
