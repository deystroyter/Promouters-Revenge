using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceLightsController : MonoBehaviour
{
    public GameObject LightGO1;

    public GameObject LightGO2;


    // Update is called once per frame
    void Update()
    {
        LightGO1.transform.Rotate(new Vector3(0, 90, 0) * Time.deltaTime);
        LightGO2.transform.Rotate(new Vector3(0, 90, 0) * Time.deltaTime);
    }
}