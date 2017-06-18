using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageSort2 : Sort {

    private void Awake()
    {
        Pa = 2;
        PoMin = 0;
        Po = 0;
        Dgt = 0;
        Cd = 2;
    }

    public override void Effet(Case c)
    {
        foreach (Personnage p in Partie.personnages)
        {
            if (p.CasePersonnage == c)
            {
                p.PaActuel += 6;
                p.DgtBonusActuel += 50;
                p.BuffPaDuree.Add(1);
                p.BuffPaValeur.Add(-4);
                p.BuffResDuree.Add(1);
                p.BuffResValeur.Add(-50);
            }
        }
    }
}
