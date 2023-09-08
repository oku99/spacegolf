Shader "Custom/SolidAlways"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)

    }
    SubShader
    {
        
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        ZTest Always
        Color [_Color]

        Pass 
        {
        }
    }

    FallBack "Diffuse"
}
