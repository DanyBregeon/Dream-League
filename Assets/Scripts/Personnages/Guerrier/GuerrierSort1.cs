using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuerrierSort1 : Sort {

    private void Awake()
    {
        Pa = 3;
        PoMin = 1;
        Po = 1;
        Dgt = 60;
        Cd = 0;
    }

    public override void Effet(Case c)
    {
        if (Partie.personnageTour.GetType() == typeof(Guerrier))
        {

            foreach (Personnage p in Partie.personnages)
            {
                if (p.CasePersonnage == c)
                {
                    if (((Guerrier)Partie.personnageTour).Furie >= 250)
                    {
                        p.BuffPaDuree.Add(1);
                        p.BuffPaValeur.Add(-1);
                        p.AfficherText(-1, Constantes.bleutextPa, c);
                    }
                    Dgt = 60;
                    Dgt = (int)(Dgt * (1 + (float)((Guerrier)Partie.personnageTour).Furie / 1000f));
                    if (Partie.teamA.Contains(p) && Partie.teamB.Contains(Partie.personnageTour) || Partie.teamB.Contains(p) && Partie.teamA.Contains(Partie.personnageTour))
                    {
                        if(((Guerrier)Partie.personnageTour).Furie + Dgt <= ((Guerrier)Partie.personnageTour).FurieMax){
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
}
