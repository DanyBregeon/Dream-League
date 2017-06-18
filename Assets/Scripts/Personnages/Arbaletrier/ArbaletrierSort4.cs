using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArbaletrierSort4 : Sort
{
    private void Awake()
    {
        Pa = 2;
        PoMin = 0;
        Po = 0;
        Dgt = 0;
        Cd = 4;
    }

    public override void Effet(Case c)
    {
        foreach (Personnage p in Partie.personnages)
        {
            if (p.CasePersonnage == c)
            {
                p.DgtBonusActuel += 20;
                p.BuffDgtDuree.Add(1);
                p.BuffDgtValeur.Add(20);
                ((Arbaletrier)Partie.personnageTour).Ultime = true;
                ((Arbaletrier)Partie.personnageTour).UltimeDuree = 2;
            }
        }
    }
}
