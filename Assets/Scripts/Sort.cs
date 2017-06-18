using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sort : MonoBehaviour {

    private int poMin;
    private int po;
    private int pa;
    private int dgt;
    private bool cac;
    private int cd;
    private bool zoneActive;
    private bool sortDeZone;
    private bool sortEnLigne;
    private bool ciblePersonnageObligatoire;
    private bool cibleNonPersonnage;
    private int dgtBoost;

    public bool ZoneActive
    {
        get
        {
            return zoneActive;
        }

        set
        {
            zoneActive = value;
        }
    }

    public int Pa
    {
        get
        {
            return pa;
        }

        set
        {
            pa = value;
        }
    }

    public int Dgt
    {
        get
        {
            return dgt;
        }

        set
        {
            dgt = value;
        }
    }

    public int Cd
    {
        get
        {
            return cd;
        }

        set
        {
            cd = value;
        }
    }

    public bool SortDeZone
    {
        get
        {
            return sortDeZone;
        }

        set
        {
            sortDeZone = value;
        }
    }

    public bool CibleNonPersonnage
    {
        get
        {
            return cibleNonPersonnage;
        }

        set
        {
            cibleNonPersonnage = value;
        }
    }

    public int PoMin
    {
        get
        {
            return poMin;
        }

        set
        {
            poMin = value;
        }
    }

    public int Po
    {
        get
        {
            return po;
        }

        set
        {
            po = value;
        }
    }

    public bool SortEnLigne
    {
        get
        {
            return sortEnLigne;
        }

        set
        {
            sortEnLigne = value;
        }
    }

    public bool CiblePersonnageObligatoire
    {
        get
        {
            return ciblePersonnageObligatoire;
        }

        set
        {
            ciblePersonnageObligatoire = value;
        }
    }

    public virtual void Effet(Case c)
    {

    }

    public virtual bool ZoneEffet(Case cible, Case c)
    {
        return false;
    }

    public void CleanZone()
    {
        ZoneActive = false;
        for (int i = 0; i < Partie.plateau.GetLength(0); i++)
        {
            for (int j = 0; j < Partie.plateau.GetLength(1); j++)
            {
                if (Partie.plateau[i, j].GetComponent<SpriteRenderer>().color != Color.white)
                {
                    Partie.plateau[i, j].GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
    public virtual void Activation()
    {
        if (ZoneActive)
        {           
            CleanZone();
        }
        else
        {
            CleanZone();
            ZoneActive = true;
            for (int i = 0; i < Partie.plateau.GetLength(0); i++)
            {
                for (int j = 0; j < Partie.plateau.GetLength(1); j++)
                {
                    int distance  = Math.Abs(Partie.personnageTour.CasePersonnage.X - i) + Math.Abs(Partie.personnageTour.CasePersonnage.Y - j);
                    if (distance <= Po && distance >= PoMin && (!SortEnLigne || (SortEnLigne && (Partie.personnageTour.CasePersonnage.X == i || Partie.personnageTour.CasePersonnage.Y == j))))
                    {
                        //instance = (GameObject)Instantiate(Zone, new Vector3(Partie.plateau[i, j].gameObject.transform.position.x, Partie.plateau[i, j].gameObject.transform.position.y, (float)-0.2), Partie.plateau[i, j].gameObject.transform.rotation);
                        Partie.plateau[i, j].GetComponent<SpriteRenderer>().color = Color.blue;
                        if (CiblePersonnageObligatoire)
                        {
                            foreach (Personnage p in Partie.personnages)
                            {
                                Partie.plateau[i, j].GetComponent<SpriteRenderer>().color = Color.grey;
                                if (Partie.plateau[i, j] == p.CasePersonnage)
                                {
                                    Partie.plateau[i, j].GetComponent<SpriteRenderer>().color = Color.blue;
                                    break;
                                }
                            }
                        }
                        Vector2 depart = new Vector2(Partie.personnageTour.CasePersonnage.Go.transform.position.x, Partie.personnageTour.CasePersonnage.Go.transform.position.y + 0.1f);
                        Vector2 arrivee = new Vector2(Partie.plateau[i, j].Go.transform.position.x, Partie.plateau[i, j].Go.transform.position.y + 0.1f);

                        RaycastHit2D[] hits = Physics2D.RaycastAll(depart, new Vector2(arrivee.x - depart.x, arrivee.y - depart.y), Mathf.Sqrt(Mathf.Pow(depart.x - arrivee.x, 2) + Mathf.Pow(depart.y - arrivee.y, 2)));

                        //Debug.DrawRay(new Vector3(depart.go.transform.position.x, depart.go.transform.position.y, (float)0), new Vector3(plateau[i, j].go.transform.position.x - depart.go.transform.position.x, plateau[i, j].go.transform.position.y - depart.go.transform.position.y, 0), orange);
                        Partie.personnageTour.CasePersonnage.Traversable = true;
                        foreach (Personnage p in Partie.personnages)
                        {
                            if (Partie.plateau[i, j] == p.CasePersonnage)
                            {
                                p.CasePersonnage.Traversable = true;
                            }
                        }
                        foreach (RaycastHit2D hit in hits)
                        {
                            if (/*hit.collider.gameObject.GetComponent<Case>().Mur*/!hit.collider.gameObject.GetComponent<Case>().Traversable && hit.collider.gameObject.GetComponent<SpriteRenderer>().isVisible)
                            {
                                Partie.plateau[i, j].GetComponent<SpriteRenderer>().color = Color.gray;
                            }
                        }
                        foreach (Personnage p in Partie.personnages)
                        {
                            p.CasePersonnage.Traversable = false;
                        }

                    }
                }
            }
        }
    }
}
