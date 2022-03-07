using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField]
    Transform pointPrefab;

    [SerializeField, Range(10,100)]
    int resolution = 30;

    //[SerializeField, Range(0,3)]
    //int function;

    [SerializeField]
    FunctionLibrary.FunctionName function;

    Transform[] points;
    
    // public int power = 2;
    [SerializeField, Range(1,100)]
    int divider = 1;
    
    [SerializeField, Range(1,100)]
    float stepAmount = 2f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        FunctionLibrary.Function f = FunctionLibrary.GetFunction(function);
        
        float time = Time.time;
        float step = stepAmount / resolution;
        int depthRest = resolution / divider;

        float v = 0.5f * step - 1f;
        
    
        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++) 
        {
            if (x == depthRest) {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }
            float u = (x + 0.5f) * step - 1f;
            points[i].localPosition = f(u, v, time);

                // Transform point = points[i];
            // Vector3 position = point.localPosition;
            // //! Call using the FunctionLibrary classes and function using delegates
            // position.y = f(position.x, position.z, time);

            // //! Y-axis options
            // change the shape of the parabola 
            // position.y = Mathf.Pow(position.x,power); 

            // //? use a sine wave (updated with a function)
            //position.y = Mathf.Sin(Mathf.PI * (position.x + time));
            
            // reapply the change to the point
            // point.localPosition = position;
        }
    
        
    }

    void Awake()
    {
    
        float step = stepAmount / resolution;
        // var position = Vector3.zero;
        var scale = Vector3.one * step;

        //! assign length of points in Transform array based on resolution
        points = new Transform[resolution * resolution];

        // instead of i < resolution
        for (int i = 0; i < points.Length; i++)
        {
            // grab the prefab from the gameobject and instantiate it
            // Transform point = Instantiate(pointPrefab);
            // points[i] = point;

            //! OR

            Transform point = points[i] = Instantiate(pointPrefab);

            // set X axis (align it so it's balanced along origin)
            // position.x = (x + 0.5f) * step - 1f;
            // position.z = (z + 0.5f) * step - 1f;
            
            // point.localPosition = position;
            point.localScale = scale;

            // parent to gameObject
            point.SetParent(transform, false);
        }
        
    }
}
