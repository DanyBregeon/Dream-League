using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleChoixPersonnage : MonoBehaviour {

    [SerializeField]
    private GameObject personnage;

    public GameObject Personnage
    {
        get
        {
            return personnage;
        }

        set
        {
            personnage = value;
        }
    }
}
