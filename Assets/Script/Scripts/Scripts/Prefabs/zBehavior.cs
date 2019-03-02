using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, 3);
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.right * Time.deltaTime * 30);

	}
}
