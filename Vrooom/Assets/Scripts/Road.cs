using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    [Header("Options")]
    [SerializeField]
    bool isLoop = false;

    List<Checkpoint> checkpointList = null;

    Dictionary<GameObject, int> tourDesJoueurs;

    int tours = 1;

    private void Start()
    {
    }

    public void learnPlayers(List<GameObject> players) {
        //apprend tout les joueurs de la partie et met leur nombre de tours � 0 
        foreach (GameObject player in players) {
            tourDesJoueurs.Add(player, 0);
        }
    }

    public void configure(int prefTours) {
        //Si la map est une boucle, permettre de faire plusieurs tours (si demand�)
        if (!isLoop) { return; }
        tours = prefTours;
    }

    //Cette fonction est appel� par un checkpoint (start) ou (finish)
    //Le checkpoint (start) permet de faire plusieurs tour et de changer de Road.
    //Le checkpoint (finish) permet de mettre fin � la course.
    public void nextTurn(GameObject player, bool isFinish) {
        //if (player checkpoint != checkpointList.Count){ player hit wall; return; }

            Debug.Log(gameObject.name + " : " + player.name + " +1 tours!");

            tourDesJoueurs[player]++;
            
            if (tourDesJoueurs[player] < tours) { return; }

            Debug.Log(gameObject.name + " : Le joueur " + player.name + " � fini tout ses tours sur cette road!");

            if (isFinish) {
                //Le joueur � termin� la course
            } else {
                //Le joueur passe � la prochaine road (t�l�portation)
            }
    }

    public void setStartLine() {
        Debug.Log(gameObject.name + " contient une ligne de d�part!");
    }

    public void setFinishLine() {
        Debug.Log(gameObject.name + " contient la ligne d'arriv�!");
    }
}
