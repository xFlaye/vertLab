Shader "eaShaders/Julia"
{
    Properties
    {
        // _MainTex ("Texture", 2D) = "red" {}

        _Color("Color", Color) = (.25,.5, .75, 1)

        //define
        _Area("Area", vector) = (0, 0, 4, 4)
        _realNum("Real Number", float) = 0.355543
        _imaginaryNum("Imaginary Number", float) = 0.337292
        _Iterator("Iterator", Int) = 255
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            //! define variables (use semicolon here)
            float4 _Area;
            float _Iterator;
            float _realNum;
            float _imaginaryNum;
            float4 _Color;

            // sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                // fixed4 col = tex2D(_MainTex, i.uv);
                // // just invert the colors
                // col.rgb = 0.5 / col.rgb;
                // return col;

                fixed4 col = _Color;
                // return col;

                //! Start position assigned to uv coordinate
                // will center the uv coordinates on the rawImage between -2 and 2 (size is 0-4 as above). 
                float2 c = _Area.xy + (i.uv - 0.5) * _Area.zw; 

                float2 z;
                float iter;

                float newA;
                float newB;
                

                for (iter = 0; iter < _Iterator; iter++)
                {
                    newA = z.x*z.x - z.y*z.y + _realNum;
                    newB = 2*z.x*z.y + _imaginaryNum;

                    z.x = newA;
                    z.y = newB;

                    iter += 1;

                     if(length(z) > 2)
                    {
                        break;
                    }
                }

                col = lerp(lerp((float)iter, 0, (float)_Iterator), 1,1);

                return col;
                


                //return iter / _Iterator;

                



            }
            ENDCG
        }
    }
}
