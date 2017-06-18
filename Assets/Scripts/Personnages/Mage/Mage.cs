using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Personnage {

	// Use this for initialization
	void Start () {
        Pv = 650;
        Pa = 10;
        Pm = 4;
        PvActuel = 650;
        PaActuel = 10;
        PmActuel = 4;
        SpriteActuel = sprites[0];
        Sorts[0] = gameObject.GetComponent<MageSort1>();
        Sorts[1] = gameObject.GetComponent<MageSort2>();
        Sorts[2] = gameObject.GetComponent<MageSort3>();
        Sorts[3] = gameObject.GetComponent<MageSort4>();

        for (int i = 0; i < SortsCd.Length; i++)
        {
            SortsCd[i] = Sorts[i].Cd;
        }

        PosArrivee = PosDepart;
    }
}
