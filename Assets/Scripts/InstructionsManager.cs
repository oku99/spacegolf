using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionsManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] MenuManager menuManager;
    [SerializeField] GameObject[] pages;

    private int currentPage = 0;
    void Start()
    {
        currentPage = 0;
        menuManager = GetComponent<MenuManager>();
        handleActivePageLogic();
        Debug.Assert(menuManager);
        Debug.Assert(pages != null);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPage != 0 && SceneManager.GetActiveScene().name != "InstructionsScene"){
            currentPage = 0;
        }
    }

    public void loadNextPage(){
        if (currentPage + 1 > pages.Length){
            return;            
        }
        currentPage += 1;
        handleActivePageLogic();
    }

    public void loadPrevPage(){
        if (currentPage - 1 < 0){
            return;            
        }
        currentPage -= 1;
        handleActivePageLogic();
    }

    void handleActivePageLogic(){
        for (int index = 0; index < pages.Length; index++)
        {
            if (index != currentPage){
                pages[index].SetActive(false);
            }
            else{
                pages[index].SetActive(true);
            }
        }
    }
}
