using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Xml.Serialization;

public class SessionManager : MonoBehaviour
{
    // Class-level variables to store session data
    private int id;
    private string username;
    private int accountType;

    private bool sessionInfoFetched = false; // Whether session info has been fetched or not

    public static SessionManager instance { get; private set; }

    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method to initiate the session check
    IEnumerator GetSessionInfo()
    {
        UnityWebRequest wr = UnityWebRequest.Get("http://localhost/testapi/accinfo.php");
        yield return wr.SendWebRequest();

        if (wr.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Session check failed: " + wr.error);
        }
        else
        {
            Debug.Log("Session info: " + wr.downloadHandler.text);

            // Parse JSON response into a class (SessionInfo)
            var jsonResponse = JsonUtility.FromJson<SessionInfo>(wr.downloadHandler.text);

            // Store the data in class-level variables
            id = jsonResponse.id;
            username = jsonResponse.username;
            accountType = jsonResponse.accountType;

            sessionInfoFetched = true;

            Debug.Log("Logged in as: " + username);
            Debug.Log("Account Type: " + accountType);
        }
    }

    // Class to map the JSON response
    [System.Serializable]
    public class SessionInfo
    {
        public int id;
        public string username;
        public int accountType;
    }

    // Method to get user id
    public int GetID()
    {
        return id;
    }

    // Method to get username
    public string GetUsername()
    {
        return username;
    }

    // Method to get account type
    public int GetAccountType()
    {
        return accountType;
    }

    // Example method that uses the values outside the coroutine
    public void DisplayUserInfo()
    {
        Debug.Log("Username: " + username);
        Debug.Log("Account Type: " + accountType);
    }

    // Call this in another method to check if the user is logged in
    public void CheckSession()
    {
        StartCoroutine(GetSessionInfo());
    }

    public bool IsSessionInfoReady()
    {
        return sessionInfoFetched;
    }

    public void ClearSession()
    {
        sessionInfoFetched = false;
        id = default;
        username = default;
        accountType = default;
    }
}
