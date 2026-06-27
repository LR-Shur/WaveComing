using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameStartStudio
{
    public class SoundManager : MonoSingleton<SoundManager>
    {
       
        // Music
        private AudioSource musicSource;
        private float musicVolume = 1.0f;

        // Sound
        private AudioSource[] soundSources;

        private float soundVolume = 1.0f;
        private int soundSourceIndex;
        protected override void Awake()
        {
            base.Awake();
            //Music
            GameObject newMusicSource = new GameObject { name = "Music Source" };
            musicSource = newMusicSource.AddComponent<AudioSource>();
            newMusicSource.transform.SetParent(transform);

            // Sound
            soundSources = new AudioSource[7];
            for (int i = 0; i < soundSources.Length; i++)
            {
                GameObject newSoundSource = new GameObject{name = $"Sound Source {i+1}" };
                soundSources[i] = newSoundSource.AddComponent<AudioSource>();
                newSoundSource.transform.SetParent(transform);
            }

           
        }

        

        #region  Music

        public void PlayMusic(string musicName)
        {
            ResourceManager.Instance.LoadAsync<AudioClip>($"Music/{musicName}", clip => 
            {
                musicSource.clip = clip;
                musicSource.loop = true;
                musicSource.volume = musicVolume;
                musicSource.Play();
            }
            );
        }

        public void PauseMusic()
        {
            musicSource.Pause();
        }

        public void StopMusic()
        {
            musicSource.Stop();
        }

        public void ChangeMusicVolume(float volume)
        {
            musicVolume = volume;
            musicSource.volume = musicVolume;
        }

        #endregion
    
    

    
        #region Sound

        public void PlaySound(string soundName, bool isLoop = false, UnityAction<AudioSource> callback = null)
        {
            ResourceManager.Instance.LoadAsync<AudioClip>($"Sound/{soundName}", clip =>
            {
                AudioSource audioSource = soundSources[soundSourceIndex];
                soundSourceIndex++;
                soundSourceIndex %= soundSources.Length;  //Range 0-6

                audioSource.clip = clip;
                audioSource.loop = isLoop;
                audioSource.volume = soundVolume;
                audioSource.Play();
                callback?.Invoke(audioSource);

            }
            );
        }

        public void StopSound(string soundName)
        {
            foreach (AudioSource audioSource in soundSources)
            {
                if (audioSource.isPlaying && audioSource.clip.name == soundName)
                {
                    audioSource.Stop();
                }
            }
        }


        public void StopAllSound()
        {
            foreach (AudioSource audioSource in soundSources)
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }
        }

        public void ChangeSoundVolume(float volume)
        {
            soundVolume = volume;
            foreach (AudioSource audioSource in soundSources)
            {
                audioSource.volume = soundVolume;
            }
        }


        #endregion
    
    }
}

