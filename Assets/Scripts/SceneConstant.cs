using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneConstant : MonoBehaviour {

    public static List<string> teamJoueur2;
    public static List<string> teamJoueur1;
    public static List<string> teamJoueur;

    void Awake () {
        DontDestroyOnLoad(transform.gameObject);
    }

    // Use this for initialization
    void Start () {
        teamJoueur2 = new List<string>();
        teamJoueur1 = new List<string>();
        teamJoueur = new List<string>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public static void AjouterPersonnageTeam(string s)
    {
        teamJoueur1.Add(s);

    }

}
