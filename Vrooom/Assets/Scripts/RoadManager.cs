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
    [SerializeField]
    GenerationProcedural generationProcedural = GenerationProcedural.semi;

    [Header("Options Semi Procédural")]
    [SerializeField]
    List<Road> roadPrefabsSemiProc;


    [Header("Options Full Procédural")]
    [SerializeField]
    List<Road> roadPrefabsFullProc;

    [SerializeField]
    List<GameObject> roadprefabs = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
