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
    public static int joueur;

    private PhotonView view;
    private Core core;
    private Text textTourActuel;

    public PhotonView View
    {
        get
        {
            return view;
        }

        set
        {
            view = value;
        }
    }

    // Use this for initialization
    void Awake () {
        plateau = new Case[Constantes.taillePlateauX, Constantes.taillePlateauY];
        // !!!
        //personnageTour = GameObject.Find("PersonnageMage(Clone)").GetComponent<Personnage>();

        affichageTextPv = GameObject.Find("Pv").GetComponent<Text>();
        barreDeVie = GameObject.Find("BarreDeVie").GetComponent<Image>();
        tourActuel = 1;
        textTourActuel = GameObject.Find("TextTour").GetComponent<Text>();
        textTourActuel.text = "" + 1;

        teamA = new List<Personnage>();
        teamB = new List<Personnage>();
        personnages = new List<Personnage>();

        // !!!
        /* if (PhotonNetwork.isMasterClient)
         {*/
        /*print(GameObject.FindGameObjectsWithTag("Personnage").Length);
            personnages.Add(GameObject.Find(SceneConstant.teamJoueur[0] + "(Clone)").GetComponent<Personnage>());
            personnages.Add(GameObject.Find(SceneConstant.teamJoueur[1] + "(Clone)").GetComponent<Personnage>());
            personnages.Add(GameObject.Find(SceneConstant.teamJoueur[2] + "(Clone)").GetComponent<Personnage>());
            personnages.Add(GameObject.Find(SceneConstant.teamJoueur[3] + "(Clone)").GetComponent<Personnage>());*/
       /* }
        else
        {
            print(SceneConstant.teamJoueur[2]);
            personnages.Add(GameObject.Find(SceneConstant.teamJoueur[2] + "(Clone)").GetComponent<Personnage>());
            personnages.Add(GameObject.Find(SceneConstant.teamJoueur[3] + "(Clone)").GetComponent<Personnage>());
            personnages.Add(GameObject.Find(SceneConstant.teamJoueur[0] + "(Clone)").GetComponent<Personnage>());
            personnages.Add(GameObject.Find(SceneConstant.teamJoueur[1] + "(Clone)").GetComponent<Personnage>());
        }*/


        /*teamA.Add(personnages[0]);
        teamA.Add(personnages[1]);
        teamB.Add(personnages[2]);
        teamB.Add(personnages[3]);
        personnageTour = personnages[0];*/
        /*teamB.Add(GameObject.Find("PersonnageGuerrier(Clone)").GetComponent<Guerrier>());
        teamA.Add(GameObject.Find("PersonnageAssassin(Clone)").GetComponent<Assassin>());
        teamB.Add(GameObject.Find("PersonnageArbaletrier(Clone)").GetComponent<Arbaletrier>());*/

        /*personnages.Add(teamA[0]);
        personnages.Add(teamB[0]);
        personnages.Add(teamA[1]);
        personnages.Add(teamB[1]);*/

        /*for (int i = personnages.Count - 1; i >= 0; i--)
        {
            personnages[i].textPv = GameObject.Find("PV" + (i + 1)).GetComponent<Text>();
            personnages[i].textPv.gameObject.SetActive(false);
            personnages[i].SpriteTimeLine = Instantiate(personnages[i].sprites[1], new Vector3(8.25f - (personnages.Count - 1 - i), -4.25f, 0), Quaternion.identity);
            personnages[i].SpriteTimeLine.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        }*/

    }
    void Start()
    {
        View = GetComponent<PhotonView>();
        core = GameObject.Find("Script").GetComponent<Core>();
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

    [PunRPC]
    public void FinTour()
    {
            for (int i = 0; i < plateau.GetLength(0); i++)
            {
                for (int j = 0; j < plateau.GetLength(1); j++)
                {
                    if (plateau[i, j].Go.GetComponent<SpriteRenderer>().color == Color.green)
                    {
                        plateau[i, j].Go.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                }
            }
            if (chemin != null)
            {
                chemin = null;
            }
            if ((personnageTour is Guerrier))
            {
                ((Guerrier)personnageTour).textFurie.text = "";
            }

            foreach (GameObject i in personnageTour.sortsIcone)
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
                textTourActuel.text = "" + tourActuel;
                personnageTour = personnages[0];
            }

            personnageTour.DebutTour();
            core.TurnManager.BeginTurn();        
    }

    [PunRPC]
    public void ActiveZone(int i)
    {
        personnageTour.DesactiveAutreZone(i);
        if (!personnageTour.Sorts[i].ZoneActive)
        {
            personnageTour.Sorts[i].ZoneActive = true;
        }
        else{
            personnageTour.Sorts[i].ZoneActive = false;
        }
    }

    // Update is called once per frame
    void Update () {
        affichageTextPv.text = personnageTour.PvActuel.ToString() + "/" + personnageTour.Pv.ToString();
        barreDeVie.fillAmount = (float) personnageTour.PvActuel / (float) personnageTour.Pv;

        if((joueur == 1 && teamA.Contains(personnageTour)) || (joueur == 2 && teamB.Contains(personnageTour)))
        {
            if (Input.GetKeyDown("space"))
            {
                View.RPC("FinTour", PhotonTargets.All);


                /*foreach(GameObject i in personnageTour.sortsIcone)
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
                personnageTour.DebutTour();*/
            }

            if (Input.GetKeyDown("a"))
            {
                if (personnageTour.PaActuel >= personnageTour.Sorts[0].Pa && personnageTour.SortsCd[0] == 0)
                {
                    personnageTour.DesactiveAutreZone(0);
                    personnageTour.Sorts[0].Activation();
                    View.RPC("ActiveZone", PhotonTargets.Others, 0);
                }

            }
            if (Input.GetKeyDown("z"))
            {
                if (personnageTour.PaActuel >= personnageTour.Sorts[1].Pa && personnageTour.SortsCd[1] == 0)
                {
                    personnageTour.DesactiveAutreZone(1);
                    personnageTour.Sorts[1].Activation();
                    View.RPC("ActiveZone", PhotonTargets.Others, 1);
                }

            }
            if (Input.GetKeyDown("e"))
            {
                if (personnageTour.PaActuel >= personnageTour.Sorts[2].Pa && personnageTour.SortsCd[2] == 0)
                {
                    personnageTour.DesactiveAutreZone(2);
                    personnageTour.Sorts[2].Activation();
                    View.RPC("ActiveZone", PhotonTargets.Others, 2);
                }

            }
            if (Input.GetKeyDown("r"))
            {
                if (personnageTour.PaActuel >= personnageTour.Sorts[3].Pa && personnageTour.SortsCd[3] == 0)
                {
                    personnageTour.DesactiveAutreZone(3);
                    personnageTour.Sorts[3].Activation();
                    View.RPC("ActiveZone", PhotonTargets.Others, 3);
                }

            }
        }
    }
}
