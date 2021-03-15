using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [SerializeField]
    RoadManager roadManager;

    void Start()
    {
        //Cr�ation des joueurs??
        Player[] players = FindObjectsOfType<Player>();

        //Configuration de la partie en fonction de menu options et des joueurs cr�es
        roadManager.configure(Options.modeProcedural, Options.nbrCarte, Options.nbrTours, players);

        //D�but de la partie
        roadManager.Begin();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
