using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Xml.Serialization;
using TMPro;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static System.Net.WebRequestMethods;

public class Application : MonoBehaviour
{
    [SerializeField] GameObject accButtonParent;
    [SerializeField]List<Button> accButtons = new List<Button>();
    [SerializeField] Button startAppButton;

    [SerializeField] GameObject loginView;
    [SerializeField] Button loginButton;
    [SerializeField] TMP_InputField accountNameField;
    [SerializeField] TMP_InputField passwordField;

    [SerializeField] GameObject selectedAccAndCharsTexts;
    [SerializeField] TMP_Text selectAccText;
    [SerializeField] TMP_Text selectedAccNameText;
    [SerializeField] TMP_Text charListText;
    [SerializeField] TMP_Text loggedInAsText;
    [SerializeField] Button returnToAccListButton;

    [SerializeField] GameObject deleteCharView;
    [SerializeField] TMP_InputField deleteCharNameInput;
    [SerializeField] Button deleteCharButton;

    [SerializeField] Button logoutButton;

    [SerializeField] Button changePasswordButton;
    [SerializeField] Button confirmNewPasswordButton;
    [SerializeField] TMP_InputField newPassWordField1;
    [SerializeField] TMP_InputField newPassWordField2;
    [SerializeField] Button cancelPasswordChangeButton;
    [SerializeField] GameObject changePassWordParent;

    [SerializeField] Button createAccountButton;
    [SerializeField] GameObject createAccountView;
    [SerializeField] TMP_InputField createAccountNameInput;
    [SerializeField] TMP_InputField createAccountPasswordInput;
    [SerializeField] Button finaliseCreateAccountButton;

    private SessionManager sessionManager;
    private int selectedAccountID;
    /// <summary>
    /// Url of the api folder where all the endpoints are.
    /// </summary>
    static readonly string baseUrl = "http://localhost/testapi/";

    [System.Serializable]
    public class Account
    {
        public int id;
        public string username;
        public int accountType;
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
        public int account_id;
        public string name;
        public int level;
    }

    [System.Serializable]
    public class CharacterList
    {
        public Character[] characters;
    }

    [System.Serializable]
    public class LoginData
    {
        public string username;
        public string password;
    }

    [System.Serializable]
    public class PasswordChangeRequest
    {
        public string new_password;
    }

    private void Awake()
    {
        try { sessionManager = GetComponent<SessionManager>(); }
        catch { Debug.LogWarning("SessionManager not found."); }
    }

    void Start()
    {
        // Fetching empty acc buttons from parent and hiding
        accButtonParent.SetActive(true);
        foreach (Button button in accButtonParent.GetComponentsInChildren<Button>()) { accButtons.Add(button); /*button.gameObject.SetActive(false);*/ }
        accButtonParent.SetActive(false);

        startAppButton.onClick.AddListener(() => { startAppButton.gameObject.SetActive(false); ShowLoginScreen(); });
    }



