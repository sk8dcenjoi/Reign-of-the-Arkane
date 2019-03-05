using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controller : MonoBehaviour
{

    private GameObject player;
    private Animator anim;
    [SerializeField] float speed;
    [SerializeField] GameObject leftHand,rightHand;
    private bool attacking;
    private int attackNum;
    private bool grounded = true;
    private bool canDodge = true;
    private bool jumped, wait, swapping, mode;
    private Rigidbody rb;
    private string forward;
    bool left = false;
    bool right = true;

    GameObject currentSpell;
    private float startTime, waitTime;

    Vector3 rot;
    SpellHandler spells;

    public enum states { death, flinch, idle, walk, run, jump, fall, land, attack, iframes, waitDelay };
    states currentState = states.idle;


    // Use this for initialization
    void Start()
    {
        spells = transform.GetComponent<SpellHandler>();
        player = GameObject.FindGameObjectWithTag("Player");
        rb = this.GetComponent<Rigidbody>();
        anim = player.GetComponent<Animator>();
        anim.SetInteger("animation", 1);
        forward = "Horizontal";
        rot = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        mode = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis(forward) < 0 && rot == transform.rotation.eulerAngles && right && !attacking)
        {
            float newY = rot.y + 180;
            if (newY == 450) { newY = 90; } else if (newY == 360) { newY = 0; }
            right = false;
            rot = transform.rotation.eulerAngles;
            rot = new Vector3(rot.x, newY, rot.z);
            transform.rotation = Quaternion.Euler(rot);
            left = true;
        }
        else if (Input.GetAxis(forward) > 0 && rot == transform.rotation.eulerAngles && left && !attacking)
        {
            float newY = rot.y + 180;
            if (newY == 450) { newY = 90; } else if (newY == 360) { newY = 0; }
            left = false;
            rot = transform.rotation.eulerAngles;
            rot = new Vector3(rot.x, newY, rot.z);
            transform.rotation = Quaternion.Euler(rot);
            right = true;
        }

        if(Input.GetButtonDown("Swap") && !swapping) { StartCoroutine("SwapMode"); }


        if (grounded)
        {
            if (Input.GetButtonDown("Iframes") && canDodge && grounded) { currentState = states.iframes; }
            if (mode)
            {
                if (!(attacking) && Input.GetButtonDown("Fire1")) { attacking = true; currentState = states.attack; attackNum = 1; }
                if (!(attacking) && Input.GetButtonDown("Fire2")) { currentState = states.attack; attackNum = 2; }
                if (!(attacking) && Input.GetButtonDown("Fire3")) { currentState = states.attack; attackNum = 3; }
            }
            else
            {
                if (!(attacking) && Input.GetButtonDown("Fire1")) { currentState = states.attack; attackNum = 4; }
                if (!(attacking) && Input.GetButtonDown("Fire2")) { currentState = states.attack; attackNum = 5; }
                if (!(attacking) && Input.GetButtonDown("Fire3")) { currentState = states.attack; attackNum = 6; }
            }


            if (anim.GetInteger("animation") == 2 && Input.GetAxis(forward) > 0) { transform.Translate(Vector3.forward * speed * Time.deltaTime); }
            if (anim.GetInteger("animation") == 2 && Input.GetAxis(forward) < 0) { transform.Translate(Vector3.forward * speed * Time.deltaTime); }
            if (anim.GetInteger("animation") == 7 && Input.GetAxis(forward) < 0) { rb.AddForce(Vector3.right * -3000, ForceMode.Acceleration); }
            if (anim.GetInteger("animation") == 7 && Input.GetAxis(forward) > 0) { rb.AddForce(Vector3.right * 3000, ForceMode.Acceleration); }
        }

        if (!grounded)
        {
            if (Input.GetAxis(forward) > 0) { transform.Translate(Vector3.forward * speed * Time.deltaTime); }
            if (Input.GetAxis(forward) < 0) { transform.Translate(Vector3.forward * speed * Time.deltaTime); }
        }


        switch (currentState)
        {
            case states.idle:
                grounded = true;
                Jump();
                if (!(attacking) && Input.GetButtonDown("Fire1")) { currentState = states.attack; attackNum = 1; }
                anim.SetInteger("animation", 1);
                if (Input.GetAxis(forward) > 0 || Input.GetAxis(forward) < 0) { currentState = states.walk; }
                break;
            case states.walk:
                Jump();
                if (!attacking) {anim.SetInteger("animation", 2); }
                
                if (!(Input.GetAxis(forward) > 0 || Input.GetAxis(forward) < 0)) { currentState = states.idle; }
                break;
            case states.jump:
                anim.SetInteger("animation", 4);
                if (jumped) { rb.AddForce(Vector3.up * 2000, ForceMode.Acceleration); jumped = false; }
                break;
            case states.fall:
                RaycastHit hit;
                if (Physics.Raycast(transform.Find("JumpCast").transform.position, transform.Find("JumpCast").transform.forward, out hit, 100))
                {
                    if (hit.distance < .40f) { currentState = states.land; } else { grounded = false; }
                }
                if (Physics.Raycast(transform.Find("JumpCastTwo").transform.position, transform.Find("JumpCastTwo").transform.forward, out hit, 100))
                {
                    if (hit.distance < .40f) { currentState = states.land; } else { grounded = false; }
                }
                break;
            case states.land:
                anim.SetInteger("animation", 6);
                grounded = true;
                if (Input.GetAxis(forward) > 0 || Input.GetAxis(forward) < 0) { currentState = states.walk; } else { currentState = states.idle; }
                break;
            case states.iframes:
                canDodge = false;
                if (Input.GetAxis(forward) < 0 || Input.GetAxis(forward) > 0) { anim.SetInteger("animation", 7); }
                StartCoroutine("dodgeCoolDown");
                if (Input.GetAxis(forward) > 0 || Input.GetAxis(forward) < 0) { currentState = states.walk; } else { currentState = states.idle; }
                break;
            case states.attack:
                if (Input.GetAxis(forward) > 0 || Input.GetAxis(forward) < 0) { currentState = states.walk; }
                switch (attackNum)
                {
                    case 1:
                        if (!wait)
                        {
                            wait = true;
                            currentSpell = spells.s1;
                            startTime = spells.s1.GetComponent<Spell>().startTime;
                            waitTime = spells.s1.GetComponent<Spell>().waitTime;
                            anim.SetInteger("animation", spells.s1.GetComponent<Spell>().animNum);
                            StartCoroutine("Attack", spells.s1.GetComponent<Spell>().waitTime);                           
                        }
                        break;
                    case 2:
                        if (!wait)
                        {
                            wait = true;
                            currentSpell = spells.s2;
                            startTime = spells.s2.GetComponent<Spell>().startTime;
                            waitTime = spells.s2.GetComponent<Spell>().waitTime;
                            anim.SetInteger("animation", spells.s1.GetComponent<Spell>().animNum);
                            StartCoroutine("Attack", spells.s1.GetComponent<Spell>().waitTime);
                        }
                        break;
                    case 3:
                        if (!wait)
                        {
                            wait = true;
                            currentSpell = spells.s3;
                            startTime = spells.s3.GetComponent<Spell>().startTime;
                            waitTime = spells.s3.GetComponent<Spell>().waitTime;
                            anim.SetInteger("animation", spells.s1.GetComponent<Spell>().animNum);
                            StartCoroutine("Attack", spells.s1.GetComponent<Spell>().waitTime);
                        }
                        break;
                    case 4:
                        if (!wait)
                        {
                            wait = true;
                            currentSpell = spells.s4;
                            startTime = spells.s4.GetComponent<Spell>().startTime;
                            waitTime = spells.s4.GetComponent<Spell>().waitTime;
                            anim.SetInteger("animation", spells.s1.GetComponent<Spell>().animNum);
                            StartCoroutine("Attack", spells.s1.GetComponent<Spell>().waitTime);
                        }
                        break;
                    case 5:
                        if (!wait)
                        {
                            wait = true;
                            currentSpell = spells.s5;
                            startTime = spells.s5.GetComponent<Spell>().startTime;
                            waitTime = spells.s5.GetComponent<Spell>().waitTime;
                            anim.SetInteger("animation", spells.s1.GetComponent<Spell>().animNum);
                            StartCoroutine("Attack", spells.s1.GetComponent<Spell>().waitTime);
                        }
                        break;
                    case 6:
                        if (!wait)
                        {
                            wait = true;
                            currentSpell = spells.s6;
                            startTime = spells.s6.GetComponent<Spell>().startTime;
                            waitTime = spells.s6.GetComponent<Spell>().waitTime;
                            anim.SetInteger("animation", spells.s1.GetComponent<Spell>().animNum);
                            StartCoroutine("Attack", spells.s1.GetComponent<Spell>().waitTime);
                        }
                        break;
                }
                break;
            case states.waitDelay:

                break;

                
        }

    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && grounded)
        {
            jumped = true; grounded = false; currentState = states.jump;
        }

    }

    private IEnumerator dodgeCoolDown()
    {
        yield return new WaitForSeconds(2f);
        canDodge = true;
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(startTime);
        Instantiate(currentSpell, leftHand.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(waitTime);
        attacking = false; wait = false; currentState = states.idle; 
    }

    private IEnumerator SwapMode()
    {
        swapping = true;
        if (mode) { mode = false; } else { mode = true; }
        yield return new WaitForSeconds(1f);
        swapping = false;
    }

    private void StartFall()
    {
        anim.SetInteger("animation", 5); currentState = states.fall;
    }
}