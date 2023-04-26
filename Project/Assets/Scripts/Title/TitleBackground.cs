using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Title
{
    public class TitleBackground : MonoBehaviour
    {
        private TitleManager _titleManager;
        
        private Texture _introBackground;
        private Texture _titleBackground;

        private RawImage _background;

        private void Awake()
        {
            _background = GetComponent<RawImage>();
        }

        private void Start()
        {
            _titleManager = TitleManager.Instance;

            _introBackground = _titleManager.IntroBackground;
            _titleBackground = _titleManager.TitleBackground;

            _background.texture = _introBackground;
        }

        private IEnumerator onChange(BackgroundTexture background)
        {
            // 점차 하얀색으로 바뀝니다.
            
            // 하얀색일 때, texture를 바꿉니다.
            switch (background)
            {
                case BackgroundTexture.Intro :
                    _background.texture = _introBackground;
                    break;
                case BackgroundTexture.Title :
                    _background.texture = _titleBackground;
                    break;
                default:
                    break;
            }
            
            // texture가 보이도록 되돌리기
            yield return null;
        }
    }
}
