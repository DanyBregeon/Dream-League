using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arbaletrier : Personnage {

    private List<int> stacks;
    private bool ultime;
    private int ultimeDuree;
    private bool boostProchaineAttaque;

    public List<int> Stacks
    {
        get
        {
            return stacks;
        }

        set
        {
            stacks = value;
        }
    }

    public bool Ultime
    {
        get
        {
            return ultime;
        }

        set
        {
            ultime = value;
        }
    }

    public bool BoostProchaineAttaque
    {
        get
        {
            return boostProchaineAttaque;
        }

        set
        {
            boostProchaineAttaque = value;
        }
    }

    public int UltimeDuree
    {
        get
        {
            return ultimeDuree;
        }

        set
        {
            ultimeDuree = value;
        }
    }

    void Start()
    {
        Pv = 700;
        Pa = 10;
        Pm = 4;
        PvActuel = 700;
        PaActuel = 10;
        PmActuel = 4;
        SpriteActuel = sprites[0];

        Sorts[0] = gameObject.GetComponent<ArbaletrierSort1>();
        Sorts[1] = gameObject.GetComponent<ArbaletrierSort2>();
        Sorts[2] = gameObject.GetComponent<ArbaletrierSort3>();
        Sorts[3] = gameObject.GetComponent<ArbaletrierSort4>();
        for (int i = 0; i < SortsCd.Length; i++)
        {
            SortsCd[i] = Sorts[i].Cd;
        }

        PosArrivee = PosDepart;
    }

    public override void DebutTour()
    {
        if(Partie.tourActuel == 1)
        {
            Stacks = new List<int>();
            foreach (Personnage p in Partie.personnages)
            {
                Stacks.Add(0);
            }
        }
        base.DebutTour();
        boostProchaineAttaque = false;
        if(ultimeDuree > 0)
        {
            ultimeDuree--;
        }
        if (ultimeDuree == 0)
        {
            ultime = false;
        }
    }
}
