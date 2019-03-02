using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellHandler : MonoBehaviour
{

    public GameObject s1;
    public GameObject s2;
    public GameObject s3;
    public GameObject s4;
    public GameObject s5;
    public GameObject s6;

    List<string> options;
    Dictionary<string, GameObject> currentSpells;
    [SerializeField] Dropdown slotOne;
    [SerializeField] GameObject one;
    [SerializeField] GameObject two;
    [SerializeField] GameObject three;

    // Use this for initialization
    void Start()
    {
        /*
        currentSpells = new Dictionary<string, GameObject>();
        currentSpells.Add(one.name, one);
        currentSpells.Add(two.name, two);
        currentSpells.Add(three.name, three);
        slotOne.AddOptions(options);*/

    }

    // Update is called once per frame
    void Update()
    {
        //slotOne.onValueChanged.AddListener(delegate { DropdownValueChanged(slotOne); });
    }

    void DropdownValueChanged(Dropdown change)
    {
        slotOne.captionText.text = "" + options[change.value];
    }

    public void addSpell()
    {
        options.Add("");
    }
}
