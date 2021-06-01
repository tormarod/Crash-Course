using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using EasyUI.Toast;
using UnityEngine.SceneManagement;

public class FirebaseAuth : MonoBehaviour
{
    [SerializeField] InputField email;
    [SerializeField] InputField password;
    [SerializeField] InputField emailSignUp;
    [SerializeField] InputField passwordSignUp;
    [SerializeField] InputField passwordRepeatSignUp;
    Firebase.Auth.FirebaseAuth auth;
    public GameObject restartPanel;
    // Start is called before the first frame update
    void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    public void Signup()
    {
        if (emailSignUp.text.Equals(""))
        {
            Toast.Show("Enter an email", 3f, ToastColor.Black);
            emailSignUp.Select();
        }
        else if (passwordSignUp.text.Equals(""))
        {
            Toast.Show("Enter a password", 3f, ToastColor.Black);
        }
        else if (!passwordSignUp.text.Equals(passwordRepeatSignUp.text))
        {
            Toast.Show("Passwords must coincide", 3f, ToastColor.Black);
            Debug.Log(emailSignUp.text);
            Debug.Log(passwordSignUp);
            Debug.Log(passwordRepeatSignUp);
            Debug.Log(passwordSignUp.ToString());
            Debug.Log(passwordRepeatSignUp.ToString());
            passwordSignUp.text = "";
            passwordRepeatSignUp.text = "";
            passwordSignUp.Select();
        }
        else
        {
            auth.CreateUserWithEmailAndPasswordAsync(emailSignUp.text, passwordSignUp.text).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled");
                    Toast.Show("Canceled sign up", 3f, ToastColor.Black);
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error" + task.Exception);
                    Toast.Show("Error creating user", 3f, ToastColor.Black);
                    return;
                }
                FirebaseUser newUser = task.Result;
                Debug.LogFormat("Firebase User was created successfully :{0} ({1})", newUser.DisplayName, newUser.UserId);
            });
            Toast.Show("User created successfully", 3f, ToastColor.Black);
            restartPanel.SetActive(false);
        }
    }
    public void CreateAccount()
    {
        restartPanel.SetActive(true);
    }

    public void GoBackToLogIn()
    {
        restartPanel.SetActive(false);
    }
    public void SignIn()
    {
        auth.SignInWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled");
                Toast.Show("Cancelled sign in", 3f, ToastColor.Black);
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error" + task.Exception);
                Toast.Show("Error signing in", 3f, ToastColor.Black);
                return;
            }
            if (task.IsCompleted)
            {
                FirebaseUser returnUser = task.Result;
                Debug.LogFormat("Firebase User was signed in successfully :{0} ({1})", returnUser.DisplayName, returnUser.UserId);
                return;
            }
        });
    }
}
