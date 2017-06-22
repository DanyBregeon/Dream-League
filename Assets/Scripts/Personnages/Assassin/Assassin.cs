using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin : Personnage {

    // Use this for initialization
    void Start()
    {
        Pv = 750;
        Pa = 8;
        Pm = 6;
        PvActuel = 750;
        PaActuel = 10;
        PmActuel = 4;
        SpriteActuel = sprites[0];
        Sorts[0] = gameObject.GetComponent<AssassinSort1>();
        Sorts[1] = gameObject.GetComponent<AssassinSort2>();
        Sorts[2] = gameObject.GetComponent<AssassinSort3>();
        Sorts[3] = gameObject.GetComponent<AssassinSort4>();
        for (int i = 0; i < SortsCd.Length; i++)
        {
            SortsCd[i] = Sorts[i].Cd;
        }

        PosArrivee = PosDepart;
    }

    public void Passif(Personnage p)
    {
        int pvVictime = p.PvActuel;
        pvVictime -= (int)(Partie.personnageTour.Sorts[Partie.personnageTour.SortActif()].Dgt * (1 + ((float)Partie.personnageTour.DgtBonusActuel) / 100f - ((float)p.ResBonusActuel) / 100f));
        if (pvVictime <= 0 && !Partie.MemeEquipe(this, p))
        {
            if(PaActuel < Pa)
            {
                PaActuel = Pa;
            }
            PmActuel += 6;
            DgtBonusActuel += 100;

            for (int i = 0; i < Partie.personnageTour.SortsCd.Length; i++)
            {
                if (Partie.personnageTour.SortsCd[i] > 0)
                {
                    Partie.personnageTour.SortsCd[i] = 0;
                    Partie.personnageTour.affichageCdSort[i].text = "";
                    Partie.personnageTour.sortsIcone[i].GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
        }
    }
}
