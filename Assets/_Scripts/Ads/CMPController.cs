using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Firebase;
using _Scripts.UI;
using GoogleMobileAds.Api;
using GoogleMobileAds.Ump.Api;
using UnityEngine;

namespace _Scripts.Ads
{
    public class CMPController : MonoBehaviour
    {
        private bool isInitAds = false;

        void Awake()
        {
            isInitAds = true;
            MobileAds.SetiOSAppPauseOnBackground(true);
            MobileAds.RaiseAdEventsOnUnityMainThread = true;
#if UNITY_IOS
            AudienceNetwork.AdSettings.SetAdvertiserTrackingEnabled(true);
#endif
        }


        private void OnEnable()
        {


            int startCMP = ManagerFirebase.Instance.firebaseInitial.cmp_begin;
            int distanceCMP = ManagerFirebase.Instance.firebaseInitial.cmp_distance;
            //Display_CMP();
            //StarCMP_Admod();
            
            int getcurrentLevel = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL, 1);
            if (IsCMPConsent())
            {
                //StarCMP();
                MaxAdsManager.Instance?.InitAds();
                return;
            }

            if (getcurrentLevel == 1)
            {


                StarCMP();

            }
            else if ((getcurrentLevel - startCMP) >= 0 && (getcurrentLevel - startCMP) % distanceCMP == 0)
            {

                StarCMP();
            }
            else
            {
                MaxAdsManager.Instance?.InitAds();
            }


        }
        
        void StarCMP()
        {
            
            
            
            
            
            if (IsCMPConsent())
            {
                SetConsent(true);
                MaxAdsManager.Instance?.InitAds();
            }
            else
            {
                SetConsent(false);
                ShowReplyCMP();
            }
        }
        void SetConsent(bool state)
        {
            //IronSource.Agent.setConsent(state);
            //PlayOnSDK.SetGdprConsent(state);
            MaxSdk.SetHasUserConsent(state);
        }
    
        


        void ShowReplyCMP()
        {
            ConsentInformation.Reset();
            ConsentRequestParameters request = new ConsentRequestParameters
            {
                TagForUnderAgeOfConsent = false,
                
                
                // For testing purposes, set the debug geography to EEA.
              
            };
          
      
            ConsentInformation.Update(request, OnConsentInfoUpdated);
            
        }
        
        
        void OnConsentInfoUpdated(FormError consentError)
        {
        if (consentError != null)//Loi
        {
            // Handle the error.
          

            Debug.LogError("CMP: Update: " + consentError);
            MaxAdsManager.Instance?.InitAds();
            return;
        }

        ConsentForm.Load((ConsentForm consentForm, FormError formError) =>
        {
           
            if (formError != null)//Loi
            {
                MaxAdsManager.Instance?.InitAds();
                return;
            }

            //Continue to show CMP
          
         
            consentForm.Show((FormError formError) =>
            {
                
                if (formError != null)
                {
                    // Consent gathering failed.
               
                    //SetConsent(false);
                    MaxAdsManager.Instance?.InitAds();
                    return;
                }

                // Consent has been gathered.
                if (ConsentInformation.CanRequestAds())
                {
                    if (IsCMPConsent())
                    {
                        //Request an ad.
                     
                        SetConsent(true);
                        MaxAdsManager.Instance?.InitAds();
                    }
                    else
                    {
                        SetConsent(false);
                        MaxAdsManager.Instance?.InitAds();
                    }
                }
                else
                {
                    SetConsent(false);
                    MaxAdsManager.Instance?.InitAds();
                }
            });
            }); 
        }
        

        void Display_CMP(FormError consentError)
        {



            ConsentForm.LoadAndShowConsentFormIfRequired((FormError loadAndShowError) =>
            {
                if (loadAndShowError != null)
                {
                    // Xử lý lỗi khi hiển thị form
                    Debug.LogError("Error loading or showing consent form: " + loadAndShowError.Message);
                }

                // Khi form đã được xử lý (người dùng đã chọn)
                if (ConsentInformation.ConsentStatus == ConsentStatus.Obtained)
                {
                    // Người dùng đã đồng ý, khởi tạo Mobile Ads SDK
                    MaxSdk.SetHasUserConsent(true);
                    MaxAdsManager.Instance?.InitAds();
                    //InitializeAdMob();
                }
                else
                {
                    MaxSdk.SetHasUserConsent(false);
                    MaxAdsManager.Instance?.InitAds();
                }
            });
            MaxAdsManager.Instance?.InitAds();
        }





        #region CMP

        public static int GetInt(string key, int defaultValue)
        {
            return GetValue<int>(key, defaultValue, "getInt");
        }

        public static float GetFloat(string key, float defaultValue)
        {
            return GetValue<float>(key, defaultValue, "getFloat");
        }

        public static string GetString(string key, string defaultValue)
        {
#if UNITY_IPHONE
            return PlayerPrefs.GetString(key, "");
#elif UNITY_ANDROID
            return GetValue<string>(key, defaultValue, "getString");
#endif
        }

