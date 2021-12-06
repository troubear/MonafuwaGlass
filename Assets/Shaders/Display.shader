Shader "MonafuwaGlass/Display"
{
    Properties
    {
        _MainTex ("Texture", 3D) = "white" {}
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 viewPos : TEXCOORD1;
                float3 viewNormal : TEXCOORD2;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler3D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.viewPos = UnityObjectToClipPos(v.vertex);
                o.viewNormal = COMPUTE_VIEW_NORMAL;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // ふいんき
                float p = i.viewPos.x / i.viewPos.w * 35.0 / 180.0;
                float n = -normalize(i.viewNormal).x * 180.0 / 35.0;
                float t = (p + n) * 0.5 + 0.5;

                fixed4 col = tex3D(_MainTex, float3(i.uv, t));

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

                return col;
            }
            ENDCG
        }
    }
}
