// examples https://developers.facebook.com/docs/unity/examples
// preview version 
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.IO;

using Facebook.Unity;
using Facebook.MiniJSON;

namespace Mkey
{
    public enum FBState { Free, Login, LoadData }
    public class FBholder : MonoBehaviour
    {
        public static FBholder Instance;
        public static FBState fbState = FBState.Free;
        public bool debugLogin = true;

        public static Action<bool, string> LoginEvent; // (logined, result)
        public static Action<bool, bool, string> LoginPublishEvent;
        public static Action LogoutEvent;

        public string playerID;
        public string playerFirstName;
        public string playerLastName;
        public Sprite playerPhoto;

        // saves last player status, can be used to automatically log in (if (LastSessionLogined) FBlogin())
        public static bool LastSessionLogined
        {
            get
            {
                if (!PlayerPrefs.HasKey("_fblastlogined_"))
                {
                    PlayerPrefs.SetInt("_fblastlogined_", 0);
                }
                return PlayerPrefs.GetInt("_fblastlogined_") > 0;
            }
            set
            {
                PlayerPrefs.SetInt("_fblastlogined_", (value) ? 1 : 0);
            }
        }

        private void Awake()
        {
            if (Instance) Destroy(gameObject);
            else Instance = this;
            Initialize();
        }

        private void Start()
        {
            // add listeners for login event
            LoginEvent += (logined, result) => { if (SettingsMenuController.Instance) SettingsMenuController.Instance.RefreshWindow(); };
            LoginEvent += (logined, result) => { if (logined) BubblesPlayer.Instance.AddFbCoins(); }; // add facebook gift 
            // if (LastSessionLogined) FBlogin();// as options
        }

        #region init
        public void Initialize()
        {
            Debug.Log("FB Initialize");
            if (!FB.IsInitialized)
            {
                FB.Init(() =>
                {
                    if (FB.IsInitialized)
                    {
                        Debug.Log("Facebook SDK is initialized");
                        FB.ActivateApp(); //Signal an app activation App Event
                    }
                    else
                    {
                        Debug.Log("Failed to Initialize Facebook SDK");
                    }

                }, (isUnityShown) =>
                {
                    if (!isUnityShown)
                    {
                        Time.timeScale = 0;// Pause the game - we will need to hide
                    }
                    else
                    {
                        Time.timeScale = 1;// Resume the game - we're getting focus again
                    }

                });
            }
            else
                FB.ActivateApp(); // Already initialized, signal an app activation App Event
        }

        #endregion init

        #region login
        List<string> permissions;
        int loginTryCount = 10;
        public void FBlogin()
        {
            if (debugLogin) Debug.Log("Try facebook login");
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                if (debugLogin) Debug.Log("Error. Check internet connection!");
                return;
            }
            fbState = FBState.Login;
            permissions = new List<string>();
            permissions.Add("public_profile");
            permissions.Add("email");
            permissions.Add("user_friends"); // required appreview
            FB.LogInWithReadPermissions(permissions, (result) =>
            {
                fbState = FBState.Free;
                if (FB.IsLoggedIn)
                {
                    playerID = null;
                    playerFirstName = null;
                    playerLastName = null;
                    playerPhoto = null;

                    if (debugLogin) Debug.Log("facebook is logged in, app token :" + AccessToken.CurrentAccessToken.TokenString);
                    LastSessionLogined = true;
                    LoadAllFBData();
                }
                else
                {
                    if (debugLogin) Debug.Log("facebook is not logged in, loginTryCount : " + loginTryCount);
                    if (result.Error != null)
                    {
                        Debug.Log(result.Error);
                    }
                    if (!result.Cancelled)
                    {
                        if (loginTryCount-- > 0)
                        {
                            FBlogin(); // try next login
                        }
                        else
                        {
                            loginTryCount = 10;
                        }
                    }
                    else
                    {
                        if (debugLogin) Debug.Log("Login cancelled");
                    }
                }

                if (LoginEvent != null) LoginEvent(IsLogined, result.Error);
            });
        }

        public void FBlogOut()
        {
            FB.LogOut();
            LastSessionLogined = false;
            if (LogoutEvent != null) LogoutEvent();
            StartCoroutine(WaitLogOut(() =>
            {
                Debug.Log("IsLogined: " + IsLogined);
            }));
        }

        public void FBlogOut(Action logOutCallBack)
        {
            FB.LogOut();
            LastSessionLogined = false;
            StartCoroutine(WaitLogOut(() =>
            {
                Debug.Log("IsLogined: " + IsLogined);
                if (logOutCallBack != null) logOutCallBack();
            }));

        }

        IEnumerator WaitLogOut(Action logOutCallBack)
        {
            while (IsLogined)
                yield return null;
            if (logOutCallBack != null) logOutCallBack();
        }

        public static bool IsLogined
        {
            get
            {
                return FB.IsLoggedIn;
            }
        }

