using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinSort4 : Sort {

    private void Awake()
    {
        Pa = 3;
        PoMin = 1;
        Po = 4;
        Dgt = 50;
        Cd = 2;
        CiblePersonnageObligatoire = true;
    }

    public override void Effet(Case c)
    {
        foreach (Personnage p in Partie.personnages)
        {
            if (p.CasePersonnage == c)
            {
                int spriteIndex = p.sprites.IndexOf(p.SpriteActuel);
                Partie.personnageTour.CasePersonnage.Traversable = true;
                if (spriteIndex == 0)
                {
                    Partie.personnageTour.CasePersonnage = Partie.plateau[p.CasePersonnage.X, p.CasePersonnage.Y - 1];   
                }
                else if (spriteIndex == 1)
                {
                    Partie.personnageTour.CasePersonnage = Partie.plateau[p.CasePersonnage.X - 1, p.CasePersonnage.Y];
                }
                else if (spriteIndex == 2)
                {
                    Partie.personnageTour.CasePersonnage = Partie.plateau[p.CasePersonnage.X, p.CasePersonnage.Y + 1];
                }
                else if (spriteIndex == 3)
                {
                    Partie.personnageTour.CasePersonnage = Partie.plateau[p.CasePersonnage.X + 1, p.CasePersonnage.Y];
                }
                Vector3 arrivee = new Vector3(Partie.personnageTour.CasePersonnage.transform.position.x, Partie.personnageTour.CasePersonnage.transform.position.y + 0.25f, Partie.personnageTour.CasePersonnage.transform.position.z - 0.0075f);
                Partie.personnageTour.transform.position = Vector3.MoveTowards(Partie.personnageTour.transform.position, arrivee, 1);
                Partie.personnageTour.PosArrivee = arrivee;
                Partie.personnageTour.PosDepart = arrivee;
                Partie.personnageTour.CasePersonnage.Traversable = false;
                ((Assassin)Partie.personnageTour).Passif(p);
            }
        }
    }

    public override void Activation()
    {
        base.Activation();
        if (ZoneActive)
        {
            for (int i = 0; i < Partie.plateau.GetLength(0); i++)
            {
                for (int j = 0; j < Partie.plateau.GetLength(1); j++)
                {
                    foreach (Personnage p in Partie.personnages)
                    {
                        if (Partie.plateau[i, j] == p.CasePersonnage)
                        {
                            if(Partie.plateau[i, j].GetComponent<SpriteRenderer>().color == Color.blue)
                            {
                                int spriteIndex = p.sprites.IndexOf(p.SpriteActuel);
                                if (spriteIndex == 0)
                                {
                                    if (j < 1 || !Partie.plateau[i, j - 1].Traversable)
                                    {
                                        Partie.plateau[i, j].GetComponent<SpriteRenderer>().color = Color.grey;
                                    }
                                }
                                else if (spriteIndex == 1)
                                {
                                    if (i < 1 || !Partie.plateau[i - 1, j].Traversable)
                                    {
                                        Partie.plateau[i, j].GetComponent<SpriteRenderer>().color = Color.grey;
                                    }
                                }
                                else if (spriteIndex == 2)
                                {
                                    if (j + 1 >= Partie.plateau.GetLength(1) || !Partie.plateau[i, j + 1].Traversable)
                                    {
                                        Partie.plateau[i, j].GetComponent<SpriteRenderer>().color = Color.grey;
                                    }
                                }
                                else if (spriteIndex == 3)
                                {
                                    if (i + 1 >= Partie.plateau.GetLength(0) || !Partie.plateau[i + 1, j].Traversable)
                                    {
                                        Partie.plateau[i, j].GetComponent<SpriteRenderer>().color = Color.grey;
                                    }
                                }

                            }                           
                        }
                    }
                }
            }
        }
    }
}
