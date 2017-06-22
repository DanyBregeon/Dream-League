using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuerrierSort2 : Sort {

    private void Awake()
    {
        Pa = 4;
        PoMin = 0;
        Po = 0;
        Dgt = 80;
        Cd = 1;
        SortDeZone = true;
    }

    public override void Effet(Case c)
    {
        if (Partie.personnageTour.GetType() == typeof(Guerrier))
        {
            foreach (Personnage p in Partie.personnages)
            {
                if (p.CasePersonnage == c)
                {
                    Dgt = 80;
                    Dgt = (int)(Dgt * (1 + (float)((Guerrier)Partie.personnageTour).Furie / 1000f));
                    if (Partie.teamA.Contains(p) && Partie.teamB.Contains(Partie.personnageTour) || Partie.teamB.Contains(p) && Partie.teamA.Contains(Partie.personnageTour))
                    {
                        if (((Guerrier)Partie.personnageTour).Furie + Dgt <= ((Guerrier)Partie.personnageTour).FurieMax)
                        {
                            ((Guerrier)Partie.personnageTour).Furie += Dgt;
                        }
                        else
                        {
                            ((Guerrier)Partie.personnageTour).Furie = ((Guerrier)Partie.personnageTour).FurieMax;
                        }
                    }
                }
            }

        }
    }

    public override bool ZoneEffet(Case cible, Case c)
    {
        int distance = Math.Abs(cible.X - c.X) + Math.Abs(cible.Y - c.Y);

        if (((Guerrier)Partie.personnageTour).Furie >= 250)
        {
            if (distance <= 3 && (cible.X != c.X || cible.Y != c.Y))
            {
                return true;
            }
        }
        else
        {
            if (distance <= 2 && (cible.X != c.X || cible.Y != c.Y))
            {
                return true;
            }
        }
        return false;
    }
}
