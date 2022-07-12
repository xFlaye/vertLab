using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class juliaEx : MonoBehaviour
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
        //material.SetFloat("_Angle", smoothAngle);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateShader();
    }
}
