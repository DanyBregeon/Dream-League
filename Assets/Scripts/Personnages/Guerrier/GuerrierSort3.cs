using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuerrierSort3 : Sort
{

    private void Awake()
    {
        Pa = 5;
        PoMin = 1;
        Po = 7;
        Dgt = 30;
        Cd = 1;
        SortEnLigne = true;
        CiblePersonnageObligatoire = true;
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
                        p.ResBonusActuel -= 30;
                        //p.BuffResDuree.Add(1);
                        //p.BuffResValeur.Add(-30);
                    }
                    Dgt = 40;
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
                    List<Case> cheminCharge = new List<Case>();
                    Partie.chemin = new Stack<Case>();

                    Case caseSuivante = Partie.personnageTour.CasePersonnage;
                    while (caseSuivante.Traversable || caseSuivante == Partie.personnageTour.CasePersonnage)
                    {
                        if (Partie.personnageTour.CasePersonnage.X > c.X && Partie.personnageTour.CasePersonnage.Y == c.Y)
                        {
                            caseSuivante = Partie.plateau[caseSuivante.X - 1, caseSuivante.Y];                           
                        }
                        else if(Partie.personnageTour.CasePersonnage.X < c.X && Partie.personnageTour.CasePersonnage.Y == c.Y)
                        {
                            caseSuivante = Partie.plateau[caseSuivante.X + 1, caseSuivante.Y];
                        }
                        else if (Partie.personnageTour.CasePersonnage.X == c.X && Partie.personnageTour.CasePersonnage.Y > c.Y)
                        {
                            caseSuivante = Partie.plateau[caseSuivante.X, caseSuivante.Y - 1];
                        }
                        else
                        {
                            caseSuivante = Partie.plateau[caseSuivante.X, caseSuivante.Y + 1];
                        }
                        if (caseSuivante.Traversable)
                        {
                            cheminCharge.Add(caseSuivante);
                        }
                    }
                    for (int i = cheminCharge.Count - 1; i >= 0; i--)
                    {
                        Partie.chemin.Push(cheminCharge[i]);
                    }

                    if(cheminCharge.Count > 0)
                    {
                        Partie.personnageTour.CasePersonnage.Traversable = true;
                        Partie.personnageTour.PosArrivee = Partie.personnageTour.PosDepart;
                        Partie.personnageTour.EnDeplacement = true;
                        cheminCharge[cheminCharge.Count - 1].Traversable = false;
                    }

                }

            }
        }
    }
}
