using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Transform cam;
    public enum BillboardTypes { CameraForward, OnlyYZ }
    public BillboardTypes type = BillboardTypes.OnlyYZ;
    void Start()
    {
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (type == BillboardTypes.CameraForward)
        {
        transform.forward = cam.forward;
        }
        else if (type == BillboardTypes.OnlyYZ)
        {
            transform.forward = cam.forward;
            transform.forward.Scale(new Vector3(0, 1, 1));
        }
    }
}
