using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    [Header("Options")]
    [SerializeField]
    bool isLoop = false;

    [SerializeField]
    List<Checkpoint> checkpointList = null;

    Dictionary<int, int> tourDesJoueurs = new Dictionary<int, int>();

    int tours = 1;

    private void Start()
    {
    }

    public void learnPlayers(List<GameObject> players) {
        //apprend tout les joueurs de la partie et met leur nombre de tours à 0 
        foreach (GameObject player in players) {
            tourDesJoueurs.Add(player.GetInstanceID(), 0);
        }
    }

    public void configure(int prefTours) {
        //Si la map est une boucle, permettre de faire plusieurs tours (si demandé)
        if (!isLoop) { return; }
        tours = prefTours;
    }

    //Cette fonction est appelé par un checkpoint (start) ou (finish)
    //Le checkpoint (start) permet de faire plusieurs tour et de changer de Road.
    //Le checkpoint (finish) permet de mettre fin à la course.
    public void nextTurn(GameObject player, bool isFinish) {
        //if (player checkpoint != checkpointList.Count){ player hit wall; return; }

            Debug.Log(gameObject.name + " : " + player.name + " +1 tours!");

            tourDesJoueurs[player.GetInstanceID()]++;
            
            if (tourDesJoueurs[player.GetInstanceID()] < tours) { return; }

            Debug.Log(gameObject.name + " : Le joueur " + player.name + " à fini tout ses tours sur cette road!");

            if (isFinish) {
                //Le joueur à terminé la course
            } else {
                //Le joueur passe à la prochaine road (téléportation)
            }
    }

    public Transform setStartLine() {
        checkpointList[0].setStartLine();
        return checkpointList[0].transform;
    }

    public void setFinishLine() {
        if (isLoop)
            checkpointList[0].setFinishLine();
        else
            checkpointList[checkpointList.Count-1].setFinishLine();
    }
}
