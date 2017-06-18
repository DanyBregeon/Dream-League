using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArbaletrierSort1 : Sort {

    private void Awake()
    {
        Pa = 4;
        PoMin = 1;
        Po = 5;
        Dgt = 60;
        Cd = 0;
    }

    public override void Effet(Case c)
    {
        Dgt = 60;
        if (((Arbaletrier)Partie.personnageTour).BoostProchaineAttaque)
        {
            Dgt = (int)(((float)Dgt) * 1.5f);
            ((Arbaletrier)Partie.personnageTour).BoostProchaineAttaque = false;
        }
        foreach (Personnage p in Partie.personnages)
        {
            if (p.CasePersonnage == c)
            {
                    if(((Arbaletrier)Partie.personnageTour).Stacks[Partie.personnages.IndexOf(p)] < 2)
                    {
                        ((Arbaletrier)Partie.personnageTour).Stacks[Partie.personnages.IndexOf(p)]++;
                    }
                    else
                    {
                        ((Arbaletrier)Partie.personnageTour).Stacks[Partie.personnages.IndexOf(p)] = 0;
                        Dgt = (int) (((float) Dgt) * 1.5f);
                        if(p.ResBonusActuel > 0)
                        {
                            p.PvActuel -= (int)(Dgt * (1 + ((float)Partie.personnageTour.DgtBonusActuel) / 100f));
                            Dgt = 0;
                        }
                        if(Partie.personnageTour.SortsCd[1] > 0)
                        {
                            Partie.personnageTour.SortsCd[1]--;
                            Partie.personnageTour.affichageCdSort[1].text = "";
                            Partie.personnageTour.sortsIcone[1].GetComponent<SpriteRenderer>().color = Color.white;
                        }
                    }
                    for(int i = 0; i < ((Arbaletrier)Partie.personnageTour).Stacks.Count; i++)
                    {
                        if(i != Partie.personnages.IndexOf(p))
                        {
                            ((Arbaletrier)Partie.personnageTour).Stacks[i] = 0;
                        }
                    }
                if (((Arbaletrier)Partie.personnageTour).Ultime)
                {
                    p.BuffPmDuree.Add(1);
                    p.BuffPmValeur.Add(-1);
                    Partie.personnageTour.PmActuel++;
                }
            }
        }
    }
}
