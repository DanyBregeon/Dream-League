using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinSort1 : Sort {

    private void Awake()
    {
        Pa = 3;
        PoMin = 1;
        Po = 4;
        Dgt = 50;
        Cd = 2;
    }

    public override void Effet(Case c)
    {
        foreach (Personnage p in Partie.personnages)
        {
            if (p.CasePersonnage == c)
            {
                p.ResBonusActuel -= 20;
                p.BuffResDuree.Add(1);
                p.BuffResValeur.Add(-20);
                ((Assassin)Partie.personnageTour).Passif(p);
            }
        }
    }
}
