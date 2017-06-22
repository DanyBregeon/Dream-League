using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArbaletrierSort2 : Sort {

    private void Awake()
    {
        Pa = 2;
        PoMin = 2;
        Po = 2;
        Dgt = 0;
        Cd = 1;
        SortEnLigne = true;
        CibleNonPersonnage = true;
    }

    public override void Effet(Case c)
    {
        ((Arbaletrier)Partie.personnageTour).BoostProchaineAttaque = true;
        if (((Arbaletrier)Partie.personnageTour).Ultime)
        {
            Partie.personnageTour.PaActuel += 2;
        }

        List<Case> cheminCharge = new List<Case>();
        Partie.personnageTour.CheminAParcourir = new Stack<Case>();

        Case caseSuivante = Partie.personnageTour.CasePersonnage;
        while ((caseSuivante.Traversable || caseSuivante == Partie.personnageTour.CasePersonnage) && caseSuivante != c)
        {
            if (Partie.personnageTour.CasePersonnage.X > c.X && Partie.personnageTour.CasePersonnage.Y == c.Y)
            {
                caseSuivante = Partie.plateau[caseSuivante.X - 1, caseSuivante.Y];
            }
            else if (Partie.personnageTour.CasePersonnage.X < c.X && Partie.personnageTour.CasePersonnage.Y == c.Y)
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
            Partie.personnageTour.CheminAParcourir.Push(cheminCharge[i]);
        }

        if (cheminCharge.Count > 0)
        {
            Partie.personnageTour.CasePersonnage.Traversable = true;
            Partie.personnageTour.PosArrivee = Partie.personnageTour.PosDepart;
            Partie.personnageTour.EnDeplacement = true;
            cheminCharge[cheminCharge.Count - 1].Traversable = false;
        }
    }
}
