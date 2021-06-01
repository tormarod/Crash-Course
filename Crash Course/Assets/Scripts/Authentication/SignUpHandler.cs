using System;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using EasyUI.Toast;

public class SignUpHandler : MonoBehaviour
{
    public GameObject restartPanel;
    public TMP_InputField emailTextBox;
    public TMP_InputField passwordTextBox;
    public TMP_InputField confirmPasswordTextBox;
    public Button backButton;
    public Button signupButton;
    protected Firebase.Auth.FirebaseAuth auth;
    protected string displayName = "";

    // Start is called before the first frame update
    void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        signupButton.onClick.AddListener(() => canSubmit());
        backButton.onClick.AddListener(() => restartPanel.SetActive(false));
    }

    private void canSubmit()
    {
        if (passwordTextBox.text != confirmPasswordTextBox.text)
        {
            Toast.Show("Passwords do not match", 3f, ToastColor.Black);
        }
        else
        {
            CreateUserWithEmailAsync();
        }
    }

    // Create a user with the email and password.
    public Task CreateUserWithEmailAsync()
    {
        string email = emailTextBox.text;
        string password = passwordTextBox.text;

        Debug.Log(String.Format("Attempting to create user {0}...", email));
        DisableUI();

        // This passes the current displayName through to HandleCreateUserAsync
        // so that it can be passed to UpdateUserProfile().  displayName will be
        // reset by AuthStateChanged() when the new user is created and signed in.
        return auth.CreateUserWithEmailAndPasswordAsync(email, password)
          .ContinueWithOnMainThread((task) => {
              EnableUI();
              LogTaskCompletion(task, "User Creation");
              return task;
          }).Unwrap();
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

    void DisableUI()
    {
        emailTextBox.DeactivateInputField();
        passwordTextBox.DeactivateInputField();
        confirmPasswordTextBox.DeactivateInputField();
        backButton.interactable = false;
        signupButton.interactable = false;
    }

    void EnableUI()
    {
        emailTextBox.ActivateInputField();
        passwordTextBox.ActivateInputField();
        confirmPasswordTextBox.ActivateInputField();
        backButton.interactable = true;
        signupButton.interactable = true;
    }

    // Update the user's display name with the currently selected display name.
    public Task UpdateUserProfileAsync(string newDisplayName = null)
    {
        if (auth.CurrentUser == null)
        {
            Debug.Log("Not signed in, unable to update user profile");
            return Task.FromResult(0);
        }
        displayName = newDisplayName ?? displayName;
        Debug.Log("Updating user profile " + displayName);
        return auth.CurrentUser.UpdateUserProfileAsync(new Firebase.Auth.UserProfile
        {
            DisplayName = displayName,
            PhotoUrl = auth.CurrentUser.PhotoUrl,
        });
    }

    private void GetErrorMessage(AuthError errorCode)
    {
        switch (errorCode)
        {
            case AuthError.MissingPassword:
                Toast.Show("Missing Password", 3f, ToastColor.Black);
                break;
            case AuthError.WeakPassword:
                Toast.Show("Too weak of a password", 3f, ToastColor.Black);
                break;
            case AuthError.InvalidEmail:
                Toast.Show("Invalid email", 3f, ToastColor.Black);
                break;
            case AuthError.MissingEmail:
                Toast.Show("Missing email", 3f, ToastColor.Black);
                break;
            case AuthError.UserNotFound:
                Toast.Show("Account not found", 3f, ToastColor.Black);
                break;
            case AuthError.EmailAlreadyInUse:
                Toast.Show("Email already in use", 3f, ToastColor.Black);
                break;
            default:
                Toast.Show("Unknown error occurred", 3f, ToastColor.Black);
                break;
        }
    }
}