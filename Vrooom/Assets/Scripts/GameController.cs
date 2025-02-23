using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [SerializeField]
    RoadManager roadManager;

    [SerializeField]
    Player[] players;

    void Start()
    {
        //Cr�ation des joueurs??
        //Player[] players = FindObjectsOfType<Player>();

        //Configuration de la partie en fonction de menu options et des joueurs cr�es
        roadManager.configure(Options.modeProcedural, Options.nbrCarte, Options.nbrTours, players);

        //D�but de la partie
        roadManager.Begin();

        activatePlayers();
    }

    private void activatePlayers() {
        foreach (Player player in players) {
            player.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
