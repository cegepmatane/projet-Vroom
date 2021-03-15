using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [SerializeField]
    RoadManager roadManager;

    void Start()
    {
        //Création des joueurs??
        Player[] players = FindObjectsOfType<Player>();

        //Configuration de la partie en fonction de menu options et des joueurs crées
        roadManager.configure(Options.modeProcedural, Options.nbrCarte, Options.nbrTours, players);

        //Début de la partie
        roadManager.Begin();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
