using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class BackgroundProcess
{
    private static BackgroundProcess _instance = null;
    private static GameObject _audioGameObject = null;
    private AudioSource _audio;
    private GameObject _quitMenu;
    private GameObject _loadPage;

    private float _maxVolume;
    public AudioSource Audio => _audio;
    
    private BackgroundProcess()
    {
        _audioGameObject = new GameObject("BackgroundAudio");
        _audio = _audioGameObject.AddComponent<AudioSource>();
        _audio.clip = Resources.Load<AudioClip>("lobby");
        
        UnityEngine.Object.DontDestroyOnLoad(_audioGameObject);
        // UnityEngine.Object.DontDestroyOnLoad(_quitMenu);
        _maxVolume = 0.5f;
    }
    
    // 싱글톤 인스턴스
    public static BackgroundProcess Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BackgroundProcess();
            }
            
            return _instance;
        }
    }

    public bool IsPlaying()
    {
        return _audio.isPlaying;
    }
    public void OnPlayMusic()
    {
        _audio.Play();
    }
    public void OnStopMusic()
    {
        _audio.Stop();
    }
    public IEnumerator MusicFadeOut(float duration)
    {
        while (_audio.volume > 0)
        {
            _audio.volume -= Time.deltaTime / duration;
            yield return null;
        }
    }
    public IEnumerator MusicFadeIn(float duration)
    {
        while (_audio.volume < _maxVolume)
        {
            _audio.volume += Time.deltaTime / duration;
            yield return null;
        }
    } 
}