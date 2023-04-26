using System;
using TMPro;
using UnityEngine;

namespace Title
{
    public class TitleLobby : MonoBehaviour
    {
        // Dependencies
        private TitleManager        _titleManager;
        private TextMeshProUGUI     _version;
        private RectTransform       _logoTransform;
        private Vector3             _logoPivotScale;

        private Vector3 _maxLogoScale;
        private float _logoBounceWeight;
        private float _logoBounceBPS;

        private float _intervalTime;
        private Tuple<Vector2, float>[] _audioBandsPos;

        private void Awake()
        {
            _logoTransform = transform.GetChild(1).GetComponent<RectTransform>();
            _version = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        }

        // Start is called before the first frame update
        void Start()
        {
            _titleManager = TitleManager.Instance;
            
            _version.text = VersionInfo.Current;
            _logoPivotScale = _logoTransform.localScale;

            _logoBounceWeight = _titleManager.LogoBounceWeight;
            _logoBounceBPS = _titleManager.LogoBounceBPS;

            _maxLogoScale = _logoPivotScale;
            _maxLogoScale.x += _logoBounceWeight;
            _maxLogoScale.y += _logoBounceWeight;
            _maxLogoScale.z += _logoBounceWeight;

            _intervalTime = 0f;

            _audioBandsPos = new Tuple<Vector2, float>[(int)_titleManager.AudioBandCount];
        }

        // Update is called once per frame
        void Update()
        {
            // 노래가 켜져있으면 Logo를 바운스
            if (_titleManager.AudioSource.isPlaying)
            {
                onLogoBounce();
            }
        }

        private void onLogoBounce()
        {
            _intervalTime += Time.deltaTime * _logoBounceBPS;
            if (_intervalTime >= Mathf.PI) {
                _intervalTime = 0.0f;
            }

            float scale = _logoBounceWeight * Mathf.Sin(_intervalTime);

            Vector3 newScale = _logoPivotScale + new Vector3(scale, scale, scale);
            _logoTransform.localScale = newScale;
        }

        // private void createAudioBands()
        // {
        //     float count = (int)_titleManager.AudioBandCount;
        //     float angleStep = 360f / count;
        //     
        //     for (int i = 0; i < count; i++)
        //     {
        //         float angle = i * angleStep; // 현재 점의 각도
        //         float x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius; // x 좌표
        //         float y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius; // z 좌표
        //
        //         _audioBandsPos[i] = new Tuple<Vector2, float>(new Vector2(x, y), angle);
        //     }
        // }
    }
}
