using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transCtrl : MonoBehaviour
{
    
    public Material ctrlMat;
    public float health;
    public float maxHealth;
    
    
    // Start is called before the first frame update
    void Start()
    {
        // set the Dissolve Value slider to change based on the percentage of the health
        ctrlMat.SetFloat("_Dissolve_Value", health/maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        // press spacebar to engage (this is where the external input will happen)
        if(Input.GetKeyDown(KeyCode.Space))
        {
            // reduce health by 5 units
            health -= 5;
            // update the shader
            ctrlMat.SetFloat("_Dissolve_Value", health/maxHealth);
        }
    }
}
