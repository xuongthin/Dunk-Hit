Shader "Unlit/Background"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color1 ("Up Color", Color) = (1, 1, 1, 1)
        _Color2 ("Down Color", Color) = (1, 1, 1, 1)
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _Alpha ("Alpha", Range(0, 1)) = 1
        _Scale ("Noise Scale", Float) = 0
        _Speed ("Noise Speed", Vector) = (.0, -0.5, .0, .0)
        _Offset ("Noise Offset", Float) = 0
        _Slope ("Noise Slope", Float) = 1
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            fixed4 _Color1;
            fixed4 _Color2;
            sampler2D _NoiseTex;
            float _Alpha;
            float4 _MainTex_ST;
            float _Scale;
            float4 _Speed;
            float _Slope;
            float _Offset;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = lerp(_Color2, _Color1, i.uv.y);
                fixed4 noise = tex2D(_NoiseTex, i.uv * _Scale + _Speed.xy * _Time);
                fixed a = noise.r;
                a = a - ((i.uv.y - 0.5) * _Slope + 0.5 + _Offset);
                a = clamp(a, 0.0, 1.0);
                col.a = a * _Alpha;
                return col;
            }
            ENDCG
        }
    }
}
