using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System.Threading.Tasks;


public class GPGSManager : MonoBehaviour
{
    private PlayGamesClientConfiguration clientConfiguration;
    private string authCode;

    void Start()
    {
        ConfigureGPGS();
        SignIntoGPGS(SignInInteractivity.CanPromptAlways, clientConfiguration);
    }

    public void ConfigureGPGS()
    {
        clientConfiguration = new PlayGamesClientConfiguration.Builder().Build();
    }

    public void SignIntoGPGS(SignInInteractivity interactivity,PlayGamesClientConfiguration configuration)
    {
        PlayGamesPlatform.InitializeInstance(configuration);
        PlayGamesPlatform.Activate();

        PlayGamesPlatform.Instance.Authenticate(interactivity, (result) =>
        {
            if (result == SignInStatus.Success)
            {
                Debug.Log("GPGS 인증 성공");
            }
            else
            {
                Debug.Log("GPGS 인증 실패 :" + result);
            }
        });

       /* Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
              
                authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
                Debug.Log("GPGS 인증 성공 :" + authCode);

                *//*  Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
                  Firebase.Auth.Credential credential =
                      Firebase.Auth.PlayGamesAuthProvider.GetCredential(authCode);
                  auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
                      if (task.IsCanceled)
                      {
                          Debug.LogError("SignInWithCredentialAsync was canceled.");
                          return;
                      }
                      if (task.IsFaulted)
                      {
                          Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                          return;
                      }

                      Firebase.Auth.FirebaseUser newUser = task.Result;
                      Debug.LogFormat("User signed in successfully: {0} ({1})",
                          newUser.DisplayName, newUser.UserId);
                  });*//*
            }
            else
            {
                Debug.Log("GPGS 인증 실패");
            }
        });*/
    }
    
}
