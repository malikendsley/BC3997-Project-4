using System;
using UnityEngine;

public class managerEyeLookAt : MonoBehaviour
{

    public GameObject eyeL;
    public GameObject eyeR;
    private Renderer renderEyeL, renderEyeR;
    public Transform objPivotEye;
    public Transform objPivotLookAt;


    // Start is called before the first frame update
    void Start()
    {
        renderEyeL = eyeL.GetComponent<Renderer>();
        renderEyeR = eyeR.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Clamping
        objPivotEye.LookAt(objPivotLookAt);
        Vector2 tempEyeRot = new(objPivotEye.rotation.y, objPivotEye.rotation.x);
        renderEyeL.material.mainTextureOffset = tempEyeRot;
        renderEyeR.material.mainTextureOffset = tempEyeRot;
    }
}
