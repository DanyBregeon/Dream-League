using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinSort3 : Sort {

    private void Awake()
    {
        Pa = 4;
        PoMin = 1;
        Po = 1;
        Dgt = 80;
        Cd = 1;
    }

    public override void Effet(Case c)
    {
        Dgt = 80;
        foreach (Personnage p in Partie.personnages)
        {
            if (p.CasePersonnage == c)
            {
                if (((float)p.PvActuel / (float)p.Pv) < 0.5f)
                {
                    Dgt += 20;
                }
                if (((float)p.PvActuel / (float)p.Pv) < 0.25f)
                {
                    Dgt += 20;
                }
                ((Assassin)Partie.personnageTour).Passif(p);
            }
        }
    }
}
