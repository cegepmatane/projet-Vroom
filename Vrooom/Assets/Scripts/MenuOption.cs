using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static RoadManager;

public class MenuOption : MonoBehaviour
{

    public GenerationProcedural procedural;
    public Toggle fullProcedural;
    public Toggle semiProcedural;
    public TMP_InputField nbrSegments;
    public TMP_InputField nbrCarte;
    public TMP_InputField nbrTours;

    // Start is called before the first frame update
    void Start()
    {
        if (Options.modeProcedural == GenerationProcedural.full)
        {
            fullProcedural.isOn = true;
            semiProcedural.isOn = false;
        } else
        {
            semiProcedural.isOn = true;
            fullProcedural.isOn = false;
        }
        nbrSegments.text = Options.nbrSegment.ToString();
        nbrCarte.text = Options.nbrCarte.ToString();
        nbrTours.text = Options.nbrTours.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setFullProcedural(bool a_bool)
    {
        if (!a_bool)
        {
            return;
        }
        Options.modeProcedural = GenerationProcedural.full;
        Debug.Log(Options.modeProcedural);
    }
    
    public void setSemiProcedural(bool a_bool)
    {
        if (!a_bool)
        {
            return;
        }
        Options.modeProcedural = GenerationProcedural.semi;
        Debug.Log(Options.modeProcedural);
    }

    public void setAllOptions()
    {
        setNbrSegments();
        setNbrCarte();
        setNbrTours();
    }

    public void setNbrSegments()
    {
        Options.nbrSegment = int.Parse(nbrSegments.text);
    }

    public void setNbrCarte()
    {
        Options.nbrCarte = int.Parse(nbrCarte.text);
    }

    public void setNbrTours()
    {
        Options.nbrTours = int.Parse(nbrTours.text);
    }

}
