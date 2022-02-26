using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getData : MonoBehaviour
{
    
    [SerializeField]
    private MQTT mqttObj;
    
   
    // Update is called once per frame
    void LateUpdate()
    {
        // Debug.Log("from getData" + mqttObj.fData);
        // float mover = mqttObj.fData;
        // float normalization = 0.0f;

        // normalization = 1*Time.deltaTime/mover + Random.Range(-10.0f, 10.0f); 

        // gameObject.transform.position = new Vector3(normalization,0,0);
        
    }
}
