using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{

    private GameObject player;
    private Transform camSpot;
    private Vector3 cameraOffset;
    private bool stop;
    private Vector3 rot;
    Vector3 offset;
    public bool right;
    float nextRot;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        camSpot = player.transform.Find("CamSpot");
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 19, player.transform.position.z - 25);
        cameraOffset = transform.position - camSpot.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!stop)
        {
            transform.position = camSpot.position + cameraOffset;
            rot.y = transform.rotation.eulerAngles.y;
            nextRot = (int)rot.y;
            if(Input.GetAxis("Horizontal") < 0)
            {
                right = false; if (nextRot == 0) { nextRot = nextRot + 90; } else { nextRot = nextRot + 90; }
            } else { right = true; if (nextRot == 0) { nextRot = 360 - 90; } else { nextRot = nextRot - 90; } }
        }

    }

    public void TurnCam()
    {
        bool cont = true;
        float distance = nextRot - transform.rotation.eulerAngles.y;
        if(distance > -2 && distance < 2) { cont = false; }
        //Debug.Log(distance);

        if (cont)
        {
            stop = true;
            Vector3 newPos;
            Quaternion camH;
            if (right) {camH = Quaternion.AngleAxis(-.80f * 1, this.transform.up); }
            else { camH = Quaternion.AngleAxis(.80f * 1, this.transform.up);}          
            cameraOffset = camH * cameraOffset;
            newPos = camSpot.position + cameraOffset;

            transform.position = Vector3.Slerp(transform.position, newPos, 1);
            transform.LookAt(camSpot);
        }
        else
        {
            if (right) { rot = new Vector3(25.6f, rot.y - 90, rot.z); }
            else { rot = new Vector3(25.6f, rot.y + 90, rot.z);}
            
            transform.rotation = Quaternion.Euler(rot); stop = false; player.GetComponent<Controller>().ExitPortal();
        }

    }
}