        private static T GetValue<T>(string key, T defaultValue, string methodName)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                    {
                        string packageName = activity.Call<string>("getPackageName");
                        using (var prefs = activity.Call<AndroidJavaObject>("getSharedPreferences",
                                   packageName + "_preferences", 0))
                        {
                            return prefs.Call<T>(methodName, key, defaultValue);
                        }
                    }
                }
            }

            return defaultValue;
        }

        // Number:
        // 1 - GDPR applies in current context
        // 0 - GDPR does not apply in current context
        // Unset - undetermined (default before initialization)
        public static int GetGDPRApplicability()
        {
            return GetInt("IABTCF_gdprApplies", -1); // Default to -1 (undetermined) if not set
        }

        /// <summary>
        /// Check if user accepted consent
        /// </summary>
        bool IsCMPConsent()
        {
            //DebugCustom.LogError(() => "isCMPConsent CanShowAds: " + CanShowAds() + " ----- CanShowPersonalizedAds: " + CanShowPersonalizedAds());
            if (CanShowAds() || CanShowPersonalizedAds())
            {
                string CMPString = GetString("IABTCF_AddtlConsent", "NO");

                if (CMPString != null)
                {
                    if (CMPString.Contains("2878"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private const string PURPOSE_CONSENT_KEY = "IABTCF_PurposeConsents";
        private const string VENDOR_CONSENT_KEY = "IABTCF_VendorConsents";
        private const string VENDOR_LI_KEY = "IABTCF_VendorLegitimateInterests";
        private const string PURPOSE_LI_KEY = "IABTCF_PurposeLegitimateInterests";

        bool CanShowAds()
        {
            string purposeConsent = "";
            string vendorConsent = "";
            string vendorLI = "";
            string purposeLI = "";
#if UNITY_IPHONE
        purposeConsent = PlayerPrefs.GetString(PURPOSE_CONSENT_KEY, "");
        vendorConsent = PlayerPrefs.GetString(VENDOR_CONSENT_KEY, "");
        vendorLI = PlayerPrefs.GetString(VENDOR_LI_KEY, "");
        purposeLI = PlayerPrefs.GetString(PURPOSE_LI_KEY, "");
#elif UNITY_ANDROID
            purposeConsent = GT.Utils.PlayerPrefsNative.GetString(PURPOSE_CONSENT_KEY, "");
            vendorConsent = GT.Utils.PlayerPrefsNative.GetString(VENDOR_CONSENT_KEY, "");
            vendorLI = GT.Utils.PlayerPrefsNative.GetString(VENDOR_LI_KEY, "");
            purposeLI = GT.Utils.PlayerPrefsNative.GetString(PURPOSE_LI_KEY, "");
#endif

            int googleId = 755;
            bool hasGoogleVendorConsent = HasAttribute(vendorConsent, googleId);
            bool hasGoogleVendorLI = HasAttribute(vendorLI, googleId);

            // Minimum required for at least non-personalized ads
            return HasConsentFor(new List<int> { 1 }, purposeConsent, hasGoogleVendorConsent)
                   && HasConsentOrLegitimateInterestFor(new List<int> { 2, 7, 9, 10 }, purposeConsent, purposeLI, hasGoogleVendorConsent, hasGoogleVendorLI);

        }

        // Check if personalized ads can be shown
        bool CanShowPersonalizedAds()
        {
            string purposeConsent = "";
            string vendorConsent = "";
            string vendorLI = "";
            string purposeLI = "";

#if UNITY_IPHONE
        purposeConsent = PlayerPrefs.GetString(PURPOSE_CONSENT_KEY, "");
        vendorConsent = PlayerPrefs.GetString(VENDOR_CONSENT_KEY, "");
        vendorLI = PlayerPrefs.GetString(VENDOR_LI_KEY, "");
        purposeLI = PlayerPrefs.GetString(PURPOSE_LI_KEY, "");
#elif UNITY_ANDROID
            purposeConsent = PlayerPrefs.GetString(PURPOSE_CONSENT_KEY, "");
            vendorConsent = PlayerPrefs.GetString(VENDOR_CONSENT_KEY, "");
            vendorLI = PlayerPrefs.GetString(VENDOR_LI_KEY, "");
            purposeLI = PlayerPrefs.GetString(PURPOSE_LI_KEY, "");
#endif
            int googleId = 755;
            bool hasGoogleVendorConsent = HasAttribute(vendorConsent, googleId);
            bool hasGoogleVendorLI = HasAttribute(vendorLI, googleId);

            return HasConsentFor(new List<int> { 1, 3, 4 }, purposeConsent, hasGoogleVendorConsent)
                   && HasConsentOrLegitimateInterestFor(new List<int> { 2, 7, 9, 10 }, purposeConsent, purposeLI, hasGoogleVendorConsent, hasGoogleVendorLI);
        }


        

        // Check if a binary string has a "1" at position "index" (1-based)
        bool HasAttribute(string input, int index)
        {
            return input.Length >= index && input[index - 1] == '1';
        }

        // Check if consent is given for a list of purposes
        bool HasConsentFor(List<int> purposes, string purposeConsent, bool hasVendorConsent)
        {
            return purposes.All(p => HasAttribute(purposeConsent, p)) && hasVendorConsent;
        }

        // Check if a vendor either has consent or legitimate interest for a list of purposes
        bool HasConsentOrLegitimateInterestFor(List<int> purposes, string purposeConsent, string purposeLI,
            bool hasVendorConsent, bool hasVendorLI)
        {
            return purposes.All(p =>
                (HasAttribute(purposeLI, p) && hasVendorLI) ||
                (HasAttribute(purposeConsent, p) && hasVendorConsent)
            );
        }

        #endregion
    }

}