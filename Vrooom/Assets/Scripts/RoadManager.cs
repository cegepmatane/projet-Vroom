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
    [Tooltip("Nombre d'objets de type Road qui compose une map. une valeur n�gative entraine une g�n�ration infini")]
    [SerializeField]
    int nombreDeMap = 1;
    [SerializeField]
    GenerationProcedural generationProcedural = GenerationProcedural.semi;

    [Header("Options Semi Proc�dural")]
    [SerializeField]
    int toursParMap = 1;

    [SerializeField]
    List<GameObject> roadPrefabsSemiProc;


    [Header("Options Full Proc�dural")]
    [SerializeField]
    List<GameObject> roadPrefabsFullProc;

    [SerializeField]
    List<GameObject> map = new List<GameObject>();

    void Start() {
        generateNext();
    }

    public void generateNext() {
        if (nombreDeMap > 0 && map.Count >= nombreDeMap) { return; }

        //get un segment (Road) random
        int indexNext = (int)Random.Range(0, roadPrefabsSemiProc.Count);

        GameObject nextPrefab = null;
        switch (generationProcedural) {
            case GenerationProcedural.semi:
                nextPrefab = roadPrefabsSemiProc[indexNext];
                break;
            case GenerationProcedural.full:
                nextPrefab = roadPrefabsFullProc[indexNext];
                break;
            default:
                break;
        }

        map.Add(Instantiate(nextPrefab));

        if (map.Count == nombreDeMap) {
            Debug.Log("Derni�re Road!");
            //map[map.Count - 1];
            //TODO map.setFinishLine();
        }
    }
}