        /// <summary>
        /// Run sequence to load user profile, apprequest, friends profiles, invitable friends profiles 
        /// </summary>
        private void LoadAllFBData()
        {
            fbState = FBState.LoadData;
            TweenSeq tS = new TweenSeq();

            tS.Add((callBack) =>
            {
                GetPlayerTextInfo(callBack);
            });

            tS.Add((callBack) =>
            {
                GetPlayerPhoto(callBack);
            });

            tS.Add((callBack) =>
            {
                fbState = FBState.Free;
                if (callBack != null) callBack();
            });

            tS.Start();
        }

        #endregion login

        #region player info
        int getPlayerInfoTryCount = 10;

        /// <summary>
        /// Fetch player first name, last name and id, with try count = getPlayerInfoTryCount 
        /// </summary>
        public void GetPlayerTextInfo(Action completeCallBack)
        {
            TweenSeq tS = new TweenSeq();
            for (int i = 0; i < getPlayerInfoTryCount; i++)
            {
                tS.Add((callBack) =>
                {
                    TryGetPlayerTextInfo(callBack);
                });
            }

            tS.Add((callBack) =>
            {
                if (completeCallBack != null) completeCallBack();
            });
            tS.Start();
        }

        /// <summary>
        /// Fetch player first name, id and photo
        /// </summary>
        public void TryGetPlayerTextInfo(Action completeCallBack)
        {
            if (string.IsNullOrEmpty(playerID))
            {
                if (debugLogin) Debug.Log("Try to get player text info...");
                FB.API("/me?fields=first_name,last_name,id,email", HttpMethod.GET,
                    (result) =>
                    {
                        if (result.Error != null)
                        {
                            Debug.Log(result.Error);
                        }
                        else
                        {
                            playerFirstName = (string)result.ResultDictionary["first_name"];
                            playerLastName = (string)result.ResultDictionary["last_name"];
                            playerID = (string)result.ResultDictionary["id"];
                            if (debugLogin) Debug.Log("Player text info received. PlayerName: " + playerFirstName + " " + playerLastName + " ; playerID: " + playerID);

                        }
                        if (completeCallBack != null) completeCallBack();
                    });
            }
            else
            {
                if (completeCallBack != null) completeCallBack();
            }
        }

        /// <summary>
        /// Fetch player first name, id and photo
        /// </summary>
        public void GetPlayerPhoto(Action completeCallBack)
        {
            if (string.IsNullOrEmpty(playerID))
            {
                if (completeCallBack != null) completeCallBack();
                return;
            }

            TweenSeq tS = new TweenSeq();
            for (int i = 0; i < getPlayerInfoTryCount; i++)
            {
                tS.Add((callBack) =>
                {
                    TryGetPlayerPhoto(callBack);
                });
            }

            tS.Add((callBack) =>
            {
                if (completeCallBack != null) completeCallBack();
            });
            tS.Start();
        }

        /// <summary>
        /// Fetch player first name, id and photo
        /// </summary>
        public void TryGetPlayerPhoto(Action completeCallBack)
        {
            if (playerPhoto == null)
            {
                if (debugLogin) Debug.Log("Try to get player photo...");
                FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, (result) =>
                {
                    if (result.Texture != null)
                    {
                        if (debugLogin) Debug.Log("Player photo received..");
                        playerPhoto = Sprite.Create(result.Texture, new Rect(0, 0, result.Texture.width, result.Texture.height), new Vector2(0.5f, 0.5f));
                    }
                    else
                    {
                        Debug.Log("NO player photo, new query: ....");
                    }
                    if (completeCallBack != null) completeCallBack();
                });
            }
            else
            {
                if (completeCallBack != null) completeCallBack();
            }
        }

        #endregion player info

        #region app link
        public void GetAppLink()
        {
            if (Constants.IsMobile)
            {
                FB.Mobile.FetchDeferredAppLinkData(this.GetAppLinkCallBack);
                return;
            }
            FB.GetAppLink(this.GetAppLinkCallBack);
        }

        protected void GetAppLinkCallBack(IResult result)
        {
            string LastResponse = string.Empty;
            string Status = string.Empty;
            if (result == null)
            {
                LastResponse = "Null Response\n";
                Debug.Log(LastResponse);
                return;
            }

            if (!string.IsNullOrEmpty(result.Error))
            {
                Status = "Error - Check log for details";
                LastResponse = "Error Response:\n" + result.Error;
            }
            else if (result.Cancelled)
            {
                Status = "Cancelled - Check log for details";
                LastResponse = "Cancelled Response:\n" + result.RawResult;
            }
            else if (!string.IsNullOrEmpty(result.RawResult))
            {
                Status = "Success - Check log for details";
                LastResponse = "Success Response:\n" + result.RawResult;
            }
            else
            {
                LastResponse = "Empty Response\n";
            }

            Debug.Log(result.ToString());
        }
        #endregion app link

    }

}