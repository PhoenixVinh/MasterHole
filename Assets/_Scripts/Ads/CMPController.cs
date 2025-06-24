using System;
using System.Collections.Generic;
using System.Linq;
using GoogleMobileAds.Api;
using GoogleMobileAds.Ump.Api;
using UnityEngine;

namespace _Scripts.Ads
{
    public class CMPController : MonoBehaviour
    {
        private bool isInitAds = false;
        void Awake() {
            isInitAds = true;
            MobileAds.SetiOSAppPauseOnBackground(true);
            MobileAds.RaiseAdEventsOnUnityMainThread = true;
        #if UNITY_IOS
            AudienceNetwork.AdSettings.SetAdvertiserTrackingEnabled(true);
        #endif
        }


        private void Start()
        {

            Display_CMP();
            //StarCMP_Admod();
        }
        
        void StarCMP_Admod()
        {

            Debug.LogError("CMP: START :isCMPConsent: " + isCMPConsent());
            // InitLoadAds(true);
            if (isCMPConsent())
            {
                 //Debug.Lo
            }
            else
            {
                ResetCMP_Admod();
            }

        }


        void Display_CMP()
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
                    Debug.Log("Consent obtained. Initializing Mobile Ads SDK.");
                    //InitializeAdMob();
                }
                else
                {
                    // Người dùng đã từ chối hoặc chọn Manage Options, 
                    // bạn cần xử lý cho các trường hợp không có quảng cáo cá nhân hóa
                    Debug.Log("Consent not obtained or denied. Ads may be limited.");
                    //InitializeAdMob(); // Vẫn khởi tạo để hiển thị quảng cáo không cá nhân hóa
                }
            });}
    


        bool isCMPConsent()
        {
            Debug.LogError("isCMPConsent CanShowAds: " + CanShowAds() + " ----- CanShowPersonalizedAds: " + CanShowPersonalizedAds());
            if (CanShowAds() || CanShowPersonalizedAds())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        

        
        void OnConsentInfoUpdated(FormError consentError)
        {
            if (consentError != null)
            {
                // Thêm phần tiếp tục loading game nhé

                // Handle the error.
                // InitLoadAds(true);
                // InitLoadingAds();
                // ShowAdIfAvailable();
                Debug.LogError(consentError);
                return;
            }
            // If the error is null, the consent information state was updated.
            // You are now ready to check if a form is available.
            ConsentForm.LoadAndShowConsentFormIfRequired((FormError formError) =>
            {
                Debug.LogError("CMP: START 02 : " + "false");
                if (formError != null)
                {
                    // Thêm phần tiếp tục loading game nhé

                    // Consent gathering failed.
                    // InitLoadAds(true);
                    // InitLoadingAds();
                    // ShowAdIfAvailable();

                    UnityEngine.Debug.LogError(consentError);
                    return;
                }

                // Consent has been gathered.
                if (ConsentInformation.CanRequestAds()) // Ham nay chi dam bao la da hoan thanh dc CMP
                {
                    Debug.LogError("CMP: START 03 : " + isCMPConsent());
                    if (isCMPConsent())
                    {
                        // Thêm phần tiếp tục loading game nhé

                        //Request an ad.
                        // InitLoadAds(true);
                        // InitLoadingAds();
                        // ShowAdIfAvailable();
                    }
                    else
                    {
                   
                        // Thêm phần tiếp tục loading game nhé

                        // InitLoadAds(false);
                        // InitLoadingAds();
                        // ShowAdIfAvailable();
                    }
                }
                else
                {
                    // Thêm phần tiếp tục loading game nhé

                    // InitLoadAds(false);
                    //
                    // InitLoadingAds();
                    // ShowAdIfAvailable();
                }
            });

        }

        
        
        
        
        
        
        
        
        

        private const string PURPOSE_CONSENT_KEY = "IABTCF_PurposeConsents";
        private const string VENDOR_CONSENT_KEY = "IABTCF_VendorConsents";
        private const string VENDOR_LI_KEY = "IABTCF_VendorLegitimateInterests";
        private const string PURPOSE_LI_KEY = "IABTCF_PurposeLegitimateInterests";
        
        
        
        
        
        
        // Check if ads can be shown
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
        bool HasConsentOrLegitimateInterestFor(List<int> purposes, string purposeConsent, string purposeLI, bool hasVendorConsent, bool hasVendorLI)
        {

            return purposes.All(p =>
                (HasAttribute(purposeLI, p) && hasVendorLI) ||
                (HasAttribute(purposeConsent, p) && hasVendorConsent)
            );
        }
        public void ResetCMP_Admod()
        {
            ConsentInformation.Reset();
            ConsentRequestParameters request = new ConsentRequestParameters
            {
                TagForUnderAgeOfConsent = false,
            };
            ConsentInformation.Update(request, OnConsentInfoUpdated);
        }

        




    }
}