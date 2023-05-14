using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class BackgroundProcess
{
    private static BackgroundProcess _instance = null;
    private static GameObject _audioGameObject = null;
    private AudioSource _audioSource;
    private GameObject _quitMenu;
    private GameObject _loadPage;

    private float _maxVolume;
    public AudioSource AudioSource { get { return _audioSource; } set { _audioSource = value; } }
    
    private BackgroundProcess()
    {
        _audioGameObject = new GameObject("BackgroundAudio");
        _audioSource = _audioGameObject.AddComponent<AudioSource>();
        _audioSource.clip = Resources.Load<AudioClip>("lobby");
        
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
        return _audioSource.isPlaying;
    }
    public void OnPlayMusic()
    {
        _audioSource.Play();
    }
    public void OnStopMusic()
    {
        _audioSource.Stop();
    }
    public IEnumerator MusicFadeOut(float duration)
    {
        while (_audioSource.volume > 0)
        {
            _audioSource.volume -= Time.deltaTime / duration;
            yield return null;
        }

        _audioSource.Stop();
    }
    public IEnumerator MusicFadeIn(float duration)
    {
        _audioSource.Play();
        
        while (_audioSource.volume < _maxVolume)
        {
            _audioSource.volume += Time.deltaTime / duration;
            yield return null;
        }
    }
}