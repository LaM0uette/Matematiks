using Game.Modules.Utils;
using Obvious.Soap;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Game.Modules.Ads
{
    public class RewardedAdsGem : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        #region Statements

        [SerializeField] private ScriptableEventInt _shopGemEvent;
        [SerializeField] private string _androidAdUnitId = "Rewarded_Android";
        [SerializeField] private string _iOSAdUnitId = "Rewarded_iOS";
        
        private string _adUnitId = null; // This will remain null for unsupported platforms

        private void Awake()
        {
            // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
            _adUnitId = _androidAdUnitId;
#endif
        }

        #endregion

        #region Functions

        // Call this public method when you want to get an ad ready to show.
        public void LoadAd()
        {
            // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
            Debug.Log("Loading Ad: " + _adUnitId);
            Advertisement.Load(_adUnitId, this);
        }

        // If the ad successfully loads
        public void OnUnityAdsAdLoaded(string adUnitId)
        {
            Debug.Log("Ad Loaded: " + adUnitId);
        }

        // Implement a method to execute when the user clicks the button:
        public void ShowAd()
        {
            // Show the ad:
            Advertisement.Show(_adUnitId, this);
        }

        // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
        public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
        {
            if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                const int reward = 150;
                
                var gem = Saver.Gem.LoadInt();
                gem += reward;
                Saver.Gem.Save(gem);
                
                _shopGemEvent.Raise(reward);
                Debug.Log("Unity Ads Rewarded Ad Completed");
            }
        }

        // Implement Load and Show Listener error callbacks:
        public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
            // Use the error details to determine whether to try to load another ad.
        }

        public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
        {
            Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
            // Use the error details to determine whether to try to load another ad.
        }

        public void OnUnityAdsShowStart(string adUnitId)
        {
        }

        public void OnUnityAdsShowClick(string adUnitId)
        {
        }

        #endregion
    }
}