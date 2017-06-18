using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArbaletrierSort3 : Sort {

    private void Awake()
    {
        Pa = 6;
        PoMin = 1;
        Po = 5;
        Dgt = 90;
        Cd = 2;
        SortEnLigne = true;
    }

    public override void Effet(Case c)
    {
        Dgt = 90;
        if (((Arbaletrier)Partie.personnageTour).BoostProchaineAttaque)
        {
            Dgt = (int)(((float)Dgt) * 1.5f);
            ((Arbaletrier)Partie.personnageTour).BoostProchaineAttaque = false;
        }
        foreach (Personnage p in Partie.personnages)
        {
            if (p.CasePersonnage == c)
            {
                if (((Arbaletrier)Partie.personnageTour).Stacks[Partie.personnages.IndexOf(p)] < 2)
                {
                    ((Arbaletrier)Partie.personnageTour).Stacks[Partie.personnages.IndexOf(p)]++;
                }
                else
                {
                    ((Arbaletrier)Partie.personnageTour).Stacks[Partie.personnages.IndexOf(p)] = 0;
                    Dgt = (int)(((float)Dgt) * 1.5f);
                    if (p.ResBonusActuel > 0)
                    {
                        p.PvActuel -= (int)(Dgt * (1 + ((float)Partie.personnageTour.DgtBonusActuel) / 100f));
                        Dgt = 0;
                    }
                    if (Partie.personnageTour.SortsCd[1] > 0)
                    {
                        Partie.personnageTour.SortsCd[1]--;
                        Partie.personnageTour.affichageCdSort[1].text = "";
                        Partie.personnageTour.sortsIcone[1].GetComponent<SpriteRenderer>().color = Color.white;
                    }
                }
                for (int i = 0; i < ((Arbaletrier)Partie.personnageTour).Stacks.Count; i++)
                {
                    if (i != Partie.personnages.IndexOf(p))
                    {
                        ((Arbaletrier)Partie.personnageTour).Stacks[i] = 0;
                    }
                }

                List<Case> cheminCharge = new List<Case>();
                Partie.chemin = new Stack<Case>();

                Case caseSuivante = c;
                while ((caseSuivante.Traversable || caseSuivante == c) && !(caseSuivante.X == c.X + 2 || caseSuivante.Y == c.Y + 2 || caseSuivante.X == c.X - 2 || caseSuivante.Y == c.Y - 2))
                {
                    if (Partie.personnageTour.CasePersonnage.X > c.X && Partie.personnageTour.CasePersonnage.Y == c.Y)
                    {
                        if(caseSuivante.X > 0)
                        {
                            caseSuivante = Partie.plateau[caseSuivante.X - 1, caseSuivante.Y];
                        }
                        else
                        {
                            break;
                        }

                    }
                    else if (Partie.personnageTour.CasePersonnage.X < c.X && Partie.personnageTour.CasePersonnage.Y == c.Y)
                    {
                        if (caseSuivante.X < Partie.plateau.GetLength(0) - 1)
                        {
                            caseSuivante = Partie.plateau[caseSuivante.X + 1, caseSuivante.Y];
                        }
                        else
                        {
                            PousserContreMur(p);
                            break;
                        }
                    }
                    else if (Partie.personnageTour.CasePersonnage.X == c.X && Partie.personnageTour.CasePersonnage.Y > c.Y)
                    {
                        if (caseSuivante.Y > 0)
                        {
                            caseSuivante = Partie.plateau[caseSuivante.X, caseSuivante.Y - 1];
                        }
                        else
                        {
                            PousserContreMur(p);
                            break;
                        }
                    }
                    else
                    {
                        if (caseSuivante.Y < Partie.plateau.GetLength(1) - 1)
                        {
                            caseSuivante = Partie.plateau[caseSuivante.X, caseSuivante.Y + 1];
                        }
                        else
                        {
                            PousserContreMur(p);
                            break;
                        }
                    }
                    if (caseSuivante.Traversable)
                    {
                        cheminCharge.Add(caseSuivante);
                    }
                    else
                    {
                        PousserContreMur(p);
                    }
                }
                for (int i = cheminCharge.Count - 1; i >= 0; i--)
                {
                    Partie.chemin.Push(cheminCharge[i]);
                }

                if (cheminCharge.Count > 0)
                {
                    c.Traversable = true;
                    p.PosArrivee = p.PosDepart;
                    p.EnDeplacement = true;
                    cheminCharge[cheminCharge.Count - 1].Traversable = false;
                }
            }
        }
    }

    private void PousserContreMur(Personnage p)
    {
        Dgt = (int)(((float)Dgt) * 1.5f);
        if (((Arbaletrier)Partie.personnageTour).Ultime)
        {
            p.BuffPaDuree.Add(1);
            p.BuffPaValeur.Add(-3);
            p.BuffPmDuree.Add(1);
            p.BuffPmValeur.Add(-3);
        }
        if (Partie.personnageTour.SortsCd[1] > 0)
        {
            Partie.personnageTour.SortsCd[1]--;
            Partie.personnageTour.affichageCdSort[1].text = "";
            Partie.personnageTour.sortsIcone[1].GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
