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
        
        private string _adUnitId;

        private void Awake()
        {
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
            _adUnitId = _androidAdUnitId;
#endif
        }

        private void Start()
        {
            LoadAd();
        }

        #endregion

        #region Functions

        public void LoadAd()
        {
            Debug.Log("Loading Ad: " + _adUnitId);
            Advertisement.Load(_adUnitId, this);
        }

        public void OnUnityAdsAdLoaded(string adUnitId)
        {
            Debug.Log("Ad Loaded: " + adUnitId);
        }

        public void ShowAd()
        {
            Advertisement.Show(_adUnitId, this);
        }

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
                
                LoadAd();
            }
        }

        public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
            LoadAd();
        }

        public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
        {
            Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
            LoadAd();
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