using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triangle : MonoBehaviour
{
    public Transform pointA, pointB, pointC;
    public manager managerScript;
    public bool firstTriangle = true;

    public List<float> factores;
    public List<float> nuevosFactores;

    int iteration = 0;
    float factor, sizeFactor = 3;

    private void Start()
    {
        managerScript = GameObject.Find("Main Camera").GetComponent<manager>();    
    }

    private void Update()
    {
        //if press m key
        if (Input.GetKeyDown(KeyCode.M))
        {
            //add 1 iteration
            if (iteration >= 1)
            {
                //calculate new factors
                factor = factores[0] - (factores[0] / 3);
                nuevosFactores.Clear();
                for (int i = 0; i < factores.Count; i++)
                {
                    nuevosFactores.Add(factores[i] - factor);
                    nuevosFactores.Add(factores[i] + factor);
                }

                factores.Clear();
                for (int i = 0; i < nuevosFactores.Count; i++)
                {
                    factores.Add(nuevosFactores[i]);
                }

                sizeFactor *= 3;
            }

            if (firstTriangle)
            {
                //instantiate new triangles in 3 sides of the triangle
                duplicar1();
                duplicar2();
                duplicar3();
            }
            else
            {
                //instantiate new triangles in 2 sides of the triangle
                duplicar1();
                duplicar2();
            }

            iteration++;
            managerScript.actualizarTextos();
        }
    }

    public void duplicar1()
    {
        for (int i = 0; i < nuevosFactores.Count; i++)
        {
            GameObject duplicado1 = Instantiate(gameObject);
            duplicado1.GetComponent<triangle>().firstTriangle = false;
            duplicado1.GetComponent<triangle>().factores.Clear();
            duplicado1.GetComponent<triangle>().factores.Add(0.5f);

            duplicado1.GetComponent<triangle>().nuevosFactores.Clear();
            duplicado1.GetComponent<triangle>().nuevosFactores.Add(0.5f);

            duplicado1.transform.position = Vector2.Lerp(pointA.position, pointB.position, nuevosFactores[i]);
            duplicado1.transform.Rotate(new Vector3(0f, 0f, -63.587f));
            duplicado1.transform.localScale = duplicado1.transform.localScale / sizeFactor;
            managerScript.triangles++;
        }
    }

    public void duplicar2()
    {
        for (int i = 0; i < nuevosFactores.Count; i++)
        {
            GameObject duplicado1 = Instantiate(gameObject);
            duplicado1.GetComponent<triangle>().firstTriangle = false;
            duplicado1.GetComponent<triangle>().factores.Clear();
            duplicado1.GetComponent<triangle>().factores.Add(0.5f);

            duplicado1.GetComponent<triangle>().nuevosFactores.Clear();
            duplicado1.GetComponent<triangle>().nuevosFactores.Add(0.5f);
            duplicado1.transform.position = Vector2.Lerp(pointA.position, pointC.position, nuevosFactores[i]);
            duplicado1.transform.Rotate(new Vector3(0f, 0f, 63.587f));
            duplicado1.transform.localScale = duplicado1.transform.localScale / sizeFactor;
            managerScript.triangles++;
        }
    }

    public void duplicar3()
    {
        for (int i = 0; i < nuevosFactores.Count; i++)
        {
            GameObject duplicado1 = Instantiate(gameObject);
            duplicado1.GetComponent<triangle>().firstTriangle = false;
            duplicado1.GetComponent<triangle>().factores.Clear();
            duplicado1.GetComponent<triangle>().factores.Add(0.5f);

            duplicado1.GetComponent<triangle>().nuevosFactores.Clear();
            duplicado1.GetComponent<triangle>().nuevosFactores.Add(0.5f);
            duplicado1.transform.position = Vector2.Lerp(pointB.position, pointC.position, nuevosFactores[i]);
            duplicado1.transform.Rotate(new Vector3(0f, 0f, 180));
            duplicado1.transform.localScale = duplicado1.transform.localScale / sizeFactor;
            managerScript.triangles++;
        }
    }
}