using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Case : MonoBehaviour {

    //private Entitee entitee;
    private int x, y;
    private int cout;
    private int heuristique;
    private bool traversable;
    private Case pere;
    private char etat;
    private GameObject go;
    private bool mur;

    public bool Traversable
    {
        get
        {
            return traversable;
        }

        set
        {
            traversable = value;
        }
    }

    public int X
    {
        get
        {
            return x;
        }

        set
        {
            x = value;
        }
    }

    public int Y
    {
        get
        {
            return y;
        }

        set
        {
            y = value;
        }
    }

    public Case Pere
    {
        get
        {
            return pere;
        }

        set
        {
            pere = value;
        }
    }

    public int Cout
    {
        get
        {
            return cout;
        }

        set
        {
            cout = value;
        }
    }

    public int Heuristique
    {
        get
        {
            return heuristique;
        }

        set
        {
            heuristique = value;
        }
    }

    public GameObject Go
    {
        get
        {
            return go;
        }

        set
        {
            go = value;
        }
    }

    public bool Mur
    {
        get
        {
            return mur;
        }

        set
        {
            mur = value;
        }
    }

    // Use this for initialization
    void Start () {
        Go = gameObject;
        string s = go.transform.parent.name.Substring(5, go.transform.parent.name.Length - 5);
            x = int.Parse(s) -1;
            y = 101 - x - (int) Mathf.Round(Go.transform.position.z * 100);
            Partie.plateau[x, y] = this;
        traversable = true;
        foreach(GameObject murr in GameObject.FindGameObjectsWithTag("Mur"))
        {
            if(Mathf.Round(murr.transform.position.x * 100) == Mathf.Round(go.transform.position.x * 100) && Mathf.Round(murr.transform.position.y * 100) == Mathf.Round(go.transform.position.y*100) + 18)
            {
                traversable = false;
                mur = true;
                break;
            }
        }
        if (!go.GetComponent<SpriteRenderer>().enabled)
        {
            traversable = false;
        }
        //test
        if (x == 6 && y == 12)
        {
            Partie.personnageTour.CasePersonnage = this;
            traversable = false;
        }
        if (x == 7 && y == 1)
        {
            // !!!
            GameObject.Find("PersonnageGuerrier(Clone)").GetComponent<Guerrier>().CasePersonnage = this;
            traversable = false;
        }
        if (x == 5 && y == 1)
        {
            // !!!
            GameObject.Find("PersonnageArbaletrier(Clone)").GetComponent<Arbaletrier>().CasePersonnage = this;
            traversable = false;
        }
        if (x == 8 && y == 12)
        {
            // !!!
            GameObject.Find("PersonnageAssassin(Clone)").GetComponent<Assassin>().CasePersonnage = this;
            traversable = false;
        }
    }

    void OnMouseEnter()
    {
        if (traversable && !Partie.personnageTour.EnDeplacement && !Partie.personnageTour.ZoneActive())
        {
            Case depart = Partie.personnageTour.CasePersonnage;
            Partie.chemin = Partie.CheminPlusCourt(depart, this);
        }
        
        foreach(Personnage p in Partie.personnages)
        {
            if(p.CasePersonnage == this)
            {
                p.textPv.gameObject.SetActive(true);
            }
        }

    }
    private void OnMouseOver()
    {
        if (Partie.personnageTour.ZoneActive())
        {
            if (gameObject.GetComponent<SpriteRenderer>().color == Color.blue)
            {
                if (Partie.personnageTour.Sorts[Partie.personnageTour.SortActif()].SortDeZone)
                {
                    for (int i = 0; i < Partie.plateau.GetLength(0); i++)
                    {
                        for (int j = 0; j < Partie.plateau.GetLength(1); j++)
                        {
                            if (Partie.personnageTour.Sorts[Partie.personnageTour.SortActif()].ZoneEffet(this, Partie.plateau[i, j]))
                            {
                                Partie.plateau[i, j].GetComponent<SpriteRenderer>().color = Color.red;
                            }
                        }
                    }
                }
                else
                {
                    gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                }
            }
        }
    }

    void OnMouseExit()
    {
        if (!Partie.personnageTour.ZoneActive())
        {
            for (int i = 0; i < Partie.plateau.GetLength(0); i++)
            {
                for (int j = 0; j < Partie.plateau.GetLength(1); j++)
                {
                    if (Partie.plateau[i, j].Go.GetComponent<SpriteRenderer>().color == Color.green)
                    {
                        Partie.plateau[i, j].Go.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                }
            }
        }
        if (Partie.personnageTour.ZoneActive())
        {
            if (Partie.personnageTour.Sorts[Partie.personnageTour.SortActif()].SortDeZone)
            {
                int sortActif = Partie.personnageTour.SortActif();
                Partie.personnageTour.Sorts[Partie.personnageTour.SortActif()].CleanZone();
                Partie.personnageTour.Sorts[sortActif].Activation();
            }
            else
            {
                if (gameObject.GetComponent<SpriteRenderer>().color == Color.red)
                {
                    gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                }
            }

        }
        foreach (Personnage p in Partie.personnages)
        {
            if (p.CasePersonnage == this)
            {
                p.textPv.gameObject.SetActive(false);
            }
        }

    }


    void OnMouseDown()
    {
        if (traversable && !Partie.personnageTour.EnDeplacement && !Partie.personnageTour.ZoneActive())
        {
            if (Partie.chemin != null)
            {
                String chemin = "";
                foreach (Case c in Partie.chemin)
                {
                    chemin += (c.gameObject.name + "/");
                }
                Partie.personnageTour.SeDeplacerVers(this.gameObject.name, chemin);
                /*Partie.personnageTour.CasePersonnage.Traversable = true;
                Partie.personnageTour.PosArrivee = Partie.personnageTour.PosDepart;
                Partie.personnageTour.EnDeplacement = true;
                Partie.personnageTour.PmActuel -= Partie.chemin.Count;
                Traversable = false;*/
            }
            for (int i = 0; i < Partie.plateau.GetLength(0); i++)
            {
                for (int j = 0; j < Partie.plateau.GetLength(1); j++)
                {
                    if (Partie.plateau[i, j].Go.GetComponent<SpriteRenderer>().color == Color.green)
                    {
                        Partie.plateau[i, j].Go.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                }
            }
        }
        if (Partie.personnageTour.ZoneActive())
        {
            if(gameObject.GetComponent<SpriteRenderer>().color == Color.red || gameObject.GetComponent<SpriteRenderer>().color == Color.blue)
            {
                if(Partie.personnageTour.PaActuel >= Partie.personnageTour.Sorts[Partie.personnageTour.SortActif()].Pa)
                {
                    Partie.personnageTour.PaActuel -= Partie.personnageTour.Sorts[Partie.personnageTour.SortActif()].Pa;

                    Partie.personnageTour.SortsCd[Partie.personnageTour.SortActif()] = Partie.personnageTour.Sorts[Partie.personnageTour.SortActif()].Cd;
                    if (Partie.personnageTour.SortsCd[Partie.personnageTour.SortActif()] > 0)
                    {
                        Partie.personnageTour.affichageCdSort[Partie.personnageTour.SortActif()].text = Partie.personnageTour.SortsCd[Partie.personnageTour.SortActif()].ToString();
                        Partie.personnageTour.sortsIcone[Partie.personnageTour.SortActif()].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.6f);
                    }
                    else
                    {
                        Partie.personnageTour.affichageCdSort[Partie.personnageTour.SortActif()].text = "";
                        Partie.personnageTour.sortsIcone[Partie.personnageTour.SortActif()].GetComponent<SpriteRenderer>().color = Color.white;
                    }

                    if (Partie.personnageTour.Sorts[Partie.personnageTour.SortActif()].SortDeZone)
                    {
                        for (int i = 0; i < Partie.plateau.GetLength(0); i++)
                        {
                            for (int j = 0; j < Partie.plateau.GetLength(1); j++)
                            {
                                if (Partie.personnageTour.Sorts[Partie.personnageTour.SortActif()].ZoneEffet(this, Partie.plateau[i, j]))
                                {
                                    foreach (Personnage p in Partie.personnages)
                                    {
                                        if (p.CasePersonnage == Partie.plateau[i, j])
                                        {
                                            Partie.personnageTour.Sorts[Partie.personnageTour.SortActif()].Effet(p.CasePersonnage);
                                            p.PvActuel -= (int)(Partie.personnageTour.Sorts[Partie.personnageTour.SortActif()].Dgt * (1 + ((float)Partie.personnageTour.DgtBonusActuel) / 100f - ((float)p.ResBonusActuel) / 100f));

                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Partie.personnageTour.Sorts[Partie.personnageTour.SortActif()].CibleNonPersonnage)
                        {
                            Partie.personnageTour.Sorts[Partie.personnageTour.SortActif()].Effet(this);
                        }
                        else
                        {
                            foreach (Personnage p in Partie.personnages)
                            {
                                if (p.CasePersonnage == this)
                                {
                                    Partie.personnageTour.Sorts[Partie.personnageTour.SortActif()].Effet(this);
                                    p.PvActuel -= (int)(Partie.personnageTour.Sorts[Partie.personnageTour.SortActif()].Dgt * (1 + ((float)Partie.personnageTour.DgtBonusActuel) / 100f - ((float)p.ResBonusActuel) / 100f));
                                }
                            }
                        }

                    }
                    for (int i = 0; i < Partie.personnageTour.Sorts.Length; i++)
                    {
                        if (Partie.personnageTour.PaActuel < Partie.personnageTour.Sorts[i].Pa)
                        {
                            Partie.personnageTour.sortsIcone[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.6f);
                        }
                    }
                    Partie.personnageTour.Sorts[Partie.personnageTour.SortActif()].CleanZone();

                }
            }
        }
        

    }

    // Update is called once per frame
    void Update () {

	}
}
