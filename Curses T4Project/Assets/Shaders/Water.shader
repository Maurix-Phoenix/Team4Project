Shader "Unlit/Water"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Test ("Texture", 2D) = "black" {}

        _WaterMainColor ("MainColor", Color) = (0.2,0.2,1,1)
        _WaterSurfaceColor("SurfaceColor", Color) = (0.2,0.2,0.8,1)
        _WaterDeepColor("DeepestColor", Color) = (0.1,0.1,1,1)

        _amplitude("WaveAmplitude", Float) = 1
        _frequency("WaveFrequency", Float) = 1
        _waveSpeed("WaveSpeed", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" 
               "Queue" = "Transparent"
        }

        //blend multiply
        Blend DstColor zero
        ZWrite Off


      
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            //#pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;

                //test
                float2 uv2 : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                //UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _Test;
            float4 _Test_ST;

            float4 _WaterMainColor;
            float4 _WaterSurfaceColor;
            float _WaterDeepColor;


            float _amplitude;
            float _frequency;
            float _waveSpeed;


           

            v2f vert (appdata v)
            {
                v2f o;



                if( v.uv.y == 0)
                {
                   v.vertex.z += _amplitude * sin(v.uv.x * _frequency + _Time.w * _waveSpeed); 
                }
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
               
                //UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv);
                
                float4 col = lerp(_WaterSurfaceColor, _WaterDeepColor, i.uv.y);
                col = lerp(_WaterSurfaceColor, col, i.uv.y);

                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                //return col;
                //return _WaterMainColor;
                return col;
            }
            ENDCG
        }
    }
}
