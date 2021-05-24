using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PopUpController : MonoBehaviour
{
    public static PopUpController Instance = null;

    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI popupText;

    private List<CategoriesProgressController.ScienceCategory> categoriesToPopUp;

    public bool isAnimationPlaying = false;

    private void Awake()
    {
        if (!Instance)
            Instance = this;

        categoriesToPopUp = new List<CategoriesProgressController.ScienceCategory>();
    }

    public void AddToList(CategoriesProgressController.ScienceCategory scienceCategory)
    {
        categoriesToPopUp.Add(scienceCategory);

        if(!isAnimationPlaying)
        {
            isAnimationPlaying = true;
            SetPopupText(scienceCategory);
            categoriesToPopUp.Remove(scienceCategory);
            animator.SetTrigger("Popup");
        }

    }

    public void CheckForNextPopups()
    {
        isAnimationPlaying = false;
        Debug.Log("XD");
        if (categoriesToPopUp.Count > 0)
        {
            SetPopupText(categoriesToPopUp[0]);
            categoriesToPopUp.Remove(categoriesToPopUp[0]);
            animator.SetTrigger("Popup");
            isAnimationPlaying = true;
        }
    }

    public void SetPopupText(CategoriesProgressController.ScienceCategory scienceCategory)
    {
        // Wartosci na sztywno, do zmiany potem > Czerpac z team stats
        if(scienceCategory == CategoriesProgressController.ScienceCategory.Energetyka)
        {
            popupText.text = "Zasięg ekspedycji zwiększył się o 1";
        }
        if (scienceCategory == CategoriesProgressController.ScienceCategory.Transport)
        {
            popupText.text = "Pojemność ekspedycji zwiększyła się o 100";
        }
        if (scienceCategory == CategoriesProgressController.ScienceCategory.Telekomunikacja)
        {
            popupText.text = "Zwiększyła się szansa na dodatkowe odkrycia podczas ekspedycji";
        }
        if (scienceCategory == CategoriesProgressController.ScienceCategory.Społeczność)
        {
            popupText.text = "Powstał nowy zespół do ekspedycji";
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
