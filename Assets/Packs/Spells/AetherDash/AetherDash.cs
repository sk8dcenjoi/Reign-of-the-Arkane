using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AetherDash : MonoBehaviour {
    private GameObject player;
    Rigidbody rb;
    bool stop;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = player.GetComponent<Rigidbody>();
        

	}
	
	// Update is called once per frame
	void Update () {
        if (!stop)
        {
            if(player.GetComponent<Controller>().left) { rb.AddForce(Camera.main.transform.right * (-3000f)); } else { rb.AddForce(Camera.main.transform.right * (3000f)); }
            StartCoroutine("Stop");
        }

	}

    private IEnumerator Stop()
    {
        yield return new WaitForSeconds(.2f);
        rb.isKinematic = true;
        stop = true;

        yield return new WaitForSeconds(.5f);
        rb.isKinematic = false;
    }
}
