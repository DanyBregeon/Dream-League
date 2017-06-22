using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonMenu : MonoBehaviour {

    [SerializeField]
    private Text textErreur;

    [SerializeField]
    private List<Toggle> listePersonnageChoix;

    [SerializeField]
    private int nbChoix;

    // Use this for initialization
    void Start () {
        /*Button b = gameObject.GetComponent<Button>();
        b.onClick.AddListener(delegate () { StartGame("Level1"); });*/
    }

    public void StartGame(string level)
    {
        int nbChoixActif = 0;
        foreach(Toggle c in listePersonnageChoix)
        {
            if (c.isOn)
            {
                nbChoixActif++;
                SceneConstant.AjouterPersonnageTeam(c.gameObject.GetComponent<ToggleChoixPersonnage>().Personnage.name);
            }
        }
        if(nbChoixActif == nbChoix)
        {
            
            SceneManager.LoadScene("Arene" + level);
        }
        else
        {
            SceneConstant.teamJoueur.Clear();
            textErreur.text = "Vous devez sélectionner " + nbChoix + " personnages";
        }
    }
}
