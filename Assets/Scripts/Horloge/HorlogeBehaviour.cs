using UnityEngine;

public class HorlogeBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject[] branches;
    
    [SerializeField] private GameObject[] branchesPivots;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private KeyCode RotateLeftInput;
    [SerializeField] private KeyCode RotateRightInput;
    [SerializeField] private KeyCode MoveRightInput;
    [SerializeField] private KeyCode MoveLeftInput;
    [SerializeField] private KeyCode SelectInput;

    private int currentBranchIndex = 0;
    private bool isInteracting = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HighlightCurrentBranch();
    }

    // Update is called once per frame
    void Update()
    {
        SelectionInputs();
        if (isInteracting)
        {
            InteractionInputs();
        }
        if (!isInteracting)
        {
           
        }
    }

    void SelectionInputs()
    {
        if(Input.GetKeyDown(MoveRightInput) && !isInteracting)
        {
            ChangeSelection(1);
        }
        if(Input.GetKeyDown(MoveLeftInput) && !isInteracting)
        {
            ChangeSelection(-1);
        }
         if (Input.GetKeyDown(SelectInput))
        {
            isInteracting = !isInteracting;
            Debug.Log(isInteracting ? "Interacting with branch" : "Stopped interacting");
             if (CheckTime())
            {
                Debug.Log("Bonne combinaison ! Énigme résolue !");
            }
            else
            {
                Debug.Log("Mauvaise combinaison, réessayez.");
            }
        }

    }
    void InteractionInputs()
    {
      if (Input.GetKeyDown(RotateRightInput))
            {
                RotateBranch(30f); // Tourne de +30°
            }
             else if (Input.GetKeyDown(RotateLeftInput))
            {
                RotateBranch(-30f); // Tourne de -30°
            }
    }
    void ChangeSelection(int direction)
    {
       // Remet la branche actuelle à son matériau par défaut
        branches[currentBranchIndex].GetComponent<Renderer>().material = defaultMaterial;

        // Change l’index de la branche en suivant la direction (cyclique)
        currentBranchIndex = (currentBranchIndex + direction + branches.Length) % branches.Length;

        // Applique le matériau de surbrillance à la nouvelle branche sélectionnée
        HighlightCurrentBranch();
    }
    void HighlightCurrentBranch()
    {
        branches[currentBranchIndex].GetComponent<Renderer>().material = highlightMaterial;
    }
    void RotateBranch(float angle)
    {
        float currentZRotation = branchesPivots[currentBranchIndex].transform.localEulerAngles.z;
        float newZRotation = (currentZRotation + angle) % 360f;
        branchesPivots[currentBranchIndex].transform.localRotation = Quaternion.Euler(0f, 0f, newZRotation);
    }
    bool CheckTime()
    {
        float hourAngle = Mathf.Round(branchesPivots[0].transform.localEulerAngles.z);
        float minuteAngle = Mathf.Round(branchesPivots[1].transform.localEulerAngles.z);
        float secondAngle = Mathf.Round(branchesPivots[2].transform.localEulerAngles.z);
        return hourAngle == 90f && minuteAngle == 180f && secondAngle == 0f;
}
}  
