using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSlice : MonoBehaviour {
    public Transform A;
    public Transform B;
    public Transform C;
    public Transform water;

    private Material mat;

    void Awake()
    {
        mat = water.GetComponent<MeshRenderer>().material;
    }

	void Update ()
    {
        mat.SetVector("_A", A.position);
        mat.SetVector("_B", B.position);
        mat.SetVector("_C", C.position);
    }
}
