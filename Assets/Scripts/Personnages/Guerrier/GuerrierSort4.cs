using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuerrierSort4 : Sort {

    private void Awake()
    {
        Pa = 7;
        PoMin = 1;
        Po = 1;
        Dgt = 160;
        Cd = 3;
    }

    public override void Effet(Case c)
    {
        if (Partie.personnageTour.GetType() == typeof(Guerrier))
        {
            Dgt = 160;
            foreach (Personnage p in Partie.personnages)
            {
                if (p.CasePersonnage == c)
                {
                    if (((Guerrier)Partie.personnageTour).Furie == ((Guerrier)Partie.personnageTour).FurieMax)
                    {
                        Dgt = 240;
                        Dgt = (int)(Dgt * (1 + (float)((Guerrier)Partie.personnageTour).Furie / 1000f));
                        ((Guerrier)Partie.personnageTour).Furie = 0;
                    }
                    else
                    {
                        Dgt = 160;
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
    }
}
