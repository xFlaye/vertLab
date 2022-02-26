using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour
{
    public Mesh mesh;
    public Material material;

    // set number of children interation depths
    public int maxDepth;
    private int depth;

    public float childScale;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<MeshFilter>().mesh = mesh;
        gameObject.AddComponent<MeshRenderer>().material = material;

        // iterate over depth
        if (depth < maxDepth)
        {
            StartCoroutine(CreateChildren());           
        }
    }

    private IEnumerator CreateChildren()
    {
        yield return new WaitForSeconds(0.5f);
        new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, Vector3.up, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, Vector3.right, Quaternion.Euler(0f, 0f, -90f));
        yield return new WaitForSeconds(0.5f);
        new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, Vector3.left, Quaternion.Euler(0f, 0f, 90f));
    }

    private void Initialize(Fractal parent, Vector3 direction, Quaternion orientation)
    {
        mesh = parent.mesh;
        material = parent.material;
        maxDepth = parent.maxDepth;
        depth = parent.depth + 1;
        childScale = parent.childScale;
        // create paretn/child hierarchy
        transform.parent = parent.transform;
        // set scale for children
        transform.localScale = Vector3.one * childScale;
        // set position
        transform.localPosition = direction * (0.5f + 0.5f * childScale);
        // set rotation
        transform.localRotation = orientation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
