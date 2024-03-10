Shader "CustomRenderTexture/OpaqueBachPlanes"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex("InputTex", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        // Habilita el culling de caras traseras
        Cull Back

        Blend One Zero

        Pass
        {
            Name "OpaqueBachPlanes"

            CGPROGRAM
            #include "UnityCustomRenderTexture.cginc"
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag
            #pragma target 3.0

            float4      _Color;
            sampler2D   _MainTex;

            float4 frag(v2f_customrendertexture IN) : SV_Target
            {
                float2 uv = IN.localTexcoord.xy;
                float4 color = tex2D(_MainTex, uv) * _Color;

                // TODO: Replace this by actual code!
                uint2 p = uv.xy * 256;
                return countbits(~(p.x & p.y) + 1) % 2 * float4(uv, 1, 1) * color;
            }
            ENDCG
        }
    }
}
