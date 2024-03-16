using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Modules.Manager
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }
        
        [Space, Title("Properties")]
        [SerializeField] private float _defaultVolume = 0.3f;

        [Space, Title("Clips")] 
        public AudioClip PopClip;
        public AudioClip Bonus01Clip;
        public AudioClip Bonus02Clip;
        public AudioClip Bonus03Clip;
        public AudioClip Bonus04Clip;
        public AudioClip[] BallClips;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }
        
        public void PlayPopSound() => PlaySound(PopClip);
        public void PlayBonus01Sound() => PlaySound(Bonus01Clip);
        public void PlayBonus02Sound() => PlaySound(Bonus02Clip);
        public void PlayBonus03Sound() => PlaySound(Bonus03Clip);
        public void PlayBonus04Sound() => PlaySound(Bonus04Clip);
        
        public void PlayBallSound(int index)
        {
            if (index >= 0 && index < BallClips.Length)
            {
                var clip = BallClips[index];
                PlaySound(clip);
            }
            else
            {
                Debug.LogError("Index hors de la plage des clips audio disponibles.");
            }
        }

        private void PlaySound(AudioClip clip)
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.volume = _defaultVolume;
            
            audioSource.clip = clip;
            audioSource.Play();
            
            Destroy(audioSource, clip.length);
        }
    }
}
