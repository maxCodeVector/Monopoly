using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skyRotate : MonoBehaviour {
	float num;
	// Use this for initialization
	void Start () {
		
	}
	// Update is called once per frame
	void Update () {
        num = RenderSettings.skybox.GetFloat("_Rotation");
        RenderSettings.skybox.SetFloat("_Rotation", num + 0.005f);
    }
}
