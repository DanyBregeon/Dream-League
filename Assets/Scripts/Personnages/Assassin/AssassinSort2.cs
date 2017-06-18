using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinSort2 : Sort {

    private void Awake()
    {
        Pa = 2;
        PoMin = 1;
        Po = 1;
        Dgt = 40;
        Cd = 0;
    }

    public override void Effet(Case c)
    {
        Dgt = 40;
        int min = 10000;
        if (Partie.teamA.Contains(Partie.personnageTour))
        {
            foreach (Personnage p in Partie.teamB)
            {
                if(p.PvActuel < min)
                {
                    min = p.PvActuel;
                }
            }
        }
        else
        {
            foreach (Personnage p in Partie.teamA)
            {
                if (p.PvActuel < min)
                {
                    min = p.PvActuel;
                }
            }
        }
        foreach (Personnage p in Partie.personnages)
        {
            if (p.CasePersonnage == c)
            {
                if(p.PvActuel <= min)
                {
                    Dgt += 10;
                }
                ((Assassin)Partie.personnageTour).Passif(p);
            }
        }
    }
}
