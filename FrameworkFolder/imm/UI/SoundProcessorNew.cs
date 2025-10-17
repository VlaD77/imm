using System;
using System.Collections.Generic;
using imm.Core;
using UnityEngine;

namespace imm.UI
{
    public sealed class SFXToken
    {
        private readonly AudioSource _audioSource;

        public AudioSource Source
        {
            get
            {
                return _audioSource;
            }
        }

        public SFXToken(AudioSource audioSource)
        {
            _audioSource = audioSource;
        }

        public void Hold()
        {
            _audioSource.loop = true;
        }

        public void Release()
        {
            _audioSource.loop = false;
            _audioSource.Stop();

            _audioSource.volume = 1f;
        }
    }

    public interface ISoundMap
    {
        string GetEffectPath(string name);

        string GetSoundThemePath(string name);
    }

    public sealed class SoundMap<TEffect, TSoundTheme> : ISoundMap
    {
        private Dictionary<string, string> _sfxToPath;
        private Dictionary<string, string> _musicToPath;

        public SoundMap()
        {
            _sfxToPath = new Dictionary<string, string>();
            _musicToPath = new Dictionary<string, string>();
        }

        public void AddEffect(TEffect effect, string path)
        {
            if (_sfxToPath.ContainsKey(effect.ToString())) {
                _sfxToPath[effect.ToString()] = path;
            }
            else
            {
                _sfxToPath.Add(effect.ToString(), path);
            }
        }

        public void AddSoundTheme(TSoundTheme theme, string path)
        {
            _musicToPath.Add(theme.ToString(), path);
        }

        public string GetEffectPath(string name)
        {
            return _sfxToPath[name];
        }

        public string GetSoundThemePath(string name)
        {
            return _musicToPath[name];
        }
    }

    public abstract class AbstractSoundManager<TType, TEffect, TSoundTheme>
        where TType : AbstractSoundManager<TType, TEffect, TSoundTheme>, new()
    {
        public bool needPlayMusic = true;
        public bool needPlaySound = true;
        private SoundMap<TEffect, TSoundTheme> _map;

        protected AbstractSoundManager()
        {
            _map = new SoundMap<TEffect, TSoundTheme>();
        }

        protected abstract void BuildSoundMap(SoundMap<TEffect, TSoundTheme> map);

        private static AbstractSoundManager<TType, TEffect, TSoundTheme> _instance;

        public static AbstractSoundManager<TType, TEffect, TSoundTheme> Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TType();

                    _instance.BuildSoundMap(_instance._map);

                    SoundProcessorNew.Instance.Initialize(_instance._map);
                }

