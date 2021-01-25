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
    [Tooltip("Nombre d'objets de type Road qui compose une map. une valeur <= 0 entraine une génération infini")]
    [SerializeField]
    int nombreDeRoad = 1;
    [SerializeField]
    GenerationProcedural generationProcedural = GenerationProcedural.semi;

    [Header("Options Semi Procédural")]
    [Tooltip("Nombre de tours demandé sur chaque Road. Si une Road n'est pas une boucle la valeur sera forcé à 1 sinon elle prendra celle-ci")]
    [SerializeField]
    int toursParRoad = 1;

    [SerializeField]
    List<GameObject> roadPrefabsSemiProc;


    [Header("Options Full Procédural")]
    [SerializeField]
    List<GameObject> roadPrefabsFullProc;

    [SerializeField]
    List<GameObject> map = new List<GameObject>();

    List<GameObject> joueurs = new List<GameObject>();

    void Start() {
        //Récupérer une référence à tout les joueurs/AI en jeu
        CarMovement[] players = FindObjectsOfType<CarMovement>();
        for (int i=0; i< players.Length; i++) {
            joueurs.Add(players[i].gameObject);
        }

        //Génère la 1ere road
        Road premiereRoad = generateNext();
        premiereRoad.setStartLine(); //Nécéssaire pour la 1ere road en Full procédual
    }

    public Road generateNext() {
        //Si un nombre de road est ciblé ET que ce nombre est atteint.
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
                Debug.Log("Impossible! la gestion procédurale est ni (semi) ni (full)?");
                break;
        }

        //Crée la prochaine Road
        GameObject nextRoadObject = Instantiate(nextPrefab); //TODO placer la Road quand on l'instantie
        map.Add(nextRoadObject);
        Road nextRoad = nextRoadObject.GetComponent<Road>();
        nextRoad.learnPlayers(joueurs);
        nextRoad.configure(toursParRoad);

        //Si le mode est semi procédural, toute les roads ont une ligne de départ
        if (generationProcedural == GenerationProcedural.semi) {
            nextRoad.setStartLine();
        }

        //Si les nombre de road sible est atteint, ajouter une ligne d'arrivée sur la derrnière road
        if (map.Count == nombreDeRoad) {
            Debug.Log("nextRoad = Dernière Road!");
            nextRoad.setFinishLine();
        }

        return nextRoad;
    }
}
