using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStrike : MonoBehaviour {

    [SerializeField] float destroyTime;
    [SerializeField] float speed;
    private GameObject player;
    private Animator anim;
    bool goLeft;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = player.GetComponent<Animator>();
        Destroy(this.gameObject, destroyTime);
        if (player.GetComponent<Controller>().left) { goLeft = true; }

	}
	
	// Update is called once per frame
	void Update () {
        if(goLeft) { this.GetComponent<Rigidbody>().AddForce(Camera.main.transform.right * -speed, ForceMode.Acceleration);}
        else { this.GetComponent<Rigidbody>().AddForce(Camera.main.transform.right * speed, ForceMode.Acceleration); }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Enemy")
        {
            //Enemy Takes Damage
        }
    }
}
