using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum AudioCondition
{
    ALL,
    SCENE_MENU,
    SCENE_AWAKE,
    SCENE_MENU_GAME_START,
    SCENE_VILLAGE,
    SCENE_FOREST,
    DRAGON_BATTLE_START,
    DRAGON_FIREBALL,
    MONSTER_HIT,
    PLAYER_WALK,
    PLAYER_RUN,
    PLAYER_HIT
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public List<AudioSource> sounds = new List<AudioSource>();
    
    private Dictionary<AudioCondition, Dictionary<AudioCondition, AudioClip>> audios = new Dictionary<AudioCondition, Dictionary<AudioCondition, AudioClip>>();

    private float[] normalVolumes = new float[] { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1.0f };

    public float _DRAGON_BATTLE_SOUND { get => normalVolumes[1]; }
    public float _DRAGON_FIREBALL_SOUND { get => normalVolumes[4]; }
    public float _HIT_SOUND { get => normalVolumes[2]; }
    public float _VILLAGE_SOUND { get => normalVolumes[1]; }
    public float _FOREST_SOUND { get => 0.05f; }
    public float _WALK_SOUND { get => normalVolumes[1]; }
    public float _GAME_START_SOUND { get => normalVolumes[4]; }
    private float FADE_SPEED { get => 0.00005f; }

private void Awake()
    {
        instance = this;

        if (SceneInfoManager.instance.currentScene == SceneType.Menu)
        {
            AddAudioCase(AudioCondition.SCENE_MENU, AudioCondition.SCENE_AWAKE, Resources.Load<AudioClip>("Sounds/main_menu"));
            AddAudioCase(AudioCondition.SCENE_MENU, AudioCondition.SCENE_MENU_GAME_START, Resources.Load<AudioClip>("Sounds/game_start"));
            return;
        }
        
        AddAudioCase(AudioCondition.SCENE_VILLAGE, AudioCondition.SCENE_AWAKE, Resources.Load<AudioClip>("Sounds/village"));
        AddAudioCase(AudioCondition.SCENE_FOREST, AudioCondition.SCENE_AWAKE, Resources.Load<AudioClip>("Sounds/forest"));
        AddAudioCase(AudioCondition.SCENE_FOREST, AudioCondition.DRAGON_BATTLE_START, Resources.Load<AudioClip>("Sounds/dragon_boss"));
        AddAudioCase(AudioCondition.ALL, AudioCondition.DRAGON_FIREBALL, Resources.Load<AudioClip>("Sounds/dragon_fireball"));
        AddAudioCase(AudioCondition.ALL, AudioCondition.MONSTER_HIT, Resources.Load<AudioClip>("Sounds/monster_hit"));
        AddAudioCase(AudioCondition.ALL, AudioCondition.PLAYER_WALK, Resources.Load<AudioClip>("Sounds/walking"));
        AddAudioCase(AudioCondition.ALL, AudioCondition.PLAYER_RUN, Resources.Load<AudioClip>("Sounds/running"));
        AddAudioCase(AudioCondition.ALL, AudioCondition.PLAYER_HIT, Resources.Load<AudioClip>("Sounds/player_hit"));
    }

    private void AddAudioCase(AudioCondition current, AudioCondition condition, AudioClip output)
    {
        // No Key Exists
        if (!audios.TryGetValue(current, out _))
        {
            Dictionary<AudioCondition, AudioClip> dic = new Dictionary<AudioCondition, AudioClip>();
            dic.Add(condition, output);

            audios.Add(current, dic);
        }
        else // Key Exists
            audios[current].Add(condition, output);
    }

    public AudioClip GetAudio(AudioCondition current, AudioCondition condition)
    {
        if (audios[current].TryGetValue(condition, out AudioClip value))
            return value;

        return null;
    }

    /// <summary>
    /// 오브젝트 풀링이 적용된 AudioSource.PlayAtPoint()
    /// </summary>
    /// <param name="sound">소리 파일</param>
    /// <param name="speaker">소리를 출력할 위치</param>
    public AudioSource PlayAudioAtPoint(AudioClip sound, Vector3 speaker)
    {
        AudioSource existSound = sounds.Find(element => element.name == sound.name && !element.gameObject.activeSelf);

        if (existSound != null)
        {
            existSound.gameObject.SetActive(true);
            existSound.transform.position = speaker;
            existSound.Play();
            StartCoroutine(AutoTurnOff(existSound));
            return existSound;
        }

        GameObject _obj = new GameObject(sound.name);
        _obj.transform.SetParent(this.transform);
        _obj.transform.position = speaker;

        AudioSource obj = _obj.AddComponent<AudioSource>();
        sounds.Add(obj);

        obj.clip = sound;
        obj.spatialBlend = 1.0f;
        obj.Play();
        StartCoroutine(AutoTurnOff(obj));

        return obj;
    }

    /// <summary>
    /// 오브젝트 풀링이 적용된 AudioSource.PlayAtPoint()
    /// </summary>
    /// <param name="sound">소리 파일</param>
    /// <param name="speaker">소리를 출력할 위치</param>
    /// <param name="volume">소리 음량 0.0 ~ 1.0</param>
    public AudioSource PlayAudioAtPoint(AudioClip sound, Vector3 speaker, float volume)
    {
        AudioSource source = PlayAudioAtPoint(sound, speaker);

        source.volume = volume;

        StartCoroutine(AutoTurnOff(source));

        return source;
    }

    /// <summary>
    /// 스테레오타입의 음악을 재생시킵니다.
    /// </summary>
    /// <param name="sound">소리 파일</param>
    public AudioSource PlayAudio(AudioClip sound)
    {
        AudioSource existSound = sounds.Find(element => element.name == sound.name && !element.gameObject.activeSelf);

        if (existSound != null)
        {
            existSound.gameObject.SetActive(true);
            existSound.Play();
            StartCoroutine(AutoTurnOff(existSound));
            return existSound;
        }

        GameObject _obj = new GameObject(sound.name);
        _obj.transform.SetParent(this.transform);

        AudioSource obj = _obj.AddComponent<AudioSource>();
        sounds.Add(obj);

        obj.clip = sound;
        obj.Play();
        StartCoroutine(AutoTurnOff(obj));

        return obj;
    }

    /// <summary>
    /// 스테레오타입의 음악을 재생시킵니다.
    /// </summary>
    /// <param name="sound">소리 파일</param>
    /// <param name="volume">소리 음량 0.0 ~ 1.0</param>
    public AudioSource PlayAudio(AudioClip sound, float volume)
    {
        AudioSource existSound = sounds.Find(element => element.name == sound.name && !element.gameObject.activeSelf);

        if (existSound != null)
        {
            existSound.gameObject.SetActive(true);
            existSound.Play();
            existSound.volume = volume;
            StartCoroutine(AutoTurnOff(existSound));
            return existSound;
        }

        GameObject _obj = new GameObject(sound.name);
        _obj.transform.SetParent(this.transform);

        AudioSource obj = _obj.AddComponent<AudioSource>();
        sounds.Add(obj);

        obj.clip = sound;
        obj.Play();
        obj.volume = volume;
        StartCoroutine(AutoTurnOff(obj));

        return obj;
    }

    /// <summary>
    /// 음악 볼륨을 천천히 증가시킵니다
    /// </summary>
    /// <param name="source">재생시켰던 음악</param>
    /// <param name="destVolume">최종 볼륨</param>
    public void PlayAudioFadeIn(AudioSource source, float destVolume)
    {
        StartCoroutine(SoundFadeIn(source, destVolume));
    }

    /// <summary>
    /// 음악 볼륨을 천천히 감소시킨다음 정지합니다.
    /// </summary>
    /// <param name="source">재생시켰던 음악</param>
    public void StopAudioFadeOut(AudioSource source)
    {
        if (!source.isPlaying)
            return;

        StartCoroutine(SoundFadeOut(source));
    }

    private IEnumerator AutoTurnOff(AudioSource source)
    {
        while (source.isPlaying)
        {
            yield return null;
        }

        source.gameObject.SetActive(false);
    }

    private IEnumerator SoundFadeIn(AudioSource source, float destVolume)
    {
        source.volume = 0;

        while (source.volume < destVolume - 0.1f)
        {
            source.volume += Time.deltaTime;

            yield return null;
        }

        source.volume = destVolume;
    }

    private IEnumerator SoundFadeOut(AudioSource source)
    {
        while (source.volume < 0f)
        {
            source.volume -= FADE_SPEED;

            yield return null;
        }

        source.volume = 0;
        source.Stop();
    }

    /// <summary>
    /// <para>
    /// 테스트용 함수
    /// </para>
    /// AudioManager의 소리를 전체를 확인합니다.
    /// </summary>
    private void AllSoundsName()
    {
        foreach (KeyValuePair<AudioCondition, Dictionary<AudioCondition, AudioClip>> aud in audios)
        {
            foreach (KeyValuePair<AudioCondition, AudioClip> dic in aud.Value)
            {
                Debug.Log(dic.Value);
            }
        }
    }
}
