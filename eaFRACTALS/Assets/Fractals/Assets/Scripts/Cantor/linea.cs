using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class linea : MonoBehaviour
{
    public Transform a, b;
    public List<float> factores;
    public float distance;

    private void Awake()
    {
        factores.Clear();
        calculateFactors();
    }

    void Update()
    {
        //if press M key
        if (Input.GetKeyDown(KeyCode.M))
        {
            //instantiate this line, as far as distance variable, the third part of the size, in the side of this line
            GameObject nuevaLinea = Instantiate(this.gameObject);
            nuevaLinea.transform.position = Vector2.Lerp(a.position, b.position, factores[0]);
            nuevaLinea.transform.Translate(new Vector3(0f, distance, 0f));
            nuevaLinea.transform.localScale = new Vector3(nuevaLinea.transform.localScale.x / 3, nuevaLinea.transform.localScale.y, nuevaLinea.transform.localScale.z);

            GameObject nuevaLinea2 = Instantiate(this.gameObject);
            nuevaLinea2.transform.position = Vector2.Lerp(a.position, b.position, Mathf.Abs(factores[1]));
            nuevaLinea2.transform.localScale = new Vector3(nuevaLinea2.transform.localScale.x / 3, nuevaLinea.transform.localScale.y, nuevaLinea.transform.localScale.z);
            nuevaLinea2.transform.Translate(new Vector3(0f, distance, 0f));

            //destroy this script
            Destroy(this);
        }
    }

    void calculateFactors()
    {
        float factor1 = (1 / 3f) / 2f;
        factores.Add(factor1);
        factores.Add((factor1 * 5f));
    }
}