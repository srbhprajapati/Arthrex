Shader "Unlit/Arthrex_unlit"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _MotionTex("Motion Texture", 2D) = "white" {}
        _ImageTex("Image Texture", 2D) = "white" {}
    }
        SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 100

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 tangent : TANGENT;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 texCoordData : TEXCOORD1;
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _ImageTex;
            sampler2D _MotionTex;
            float4 _MainTex_ST;

            v2f vert(appdata v)
            {
                v2f o;
                float2 posXY = float2(v.tangent.x + 0.5f, v.tangent.y + 0.5f);
                float4 dCol = tex2Dlod(_MotionTex, float4(posXY.xy,0,0));

                //animate on the Y axis based on Motion Texture - 4.0f scale factor
                v.vertex.y += dCol.x * 4.0f;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                o.texCoordData = float2(v.tangent.x + 0.5f, v.tangent.y + 0.5f);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the image texture
                fixed4 imCol = tex2D(_ImageTex, i.texCoordData);

                //Provide circular region for display of color
                float2 uvFromCenter = float2(i.uv.x - 0.5f, i.uv.y - 0.5f);
                float colorMask = length(uvFromCenter) < (0.5f) ? 0.75f : 0.0f;
                float alphaMask = length(uvFromCenter) < (0.5f) ? 1.0f : 0.0f;

                //Mix particle color with particle area mask
                fixed4 col = imCol * fixed4(colorMask, colorMask, colorMask, alphaMask);

                return col;
            }
            ENDCG
        }
    }
}
