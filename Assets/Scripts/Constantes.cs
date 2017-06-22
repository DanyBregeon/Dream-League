using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constantes : MonoBehaviour {

    public static int taillePlateauX = 14;
    public static int taillePlateauY = 14;
    public static Color vertTextPm;
    public static Color bleutextPa;

    // Use this for initialization
    void Start () {
        vertTextPm = new Color(34f / 225f, 139f / 255f, 34f / 255f);
        bleutextPa = new Color(24f / 225f, 116f / 255f, 205f / 255f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
