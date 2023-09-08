Shader "Unlit/BlackHole"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _RotationSpeed("Rotation Speed", float) = 1.0
        _Color ("Color", color) = (1,1,1,1)
    }
    
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha 
        LOD 100
        
        // https://docs.unity3d.com/2017.3/Documentation/Manual/SL-CullAndDepth.html
        Pass 
        {
            ZWrite ON
            ColorMask 0
        }

        Pass
        {
            ZWrite OFF
            Ztest LEqual

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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _RotationSpeed;
            float _Tilting;
            fixed4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            
            // https://stackoverflow.com/questions/41525411/how-to-transparent-unity3d-custom-shader
            fixed4 frag (v2f i) : SV_Target
            {
                float radius = pow(i.uv.x - 0.5, 2) + pow(i.uv.y - 0.5, 2);
                radius = sqrt(radius);

                float Cos = cos(_Time * _RotationSpeed / (1.0 + radius*2.0));
                float Sin = sin(_Time * _RotationSpeed / (1.0 + radius*2.0));
                float2x2 rotationMatrix  = float2x2(Cos, -Sin, Sin, Cos);

                i.uv = mul(i.uv - 0.5, rotationMatrix) + 0.5;
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                if(radius <= 0.2){
                    col.a = _Color.a / pow(radius, 1.5);
                }else{
                    col.a = _Color.a / pow(radius, (1.5 - radius*5.0));//pow(2, (radius-0.2)*10.0);
                }
                
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
