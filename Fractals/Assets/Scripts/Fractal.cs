using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour
{
    public Mesh[] meshes; // allows for multiple mesh inputs
    public Material material; // original material

    // set number of children interation depths
    public int maxDepth;
    private int depth;

    public float childScale;
    public float spawnProbability;

    public float maxRotationSpeed;
    private float rotationSpeed;
    
    // add twist values
    public float maxTwist;

    private Material[,] materials; // different colors used on meshes based on material

    // Start is called before the first frame update
    private void Start()
    {
        // set rotation speed
        rotationSpeed = Random.Range(-maxRotationSpeed, maxRotationSpeed);
        transform.Rotate(Random.Range(-maxTwist, maxTwist), 0f, 0f);

        if (materials == null)
        {
            InitializeMaterials();
        }

        gameObject.AddComponent<MeshFilter>().mesh = meshes[Random.Range(0, meshes.Length)];
        gameObject.AddComponent<MeshRenderer>().material = materials[depth, Random.Range(0,2)];
        //! create a colour ramp
        // iterate over depth
        if (depth < maxDepth)
        {
            StartCoroutine(CreateChildren());  
        }
    }

    private static Vector3[] childDirections ={
        Vector3.up,     //0
        Vector3.right,  //1
        Vector3.left,    //2
        Vector3.forward,
        Vector3.back

    };

    private static Quaternion[] childOrientations = {
        Quaternion.identity,
        Quaternion.Euler(0f, 0f, -90f),
        Quaternion.Euler(0f, 0f, 90f),
        Quaternion.Euler(90f, 0f, 0f),
        Quaternion.Euler(-90f, 0f, 0f)
    };

  
    private IEnumerator CreateChildren()
    {
        
        //! old version
        // yield return new WaitForSeconds(0.5f);
        // new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, Vector3.up, Quaternion.identity);
        // yield return new WaitForSeconds(0.5f);
        // new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, Vector3.right, Quaternion.Euler(0f, 0f, -90f));
        // yield return new WaitForSeconds(0.5f);
        // new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, Vector3.left, Quaternion.Euler(0f, 0f, 90f));
        
       


        //! Updated version
        for (int i = 0; i < childDirections.Length; i++){
            if (Random.value < spawnProbability)
            {
                yield return new WaitForSeconds(Random.Range(0.1f,0.5f));
                //! recreate a new object with the Fractal class attached to it
                new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, i);
            }
            
        }

      

    }

    //OLD WAY  -->private void Initialize(Fractal parent, Vector3 direction, Quaternion orientation)
    private void Initialize(Fractal parent, int ChildIndex)
    {
        meshes = parent.meshes;
        materials = parent.materials;
        maxDepth = parent.maxDepth;
        depth = parent.depth + 1;
        spawnProbability = parent.spawnProbability;
        maxRotationSpeed = parent.maxRotationSpeed;
        maxTwist = parent.maxTwist;
        
        childScale = parent.childScale;
        // create paretn/child hierarchy
        transform.parent = parent.transform;
        // set scale for children
        transform.localScale = Vector3.one * childScale;
        

        
        //! Elegant way of recursing through the arrays
        // set position
        transform.localPosition = childDirections[ChildIndex] * (0.5f + 0.5f * childScale);
        // set rotation
        transform.localRotation = childOrientations[ChildIndex];
        
    }

    private void InitializeMaterials()
    {
        materials = new Material[maxDepth + 1,2];
        for (int i = 0; i <= maxDepth; i++)
        {
            // add magenta?
            float t = i / (maxDepth -1f);
            t *= t;
            materials[i, 0] = new Material(material);
            materials[i, 0].color = Color.Lerp(Color.white, Color.yellow,t);
            materials[i, 1] = new Material(material);
            materials[i, 1].color = Color.Lerp(Color.white, Color.cyan,t);
        }
        materials[maxDepth,0].color = Color.magenta;
        materials[maxDepth,1].color = Color.red;
    }

   
    // Update is called once per frame
    void Update()
    {
        // add rotation
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}