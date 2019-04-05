using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEvents : MonoBehaviour {


    private GameObject player;
    private Animator anim;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = player.GetComponent<Animator>();
    }

	// Update is called once per frame
	void Update () {
		
	}

    private void Cast()
    {
        anim.SetInteger("animation", 151);
    }
}
