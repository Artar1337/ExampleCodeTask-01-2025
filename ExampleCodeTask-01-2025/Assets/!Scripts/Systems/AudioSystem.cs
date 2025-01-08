using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

public interface IAudioSystem
{
    UniTask Init();
    void PlaySoundOneShot(SoundType sound);
    void PlaySoundLoop(SoundType sound);
    void PlayMusic(SoundType music);
    void StopMusic();
    void StopLoopSound();
    /// <param name="volume">0 - 1</param>
    void SetSoundMixerVolume(float volume);
    /// <param name="volume">0 - 1</param>
    void SetMusicMixerVolume(float volume);
}

public class AudioSystem : IAudioSystem, IDisposable
{
    private const float TIME_FOR_FADE = 1f;

    [Inject] private IResourcesSystem _resources;

    private readonly Dictionary<SoundType, AudioClip> _sounds = new();

    private AudioSource _soundSource;
    private AudioSource _loopSoundSource;
    private AudioSource _musicSource;
    private CancellationTokenSource _cts;

    public async UniTask Init()
    {
        var settings = await _resources.Load<SoundSettings>();

        foreach (var element in settings.Data)
        {
            _sounds.Add(element.Type, element.Clip);
        }

        var mediator = await _resources.Instantiate<AudioSystemMediator>("AudioSources");
        _soundSource = mediator.Sound;
        _loopSoundSource = mediator.LoopSound;
        _musicSource = mediator.Music;
    }

    public void PlaySoundOneShot(SoundType sound)
    {
        _soundSource.PlayOneShot(GetClipByType(sound));
    }

    public void PlaySoundLoop(SoundType sound)
    {
        _loopSoundSource.loop = true;
        _loopSoundSource.clip = GetClipByType(sound);
        _loopSoundSource.Play();
    }

    public void StopMusic()
    {
        _musicSource.Stop();
    }

    public void StopLoopSound()
    {
        _loopSoundSource.Stop();
    }

    public void PlayMusic(SoundType music)
    {
        SwitchMusicAsync(music).Forget();
    }

    public void Dispose()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }

    private AudioClip GetClipByType(SoundType sound)
    {
        if (!_sounds.ContainsKey(sound))
        {
            Debug.LogError($"AudioSystem | Can't play {sound} - no entry in settings found!");
            return null;
        }

        return _sounds[sound];
    }

    private async UniTask SwitchMusicAsync(SoundType music)
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = new();

        var newMusic = GetClipByType(music);

        if (_musicSource.isPlaying)
        {
            try
            {
                await FadeAudioSourceVolumeAsync(_musicSource, 0f, TIME_FOR_FADE);
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }

        _musicSource.loop = true;
        _musicSource.clip = newMusic;
        _musicSource.Play();

        try
        {
            await FadeAudioSourceVolumeAsync(_musicSource, 1f, TIME_FOR_FADE);
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }

    private async UniTask FadeAudioSourceVolumeAsync(
        AudioSource src, float endvolume, float time)
    {
        var startVolume = src.volume;
        float timeLeft = time;

        while (timeLeft > 0f)
        {
            await UniTask.Yield(cancellationToken: _cts.Token);

            timeLeft -= Time.deltaTime;
            src.volume = Mathf.Lerp(endvolume, startVolume, timeLeft / time);
        }
    }

    public void SetSoundMixerVolume(float volume)
    {
        _soundSource.outputAudioMixerGroup.audioMixer.SetFloat("Sound",
            Convert01RangeToMixerRange(volume));
    }

    public void SetMusicMixerVolume(float volume)
    {
        _soundSource.outputAudioMixerGroup.audioMixer.SetFloat("Music",
            Convert01RangeToMixerRange(volume));
    }

    private float Convert01RangeToMixerRange(float volume)
    {
        return Mathf.Approximately(0f, volume) ? -80 : Mathf.Log10(volume) * 20;
    }
}
