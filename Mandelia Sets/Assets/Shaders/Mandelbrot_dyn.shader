Shader "Fractals/Mandelbrot_dyn"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,0.5)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        // _Glossiness ("Smoothness", Range(0,1)) = 0.5
        // _Metallic ("Metallic", Range(0,1)) = 0.0
        // _Zoom("Zoom", Vector) = (1,1,1,1)
        // _Pan("Pan", Vector) = (1,1,1,1)
        _Aspect("Aspect Ratio",Float ) = 1
        _Iterations("Iterations", Range(1,300)) = 150

        _Area("Area", vector) = (0, 0, 4, 4)
        _Angle("Angle", range(-3.1415, 3.1415)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

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

        // half _Glossiness;
        // half _Metallic;
        fixed4 _Color;

        float _Iterations;
        float _Aspect;

        float4 _Zoom;
        float4 _Pan;

        //! updates
        float4 _Area;
        float _Angle;

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


        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            //fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

            // set the uvs on the plane
            // following tutorial from https://www.youtube.com/watch?v=SVj0LWmQD-E

            

            float2 c = _Area.xy + (IN.uv_MainTex - 0.5) * _Area.zw;
            // float2 c = (IN.uv_MainTex - 0.5) * _Zoom.xy * float2(1, _Aspect) - _Pan.xy;
            c = rot(c, _Area.xy, _Angle);
            float2 v = 0;
            float m = 0;

            const float r = 5;

            for(int n = 0; n < _Iterations; n++) {
                // calculate a 2D vector
                v = float2(v.x * v.x - v.y * v.y, v.x * v.y * 2) + c;

                if(dot(v,v) < (r * r -1 )){
                    m++;
                }

                // clamp v vector
                v = clamp(v, -r, r);
            }

            float4 color;
            if (m == floor(_Iterations)){
                color = float4(0,0,0,1); // return black color
            }
            else {
                color = float4(sin(m/4), sin(m/5), sin(m/7), 1) / 4 + 0.75;
            }

            



            o.Albedo = color;
            // Metallic and smoothness come from slider variables
            
            o.Emission = color;
            // o.Metallic = _Metallic;
            // o.Smoothness = _Glossiness;
            o.Alpha = color.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
