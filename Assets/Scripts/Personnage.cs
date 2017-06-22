using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Personnage : Entitee
{

    public List<GameObject> sprites;
    public List<GameObject> sortsIcone;
    public List<Text> affichageCdSort;
    private GameObject spriteActuel;
    private GameObject spriteTimeLine;
    private int pv;
    private int pa;
    private int pm;
    private int dgtBonus;
    private int resBonus;
    private int pvActuel;
    private int paActuel;
    private int pmActuel;
    private int dgtBonusActuel;
    private int resBonusActuel;

    [SerializeField]
    private GameObject affichageTextAction;

    [SerializeField]
    private Sort[] sorts;

    private int[] sortsCd;
    private bool enDeplacement;
    private Vector3 posDepart, posArrivee;
    //private float tempsDebutDeplacement;
    private Case casePersonnage;
    public GameObject fondTextPv;
    public Text textPv;
    private Text affichageTextPa;
    private Text affichageTextPm;
    //private List<List<int>[]> listeBuff;
    private List<int> buffPaDuree;
    private List<int> buffPaValeur;
    private List<int> buffPmDuree;
    private List<int> buffPmValeur;
    private List<int> buffDgtDuree;
    private List<int> buffDgtValeur;
    private List<int> buffResDuree;
    private List<int> buffResValeur;

    private Stack<Case> cheminAParcourir;

    private PhotonView view;

    public int Pv
    {
        get
        {
            return pv;
        }

        set
        {
            pv = value;
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

    public int Pm
    {
        get
        {
            return pm;
        }

        set
        {
            pm = value;
        }
    }

    public GameObject SpriteActuel
    {
        get
        {
            return spriteActuel;
        }

        set
        {
            spriteActuel = value;
            int spriteActuelIndex = sprites.IndexOf(spriteActuel);
            GameObject sprite = sprites[spriteActuelIndex];
            for (int i = 0; i < sprites.Count; i++)
            {
                if (sprite != null)
                {
                    if (i == spriteActuelIndex)
                    {
                        sprites[i].SetActive(true);
                    }
                    else
                    {
                        sprites[i].SetActive(false);
                    }
                }
            }
        }
    }

    public bool EnDeplacement
    {
        get
        {
            return enDeplacement;
        }

        set
        {
            enDeplacement = value;
        }
    }

    public Vector3 PosDepart
    {
        get
        {
            return posDepart;
        }

        set
        {
            posDepart = value;
        }
    }

    public Vector3 PosArrivee
    {
        get
        {
            return posArrivee;
        }

        set
        {
            posArrivee = value;
            if (fondTextPv != null)
                fondTextPv.transform.position = GameObject.Find("Main Camera").GetComponent<Camera>().WorldToScreenPoint(new Vector3(posArrivee.x, posArrivee.y + 0.4f, posArrivee.z));
        }
    }

    public Case CasePersonnage
    {
        get
        {
            return casePersonnage;
        }

        set
        {
            casePersonnage = value;
        }
    }

    public int PvActuel
    {
        get
        {
            return pvActuel;
        }

        set
        {
            pvActuel = value;
            if(textPv != null)
            textPv.text = pvActuel.ToString();
        }
    }

    public int PaActuel
    {
        get
        {
            return paActuel;
        }

        set
        {
            if (value - paActuel != 0 && casePersonnage != null)
            {
                AfficherText(value - paActuel, Constantes.bleutextPa, casePersonnage);
            }
            paActuel = value;
            affichageTextPa.text = paActuel.ToString();
        }
    }

    public int PmActuel
    {
        get
        {
            return pmActuel;
        }

        set
        {
            if (value - pmActuel != 0 && casePersonnage != null)
            {
                AfficherText(value - pmActuel, Constantes.vertTextPm, casePersonnage);
            }
            pmActuel = value;
            affichageTextPm.text = pmActuel.ToString();
        }
    }

    public Sort[] Sorts
    {
        get
        {
            return sorts;
        }

        set
        {
            sorts = value;
        }
    }

    public int DgtBonus
    {
        get
        {
            return dgtBonus;
        }

        set
        {
            dgtBonus = value;
        }
    }

    public List<int> BuffPaDuree
    {
        get
        {
            return buffPaDuree;
        }

        set
        {
            buffPaDuree = value;
        }
    }

    public List<int> BuffPaValeur
    {
        get
        {
            return buffPaValeur;
        }

        set
        {
            buffPaValeur = value;
        }
    }

    public int DgtBonusActuel
    {
        get
        {
            return dgtBonusActuel;
        }

        set
        {
            dgtBonusActuel = value;
        }
    }

    public int ResBonusActuel
    {
        get
        {
            return resBonusActuel;
        }

        set
        {
            resBonusActuel = value;
        }
    }

    public List<int> BuffPmDuree
    {
        get
        {
            return buffPmDuree;
        }

        set
        {
            buffPmDuree = value;
        }
    }

    public List<int> BuffPmValeur
    {
        get
        {
            return buffPmValeur;
        }

        set
        {
            buffPmValeur = value;
        }
    }

    public List<int> BuffDgtDuree
    {
        get
        {
            return buffDgtDuree;
        }

        set
        {
            buffDgtDuree = value;
        }
    }

    public List<int> BuffDgtValeur
    {
        get
        {
            return buffDgtValeur;
        }

        set
        {
            buffDgtValeur = value;
        }
    }

    public List<int> BuffResDuree
    {
        get
        {
            return buffResDuree;
        }

        set
        {
            buffResDuree = value;
        }
    }

    public List<int> BuffResValeur
    {
        get
        {
            return buffResValeur;
        }

        set
        {
            buffResValeur = value;
        }
    }

    public int[] SortsCd
    {
        get
        {
            return sortsCd;
        }

        set
        {
            sortsCd = value;
        }
    }

    public int ResBonus
    {
        get
        {
            return resBonus;
        }

        set
        {
            resBonus = value;
        }
    }

    public GameObject SpriteTimeLine
    {
        get
        {
            return spriteTimeLine;
        }

        set
        {
            spriteTimeLine = value;
        }
    }

    public Stack<Case> CheminAParcourir
    {
        get
        {
            return cheminAParcourir;
        }

        set
        {
            cheminAParcourir = value;
        }
    }


    private void Awake()
    {
        PosDepart = transform.position;
        SortsCd = new int[4];
        affichageTextPa = GameObject.Find("Pa").GetComponent<Text>();
        affichageTextPm = GameObject.Find("Pm").GetComponent<Text>();
        affichageCdSort = new List<Text>()
        {
            GameObject.Find("CdSpell1").GetComponent<Text>(),
            GameObject.Find("CdSpell2").GetComponent<Text>(),
            GameObject.Find("CdSpell3").GetComponent<Text>(),
            GameObject.Find("CdSpell4").GetComponent<Text>()
        }
        ;

        buffPaDuree = new List<int>();
        buffPaValeur = new List<int>();
        BuffPmDuree = new List<int>();
        BuffPmValeur = new List<int>();
        BuffDgtDuree = new List<int>();
        BuffDgtValeur = new List<int>();
        BuffResDuree = new List<int>();
        BuffResValeur = new List<int>();

        CheminAParcourir = new Stack<Case>();

        view = GetComponent<PhotonView>();

        for(int i = 0; i<sortsIcone.Count; i++)
        {
            sortsIcone[i] = Instantiate(sortsIcone[i], sortsIcone[i].transform.position, sortsIcone[i].transform.rotation);
            sortsIcone[i].SetActive(false);
        }

        /*listeBuff = new List<List<int>[]>()
        {
            new List<int>[2] { buffPaDuree, buffPaValeur },
            new List<int>[2] { buffPmDuree, buffPmValeur },
            new List<int>[2] { buffDgtDuree, buffDgtValeur },
            new List<int>[2] { buffResDuree, buffResValeur }
        };*/
    }

    public virtual void DebutTour()
    {
        foreach (GameObject i in sortsIcone)
        {
            i.SetActive(true);
        }

        paActuel = pa;
        affichageTextPa.text = paActuel.ToString();
        pmActuel = pm;
        affichageTextPm.text = pmActuel.ToString();
        dgtBonusActuel = dgtBonus;
        resBonusActuel = ResBonus;

        for (int i = 0; i < SortsCd.Length; i++)
        {
            if(SortsCd[i] > 0)
            {
                SortsCd[i]--;
                affichageCdSort[i].text = SortsCd[i].ToString();
                sortsIcone[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.6f);
            }
            if (SortsCd[i] == 0)
            {
                affichageCdSort[i].text = "";
                sortsIcone[i].GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

        for (int i = 0; i < BuffPaDuree.Count; i++)
        {
            if (BuffPaDuree[i] > 0)
            {
                paActuel += BuffPaValeur[i];
                affichageTextPa.text = paActuel.ToString();
                BuffPaDuree[i]--;
            }
            else
            {
                BuffPaDuree.RemoveAt(i);
                BuffPaValeur.RemoveAt(i);
                i--;
            }
        }
        for (int i = 0; i < BuffPmDuree.Count; i++)
        {
            if (BuffPmDuree[i] > 0)
            {
                pmActuel += BuffPmValeur[i];
                affichageTextPm.text = pmActuel.ToString();
                BuffPmDuree[i]--;
            }
            else
            {
                BuffPmDuree.RemoveAt(i);
                BuffPmValeur.RemoveAt(i);
                i--;
            }
        }
        for (int i = 0; i < BuffDgtDuree.Count; i++)
        {
            if (BuffDgtDuree[i] > 0)
            {
                DgtBonusActuel += BuffDgtValeur[i];
                BuffDgtDuree[i]--;
            }
            else
            {
                BuffDgtDuree.RemoveAt(i);
                BuffDgtValeur.RemoveAt(i);
                i--;
            }
        }
        for (int i = 0; i < BuffResDuree.Count; i++)
        {
            if (BuffResDuree[i] > 0)
            {
                ResBonusActuel += BuffResValeur[i];
                BuffResDuree[i]--;
            }
            else
            {
                BuffResDuree.RemoveAt(i);
                BuffResValeur.RemoveAt(i);
                i--;
            }
        }
        for (int i = 0; i < Sorts.Length; i++)
        {
            if (PaActuel < Sorts[i].Pa)
            {
                sortsIcone[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.6f);
            }
        }
        TimeLineAJour();
    }


    public void DesactiveAutreZone(int indexSort)
    {
        for (int i = 0; i < sorts.Length; i++)
        {
            if (i != indexSort)
            {
                sorts[i].ZoneActive = false;
            }
        }
    }

    public void TimeLineAJour()
    {
        for (int i = 0; i < Partie.personnages.Count; i++)
        {
            if (this != Partie.personnages[i])
            {
                if(Partie.personnages[i].spriteTimeLine.GetComponent<SpriteRenderer>().color != new Color(1, 1, 1, 0.15f))
                Partie.personnages[i].spriteTimeLine.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.5f);
            }
            spriteTimeLine.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public bool ZoneActive()
    {
        for (int i = 0; i < sorts.Length; i++)
        {
            if (sorts[i].ZoneActive)
            {
                return true;
            }
        }
        return false;
    }

    public int SortActif()
    {
        for (int i = 0; i < sorts.Length; i++)
        {
            if (sorts[i].ZoneActive)
            {
                return i;
            }
        }
        return -1;
    }

    public bool CheckMourir(Personnage p)
    {
        if(p.PvActuel <= 0)
        {
            Partie.personnages.Remove(p);
            if (Partie.teamA.Contains(p))
            {
                Partie.teamA.Remove(p);
            }
            else
            {
                Partie.teamB.Remove(p);
            }
            p.spriteTimeLine.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.15f);
            p.casePersonnage.Traversable = true;
            p.textPv.text = "";
            p.fondTextPv.GetComponent<Image>().rectTransform.localScale = new Vector3(0, 0, 0);
            Destroy(p.gameObject);
            return true;
        }
        return false;
    }

    public void SeDeplacerVers(string c, String chemin)
    {
        view.RPC("SeDeplacer", PhotonTargets.All, c, chemin);
    }

    public void LancerSortSur(string c)
    {
        view.RPC("LancerSort", PhotonTargets.All, c);
    }

    [PunRPC]
    protected void SeDeplacer(string c, String chemin)
    {
        if(this == Partie.personnageTour)
        {
            Partie.chemin = new Stack<Case>();
            CheminAParcourir = new Stack<Case>();
            String[] liste;
            liste = chemin.Split('/');
            /*for (int i = 0; i < liste.Length - 1; i++)
            {
                Partie.chemin.Push(GameObject.Find(liste[i]).GetComponent<Case>());
            }*/
            for (int i = liste.Length - 2; i >= 0; i--)
            {
                print(liste[i]);
                Partie.chemin.Push(GameObject.Find(liste[i]).GetComponent<Case>());
            }

            CheminAParcourir = Partie.chemin;
            CasePersonnage.Traversable = true;
            PosArrivee = Partie.personnageTour.PosDepart;
            EnDeplacement = true;
            PmActuel -= Partie.chemin.Count;
            GameObject.Find(c).GetComponent<Case>().Traversable = false;
            //AfficherText(-Partie.chemin.Count, Constantes.vertTextPm, Partie.personnageTour.casePersonnage);
        }

    }

    [PunRPC]
    protected void LancerSort(string c)
    {
        if(this == Partie.personnageTour)
        {
            Case caseCible = GameObject.Find(c).GetComponent<Case>();
            Partie.personnageTour.PaActuel -= Partie.personnageTour.Sorts[Partie.personnageTour.SortActif()].Pa;
            //AfficherText(-Partie.personnageTour.Sorts[Partie.personnageTour.SortActif()].Pa, Constantes.bleutextPa, Partie.personnageTour.casePersonnage);

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
                        if (Partie.personnageTour.Sorts[Partie.personnageTour.SortActif()].ZoneEffet(caseCible, Partie.plateau[i, j]))
                        {
                            foreach (Personnage p in Partie.personnages)
                            {
                                if (p.CasePersonnage == Partie.plateau[i, j])
                                {
                                    Partie.personnageTour.Sorts[Partie.personnageTour.SortActif()].Effet(p.CasePersonnage);
                                    p.PvActuel -= (int)(Partie.personnageTour.Sorts[Partie.personnageTour.SortActif()].Dgt * (1 + ((float)Partie.personnageTour.DgtBonusActuel) / 100f - ((float)p.ResBonusActuel) / 100f));
                                    if (Partie.personnageTour.Sorts[Partie.personnageTour.SortActif()].Dgt > 0)
                                    {
                                        AfficherText(-(int)(Partie.personnageTour.Sorts[Partie.personnageTour.SortActif()].Dgt * (1 + ((float)Partie.personnageTour.DgtBonusActuel) / 100f - ((float)p.ResBonusActuel) / 100f)), Color.red, p.CasePersonnage);
                                    }
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
                    Partie.personnageTour.Sorts[Partie.personnageTour.SortActif()].Effet(caseCible);
                }
                else
                {
                    foreach (Personnage p in Partie.personnages)
                    {
                        if (p.CasePersonnage == caseCible)
                        {
                            Partie.personnageTour.Sorts[Partie.personnageTour.SortActif()].Effet(caseCible);
                            p.PvActuel -= (int)(Partie.personnageTour.Sorts[Partie.personnageTour.SortActif()].Dgt * (1 + ((float)Partie.personnageTour.DgtBonusActuel) / 100f - ((float)p.ResBonusActuel) / 100f));
                            if (Partie.personnageTour.Sorts[Partie.personnageTour.SortActif()].Dgt > 0)
                            {
                                AfficherText(-(int)(Partie.personnageTour.Sorts[Partie.personnageTour.SortActif()].Dgt * (1 + ((float)Partie.personnageTour.DgtBonusActuel) / 100f - ((float)p.ResBonusActuel) / 100f)), Color.red, caseCible);
                            }
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

    public void AfficherText(int n, Color couleur, Case c)
    {
        c.NbTextAction++;
        GameObject textAction = Instantiate(affichageTextAction, GameObject.Find("Main Camera").GetComponent<Camera>().WorldToScreenPoint(new Vector3(c.transform.position.x, c.transform.position.y + 0.4f, -10f)), Quaternion.identity);
        textAction.GetComponent<Text>().color = couleur;
        textAction.transform.SetParent(GameObject.Find("Canvas").transform);
        textAction.GetComponent<TextStats>().EnAttente = (float)(c.NbTextAction - 1)/4f;
        textAction.GetComponent<TextStats>().C = c;
        textAction.GetComponent<TextStats>().Nombre = "" + n;
    }

    // Update is called once per frame
    void Update()
    {
        CheckMourir(this);
        posDepart = transform.position;
        if (enDeplacement)
        {
            Case caseDepart = casePersonnage;

            Case caseArrivee;
            if (/*Partie.chemin.Count > 0*/CheminAParcourir.Count > 0 && transform.position == posArrivee)
            {
                Vector3 depart = transform.position;
                Vector3 arrivee = depart;
                //caseArrivee = Partie.chemin.Pop();
                caseArrivee = CheminAParcourir.Pop();

                if (caseArrivee.X > caseDepart.X && Math.Round((float)caseArrivee.Y) == Math.Round((float)caseDepart.Y))
                {
                    SpriteActuel = sprites[1];
                    arrivee = new Vector3(depart.x - 0.43f, depart.y - 0.25f, caseArrivee.Go.transform.position.z - 0.0025f);
                }
                else if (caseArrivee.X < caseDepart.X && Math.Round((float)caseArrivee.Y) == Math.Round((float)caseDepart.Y))
                {
                    SpriteActuel = sprites[3];
                    arrivee = new Vector3(depart.x + 0.43f, depart.y + 0.25f, caseArrivee.Go.transform.position.z - 0.0025f);
                }
                else if (Math.Round((float)caseArrivee.X) == Math.Round((float)caseDepart.X) && caseArrivee.Y < caseDepart.Y)
                {
                    SpriteActuel = sprites[2];
                    arrivee = new Vector3(depart.x - 0.43f, depart.y + 0.25f, caseArrivee.Go.transform.position.z - 0.0025f);
                }
                else if (Math.Round((float)caseArrivee.X) == Math.Round((float)caseDepart.X) && caseArrivee.Y > caseDepart.Y)
                {
                    SpriteActuel = sprites[0];
                    arrivee = new Vector3(depart.x + 0.43f, depart.y - 0.25f, caseArrivee.Go.transform.position.z - 0.0025f);
                }
                //print(caseDepart + "    " + caseArrivee + "    " + depart.x + "," + depart.y + "," + depart.z + "    " + arrivee.x + "," + arrivee.y + "," + arrivee.z);

                PosDepart = depart;
                PosArrivee = arrivee;
                caseDepart = caseArrivee;
                casePersonnage = caseArrivee;

            }
            else
            {
                enDeplacement = false;
            }

        }
        if (transform.position != posArrivee)
        {
            transform.position = Vector3.MoveTowards(posDepart, posArrivee, 1);
        }

    }
}
