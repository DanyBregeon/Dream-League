using Priority_Queue;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Partie : MonoBehaviour {

    public static Case[,] plateau;
    public static Personnage personnageTour;
    public static Stack<Case> chemin;
    public static int tourActuel;
    public static List<Personnage> teamA;
    public static List<Personnage> teamB;
    public static List<Personnage> personnages;
    public static Text affichageTextPv;
    public static Image barreDeVie;

    // Use this for initialization
    void Awake () {
        plateau = new Case[Constantes.taillePlateauX, Constantes.taillePlateauY];
        // !!!
        personnageTour = GameObject.Find("PersonnageMage(Clone)").GetComponent<Mage>();
        affichageTextPv = GameObject.Find("Pv").GetComponent<Text>();
        barreDeVie = GameObject.Find("BarreDeVie").GetComponent<Image>();
        tourActuel = 1;

        teamA = new List<Personnage>();
        teamB = new List<Personnage>();
        personnages = new List<Personnage>();
        teamA.Add(personnageTour);
        personnages.Add(personnageTour);
        // !!!
        teamB.Add(GameObject.Find("PersonnageGuerrier(Clone)").GetComponent<Guerrier>());
        teamA.Add(GameObject.Find("PersonnageAssassin(Clone)").GetComponent<Assassin>());
        teamB.Add(GameObject.Find("PersonnageArbaletrier(Clone)").GetComponent<Arbaletrier>());

        personnages.Add(teamB[0]);
        personnages.Add(teamA[1]);
        personnages.Add(teamB[1]);

        for(int i = personnages.Count - 1; i >= 0; i--)
        {
            personnages[i].textPv = GameObject.Find("PV" + (i + 1)).GetComponent<Text>();
            personnages[i].textPv.enabled = false;
            personnages[i].SpriteTimeLine = Instantiate(personnages[i].sprites[1], new Vector3(8.25f - (personnages.Count - 1 - i), -4.25f, 0), Quaternion.identity);
            personnages[i].SpriteTimeLine.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        }

    }
    void Start()
    {
        
        personnageTour.DebutTour();
    }
    public static bool MemeEquipe(Personnage p1, Personnage p2)
    {
        if((teamA.Contains(p1) && teamA.Contains(p2)) || (teamB.Contains(p1) && teamB.Contains(p2)))
        {
            return true;
        }
        return false;
    }

    public static bool PositionValide(int x, int y)
    {
        if (x >= 0 && x < plateau.GetLength(0) && y >= 0 && y < plateau.GetLength(1))
        {
            if (plateau[x,y].Traversable)
            {
                return true;
            }
        }
        return false;
    }

    public static List<Case> VoisinsNoeud(Case u)
    {
        List<Case> voisins = new List<Case>();
        if (PositionValide(u.X - 1, u.Y))
        {
            voisins.Add(plateau[u.X - 1,u.Y]);
        }
        if (PositionValide(u.X + 1, u.Y))
        {
            voisins.Add(plateau[u.X + 1,u.Y]);
        }
        if (PositionValide(u.X, u.Y - 1))
        {
            voisins.Add(plateau[u.X,u.Y - 1]);
        }
        if (PositionValide(u.X, u.Y + 1))
        {
            voisins.Add(plateau[u.X,u.Y + 1]);
        }
        return voisins;
    }

    private static Stack<Case> ReconstituerChemin(Case u)
    {
        Stack<Case> route = new Stack<Case>();
        while (u.Pere != null)
        {
            route.Push(u);
            u.Go.GetComponent<SpriteRenderer>().color = Color.green;
            u = u.Pere;
        }
        return route;
    }

    public static Stack<Case> CheminPlusCourt(Case depart, Case objectif)
    {
        for (int i = 0; i < plateau.GetLength(0); i++)
        {
            for (int j = 0; j < plateau.GetLength(1); j++)
            {
                plateau[i, j].Cout = 0;
                plateau[i, j].Heuristique = 0;
                plateau[i, j].Pere = null;
            }
        }
        List<Case> closedList = new List<Case>();
        SimplePriorityQueue<Case> openList = new SimplePriorityQueue<Case>();
        openList.Enqueue(depart, 0);
        while (openList.Count > 0)
        {
            Case u = openList.Dequeue();
            if (u.X == objectif.X && u.Y == objectif.Y)
            {
                return ReconstituerChemin(u);
            }
            List<Case> voisins = VoisinsNoeud(u);
            foreach (Case v in voisins)
            {
                if (!((openList.Contains(v) && v.Cout < u.Cout + 1) || closedList.Contains(v)) && u.Cout + 1 <= personnageTour.PmActuel)
                {
                    v.Pere = u;
                    v.Cout = u.Cout + 1;
                    v.Heuristique = v.Cout + Math.Abs(objectif.X - v.X) + Math.Abs(objectif.Y - v.Y);
                    openList.Enqueue(v, v.Heuristique);
                }
            }
            closedList.Add(u);
        }
        return null;
    }

    // Update is called once per frame
    void Update () {
        affichageTextPv.text = personnageTour.PvActuel.ToString() + "/" + personnageTour.Pv.ToString();
        barreDeVie.fillAmount = (float) personnageTour.PvActuel / (float) personnageTour.Pv;

        if (Input.GetKeyDown("space"))
        {
            foreach(GameObject i in personnageTour.sortsIcone)
            {
                i.SetActive(false);
            }
            if (personnageTour.ZoneActive())
            {
                personnageTour.Sorts[personnageTour.SortActif()].CleanZone();
            }
            if (personnages.IndexOf(personnageTour) < personnages.Count - 1)
            {
                personnageTour = personnages[personnages.IndexOf(personnageTour) + 1];
            }
            else
            {
                tourActuel++;
                personnageTour = personnages[0];
            }
            personnageTour.DebutTour();
        }

        if (Input.GetKeyDown("a"))
        {
            if (personnageTour.PaActuel >= personnageTour.Sorts[0].Pa && personnageTour.SortsCd[0] == 0)
            {
                personnageTour.DesactiveAutreZone(0);
                personnageTour.Sorts[0].Activation();
            }

        }
        if (Input.GetKeyDown("z"))
        {
            if (personnageTour.PaActuel >= personnageTour.Sorts[1].Pa && personnageTour.SortsCd[1] == 0)
            {
                personnageTour.DesactiveAutreZone(1);
                personnageTour.Sorts[1].Activation();
            }

        }
        if (Input.GetKeyDown("e"))
        {
            if (personnageTour.PaActuel >= personnageTour.Sorts[2].Pa && personnageTour.SortsCd[2] == 0)
            {
                personnageTour.DesactiveAutreZone(2);
                personnageTour.Sorts[2].Activation();
            }

        }
        if (Input.GetKeyDown("r"))
        {
            if (personnageTour.PaActuel >= personnageTour.Sorts[3].Pa && personnageTour.SortsCd[3] == 0)
            {
                personnageTour.DesactiveAutreZone(3);
                personnageTour.Sorts[3].Activation();
            }

        }
    }
}
