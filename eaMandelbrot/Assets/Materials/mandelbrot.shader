Shader "eaShaders/Mandelbrot"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        // will show up in the shader UI and...
        _Area("Area", vector) = (0, 0, 4, 4)
        _Angle("Angle", range(-3.1415, 3.1415)) = 0
        _Iterator("Iterator", Int) = 255
        _Color("Color", range (0,1)) = 0.5
        _Repeat("Repeat", float) = 1
        _Speed("Speed", float) = 0.2
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


            float4 _Area;
            float _Angle, _Iterator, _Color, _Repeat, _Speed;  // use one call for similar types
            
            sampler2D _MainTex;
            


            //! rotate point in 2D FUNCTION
            // p = point, a = angle
            float2 rot(float2 p, float2 pivot, float a)
            {
                float s = sin(a);
                float c = cos(a);
                
                p -= pivot; // subtract pivot from p
                p = float2(p.x * c - p.y * s, p.x * s + p.y * c);

                // reset rotation
                p += pivot;

                return p;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Starting code for the shader - ignore
                // fixed4 col = tex2D(_MainTex, i.uv);
                // // just invert the colors
                // col.rgb = 1 - col.rgb;


                //! Start position assigned to uv coordinate 
                float2 uv = i.uv - 0.5;   
                //! mirror
                // uv = abs(uv);
                // uv = rot(uv, 0, .25 * 3.1415);
                // uv = abs(uv);

                float2 c = _Area.xy + uv * _Area.zw; // will center the uv coordinates on the rawImage between -2 and 2 (size is 0-4 as above). 
                c = rot(c, _Area.xy, _Angle);

                float r = 20; // escape radius from the origin
                float r2 = r * r; // furthest escape radius


                //! keep track of pixels
                float2 z, zPrevious;
                float iter;

                for (iter = 0; iter < _Iterator; iter ++)
                {
                    zPrevious = rot(z, 0, _Time.y);// use previous rot function
                    z = float2(z.x * z.x - z.y * z.y, 2 * z.x * z.y) + c;
                    //break line

                    //if(length(z) > r)
                    if (dot(zPrevious,z) > 2)
                    {
                        break;
                    }
                }

                //! ensure the main shape set remains black
                if (iter > _Iterator) 
                {
                    return 0;
                }

                float dist = length(z); //! distance from origin
                float fracIter = (dist -r) / (r2 -r); // remap to 0-1 range using linear interpolation

                /*
                Log2(16) -> 2? = 16 -> 4
                so log2(16) is 2 to the power of what which will give 16, which is 4
                */

                // setting up the log or R
                fracIter = log2(log(dist) / log(r) ) -1 ; //! equivalent to log based r of dist, double exponential interpolation

                //! interesting look
                // iter -= (fracIter*fracIter);
                // iter -= log(fracIter*fracIter)/r2;
                // iter -= fracIter;


            
                float m = sqrt(iter / _Iterator); // the sqrt smooths the difference between the blacks and the whites and creates banding
                //! SKIP THIS IF ONLY B&W

                // *0.5+0.5 moves things to the 0-1 range

                float4 col = sin(float4(.3, .45, .77, 1) * m * 20) * 0.5 + 0.5; // procedural using colors for rgb

                //! using a texture map (blurred gradient map)
                col = tex2D(_MainTex, float2(m * _Repeat + _Time.y * _Speed, _Color));

                float angle = atan2(z.x, z.y); // angle between -pi and pi

                col *=smoothstep(3,0,fracIter);

                // add vertical lines
                col *= 1+sin(angle*2 + _Time.y * 4) * 0.1;

                // col = fracIter;
                return col;
            }
            ENDCG
        }
    }
}
