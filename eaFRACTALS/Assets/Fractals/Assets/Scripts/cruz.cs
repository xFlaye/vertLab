using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cruz : MonoBehaviour
{
    public Transform spawner;
    public GameObject sprite;
    public float factor = 2;

    Vector3 posicion;
    public manager2 scriptCamara;

    GameObject nuevoObjeto, nuevoObjeto2, nuevoObjeto3, nuevoObjeto4;

    private void Start()
    {
        posicion = spawner.position;
    }

    private void Update()
    {
        //if press M key
        if (Input.GetKeyDown(KeyCode.M))
        {
            //spawn content covering all sides of the fractal
            spawnear1();
            spawnear2();
            spawnear3();
            spawnear4();

            //set all the content objects parent to spawner transform
            while (nuevoObjeto.transform.childCount > 0 || nuevoObjeto2.transform.childCount > 0 || nuevoObjeto3.transform.childCount > 0 || nuevoObjeto4.transform.childCount > 0)
            {
                foreach (Transform child in nuevoObjeto.transform)
                {
                    child.parent = spawner;
                    scriptCamara.objects++;
                }

                foreach (Transform child in nuevoObjeto2.transform)
                {
                    child.parent = spawner;
                    scriptCamara.objects++;
                }

                foreach (Transform child in nuevoObjeto3.transform)
                {
                    child.parent = spawner;
                    scriptCamara.objects++;
                }

                foreach (Transform child in nuevoObjeto4.transform)
                {
                    child.parent = spawner;
                    scriptCamara.objects++;
                }
            }

            //calculate factors
            if (factor == 2)
            {
                factor *= 2;
                factor += 2;
            }
            else { factor *= 2; }

            Destroy(nuevoObjeto);
            Destroy(nuevoObjeto2);
            Destroy(nuevoObjeto3);
            Destroy(nuevoObjeto4);
            scriptCamara.iteration++;
            scriptCamara.actualizarTexto();
        }
    }

    //spawn functions
    void spawnear1()
    {
        nuevoObjeto = Instantiate(spawner.gameObject, new Vector3(posicion.x, posicion.y + factor, posicion.z), this.gameObject.transform.rotation);
    }

    void spawnear2()
    {
        nuevoObjeto2 = Instantiate(spawner.gameObject, new Vector3(posicion.x, posicion.y - factor, posicion.z), this.gameObject.transform.rotation);
    }

    void spawnear3()
    {
        nuevoObjeto3 = Instantiate(spawner.gameObject, new Vector3(posicion.x + factor, posicion.y, posicion.z), this.gameObject.transform.rotation);
    }

    void spawnear4()
    {
        nuevoObjeto4 = Instantiate(spawner.gameObject, new Vector3(posicion.x - factor, posicion.y, posicion.z), this.gameObject.transform.rotation);
    }
}