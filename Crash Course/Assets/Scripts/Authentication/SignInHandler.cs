using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using EasyUI.Toast;

public class SignInHandler : MonoBehaviour
{
    public GameObject restartPanel;
    public TMP_InputField emailTextBox;
    public TMP_InputField passwordTextBox;
    public Button signinButton;
    public Button createAccountButton;
    protected Firebase.Auth.FirebaseAuth auth;
    // Whether to sign in / link or reauthentication *and* fetch user profile data.
    protected bool signInAndFetchProfile = true;

    // Start is called before the first frame update
    void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        signinButton.onClick.AddListener(() => SigninWithEmailAsync());
        createAccountButton.onClick.AddListener(() => restartPanel.SetActive(true));
    }

    // Sign-in with an email and password.
    public Task SigninWithEmailAsync()
    {
        var email = emailTextBox.text;
        var password = passwordTextBox.text;
        Debug.Log(String.Format("Attempting to sign in as {0}...", email));
        DisableUI();
        if (signInAndFetchProfile)
        {
            return auth.SignInAndRetrieveDataWithCredentialAsync(
              Firebase.Auth.EmailAuthProvider.GetCredential(email, password)).ContinueWithOnMainThread(
                HandleSignInWithSignInResult);
        }
        else
        {
            return auth.SignInWithEmailAndPasswordAsync(email, password)
              .ContinueWithOnMainThread(HandleSignInWithUser);
        }
    }

    // Called when a sign-in with profile data completes.
    void HandleSignInWithSignInResult(Task<Firebase.Auth.SignInResult> task)
    {
        EnableUI();
        if (LogTaskCompletion(task, "Sign-in"))
        {
            DisplaySignInResult(task.Result, 1);
        }
    }

    // Display user information reported
    protected void DisplaySignInResult(Firebase.Auth.SignInResult result, int indentLevel)
    {
        string indent = new String(' ', indentLevel * 2);
        var metadata = result.Meta;
        if (metadata != null)
        {
            Debug.Log(String.Format("{0}Created: {1}", indent, metadata.CreationTimestamp));
            Debug.Log(String.Format("{0}Last Sign-in: {1}", indent, metadata.LastSignInTimestamp));
        }
        var info = result.Info;
        if (info != null)
        {
            Debug.Log(String.Format("{0}Additional User Info:", indent));
            Debug.Log(String.Format("{0}  User Name: {1}", indent, info.UserName));
            Debug.Log(String.Format("{0}  Provider ID: {1}", indent, info.ProviderId));
            DisplayProfile<string>(info.Profile, indentLevel + 1);
        }
    }

    // Display additional user profile information.
    protected void DisplayProfile<T>(IDictionary<T, object> profile, int indentLevel)
    {
        string indent = new String(' ', indentLevel * 2);
        foreach (var kv in profile)
        {
            var valueDictionary = kv.Value as IDictionary<object, object>;
            if (valueDictionary != null)
            {
                Debug.Log(String.Format("{0}{1}:", indent, kv.Key));
                DisplayProfile<object>(valueDictionary, indentLevel + 1);
            }
            else
            {
                Debug.Log(String.Format("{0}{1}: {2}", indent, kv.Key, kv.Value));
            }
        }
    }

    void DisableUI()
    {
        emailTextBox.DeactivateInputField();
        passwordTextBox.DeactivateInputField();
        signinButton.interactable = false;
        createAccountButton.interactable = false;
    }

    void EnableUI()
    {
        emailTextBox.ActivateInputField();
        passwordTextBox.ActivateInputField();
        signinButton.interactable = true;
        createAccountButton.interactable = true;
    }

    // Log the result of the specified task, returning true if the task
    // completed successfully, false otherwise.
    protected bool LogTaskCompletion(Task task, string operation)
    {
        bool complete = false;
        if (task.IsCanceled)
        {
            Debug.Log(operation + " canceled.");
        }
        else if (task.IsFaulted)
        {
            Debug.Log(operation + " encounted an error.");
            foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
            {
                string authErrorCode = "";
                Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                if (firebaseEx != null)
                {
                    authErrorCode = String.Format("AuthError.{0}: ",
                      ((Firebase.Auth.AuthError)firebaseEx.ErrorCode).ToString());
                    GetErrorMessage((Firebase.Auth.AuthError)firebaseEx.ErrorCode);
                }
                Debug.Log(authErrorCode + exception.ToString());
            }
        }
        else if (task.IsCompleted)
        {
            Debug.Log(operation + " completed");
            complete = true;
        }
        return complete;
    }

    // Called when a sign-in without fetching profile data completes.
    void HandleSignInWithUser(Task<Firebase.Auth.FirebaseUser> task)
    {
        EnableUI();
        if (LogTaskCompletion(task, "Sign-in"))
        {
            Debug.Log(String.Format("{0} signed in", task.Result.DisplayName));
        }
    }

    private void GetErrorMessage(AuthError errorCode)
    {
        switch (errorCode)
        {
            case AuthError.MissingPassword:
                Toast.Show("Missing Password", 3f, ToastColor.Black);
                break;
            case AuthError.WrongPassword:
                Toast.Show("Wrong Password", 3f, ToastColor.Black);
                break;
            case AuthError.InvalidEmail:
                Toast.Show("Invalid Email", 3f, ToastColor.Black);
                break;
            case AuthError.MissingEmail:
                Toast.Show("Missing Email", 3f, ToastColor.Black);
                break;
            case AuthError.UserNotFound:
                Toast.Show("Account not found", 3f, ToastColor.Black);
                break;
            default:
                Toast.Show("Unkown error", 3f, ToastColor.Black);
                break;
        }
    }
}