using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//! using new assembly definition but it doesn't seem to work. Must read up on it further
public class mandelbrotEx : MonoBehaviour
{
    public Material material;

    public Vector2 pos;
    public float scale, angle;

    // add movement momentum smooth
    private Vector2 smoothPos;
    private float smoothScale, smoothAngle;

    private void UpdateShader()
    {
        smoothPos = Vector2.Lerp(smoothPos, pos, 0.03f);
        smoothScale = Mathf.Lerp(smoothScale, scale, 0.03f); //! because scale is a float, must use Mathf.Lerp
        smoothAngle = Mathf.Lerp(smoothAngle, angle, 0.03f);



        // compensate for stretching by dividing screen width by screen height to get a ratio. Cast them to float since they're integers
        float aspect = (float)Screen.width / (float)Screen.height;

        float scaleX = smoothScale;
        float scaleY = smoothScale;

        if (aspect > 1f)
        {
            scaleY /= aspect;
        } else
        {
            scaleX *= aspect;
        }

        //! Get info from the shader  <---- IMPOARTANT
        material.SetVector("_Area", new Vector4(smoothPos.x, smoothPos.y, scaleX, scaleY));
        material.SetFloat("_Angle", smoothAngle);
    }

    private void HandleInputs()
    {
        // zoom in and out
        if(Input.GetKey(KeyCode.KeypadPlus))
        {
            // make scale down proportional
            scale *= 0.99f; // reduce propotionaly
        }

        if(Input.GetKey(KeyCode.KeypadMinus))
        {
            // make scale down proportional
            scale *= 1.01f; // reduce propotionaly
        }


        // rotate left and right 
        if(Input.GetKey(KeyCode.Q))
        {
            angle += 0.01f; // rotate propotionaly
        }

        if(Input.GetKey(KeyCode.E))
        {
            angle -= 0.01f; // rotate propotionaly
        }

        //! Take into account the angle rotation and compensate     <--- SUPER USEFUL TRICK
        Vector2 dir = new Vector2(0.01f * scale, 0);
        float s = Mathf.Sin(angle);
        float c = Mathf.Cos(angle);

        // move-rotate left and right
        dir = new Vector2(dir.x * c , dir.x * s);
		
        //! Move WASD
        if(Input.GetKey(KeyCode.A))
        {
            // move Left
            // pos.x -= 0.01f * scale; //! keep the motion proportional 

            // replace with dir variable
            pos -= dir;
        }
        if(Input.GetKey(KeyCode.D))
        {
            // move Left
            //pos.x += 0.01f * scale; 
            pos += dir;
        }

        // move-rotate up and down
        dir = new Vector2(dir.y, -dir.x);
        if(Input.GetKey(KeyCode.W))
        {
            // move Left
            // pos.y += 0.01f * scale; 
            pos -= dir;
        }
        if(Input.GetKey(KeyCode.S))
        {
            // move Left
            // pos.y -= 0.01f * scale; 
            pos += dir;
        }
    }
    

    // Update is called once per frame
    //! FixedUpdate updates x times per second. Update updates depending on CPU speed
    void FixedUpdate()
    {
        // create User inputs to move the image
        HandleInputs();       
        UpdateShader();
    }
}
