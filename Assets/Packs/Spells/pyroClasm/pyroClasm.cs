using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pyroClasm : MonoBehaviour
{
    GameObject[] Enemies;
    [SerializeField] float SphereRange;
    float currentRange = 5;
    private string ourButton;
    private bool letGo;

    // Use this for initialization
    void Start()
    {
        Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        ourButton = GameObject.FindGameObjectWithTag("Player").GetComponent<Controller>().currentButton;
        Destroy(this.gameObject, 1);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, Vector3.right, Color.green);

        if (!Input.GetButtonUp(ourButton) && !letGo)
        {
            if (Input.GetButton(ourButton)) { Grow(); }
        }
        else { letGo = true; detonate(); }

    }


    private void Grow()
    {
        if (currentRange <= SphereRange)
        {
            currentRange += .2f;

        }
        else
        {
            detonate();
        }
    }

    private void detonate()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Controller>().endAnim();
        foreach (var x in Enemies)
        {
            if (Vector3.Distance(transform.position, x.transform.position) <= currentRange)
            {
                Debug.Log(x.transform.name);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, currentRange);
    }
}
