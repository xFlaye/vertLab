using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal_Opt : MonoBehaviour
{
    [SerializeField, Range(1,8)]
    int depth = 4;

    [SerializeField]
    Mesh mesh;

    [SerializeField]
    Material material;

    FractalPart[][] parts;

    // set directions & rotations in static arrays
    static Vector3[] directions = {
        Vector3.up, Vector3.right, Vector3.left, Vector3.forward, Vector3.back 
    };

    static Quaternion[] rotations = {
        Quaternion.identity,
        Quaternion.Euler(0f, 0f, -90f), Quaternion.Euler(0f, 0f, 90f),
        Quaternion.Euler(90f, 0f, 0f), Quaternion.Euler(-90f, 0f, 0f)
    };

    // change CreatePart so it returns a new FractalPart struct value.
    FractalPart CreatePart (int levelIndex, int childIndex, float scale) {
		var go = new GameObject("Fractal Part L" + levelIndex + " C" + childIndex);
        go.transform.localScale = scale * Vector3.one;
		go.transform.SetParent(transform, false);

        //! add the components
        go.AddComponent<MeshFilter>().mesh = mesh;
		go.AddComponent<MeshRenderer>().material = material;

        return new FractalPart {
			direction = directions[childIndex],
			rotation = rotations[childIndex],
			transform = go.transform
		};
	}
    void Awake () {
        parts = new FractalPart[depth][];

        for (int i = 0, length = 1; i < parts.Length; i++, length *= 5) {
			parts[i] = new FractalPart[length];
		}

        /* TRADITIONAL WAY OF DOING IT   

		int length = 1;
		for (int i = 0; i < parts.Length; i++) {
			parts[i] = new FractalPart[length];
			length *= 5;
        }
        */
        float scale = 1f;
		parts[0][0] = CreatePart(0, 0, scale);
		for (int li = 1; li < parts.Length; li++) {
			scale *= 0.5f;
			FractalPart[] levelParts = parts[li];
			for (int fpi = 0; fpi < levelParts.Length; fpi += 5) {
				for (int ci = 0; ci < 5; ci++) {
					levelParts[fpi + ci] = CreatePart(li, ci, scale);
				}
			}
		}
	}

    struct FractalPart 
    {
        public Vector3 direction;
        public Quaternion rotation;
        public Transform transform;
    }

    
    void Update () {
        // set animation rotation
        Quaternion deltaRotation = Quaternion.Euler(0f, 22.5f * Time.deltaTime, 0f);
        
        // start animating in order, starting at the root
        FractalPart rootPart = parts[0][0];
		rootPart.rotation *= deltaRotation;
        /*
        FractalPart is a struct, which is a value type, so changing a local variable of it doesn't change anything else. 
        We have to copy it back to its array element—replacing the old data—in order to remember that its rotation has changed.
        */
        rootPart.transform.localRotation = rootPart.rotation;
        parts[0][0] = rootPart;

		for (int li = 1; li < parts.Length; li++) {
			FractalPart[] parentParts = parts[li - 1];
			FractalPart[] levelParts = parts[li];
			for (int fpi = 0; fpi < levelParts.Length; fpi++) {
				Transform parentTransform = parentParts[fpi / 5].transform;
				FractalPart part = levelParts[fpi];
                // set the animation rotation
                part.rotation *= deltaRotation;

                //! set transforms

                part.transform.localRotation = parentTransform.localRotation * part.rotation;	

				part.transform.localPosition =
					parentTransform.localPosition +
					parentTransform.localRotation *
						(1.5f * part.transform.localScale.x * part.direction);
                        
                levelParts[fpi] = part;
			}
		}
	}
    
    
    
}
