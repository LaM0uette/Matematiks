using Game.Modules.Board;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Modules.Manager
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

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
        
        public void PlayPopSound() => PlaySound(PopClip, 0.15f);
        public void PlayBonus01Sound() => PlaySound(Bonus01Clip, 0.2f);
        public void PlayBonus02Sound() => PlaySound(Bonus02Clip, 0.4f);
        public void PlayBonus03Sound() => PlaySound(Bonus03Clip, 0.4f);
        public void PlayBonus04Sound() => PlaySound(Bonus04Clip, 0.3f);
        
        public void PlayBallSound(int index)
        {
            if (index >= 0 && index < BallClips.Length)
            {
                var clip = BallClips[index];
                PlaySound(clip, 0.06f);
            }
            else
            {
                Debug.LogError("Index hors de la plage des clips audio disponibles.");
            }
        }

        private void PlaySound(AudioClip clip, float volume)
        {
            if (BoardHandler.VolumeIsMute) 
                return;
            
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.volume = volume;
            
            audioSource.clip = clip;
            audioSource.Play();
            
            Destroy(audioSource, clip.length);
        }
    }
}
