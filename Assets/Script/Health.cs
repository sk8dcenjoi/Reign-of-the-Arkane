using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{

    public float maxHealth;
    public float currentHealth;
    private bool wait;


    [SerializeField] Image imgHP;

    // Use this for initialization
    void Start () {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.tag == "Player")
        {
            imgHP.fillAmount = currentHealth / maxHealth;
        }

        if(transform.tag == "Enemy")
        {
            imgHP.fillAmount = currentHealth / maxHealth;
        }

    }

    public void takeDamage(float num)
    {


        currentHealth -= num;

        if(currentHealth <= 0)

        if (!wait) { currentHealth -= num; }

        //Debug.Log(currentHealth);
        if (currentHealth <= 0)

        {
            //Debug.Log("He a Dead Boi Now");
        }
    }


    public void getHealth(float num)
    {
        currentHealth += num;
        
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }

    }




    public void giveHealth(float giveAmount)
    {
        if(currentHealth < 100) {currentHealth += giveAmount; }
        else { currentHealth = 100; }
    }



    public void Iframess()
    {
        StartCoroutine("Invincible");
    }

    private IEnumerator Invincible()
    {
        wait = true;
        yield return new WaitForSeconds(1f);
        wait = false;
    }

}
