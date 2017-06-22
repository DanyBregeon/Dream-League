using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageSort3 : Sort {

    private void Awake()
    {
        Pa = 4;
        PoMin = 0;
        Po = 4;
        Dgt = 25;
        Cd = 2;
        SortDeZone = true;
    }

    public override void Effet(Case c)
    {
        foreach (Personnage p in Partie.personnages)
        {
            if (p.CasePersonnage == c)
            {
                p.BuffPmDuree.Add(1);
                p.BuffPmValeur.Add(-2);
                p.AfficherText(-2, Constantes.vertTextPm, c);
            }
        }
    }

    public override bool ZoneEffet(Case cible, Case c)
    {
        int distance = Math.Abs(cible.X - c.X) + Math.Abs(cible.Y - c.Y);

        if(distance <= 2)
        {
            return true;
        }
        /*if (c.X <= cible.X + 2 && c.X >= cible.X - 2 && c.Y <= cible.Y + 2 && c.Y >= cible.Y - 2)
        {
            return true;
        }*/
        return false;
    }
}
