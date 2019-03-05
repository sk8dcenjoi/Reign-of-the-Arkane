using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraStomp : MonoBehaviour {

    [SerializeField] float maxSize;
    [SerializeField]  GameObject second;
    GameObject player;
    GameObject oops;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        Vector3 loc = player.transform.Find("SpawnLocs").transform.Find("Dual Pillars").transform.Find("Second Pillar").transform.position;
        Destroy(this.gameObject, 2);
        if (second != null)
        {
            oops = Instantiate(second, loc, Quaternion.Euler(this.gameObject.GetComponent<Spell>().rotation));
        }
        
    }
	
	// Update is called once per frame
	void Update () {
        if (oops)
        {
            if(transform.localScale.y < maxSize)
            {
                transform.localScale += new Vector3(0, .5f, 0);
            }
        }
        else
        {
            if (transform.localScale.y < maxSize * 1.01f)
            {
                transform.localScale += new Vector3(0, .4f, 0);
            }
        }

        
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Enemy")
        {
            //do damage
            //Maybe knockback
        }
    }
}
