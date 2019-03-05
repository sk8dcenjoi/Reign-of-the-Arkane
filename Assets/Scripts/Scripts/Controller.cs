using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controller : MonoBehaviour
{

    private GameObject player;
    private Animator anim;
    [SerializeField] float speed;
    [SerializeField] GameObject leftHand, rightHand, leftFoot, rightFoot;
    private bool attacking;
    private int attackNum;
    private bool grounded = true;
    private bool jumped, wait, swapping, mode;
    private Rigidbody rb;
    private string forward;
    bool left = false;
    bool right = true;

    GameObject currentSpell;
    private float startTime, waitTime;

    Vector3 rot;
    SpellHandler spells;

    public enum states { death, flinch, idle, walk, run, jump, fall, land, attack, waitDelay };
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


        if(Input.GetAxis(forward) > 0 && left && !attacking && currentState != states.waitDelay) { left = false; right = true; transform.localRotation *= Quaternion.Euler(0, 180, 0); }
        else if (Input.GetAxis(forward) < 0 && right && !attacking && currentState != states.waitDelay) { right = false; left= true; transform.localRotation *= Quaternion.Euler(0, 180, 0); }


        if (Input.GetButtonDown("Swap") && !swapping) { StartCoroutine("SwapMode"); }


        if (grounded && currentState != states.waitDelay)
        {
            if (mode)
            {
                if (!(attacking) && Input.GetButtonDown("Fire1")) { attacking = true; currentState = states.attack; attackNum = 1; }
                if (!(attacking) && Input.GetButtonDown("Fire2")) { attacking = true; currentState = states.attack; attackNum = 2; }
                if (!(attacking) && Input.GetButtonDown("Fire3")) { attacking = true; currentState = states.attack; attackNum = 3; }
            }
            else
            {
                //if (!(attacking) && Input.GetButtonDown("Fire1")) { currentState = states.attack; attackNum = 4; }
                //if (!(attacking) && Input.GetButtonDown("Fire2")) { currentState = states.attack; attackNum = 5; }
                //if (!(attacking) && Input.GetButtonDown("Fire3")) { currentState = states.attack; attackNum = 6; }
            }


            if (anim.GetInteger("animation") == 2 && (Input.GetAxis(forward) > 0)) { transform.Translate(Vector3.forward * speed * Time.deltaTime); }
            if (anim.GetInteger("animation") == 2 && (Input.GetAxis(forward) < 0)) { transform.Translate(Vector3.forward * speed * Time.deltaTime); }
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
                //Jump();
                if (!(attacking) && Input.GetButtonDown("Fire1")) { currentState = states.attack; attackNum = 1; }
                anim.SetInteger("animation", 1);
                if (Input.GetAxis(forward) > 0 || Input.GetAxis(forward) < 0) { currentState = states.walk; }
                break;
            case states.walk:
                
                if (!attacking) {Jump(); anim.SetInteger("animation", 2); }               
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
                    if (hit.distance < .4f) { currentState = states.land; } else { grounded = false; }
                }
                if (Physics.Raycast(transform.Find("JumpCastTwo").transform.position, transform.Find("JumpCastTwo").transform.forward, out hit, 100))
                {
                    if (hit.distance < .4f) { currentState = states.land; } else { grounded = false; }
                }
                break;
            case states.land:
                anim.SetInteger("animation", 6);
                grounded = true;
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
                            anim.SetInteger("animation", currentSpell.GetComponent<Spell>().animNum);
                            StartCoroutine("Attack", currentSpell.GetComponent<Spell>().waitTime);
                        }
                        break;
                    case 2:
                        if (!wait)
                        {
                            wait = true;
                            currentSpell = spells.s2;
                            startTime = spells.s2.GetComponent<Spell>().startTime;
                            waitTime = spells.s2.GetComponent<Spell>().waitTime;
                            anim.SetInteger("animation", currentSpell.GetComponent<Spell>().animNum);
                            StartCoroutine("Attack", currentSpell.GetComponent<Spell>().waitTime);
                        }
                        break;
                    case 3:
                        if (!wait)
                        {
                            wait = true;
                            currentSpell = spells.s3;
                            startTime = spells.s3.GetComponent<Spell>().startTime;
                            waitTime = spells.s3.GetComponent<Spell>().waitTime;
                            anim.SetInteger("animation", currentSpell.GetComponent<Spell>().animNum);
                            StartCoroutine("Attack", currentSpell.GetComponent<Spell>().waitTime);
                        }
                        break;
                    case 4:
                        if (!wait)
                        {
                            wait = true;
                            currentSpell = spells.s4;
                            startTime = spells.s4.GetComponent<Spell>().startTime;
                            waitTime = spells.s4.GetComponent<Spell>().waitTime;
                            anim.SetInteger("animation", currentSpell.GetComponent<Spell>().animNum);
                            StartCoroutine("Attack", currentSpell.GetComponent<Spell>().waitTime);
                        }
                        break;
                    case 5:
                        if (!wait)
                        {
                            wait = true;
                            currentSpell = spells.s5;
                            startTime = spells.s5.GetComponent<Spell>().startTime;
                            waitTime = spells.s5.GetComponent<Spell>().waitTime;
                            anim.SetInteger("animation", currentSpell.GetComponent<Spell>().animNum);
                            StartCoroutine("Attack", currentSpell.GetComponent<Spell>().waitTime);
                        }
                        break;
                    case 6:
                        if (!wait)
                        {
                            wait = true;
                            currentSpell = spells.s6;
                            startTime = spells.s6.GetComponent<Spell>().startTime;
                            waitTime = spells.s6.GetComponent<Spell>().waitTime;
                            anim.SetInteger("animation", currentSpell.GetComponent<Spell>().animNum);
                            StartCoroutine("Attack", currentSpell.GetComponent<Spell>().waitTime);
                        }
                        break;
                }
                break;
            case states.waitDelay:
                transform.Translate(Vector3.forward * 50 * Time.deltaTime);
                Camera.main.GetComponent<MainCamera>().TurnCam();
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

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(startTime);
        string loc = currentSpell.GetComponent<Spell>().spawnLoc;
        
        switch (loc)
        {
            case "leftHand":
                Instantiate(currentSpell, leftHand.transform.position, Quaternion.Euler(currentSpell.GetComponent<Spell>().rotation));
                break;
            case "rightHand":
                Instantiate(currentSpell, rightHand.transform.position, Quaternion.Euler(currentSpell.GetComponent<Spell>().rotation));
                break;
            case "leftFoot":
                Instantiate(currentSpell, leftFoot.transform.position, Quaternion.Euler(currentSpell.GetComponent<Spell>().rotation));
                break;
            case "rightFoot":
                Instantiate(currentSpell, rightFoot.transform.position, Quaternion.Euler(currentSpell.GetComponent<Spell>().rotation));
                break;
            case "Dual Pillars":
                Instantiate(currentSpell, transform.Find("SpawnLocs").transform.Find("Dual Pillars").transform.position, Quaternion.Euler(currentSpell.GetComponent<Spell>().rotation));
                break;
            default:
                Debug.Log("None Specified");
                break;
        }
        yield return new WaitForSeconds(waitTime);
        attacking = false; wait = false;
        if (Input.GetAxis(forward) != 0) { currentState = states.walk; } else { currentState = states.idle; }
        
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
        if(currentState != states.waitDelay) { anim.SetInteger("animation", 5); currentState = states.fall; }
        else { anim.SetInteger("animation", 2); }
    }

    private void ChangeRotation()
    {
        if (Camera.main.GetComponent<MainCamera>().right)
        {
            rot = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y-90, transform.rotation.eulerAngles.z);
        }
        else { rot = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 90, transform.rotation.eulerAngles.z); }

        transform.rotation = Quaternion.Euler(rot);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Portal")
        {
            currentState = states.waitDelay;
        }
        if(other.transform.tag == "Rotate") { ChangeRotation(); }

        if(other.transform.tag == "FixX")
        {
            string name = other.transform.name;
            switch (name)
            {
                case "B1":
                    transform.position = new Vector3(965, transform.position.y, transform.position.z);
                    break;
                case "B2":
                    transform.position = new Vector3(965, transform.position.y, transform.position.z);
                    break;
            }
        }
        if (other.transform.tag == "FixZ")
        {

            string name = other.transform.name;
            switch (name)
            {
                case "A1":
                    transform.position = new Vector3(transform.position.x, transform.position.y, 35);
                    break;
                case "A2":
                    transform.position = new Vector3(transform.position.x, transform.position.y, 35);
                    break;
                case "C1":
                    transform.position = new Vector3(transform.position.x, transform.position.y, 966);
                    break;
                case "C2":
                    transform.position = new Vector3(transform.position.x, transform.position.y, 966);
                    break;
            }
        }
    }

    public void ExitPortal()
    {
        currentState = states.idle;
    }
}