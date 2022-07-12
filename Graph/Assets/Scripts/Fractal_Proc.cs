using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal_Proc : MonoBehaviour
{
   [SerializeField, Range(1,8)]
    int depth = 4;

    [SerializeField]
    Mesh mesh;

    [SerializeField]
    Material material;

    FractalPart[][] parts;

    // create array matrix to do the calculations
    Matrix4x4[][] matrices;

    //! render using GPU
    ComputeBuffer[] matricesBuffers;

	static readonly int matricesId = Shader.PropertyToID("_Matrices");

    // set directions & rotations in static arrays
    static Vector3[] directions = {
        Vector3.up, Vector3.right, Vector3.left, Vector3.forward, Vector3.back 
    };

    static Quaternion[] rotations = {
        Quaternion.identity,
        Quaternion.Euler(0f, 0f, -90f), Quaternion.Euler(0f, 0f, 90f),
        Quaternion.Euler(90f, 0f, 0f), Quaternion.Euler(-90f, 0f, 0f)
    };

    
   FractalPart CreatePart (int childIndex) => new FractalPart {
		direction = directions[childIndex],
		rotation = rotations[childIndex]
	};
	
    // change awake to onEnable to release the buffer
    void OnEnable () {
        parts = new FractalPart[depth][];
		matrices = new Matrix4x4[depth][];
		matricesBuffers = new ComputeBuffer[depth];
        // A 4×4 matrix has sixteen float values, so the stride of the buffers is sixteen times four bytes.
		int stride = 16 * 4;
		for (int i = 0, length = 1; i < parts.Length; i++, length *= 5) {
			parts[i] = new FractalPart[length];
			matrices[i] = new Matrix4x4[length];
			matricesBuffers[i] = new ComputeBuffer(length, stride);
		}

        /* TRADITIONAL WAY OF DOING IT   

		int length = 1;
		for (int i = 0; i < parts.Length; i++) {
			parts[i] = new FractalPart[length];
			length *= 5;
        }
        */
        
		parts[0][0] = CreatePart(0);
		for (int li = 1; li < parts.Length; li++) {
			
			FractalPart[] levelParts = parts[li];
			for (int fpi = 0; fpi < levelParts.Length; fpi += 5) {
				for (int ci = 0; ci < 5; ci++) {
					levelParts[fpi + ci] = CreatePart(ci);
				}
			}
		}
	}

    void OnDisable () {
		for (int i = 0; i < matricesBuffers.Length; i++) {
			matricesBuffers[i].Release();
		}
        //! get rid of all array references
        parts = null;
		matrices = null;
		matricesBuffers = null;
	}

    void OnValidate () {
		if (parts != null && enabled) {
			OnDisable();
			OnEnable();
		}
	}

    struct FractalPart 
    {
        public Vector3 direction, worldPosition;
        public Quaternion rotation, worldRotation;
        public float spinAngle;
        
    }

    
    void Update () {
        // set animation rotation
        // 
        float spinAngleDelta = 22.5f * Time.deltaTime;
        
        // start animating in order, starting at the root
        FractalPart rootPart = parts[0][0];
		
		rootPart.spinAngle += spinAngleDelta;
        /*
        FractalPart is a struct, which is a value type, so changing a local variable of it doesn't change anything else. 
        We have to copy it back to its array element—replacing the old data—in order to remember that its rotation has changed.
        */
        rootPart.worldRotation = rootPart.rotation * Quaternion.Euler(0f, rootPart.spinAngle, 0f);

        parts[0][0] = rootPart;

        //! TRS = translation, rotation, scale
        matrices[0][0] = Matrix4x4.TRS(
			rootPart.worldPosition, rootPart.worldRotation, Vector3.one
		);

		float scale = 1f;
		for (int li = 1; li < parts.Length; li++) {
			scale *= 0.5f;
			FractalPart[] parentParts = parts[li - 1];
			FractalPart[] levelParts = parts[li];
            Matrix4x4[] levelMatrices = matrices[li];
			for (int fpi = 0; fpi < levelParts.Length; fpi++) {
				//Transform parentTransform = parentParts[fpi / 5].transform;
				FractalPart parent = parentParts[fpi / 5];
				FractalPart part = levelParts[fpi];

				part.spinAngle += spinAngleDelta;
				part.worldRotation =
					parent.worldRotation *
					(part.rotation * Quaternion.Euler(0f, part.spinAngle, 0f));

				part.worldPosition =
					parent.worldPosition +
					parent.worldRotation * (1.5f * scale * part.direction);
				levelParts[fpi] = part;

                levelMatrices[fpi] = Matrix4x4.TRS(
					part.worldPosition, part.worldRotation, scale * Vector3.one
				);
			}
		}
        /*
        Finally, to upload the matrices to the GPU invoke SetData on all buffers at the end of Update, 
        with the corresponding matrices array as an argument.
        */
        
        var bounds = new Bounds(Vector3.zero, 3f * Vector3.one);
		for (int i = 0; i < matricesBuffers.Length; i++) {
			ComputeBuffer buffer = matricesBuffers[i];
			buffer.SetData(matrices[i]);
			material.SetBuffer(matricesId, buffer);
			Graphics.DrawMeshInstancedProcedural(mesh, 0, material, bounds, buffer.count);
		}
	}	
    
    
}