    IEnumerator GetAccounts()
    {
        UnityWebRequest wr = UnityWebRequest.Get(baseUrl + "accounts");
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
                if (i < accountList.accounts.Length)
                {
                    int index = i;
                    accButtons[i].GetComponentInChildren<TMP_Text>().text = accountList.accounts[i].username;
                    accButtons[i].onClick.AddListener(() =>
                    { // Account button does all this when clicked
                        accButtons.ForEach(button => { button.gameObject.SetActive(false); });
                        selectAccText.gameObject.SetActive(false);
                        selectedAccAndCharsTexts.gameObject.SetActive(true);
                        selectedAccNameText.text = accountList.accounts[index].username;
                        selectedAccountID = accountList.accounts[index].id;
                        StartCoroutine(GetCharacters(accountList.accounts[index].id));
                        returnToAccListButton.gameObject.SetActive(true);
                        ActivateDeleteButton();
                        accButtons.ForEach(button=>button.onClick.RemoveAllListeners());
                    });
                }
                else { accButtons[i].gameObject.SetActive(false);}
            }
        }
    }


    IEnumerator GetCharacters(int accID)
    {
        UnityWebRequest wr = UnityWebRequest.Get(baseUrl + "characters/account_id/" + accID);
        yield return wr.SendWebRequest();
        if (wr.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(wr.error);
        }
        else
        {
            string json = "{\"characters\":" + wr.downloadHandler.text + "}";
            CharacterList characterList = JsonUtility.FromJson<CharacterList>(json);
            selectedAccAndCharsTexts.gameObject.SetActive(true);

            if (characterList.characters == null || characterList.characters.Length == 0)
            {
                charListText.text = "No characters found for this account.";
                Debug.Log("No chars found for account.");
                yield break;
            }

            Debug.Log($"{ characterList.characters.Length} characters found for account_id {accID}.");
            string text = "";
            for (int i = 0; i<characterList.characters.Length; i++)
            {
                text += characterList.characters[i].name + ", level " + characterList.characters[i].level + "\n";
            }
            charListText.text = text;

            if (sessionManager.GetAccountType() == 1)
            {
                ActivateReturnToAccListButton();
            }
        }
    }

    IEnumerator Login(string username, string password)
    {
        LoginData credentials = new LoginData { username = username, password = password };
        string jsonData = JsonUtility.ToJson(credentials);

        UnityWebRequest wr = new UnityWebRequest(baseUrl + "login.php", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        wr.uploadHandler = new UploadHandlerRaw(bodyRaw);
        wr.downloadHandler = new DownloadHandlerBuffer();
        wr.SetRequestHeader("Content-Type", "application/json");

        yield return wr.SendWebRequest();

        if (wr.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Login unsuccessful: " + wr.error);
            Debug.Log("Server responded: " + wr.downloadHandler.text);

        }
        else
        {
            Debug.Log("Server responded: " + wr.downloadHandler.text);
            sessionManager.CheckSession();
            // wait for session info (max 10sec)
            for (int i = 0; i < 1000; i++)
            {
                if (!sessionManager.IsSessionInfoReady())
                {
                    yield return new WaitForSeconds(0.01f);
                }
                else
                {
                    break;
                }
            }
            accountNameField.text = string.Empty;
            passwordField.text = string.Empty;
            CloseLoginView();
            ShowPostLoginContent(); // might not be optimal here, but use it for testing now
        }
    }

    IEnumerator Logout()
    {
        Debug.Log("Logging out");

        UnityWebRequest wr = UnityWebRequest.Get(baseUrl + "logout.php");
        yield return wr.SendWebRequest();

        if (wr.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Logout failed: " + wr.error);
        }
        else
        {
            Debug.Log("Logged out: " + wr.downloadHandler.text);
            logoutButton.gameObject.SetActive(false);
            // clear local session state
            sessionManager.ClearSession();
            HideOtherUIElements();
            ShowLoginScreen();
        }
    }

    IEnumerator CreateAccount(string username, string password)
    {
        LoginData credentials = new LoginData { username = username, password = password };
        string jsonData = JsonUtility.ToJson(credentials);

        UnityWebRequest wr = new UnityWebRequest(baseUrl + "register.php", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        wr.uploadHandler = new UploadHandlerRaw(bodyRaw);
        wr.downloadHandler = new DownloadHandlerBuffer();
        wr.SetRequestHeader("Content-Type", "application/json");

        yield return wr.SendWebRequest();

        if (wr.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Could not create account: " + wr.error);
            Debug.Log("Server responded: " + wr.downloadHandler.text);
        }
        else
        {
            Debug.Log("Server responded: " + wr.downloadHandler.text);
            createAccountNameInput.text = string.Empty;
            createAccountPasswordInput.text = string.Empty;
            CloseCreateAccountView(); // close acc creation and show login screen
        }
    }

    IEnumerator DeleteCharacter(int accID, string charName)
    {
        Debug.Log($"Deleting {charName} from account {accID}.");

        UnityWebRequest wr = UnityWebRequest.Delete(baseUrl + "characters/delete/" + accID + "/" + UnityWebRequest.EscapeURL(charName));
        wr.downloadHandler = new DownloadHandlerBuffer();
        yield return wr.SendWebRequest();
        if (wr.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(wr.error);
        }
        else
        {
            Debug.Log("Delete result: " + wr.downloadHandler.text);
            StartCoroutine(GetCharacters(accID));
        }
    }

    IEnumerator ChangePassword(string newPassword)
    {
        // Because debug log messages deserve a proper check for adding possessive suffix based on whether the name ends is s or not
        string username = sessionManager.GetUsername();
        string suffix = username.ToLower().Last() == 's' ? "'" : "'s";
        Debug.Log($"Changing {sessionManager.GetUsername()}{suffix} password to {newPassword}.");

        var jsonData = JsonUtility.ToJson(new PasswordChangeRequest { new_password = newPassword });
        UnityWebRequest wr = new UnityWebRequest(baseUrl + "changepassword.php", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        wr.uploadHandler = new UploadHandlerRaw(bodyRaw);
        wr.downloadHandler = new DownloadHandlerBuffer();
        wr.SetRequestHeader("Content-Type", "application/json");

        yield return wr.SendWebRequest();

        if (wr.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Password not changed: " + wr.error);
        }
        else
        {
            Debug.Log("Response: " + wr.downloadHandler.text);
            OnStopPasswordChange();
        }
    }

    void ShowLoginScreen() 
    {
        loggedInAsText.gameObject.SetActive(true);
        loggedInAsText.text = "Not logged in.";
        loginView.gameObject.SetActive(true);
        passwordField.inputType = TMP_InputField.InputType.Password;

        loginButton.onClick.RemoveListener(OnLoginButtonClicked);
        loginButton.onClick.AddListener(OnLoginButtonClicked);

        createAccountButton.onClick.RemoveListener(OnCreateAccountClicked);
        createAccountButton.onClick.AddListener(OnCreateAccountClicked);
    }

    void OnCreateAccountClicked()
    {
        createAccountView.gameObject.SetActive(true);
        CloseLoginView();
        finaliseCreateAccountButton.onClick.RemoveListener(OnFinaliseAccountClicked);
        finaliseCreateAccountButton.onClick.AddListener (OnFinaliseAccountClicked);
    }

    void OnFinaliseAccountClicked()
    {
        Debug.Log($"Creating new account: {createAccountNameInput} with password: {createAccountPasswordInput}");
        StartCoroutine(CreateAccount(createAccountNameInput.text, createAccountPasswordInput.text));
    }

    void OnLoginButtonClicked()
    {
        Debug.Log("pass: " + passwordField.text);
        StartCoroutine(Login(accountNameField.text, passwordField.text));
    }

    void ActivateLogoutButton() 
    { 
        logoutButton.gameObject.SetActive(true);
        logoutButton.onClick.RemoveListener(OnLogoutClicked);
        logoutButton.onClick.AddListener(OnLogoutClicked);
    }
    void OnLogoutClicked()
    {
        StartCoroutine(Logout());
    }

    // AFTER SUCCESSFUL LOGIN
    void ShowPostLoginContent()
    {
        int accID = sessionManager.GetID();
        selectedAccountID = accID;
        string userName = sessionManager.GetUsername();
        Debug.Log("Account: "+userName + " -- ID: "+accID);
        string loggedInString = $"Logged in as: {userName}\n";
        switch(sessionManager.GetAccountType())
        {
            case 0:
                loggedInString += "(standard user)";
                Debug.Log("Showing regular user view.");
                selectedAccNameText.text = userName;
                StartCoroutine(GetCharacters(accID));
                ActivateDeleteButton();
                break;
            case 1:
                loggedInString += "(administrator)";
                Debug.Log("Showing admin view.");
                ShowAccountList();
                break;
        }
        loggedInAsText.text = loggedInString;
        ActivateChangePasswordButton();
        ActivateLogoutButton();
    }

    void ShowAccountList()
    {
        accButtonParent.gameObject.SetActive(true);
        //selectAccText.gameObject.SetActive(true);
        //accButtons.ForEach(button => { button.gameObject.SetActive(true); });

        startAppButton.gameObject.SetActive(false);
        StartCoroutine(GetAccounts());
    }

    void CloseLoginView()
    {
        loginView.gameObject.SetActive(false);
    }

    /// <summary>
    /// Closes account creation window and shows login screen
    /// </summary>
    void CloseCreateAccountView()
    {
        createAccountView.gameObject.SetActive(false);
        ShowLoginScreen();
    }

    void ActivateReturnToAccListButton()
    {
        returnToAccListButton.gameObject.SetActive(true);
        returnToAccListButton.onClick.RemoveListener(OnReturnToAccListClicked);
        returnToAccListButton.onClick.AddListener(OnReturnToAccListClicked);
    }

    void OnReturnToAccListClicked()
    {
        ShowAccountList();
        selectedAccAndCharsTexts.gameObject.gameObject.SetActive(false);
        deleteCharNameInput.text = string.Empty;
        deleteCharView.gameObject.SetActive(false);
        returnToAccListButton.gameObject.SetActive(false);
    }

    void ActivateDeleteButton()
    {
        deleteCharView.gameObject.SetActive(true);
        deleteCharButton.onClick.RemoveListener(OnDeleteCharClicked);
        deleteCharButton.onClick.AddListener(OnDeleteCharClicked);
    }

    void OnDeleteCharClicked()
    {
        StartCoroutine(DeleteCharacter(selectedAccountID, deleteCharNameInput.text));
        deleteCharNameInput.text = string.Empty;
    }

    void ActivateChangePasswordButton()
    {
        changePasswordButton.gameObject.SetActive(true);
        changePasswordButton.onClick.AddListener(OnChangePassWordClicked);
    }

    void OnChangePassWordClicked()
    {
        changePassWordParent.gameObject.SetActive(true);
        changePasswordButton.gameObject.SetActive(false);
        ActivateConfirmPasswordAndCancelButtons();
    }

    void ActivateConfirmPasswordAndCancelButtons()
    {
        confirmNewPasswordButton.onClick.RemoveListener(OnConfirmPasswordChangeClicked);
        confirmNewPasswordButton.onClick.AddListener(OnConfirmPasswordChangeClicked);

        cancelPasswordChangeButton.onClick.RemoveListener(OnStopPasswordChange);
        cancelPasswordChangeButton.onClick.AddListener(OnStopPasswordChange);
    }

    void OnConfirmPasswordChangeClicked()
    {
        // if new passwords are not empty strings and match
        if(newPassWordField1.text != string.Empty && newPassWordField1.text == newPassWordField2.text) 
        {
            // actually change password
            StartCoroutine(ChangePassword(newPassWordField1.text));
        }
        else
        {
            Debug.Log("Please enter new password twice.");
        }
    }

    /// <summary>
    /// Activates the big change password button and deactivates other pwd change related fields and button.
    /// This method is called both when a password is successfully changed or when the cancel password change button is pressed.
    /// </summary>
    void OnStopPasswordChange()
    {
        ActivateChangePasswordButton();
        newPassWordField1.text = string.Empty;
        newPassWordField2.text = string.Empty;
        changePassWordParent.gameObject.SetActive(false);
    }


    void HideOtherUIElements()
    {
        accButtonParent.SetActive(false);
        deleteCharView.SetActive(false);
        changePasswordButton.gameObject.SetActive(false);
        selectedAccAndCharsTexts.SetActive(false);

    }
}
