using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class camara : MonoBehaviour
{
    public GameObject menu;
    public TextMeshProUGUI iterationText;
    public Camera cameraComponent;
    public float speed, moveSpeed;
    public int objects, iteration;

    private void Start()
    {
        actualizarTexto();    
    }

    void Update()
    {
        //move the camara and control the zoom amount
        cameraComponent.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * speed * Time.deltaTime * cameraComponent.orthographicSize;
        cameraComponent.transform.Translate(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f) * moveSpeed * cameraComponent.orthographicSize);

        if (Input.GetKeyDown(KeyCode.M))
        {
            iteration++;
            actualizarTexto();
        }

        //activate/deactivate menu
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            menu.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            menu.SetActive(false);
        }
    }

    //update texts
    public void actualizarTexto()
    {
        iterationText.text = "Duplicado: " + iteration;
    }

    public void salir()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("menu");
    }
}