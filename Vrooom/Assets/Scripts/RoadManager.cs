using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    
    enum GenerationProcedural {
        semi,
        full
    };

    [Header("Options")]
    [Tooltip("Nombre d'objets de type Road qui compose une map. une valeur <= 0 entraine une g�n�ration infini")]
    [SerializeField]
    int nombreDeRoad = 1;
    [SerializeField]
    GenerationProcedural generationProcedural = GenerationProcedural.semi;

    [Header("Options Semi Proc�dural")]
    [Tooltip("Nombre de tours demand� sur chaque Road. Si une Road n'est pas une boucle la valeur sera forc� � 1 sinon elle prendra celle-ci")]
    [SerializeField]
    int toursParRoad = 1;

    [SerializeField]
    List<GameObject> roadPrefabsSemiProc;


    [Header("Options Full Proc�dural")]
    [SerializeField]
    List<GameObject> roadPrefabsFullProc;

    [SerializeField]
    List<GameObject> map = new List<GameObject>();

    List<GameObject> joueurs = new List<GameObject>();

    void Start() {
        //R�cup�rer une r�f�rence � tout les joueurs/AI en jeu
        CarMovement[] players = FindObjectsOfType<CarMovement>();
        for (int i=0; i< players.Length; i++) {
            joueurs.Add(players[i].gameObject);
        }

        //G�n�re la 1ere road
        Road premiereRoad = generateNext();
        premiereRoad.setStartLine(); //N�c�ssaire pour la 1ere road en Full proc�dual
    }

    public Road generateNext() {
        //Si un nombre de road est cibl� ET que ce nombre est atteint.
        if (nombreDeRoad > 0 && map.Count >= nombreDeRoad) { return null; } //ne rien faire

        //get un segment (Road) prefab random
        int indexNext = -1;
        GameObject nextPrefab = null;
        switch (generationProcedural) {
            case GenerationProcedural.semi:
                indexNext = (int)Random.Range(0, roadPrefabsSemiProc.Count);
                nextPrefab = roadPrefabsSemiProc[indexNext];
                break;
            case GenerationProcedural.full:
                indexNext = (int)Random.Range(0, roadPrefabsFullProc.Count);
                nextPrefab = roadPrefabsFullProc[indexNext];
                break;
            default:
                Debug.Log("Impossible! la gestion proc�durale est ni (semi) ni (full)?");
                break;
        }

        //Cr�e la prochaine Road
        GameObject nextRoadObject = Instantiate(nextPrefab); //TODO placer la Road quand on l'instantie
        map.Add(nextRoadObject);
        Road nextRoad = nextRoadObject.GetComponent<Road>();
        nextRoad.learnPlayers(joueurs);
        nextRoad.configure(toursParRoad);

        //Si le mode est semi proc�dural, toute les roads ont une ligne de d�part
        if (generationProcedural == GenerationProcedural.semi) {
            nextRoad.setStartLine();
        }

        //Si les nombre de road sible est atteint, ajouter une ligne d'arriv�e sur la derrni�re road
        if (map.Count == nombreDeRoad) {
            Debug.Log("nextRoad = Derni�re Road!");
            nextRoad.setFinishLine();
        }

        return nextRoad;
    }
}
