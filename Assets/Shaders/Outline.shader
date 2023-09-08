// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// 3D Simplex noise implementation adapted from
// keijiro's "Noise Shader Library for Unity".
// https://github.com/keijiro/NoiseShader

Shader "Custom/Outline"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

        _N1 ("Refractive Index 1", float) = 1.0
        _N2 ("Refractive Index 2", float) = 1.7
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineStrength ("Outline Strength", float) = 1.0

        _MinScale ("Minimum Scale", float) = 1.00
        _MaxScale ("Maximum Scale", float) = 1.02

        _Speed ("Blob Speed", float) = 0.2
        _Size ("Blob Size", float) = 0.2
    }
    SubShader
    {
        Tags {"RenderType"="Opaque"}
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG


        Pass 
        {

            Tags {"Queue"="Transparent" "RenderType"="Transparent"}
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Back
            ZWrite Off
            ZTest Greater
            Offset -10, -10
            
            CGPROGRAM
            #pragma vertex vert alpha
            #pragma fragment frag alpha

            #include "UnityCG.cginc"

            #define mod(x, y) (x - y * floor(x / y))

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
            };

            struct v2f
            {
                float4 vertex : POSITION;
                float3 worldVertex : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
            };

            uniform float4 _OutlineColor;
            uniform float _OutlineStrength;
            uniform float _MinScale;
            uniform float _MaxScale;
            uniform float _N1;
            uniform float _N2;
            uniform float _Speed;
            uniform float _Size;

            // Returns the sine of a value normalized to a range between MinScale and MaxScale
            float sinToRange(float x) 
            {
                return ((sin(x) + 1.0f) / 2.0f) * (_MaxScale - _MinScale) + _MinScale;
            }

            // Translate a 3D point
            float3 displace3(float3 vertex) 
            {
                float pseudorandom = mul(unity_ObjectToWorld, vertex).x; // We use this to vary the "start time" of the displacement
                return float3(vertex.x * sinToRange(vertex.y + pseudorandom + _Time.y), vertex.y * sinToRange(vertex.x + pseudorandom + _Time.y), vertex.z);
            }

            // Wrapper for displace3
            float4 displace4(float4 vertex) 
            {
                return float4(displace3(vertex.xyz), vertex.w);
            }

            /* --- */
            // 3D Simplex noise implementation adapted from
            // keijiro's "Noise Shader Library for Unity".
            // https://github.com/keijiro/NoiseShader
            float3 wglnoise_mod289(float3 x)
            {
                return x - floor(x / 289) * 289;
            }
            
            float4 wglnoise_mod289(float4 x)
            {
                return x - floor(x / 289) * 289;
            }
            
            float4 wglnoise_permute(float4 x)
            {
                return wglnoise_mod289((x * 34 + 1) * x);
            }

            float4 SimplexNoiseGrad(float3 v)
            {
                // First corner
                float3 i  = floor(v + dot(v, 1.0 / 3));
                float3 x0 = v   - i + dot(i, 1.0 / 6);

                // Other corners
                float3 g = x0.yzx <= x0.xyz;
                float3 l = 1 - g;
                float3 i1 = min(g.xyz, l.zxy);
                float3 i2 = max(g.xyz, l.zxy);

                float3 x1 = x0 - i1 + 1.0 / 6;
                float3 x2 = x0 - i2 + 1.0 / 3;
                float3 x3 = x0 - 0.5;

                // Permutations
                i = wglnoise_mod289(i); // Avoid truncation effects in permutation
                float4 p = wglnoise_permute(    i.z + float4(0, i1.z, i2.z, 1));
                    p = wglnoise_permute(p + i.y + float4(0, i1.y, i2.y, 1));
                    p = wglnoise_permute(p + i.x + float4(0, i1.x, i2.x, 1));

                // Gradients: 7x7 points over a square, mapped onto an octahedron.
                // The ring size 17*17 = 289 is close to a multiple of 49 (49*6 = 294)
                float4 gx = lerp(-1, 1, frac(floor(p / 7) / 7));
                float4 gy = lerp(-1, 1, frac(floor(p % 7) / 7));
                float4 gz = 1 - abs(gx) - abs(gy);

                bool4 zn = gz < -0.01;
                gx += zn * (gx < -0.01 ? 1 : -1);
                gy += zn * (gy < -0.01 ? 1 : -1);

                float3 g0 = normalize(float3(gx.x, gy.x, gz.x));
                float3 g1 = normalize(float3(gx.y, gy.y, gz.y));
                float3 g2 = normalize(float3(gx.z, gy.z, gz.z));
                float3 g3 = normalize(float3(gx.w, gy.w, gz.w));

                // Compute noise and gradient at P
                float4 m  = float4(dot(x0, x0), dot(x1, x1), dot(x2, x2), dot(x3, x3));
                float4 px = float4(dot(g0, x0), dot(g1, x1), dot(g2, x2), dot(g3, x3));

                m = max(0.5 - m, 0);
                float4 m3 = m * m * m;
                float4 m4 = m * m3;

                float4 temp = -8 * m3 * px;
                float3 grad = m4.x * g0 + temp.x * x0 +
                            m4.y * g1 + temp.y * x1 +
                            m4.z * g2 + temp.z * x2 +
                            m4.w * g3 + temp.w * x3;

                return 107 * float4(grad, dot(m4, px));
            }

            float SimplexNoise(float3 v)
            {
                return SimplexNoiseGrad(v).w;
            }
            /* --- */

            // Vertex shader
            v2f vert (appdata v)
            {

                v2f o;

                // We're modifying vertices but also using normals, so we'll want to update
                // normals accordingly.
                float4 displaced = displace4(v.vertex);
                float3 bitangent = cross(v.normal, v.tangent);

                // Calculate new normals by approximating how the immediate neighbouring surfaces
                // would also be displaced, and take the cross product to those neighbours to get
                // the adjusted normal.
                float3 neighbourAlongTangent = displace3(v.vertex + v.tangent * 0.01);
                float3 neighbourAlongBitangent = displace3(v.vertex + bitangent * 0.01);

                float3 newTangent = neighbourAlongTangent - displaced;
                float3 newBitangent = neighbourAlongBitangent - displaced;

                v.normal = normalize(cross(newTangent, newBitangent));
                v.vertex = displaced;

                o.vertex = UnityObjectToClipPos(v.vertex);

                o.worldVertex = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = normalize(mul(unity_ObjectToWorld, float4(v.normal, 0.0)).xyz);

                return o;
            }

            // Fragment shader
            fixed4 frag (v2f i) : SV_Target
            {

                // Calculate Fresnel effect
                float3 viewIncidence = normalize(_WorldSpaceCameraPos - i.worldVertex);

                float r = _N1 / _N2;

                float cI = dot(viewIncidence, i.worldNormal);
                float cT = sqrt(1.0f - ((r * r) * (1.0f - (cI * cI))));

                float perpendicular = ((_N1 * cI) - (_N2 * cT)) / ((_N1 * cI) + (_N2 * cT));
                float parallel      = ((_N2 * cI) - (_N1 * cT)) / ((_N2 * cI) + (_N1 * cT));    

                float reflectance = ((perpendicular * perpendicular) + (parallel * parallel)) / 2.0f;  

                if (reflectance < 0.01f) {
                    reflectance = 0.0f;
                }

                // Apply "bubble" effect
                float noiseSum = 0.0f;
                float3 uvNoise = i.worldVertex * _Size;
                noiseSum += SimplexNoise(uvNoise + _Time.y * _Speed) * 0.25f + 0.25f;
                          
                return _OutlineColor * (reflectance * _OutlineStrength + frac(noiseSum));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
