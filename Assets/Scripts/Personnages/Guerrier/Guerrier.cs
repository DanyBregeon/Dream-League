using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guerrier : Personnage
{
    public Text textFurie;
    private int furie;
    private int furieMax;

    public int Furie
    {
        get
        {
            return furie;
        }

        set
        {
            furie = value;
            textFurie.text = furie.ToString();
        }
    }

    public int FurieMax
    {
        get
        {
            return furieMax;
        }

        set
        {
            furieMax = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        textFurie = GameObject.Find("Furie").GetComponent<Text>();
        textFurie.enabled = false;
        Furie = 0;
        FurieMax = 500;
        Pv = 850;
        Pa = 8;
        Pm = 6;
        PvActuel = 800;
        PaActuel = 8;
        PmActuel = 6;
        SpriteActuel = sprites[0];
        Sorts[0] = gameObject.GetComponent<GuerrierSort1>();
        Sorts[1] = gameObject.GetComponent<GuerrierSort2>();
        Sorts[2] = gameObject.GetComponent<GuerrierSort3>();
        Sorts[3] = gameObject.GetComponent<GuerrierSort4>();
        for (int i = 0; i < SortsCd.Length; i++)
        {
            SortsCd[i] = Sorts[i].Cd;
        }

        PosArrivee = PosDepart;
    }
}
