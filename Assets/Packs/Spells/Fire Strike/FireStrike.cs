using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStrike : MonoBehaviour {

    [SerializeField] float destroyTime;
    [SerializeField] float speed;
    private GameObject player;
    private Animator anim;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = player.GetComponent<Animator>();
        Destroy(this.gameObject, destroyTime);
	}
	
	// Update is called once per frame
	void Update () {
        this.GetComponent<Rigidbody>().AddForce(transform.forward * speed, ForceMode.Acceleration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Enemy")
        {
            //Enemy Takes Damage
        }
    }
}
