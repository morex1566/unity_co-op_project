using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Title
{
    public class TitleLobbyLogo : MonoBehaviour
    {
        [Header("Dependencies")]
        [Space(5)]
        [Tooltip("Beat Bar들을 자식으로하는 부모 오브젝트")]
        [SerializeField] private GameObject audioPeer;
        [Tooltip("Logo의 texture를 가지고 있는 객체")]
        [SerializeField] private GameObject logoImage;
        [Tooltip("현재 beat를 나타내는 texture")]
        [SerializeField] private Texture audioBand;
        [Tooltip("현재 Logo의 테두리 두께")]
        [SerializeField] private float borderPadding;

        private TitleManager _titleManager;
        private Tuple<Vector2, float>[] _audioBandTransforms;
        private float[] _samples;
        private int _bandCount;
        private List<RectTransform> _bands;
        private float _audioBandMaxScale;

        private void Start()
        {
            _titleManager = TitleManager.Instance;
            
            _bandCount = (int)_titleManager.AudioBandCount;
            _audioBandTransforms = new Tuple<Vector2, float>[_bandCount];
            _samples = new float[(int)_titleManager.AudioSamplerCount];
            _audioBandMaxScale = _titleManager.AudioBandMaxScale;

            _bands = new List<RectTransform>();
            
            createAudioBands();
        }

        private void Update()
        {
            // TODO : sampling 부분을 스레딩 처리하는게 성능상 더 좋을지도
            if (_titleManager.AudioSource.isPlaying)
            {
                _titleManager.AudioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
                updateAudioBands();
            }
        }

        private void createAudioBands()
        {
            float angleStep = 360f / _bandCount;
            float logoRadius = logoImage.GetComponent<RectTransform>().sizeDelta.x / 2 - borderPadding;
            
            // band가 생성될 위치를 지정합니다.
            for (int i = 0; i < _bandCount; i++)
            {
                float angle = i * angleStep; // 현재 점의 각도
                float x = Mathf.Sin(Mathf.Deg2Rad * angle) * logoRadius; // x 좌표
                float y = Mathf.Cos(Mathf.Deg2Rad * angle) * logoRadius; // y 좌표
        
                _audioBandTransforms[i] = new Tuple<Vector2, float>(new Vector2(x, y), angle);
            }

            int j = 0; 
            // band를 생성합니다.
            foreach (var bandTransform in _audioBandTransforms)
            {
                // 부모로 지정
                GameObject band = new GameObject();
                band.name = "band " + j;
                band.transform.SetParent(audioPeer.transform, false);
                
                RawImage img = band.AddComponent<RawImage>();
                RectTransform rect = img.GetComponent<RectTransform>();
                rect.pivot = new Vector2(0.5f, 0);

                // 나중에 업데이트 하기 위해 저장해둡니다
                _bands.Add(rect);

                // 위치, 회전값 지정
                Vector3 movePos = new Vector3(bandTransform.Item1.x, bandTransform.Item1.y, 0);
                Vector3 rotation = new Vector3(0, 0, -bandTransform.Item2);

                rect.position += movePos;
                rect.rotation = Quaternion.Euler(rotation);
                rect.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                j++;
            }
        }

        private void updateAudioBands()
        {
            // 한쪽으로만 scale이 바뀝니다
            for(int i = 0; i<_bands.Count; i++)
            {
                Vector3 nextScale = _bands[i].localScale;
                nextScale.y = _samples[i] * _audioBandMaxScale;

                if (nextScale.y < _audioBandMaxScale / 4)
                {
                    nextScale.y *= 2f;
                }
                
                _bands[i].localScale = nextScale;
            }
        }
    }
}
