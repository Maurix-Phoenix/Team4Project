Shader "Unlit/Water"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _WaterEffect("Water Effect Texture", 2D) = "white" {}

        _WaterMainColor ("MainColor", Color) = (0.2,0.2,1,1)
        _WaterSurfaceColor("SurfaceColor", Color) = (0.2,0.2,0.8,1)
        _WaterDeepColor("DeepestColor", Color) = (0.1,0.1,1,1)

        _amplitude("WaveAmplitude", Float) = 1
        _frequency("WaveFrequency", Float) = 1
        _waveSpeed("WaveSpeed", Float) = 1

        _WaterEffectFrequency("Water Effect Frequency", Float) = 1
        _WaterEffectStrenght("Water Effect Strenght", Float) = 1
        _WaterEffectThreshold ("Threshold", Range(0, 1)) = 0.5

        _refractionFrequency("Refraction Frequency", Float) = 1
        _refractionAmplitude("Refraction Strenght", Float) = 1

    }
    SubShader
    {
        Tags { "RenderType"="Transparent" 
               "Queue" = "Transparent"
        }

        //blend multiply
        Blend DstColor zero
        ZWrite Off
        //ZTest Always //draws always on top

        
        LOD 100



        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            sampler2D _MainTex;
            sampler2D _WaterEffect;

            float4 _WaterMainColor;
            float4 _WaterSurfaceColor;
            float _WaterDeepColor;


            float _WaterEffectFrequency;
            float _WaterEffectStrenght;
            float _WaterEffectThreshold;

            float _refractionAmplitude;
            float _refractionFrequency;

            float _amplitude;
            float _frequency;
            float _waveSpeed;

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



            v2f vert (appdata v)
            {
                v2f o;               


                //Wave Effect
                if( v.uv.y == 0)
                {
                   v.vertex.z += _amplitude * sin(v.uv.x * _frequency + _Time.w * _waveSpeed); 
                } 
               
               if(v.uv.y > 0 && v.uv.x > 0 && v.uv.x < 1)
               {
                    v.vertex.x += _refractionAmplitude * sin(v.uv.y * _refractionFrequency + _Time.w);
                   // v.vertex.z += _amplitude * sin(v.uv.x * _frequency + _Time.w * _waveSpeed); 
               }
                
               
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //water effect
                //float offset = _Time.y * _Speed;
                //float2 noiseUV = i.texcoord * _Scale + float2(offset, offset);
                
                float2 weUV = i.uv +  _WaterEffectFrequency * _Time.w;
                weUV.y += cos(_Time.y) * _WaterEffectFrequency;
                float4 weColor = tex2D(_WaterEffect, weUV) * _WaterEffectStrenght;



                //Water Colors
                float4 col = lerp(_WaterSurfaceColor, _WaterDeepColor, i.uv.y);

                if(i.uv.y <= 0.02 )
                {
                    col = lerp(_WaterMainColor, _WaterSurfaceColor, i.uv.y);
                }
                else
                {
                    if(i.uv.y > i.uv.y / 2 - 0.2 && i.uv.y < i.uv.y/ + 0.2 )
                    {
                        col = lerp(col, col/2, i.uv.y);
                        
                    }
                    if (weColor.r > _WaterEffectThreshold && weColor.g > _WaterEffectThreshold && weColor.b > _WaterEffectThreshold) 
                    {
                        weColor = lerp(_WaterSurfaceColor, _WaterDeepColor, i.uv.x*1.5);
                    }

                    col = lerp(col,weColor,sin(i.uv.y) * _WaterEffectStrenght * sin(i.uv.y));//* _WaterEffectStrenght;
                    //col = lerp(col, _WaterDeepColor, i.uv.y);
                    //col = lerp(col, _WaterDeepColor, sin(i.uv.y));
                    col = lerp(col, _WaterDeepColor, sin(i.uv.y)*sin(i.uv.y));
                }
                



               return col;
            }
            ENDCG
        }
    }
}
