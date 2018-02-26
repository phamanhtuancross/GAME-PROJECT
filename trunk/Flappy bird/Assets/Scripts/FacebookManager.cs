using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using System.Linq;

public class FacebookManager : MonoBehaviour {

    public Text PopUpText;
    public Color PopUpColor;
    public Button LoginButton, ShareButton, InviteButton, RestartButton, SaveRecordButtton;
    public Sprite FBsprite;
    private string username = "";

    private void Awake()
    {
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            StartCoroutine(PopUpMessage("Not Init yet."));
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
            StartCoroutine(PopUpMessage("Already Init."));
        }
    }
    
    //facebook
    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...

            if (FB.IsLoggedIn)
            {
                StartCoroutine(PopUpMessage("Init complete. Already logged in"));

                GetUserName();

                InviteButton.gameObject.SetActive(true);
            }
            else
            {
                StartCoroutine(PopUpMessage("Init complete. Not logged in"));

                InviteButton.gameObject.SetActive(false);
            }
        }
        else
        {
            StartCoroutine(PopUpMessage("Failed to Initialize the Facebook SDK"));
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    public void LoginFBWithReadPerms()
    {
        if (!FB.IsLoggedIn)
        {
            var readPerms = new List<string>() { "public_profile", "user_friends" };
            FB.LogInWithReadPermissions(readPerms, LoginCallback);

            GetUserName();
        }
        else
        {
            UIManager.Instance.ShowPanelLogout(true);
        }
    }

    public void LogOut()
    {
        FB.LogOut();
        
        StartCoroutine(PopUpMessage("Logout success"));

        Image profilePic = LoginButton.GetComponent<Image>();
        profilePic.sprite = FBsprite;

        UIManager.Instance.ShowPanelLogout(false);

        InviteButton.gameObject.SetActive(false);
    }

    private void GetUserName()
    {
        FB.API("/me?fields=first_name", HttpMethod.GET, GetUsernameCallBack);
        FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, GetProfilePicCallBack);
    }

    private void GetProfilePicCallBack(IGraphResult result)
    {
        if (result.Texture != null)
        {
            Image profilePic = LoginButton.GetComponent<Image>();
            profilePic.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2());
        }
        else
        {
            StartCoroutine(PopUpMessage("Can't get picture profile"));
        }
    }

    private void LoginFBWithPublishPerms()
    {
        var publishPerms = new List<string>() { "publish_actions" };
        FB.LogInWithPublishPermissions(publishPerms, LoginCallback);
    }

    private void LoginCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            StartCoroutine(PopUpMessage("Login success"));

            GetUserName();

            InviteButton.gameObject.SetActive(true);
        }
        else
        {
            StartCoroutine(PopUpMessage("User cancelled login"));
        }
    }

    private void GetUsernameCallBack(IGraphResult result)
    {
        if (result.Cancelled || !String.IsNullOrEmpty(result.Error))
        {
            StartCoroutine(PopUpMessage("Can't get username " + result.Error));
        }
        else
        {
            username = "Hi, " + result.ResultDictionary["first_name"];

            StartCoroutine(PopUpMessage(username));
        }
    }

    public void ShareFaceBook()
    {
        if (FB.IsLoggedIn)
        {
            ShareButton.interactable = RestartButton.interactable = SaveRecordButtton.interactable = false;

            if (!AccessToken.CurrentAccessToken.Permissions.Contains("publish_actions"))
            {
                LoginFBWithPublishPerms();
            }

            StartCoroutine(TakeScreenshot());
        }
        else
        {
            StartCoroutine(PopUpMessage("Please login to facebook first!"));
        }
    }

    private IEnumerator TakeScreenshot()
    {
        yield return new WaitForEndOfFrame();

        var width = Screen.width;
        var height = Screen.height;
        var tex = new Texture2D(width, height, TextureFormat.RGB24, false);

        // Read screen contents into the texture
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();
        byte[] screenshot = tex.EncodeToPNG();

        var wwwForm = new WWWForm();
        wwwForm.AddBinaryData("image", screenshot, "InteractiveConsole.png");
        wwwForm.AddField("message", "My B2SFresher highscore: " + ScoreManager.Instance.GetHighestScore());
        FB.API("me/photos", HttpMethod.POST, ShareScoreCallback, wwwForm);
    }

    private void ShareScoreCallback(IGraphResult result)
    {
        if (result.Cancelled || !String.IsNullOrEmpty(result.Error))
        {
            StartCoroutine(PopUpMessage("Share api call Error: " + result.Error));
        }
        else
        {
            StartCoroutine(PopUpMessage("Share api call success!"));
        }

        ShareButton.interactable = RestartButton.interactable = SaveRecordButtton.interactable = true;
    }
    
    public void InviteFriend()
    {
        //FB.Mobile.AppInvite(
        //    new Uri("https://fb.me/810530068992919"),
        //    new Uri("https://lh3.googleusercontent.com/jUTSGh-s6orLS0ghhYQWnIiHzMyknSuh9oE66bdCvbyrrYtl22BPJ6aK3vt1jIjE8Q=w300-rw"),
        //    AppInviteCallback
        //);

        FB.ShareLink(
            new Uri("https://play.google.com/store/apps/details?id=com.ntvv.nongtraivuive"),
            callback: ShareCallback
        );
    }

    private void ShareCallback(IShareResult result)
    {
        if (result.Cancelled || !String.IsNullOrEmpty(result.Error))
        {
            PopUpText.text = "Invite Error: " + result.Error;
            StartCoroutine(PopUpMessage("Invite Error: " + result.Error));
        }
        else
        {
            // Share succeeded without postID
            PopUpText.text = "Invite success!";
            StartCoroutine(PopUpMessage("Invite success!"));
        }
    }

    private IEnumerator PopUpMessage(string message)
    {
        PopUpText.color = Color.clear;
        PopUpText.text = message;

        float time = 0;        

        while (time < 2f)
        {
            PopUpText.color = Color.Lerp(PopUpText.color, PopUpColor, Time.deltaTime * 2f);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1f);
        time = 0;

        while (time < 2f)
        {
            PopUpText.color = Color.Lerp(PopUpText.color, Color.clear, Time.deltaTime * 2f);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        PopUpText.text = "";
    }    
}
