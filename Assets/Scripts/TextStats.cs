using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextStats : MonoBehaviour {

    private Text affichage;
    private float tempsDebut;
    private RectTransform transfomrText;
    private float enAttente;
    private Case c;
    private string nombre;

    public float EnAttente
    {
        get
        {
            return enAttente;
        }

        set
        {
            enAttente = value;
        }
    }

    public Case C
    {
        get
        {
            return c;
        }

        set
        {
            c = value;
        }
    }

    public string Nombre
    {
        get
        {
            return nombre;
        }

        set
        {
            nombre = value;
        }
    }

    // Use this for initialization
    void Start () {
        affichage = gameObject.GetComponent<Text>();
        transfomrText = GetComponent<RectTransform>();
        tempsDebut = Time.time + EnAttente;
	}
	
	// Update is called once per frame
	void Update () {
        float temps = Time.time;
        if(temps >= tempsDebut)
        {
            if(affichage.text == "")
            {
                affichage.text = nombre;
            }
            transfomrText.position = new Vector3(transfomrText.position.x, transfomrText.position.y + (temps - tempsDebut) * 2, 0);
            if (temps - tempsDebut <= 0.5f)
            {
                affichage.fontSize = (int)(20f * (1f + (temps - tempsDebut)));
            }
            else if (temps - tempsDebut <= 1f)
            {
                affichage.fontSize = (int)(30f * (1.5f - (temps - tempsDebut)));
            }
            else if (temps - tempsDebut >= 1.5f)
            {
                c.NbTextAction--;
                Destroy(gameObject);
            }
        }

    }
}
