using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//! by calling it here, no need to type Mathf everytime, just the function
using static UnityEngine.Mathf;

//! since it will be a publicly-accesible class, we'll convert it to a static class
public static class FunctionLibrary
{
    // public delegate float Function (float x, float z, float t);
    public delegate Vector3 Function (float u, float v, float t);

    public enum FunctionName {Wave, MultiWave, Ripple, RippleChaos, TripleWave}

    static Function[] functions = {Wave, MultiWave, Ripple, RippleChaos, TripleWave};

    // public static Function GetFunction (int index)
    // {
    //     return functions[index];
    // }

     public static Function GetFunction (FunctionName name)
    {
        return functions[(int)name]; // must convert to int
    }
    
    // return a float 
    // public static float Wave(float x, float z, float t)
    // {
    //     // will create a sine wave: f(x,t) = sin(PI(x+t))
    //     return Sin(PI * (x + z + t));
    // }
    //  public static float MultiWave(float x, float z, float t)
    // {
    //     float y = Sin(PI * (x + 0.5f * t));
    //     // add another sine wave with double the frequency
    //     y += 0.5f * Sin(2f * PI * (z + t));
    //     y += Sin(PI * (x + z + 0.25f * t));


    //     // normalize 
    //     return y * (1f/2.5f);
    // }
    // public static float Ripple (float x, float z, float t)
    // {
    //     // float d = Abs(x);
    //     float d = Sqrt(Pow(x,2)+ Pow(z,2));
    //     float y = Cos(x * (3f * d -t)*2);
    //     return y / (1f + d *d);
    // }

    // public static float RippleChaos (float x, float z, float t)
    // {
    //     float d = Abs(x);
    //     float y = Sin(PI * (4f * d - t));
    //     return y / (1f + 10f * d); // use a fixed amplitude (10)
    // } 

    // public static float TripleWave(float x, float z, float t)
    // {
    //     float d = Sqrt(x * x + z * z);
    //     float y = Sin(PI * (4f * d -t));
    //     return y / (1f + 10f * d);
    // }

    public static Vector3 Wave (float u, float v, float t)
    {
        Vector3 p;
        p.x = u;
        p.y = Sin(PI * (u + v + t));
        p.z = v;
        return p;
    }
    public static Vector3 MultiWave (float u, float v, float t)
    {
        Vector3 p;
        p.x = u;
        p.y = Sin(PI * (u + 0.5f * t));
        p.y += 0.5f * Sin(2f * PI + (v + t));
        p.y += Sin(PI * (u + v + 0.25f * t));
        p.y *= 1f/ 2.5f;
        p.z = v;
        return p;
    }

    public static Vector3 Ripple (float u, float v, float t)
    {
        float d = Sqrt(Pow(u,2) + Pow(v,2));
        Vector3 p;
        p.x = u;
        p.y = Sin(PI * (4f * d - t));
        p.y /= 1f + 10f * d;
        p.z = v;
        return p;
    }
    public static Vector3 RippleChaos (float u, float v, float t)
    {
        float d = Abs(u);
        Vector3 p;
        p.x = u;
        p.y = Sin(PI * (4f * d - t));
        p.y /= (1f + Cos(10f * d)); // use a fixed amplitude (10)
        p.z = v;
        return p;
    } 

    public static Vector3 TripleWave(float u, float v, float t)
    {
        float d = Sqrt(Pow(u,2) + Pow(v,2));
        Vector3 p;
        p.x = u;
        p.y = Cos(4f * PI + 10f * (Sin(PI * d -t)));


        p.y += Sin(PI * (4f * d -t));
        p.y /= (1f + 10f * d);
        p.z = v;
        return p;
    }

   

    
}