                return _instance;
            }
        }

        public void PlayEffect(TEffect effect)
        {
            if (this.needPlaySound)
            {
                SoundProcessorNew.Instance.PlayEffect(effect.ToString());
            }
        }
        public bool IsPlayingMusic = false;

        public void PlayMusic(TSoundTheme music, bool withSmooth = false)
        {
            if (this.needPlayMusic)
            {
                SoundProcessorNew.Instance.PlayMusic(music.ToString(), withSmooth);
                IsPlayingMusic = true;

            }
        }
        public void PlayMusicWithSync(TSoundTheme music, bool withSmooth = false)
        {
            if (this.needPlayMusic)
            {
                try
                {
                    SoundProcessorNew.Instance.PlayMusicWithSync(music.ToString(), withSmooth);
                }
                catch (Exception ex) {
                    if (ex!=null) {
                        Debug.Log("ERROR: " +ex.Message + "\n Source: " + ex.Source + "\n StackTrace: " + ex.StackTrace);
                    }
                    
                }
                IsPlayingMusic = true;

            }
        }
        public void StopMusic(TSoundTheme music, bool withSmooth = false)
        {
            SoundProcessorNew.Instance.StopMusic(false, withSmooth);
            IsPlayingMusic = false;
        }

        public void StopMusicSecond(TSoundTheme music, bool withSmooth = false)
        {
            SoundProcessorNew.Instance.StopMusicSecond(false, withSmooth);
            
            IsPlayingMusic = false;
        }
    }


    public sealed class SoundProcessorNew : imm.Core.MonoSingleton<SoundProcessorNew>
    {
        private const int SFX_POOL_SIZE = 10;

        private ISoundMap _soundMap;

        private AudioSource _musicAudioSource = null;
        private AudioSource _musicAudioSourceSecond = null;

        private readonly List<AudioSource> _effectSoundPool;

        private bool _backgroundMusicEnabled = true;
        private bool _soundEffectEnabled = true;
        private float _effectsVolume = 1f;
        private float _musicVolume = 1f;

        public void Initialize(ISoundMap soundMap)
        {
            _soundMap = soundMap;
        }


        public bool IsBackgroundMusicEnabled
        {
            get
            {
                return _backgroundMusicEnabled;
            }
            set
            {
                _backgroundMusicEnabled = value;

                if (_backgroundMusicEnabled)
                {
                    //PlayMusic( _currentMusicTheme );
                }
                else
                {
                    StopMusic(false);
                }
            }
        }

        public bool IsSoundEffectEnabled
        {
            get
            {
                return _soundEffectEnabled;
            }
            set
            {
                _soundEffectEnabled = value;
            }
        }

        public float MusicVolume
        {
            get
            {
                return _musicVolume;
            }
        }

        public float EffectsVolume
        {
            get
            {
                return _effectsVolume;
            }
        }

        public void ChangeMusicVolume(float volume)
        {
            _musicVolume = volume;
            _musicAudioSource.volume = volume;
            _musicAudioSourceSecond.volume = volume;
        }
        public void ChangeMusicFirstVolume(float volume)
        {
            _musicVolume = volume;
            _musicAudioSource.volume = volume;
            //_musicAudioSourceSecond.volume = volume;
        }

        public void ChangeMusicSecondVolume(float volume)
        {
           
            //_musicAudioSource.volume = volume;
            _musicAudioSourceSecond.volume = volume;
        }


        public void ChangeEffectsVolume(float volume)
        {
            _effectsVolume = volume;

            for (int i = 0; i < _effectSoundPool.Count; i++)
            {
                _effectSoundPool[i].volume = volume;
            }
        }

        public SoundProcessorNew()
        {
            _effectSoundPool = new List<AudioSource>();
        }

        protected override void Init()
        {
            _musicAudioSource = gameObject.AddComponent<AudioSource>();
            _musicAudioSource.loop = true;
            _musicAudioSource.playOnAwake = false;

            _musicAudioSourceSecond = gameObject.AddComponent<AudioSource>();
            _musicAudioSourceSecond.loop = true;
            _musicAudioSourceSecond.playOnAwake = false;

            for (int i = 0; i < SFX_POOL_SIZE; i++)
            {
                AudioSource audioSource = gameObject.AddComponent<AudioSource>();

                audioSource.loop = false;

                _effectSoundPool.Add(audioSource);
            }
        }

        bool hasUnload = false;
        bool hasUnloadSecond = false;
        bool isStop = false;
        bool isStopSecond = false;
        bool forBothMusic = false;
        public void StopMusic(bool hasUnload, bool withSmooth = false)
        {
            if (!withSmooth)
            {
                if (_musicAudioSource.clip != null)
                {
                    _musicAudioSource.Stop();
                    if (_effectsVolume <= 0)
                    {
                        ChangeMusicVolume(1);
                    }
                    if (hasUnload) Resources.UnloadAsset(_musicAudioSource.clip);
                }
                if (_musicAudioSourceSecond.clip != null)
                {
                    _musicAudioSourceSecond.Stop();
                    if (_effectsVolume <= 0)
                    {
                        ChangeMusicVolume(1);
                    }
                    if (hasUnloadSecond) Resources.UnloadAsset(_musicAudioSourceSecond.clip);
                }
            }
            else
            {
                this.hasUnload = hasUnload;
                this.hasUnloadSecond = this.hasUnload;
                isStop = true;
                isStopSecond = true;
                forBothMusic = true;

                ScheduleUpdate(0.1f);
            }


        }

        public void StopMusicSecond(bool hasUnload, bool withSmooth = false)
        {
            if (!withSmooth)
            {
                if (_musicAudioSourceSecond.clip != null)
                {
                    _musicAudioSourceSecond.Stop();
                    if (_effectsVolume <= 0)
                    {
                        ChangeMusicSecondVolume(1);
                    }
                    if (hasUnloadSecond) Resources.UnloadAsset(_musicAudioSourceSecond.clip);
                }
            }
            else
            {
                this.hasUnloadSecond = hasUnload;
                isStopSecond = true;
                forBothMusic = false;
                ScheduleUpdate(0.1f);
            }


        }

        protected override void OnScheduledUpdate()
        {
            if (forBothMusic)
            {
                if (isStop)
                {
                    if (_musicAudioSource.clip != null)
                    {
                        ChangeMusicVolume(MusicVolume - 0.1f);
                        if (MusicVolume <= 0)
                        {

                            isStop = false;
                            isStopSecond = false;
                            StopMusic(this.hasUnload);

                            //UnscheduleUpdate();
                        }
                    }
                }
                else if (!isStop)
                {
                    ChangeMusicVolume(MusicVolume + 0.1f);
                    if (MusicVolume >= 1)
                    {

                        isStop = false;
                        isStopSecond = false;
                        // UnscheduleUpdate();
                    }
                }
            }
            else
            {



                if (isStop)
                {
                    if (_musicAudioSource.clip != null)
                    {
                        ChangeMusicFirstVolume(MusicVolume - 0.1f);
                        if (MusicVolume <= 0)
                        {

                            isStop = false;
                            StopMusic(this.hasUnload);

                            //UnscheduleUpdate();
                        }
                    }
                }
                else if (!isStop)
                {
                    ChangeMusicFirstVolume(MusicVolume + 0.1f);
                    if (MusicVolume >= 1)
                    {

                        isStop = false;

                        // UnscheduleUpdate();
                    }
                }

                if (isStopSecond)
                {
                    if (_musicAudioSourceSecond.clip != null)
                    {
                        ChangeMusicSecondVolume(_musicAudioSourceSecond.volume - 0.1f);
                        if (_musicAudioSourceSecond.volume <= 0)
                        {

                            isStopSecond = false;
                            StopMusicSecond(this.hasUnloadSecond);
                            // UnscheduleUpdate();
                        }
                    }
                }
                else if (!isStopSecond)
                {
                    ChangeMusicSecondVolume(_musicAudioSourceSecond.volume + 0.1f);
                    if (_musicAudioSourceSecond.volume >= 1)
                    {

                        isStopSecond = false;

                        // UnscheduleUpdate();
                    }
                }
            }
            if (!isStop && !isStopSecond) {
                UnscheduleUpdate();
            }

                base.OnScheduledUpdate();


        }

        public void PlayMusic(string theme, bool withSmooth = false)
        {
            if (IsBackgroundMusicEnabled)
            {

                if (_musicAudioSource.clip != null)
                {
                    Resources.UnloadAsset(_musicAudioSource.clip);
                }
                if (_musicAudioSourceSecond.clip != null)
                {
                    Resources.UnloadAsset(_musicAudioSourceSecond.clip);
                }
                //SGResources.Load(_soundMap.GetSoundThemePath(theme)) as AudioClip;
                _musicAudioSource.clip = Resources.Load(_soundMap.GetSoundThemePath(theme)) as AudioClip;

                _musicAudioSource.Play();
                if (withSmooth)
                {
                    isStop = false;
                    ScheduleUpdate(0.1f);

                }
                else
                {
                    ChangeMusicVolume(1);
                }
            }
        }

        public void PlayMusicWithSync(string theme, bool withSmooth = false)
        {

            if (IsBackgroundMusicEnabled)
            {
                float time = 0;
                if (_musicAudioSource.clip != null)
                {
                    time = _musicAudioSource.time;
                    //Resources.UnloadAsset(_musicAudioSource.clip);
                }

                //_musicAudioSource.clip = Resources.Load(_soundMap.GetSoundThemePath(theme)) as AudioClip;
                //_musicAudioSource.time = time;

                _musicAudioSourceSecond.clip = _musicAudioSource.clip;// Resources.Load(_soundMap.GetSoundThemePath(theme)) as AudioClip;
                if (_musicAudioSourceSecond.clip != null)
                {
                    _musicAudioSourceSecond.time = Mathf.Min(time, _musicAudioSourceSecond.clip.length - 0.01f);
                }
                _musicAudioSourceSecond.volume = 1;

                _musicAudioSource.clip = Resources.Load(_soundMap.GetSoundThemePath(theme)) as AudioClip;

                if (_musicAudioSource.clip != null)
                {
                    _musicAudioSource.time = Mathf.Min(time, _musicAudioSource.clip.length - 0.01f);
                }

                try
                {
                    if (_musicAudioSource && _musicAudioSource.clip != null)
                    {
                        _musicAudioSource.Play();
                    }
                }
                catch (Exception ex) { }
                try
                {
                    if (_musicAudioSourceSecond && _musicAudioSourceSecond.clip != null)
                    {
                        _musicAudioSourceSecond.Play();
                    }
                }
                catch (Exception ex) { }

                if (withSmooth)
                {
                    isStop = false;
                    ScheduleUpdate(0.1f);

                }
                else
                {
                    //ChangeMusicVolume(1);
                    ChangeMusicFirstVolume(1);
                }
                StopMusicSecond(true, true);
            }
        }

        public SFXToken PlayEffect(string effect)
        {
            return PlayEffect(effect, 0);
        }

        public SFXToken PlayEffect(string effect, float delay)
        {
            return PlayEffect(effect, delay, _effectsVolume);
        }
        public SFXToken StopEffect(string effect)
        {
            return StopEffectSource(effect);
        }

        public SFXToken PlayEffect(string effect, float delay, float volume)
        {
            if (IsSoundEffectEnabled)
            {
                for (int i = 0; i < _effectSoundPool.Count; i++)
                {
                    if (!_effectSoundPool[i].isPlaying)
                    {
                        AudioSource audioSource = _effectSoundPool[i];
                        audioSource.clip = Resources.Load(_soundMap.GetEffectPath(effect)) as AudioClip;
                        audioSource.volume = volume;
                        audioSource.loop = false;

                        if (delay > 0)
                            audioSource.PlayDelayed(delay);
                        else
                            audioSource.Play();

                        return new SFXToken(audioSource);
                    }
                }

                MonoLog.Log(MonoLogChannel.Sound, string.Format("Effect {0} was skipped because not free slots found", effect));
            }

            return null;
        }
        public SFXToken StopEffectSource(string effect)
        {
            if (IsSoundEffectEnabled)
            {
                var gg = _effectSoundPool.FindAll(x => x.isPlaying);
                for (int i = 0; i < gg.Count; i++)
                {
                    AudioSource audioSource = gg[i];
                    // Debug.LogError("isPlaying " + audioSource.clip.name);
                }

                for (int i = 0; i < _effectSoundPool.Count; i++)
                {
                    if (_effectSoundPool[i].isPlaying)
                    {

                        AudioSource audioSource = _effectSoundPool[i];

                        if (audioSource.clip.name == effect)
                        {


                            audioSource.Stop();
                        }

                        return new SFXToken(audioSource);
                    }
                }

                MonoLog.Log(MonoLogChannel.Sound, string.Format("Effect {0} was skipped because not free slots found", effect));
            }

            return null;
        }

    }
}