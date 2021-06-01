using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using EasyUI.Toast;


public class PlayGamesServices : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }
    void Initialize()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
    .RequestServerAuthCode(false /* Don't force refresh */)
    .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
    }
    
    public void SignInWithPlayGames()
    {
        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, (success) =>
         {
             switch(success)
             {
                 case SignInStatus.Success:
                     Debug.Log("Logged in Successfully");
                     SceneManager.LoadScene("Game");
                     break;
                 default:
                     Toast.Show("Google Play login failed", 3f, ToastColor.Black);
                     Debug.Log("Logged in failed");
                     break;

             }
         });
        Social.localUser.Authenticate((bool success) =>
        {
            // handle success or failure
            if (success)
            {
                Debug.Log("Unity social success");
            }
            else
            {
                Debug.Log("Unity social failed");
            }
        });
    }
}
