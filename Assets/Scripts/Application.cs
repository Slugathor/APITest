using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Application : MonoBehaviour
{
    [SerializeField]List<Button> accButtons = new List<Button>();
    [SerializeField] Button startAppButton;

    // Start is called before the first frame update
    void Start()
    {
        startAppButton.onClick.AddListener(() => {
            accButtons.ForEach(button => { 
                button.gameObject.SetActive(true);
                button.onClick.AddListener(() => { GetCharactersFromAccount(); });
            });

            startAppButton.gameObject.SetActive(false);
            // GET ACCOUNT NAMES FROM DB
        });
    }

    void GetCharactersFromAccount() 
    {

    }

    
}
