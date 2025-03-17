using System.Collections;
using System.Collections.Generic;
using System.Net.Cache;
using TMPro;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Application : MonoBehaviour
{
    [SerializeField]List<Button> accButtons = new List<Button>();
    [SerializeField] Button startAppButton;
    [SerializeField] TMP_Text selectAccText;
    [SerializeField] TMP_Text accNameText;
    [SerializeField] TMP_Text charListText;

    [System.Serializable]
    public class Account
    {
        public int id;
        public string name;
    }
    /// <summary>
    /// Wrapper class for JsonUtility
    /// </summary>
    [System.Serializable]
    public class AccountList
    {
        public Account[] accounts;
    }

    [System.Serializable]
    public class Character
    {
        public int id;
        public int accountID;
        public string name;
        public int level;
    }

    [System.Serializable]
    public class CharacterList
    {
        public Character[] characters;
    }



    void Start()
    {
        startAppButton.onClick.AddListener(() => {
            selectAccText.gameObject.SetActive(true);
            accButtons.ForEach(button => { button.gameObject.SetActive(true); });

            startAppButton.gameObject.SetActive(false);
            StartCoroutine(GetAccounts());
        });
    }



    IEnumerator GetAccounts()
    {
        UnityWebRequest wr = UnityWebRequest.Get("http://localhost/testapi/accounts");
        yield return wr.SendWebRequest();
        if (wr.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(wr.error);
        }
        else
        {
            string json = "{\"accounts\":" + wr.downloadHandler.text + "}";
            AccountList accountList = JsonUtility.FromJson<AccountList>(json);
            Debug.Log(accountList.accounts.Length + " accounts found.");

            for (int i = 0; i < accButtons.Count; i++) 
            {
                int index = i;
                accButtons[i].GetComponentInChildren<TMP_Text>().text = accountList.accounts[i].name;
                accButtons[i].onClick.AddListener(() => { 
                    accButtons.ForEach(button => { button.gameObject.SetActive(false); });
                    selectAccText.gameObject.SetActive(false);
                    charListText.gameObject.SetActive(true);
                    accNameText.gameObject.SetActive(true);
                    Debug.Log("i: "+i);
                    accNameText.text = accountList.accounts[index].name;
                    StartCoroutine(GetCharacters(accountList.accounts[index].id));
                });
            }
        }
    }

    IEnumerator GetCharacters(int accID)
    {
        UnityWebRequest wr = UnityWebRequest.Get("http://localhost/testapi/characters/account_id/" + accID);
        yield return wr.SendWebRequest();
        if (wr.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(wr.error);
        }
        else
        {
            string json = "{\"characters\":" + wr.downloadHandler.text + "}";
            CharacterList characterList = JsonUtility.FromJson<CharacterList>(json);
            Debug.Log($"{ characterList.characters.Length} characters found for account_id {accID}.");
            string text = "";
            for (int i = 0; i<characterList.characters.Length; i++)
            {
                text += characterList.characters[i].name + ", level " + characterList.characters[i].level + "\n";
            }
            charListText.text = text;
        }
    }
}
