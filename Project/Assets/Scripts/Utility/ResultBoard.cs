using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Utility
{
    public class ResultBoard : MonoBehaviour
    {
        public static ResultBoard Instance;
        
        [Header("Dependencies")] 
        [Space(5)] 
        [SerializeField] private TextMeshProUGUI mapName;
        [SerializeField] private TextMeshProUGUI grade;
        [SerializeField] private TextMeshProUGUI score;
        [SerializeField] private TextMeshProUGUI maxCombo;
        [SerializeField] private TextMeshProUGUI hpRemain;
        [SerializeField] private TextMeshProUGUI playAt;
        [SerializeField] private TextMeshProUGUI accurancy;
        [SerializeField] private Image scoreGraph;
        [SerializeField] public GameObject TagPanel;


        // Dataset
        private string _MapName;
        private string _Grade;
        private int _Score;
        private int _MaxCombo;
        private int _MaxHp;
        private int _HpRemain;
        private List<GameObject> _Tags;
        private string _PlayAt;
        private float _accurancy;

        private void Awake()
        {
            // 싱글톤
            if (Instance == null)
            {
                Instance = this;
            }
            else if(Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            _MapName = Regex.Replace(GameManager.Instance.MapData.Filename, @"(\.txt|\.wav|\.mp3)", "");
            _PlayAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            _MaxCombo = FindObjectOfType<Sword>().GetMaxCombo();
        }

        // Start is called before the first frame update
        void Start()
        {
            calculateAccurancy();
            calculateGrade(_accurancy);
            calculateScore();
            
            // 이제 결과창과 연동시킵니다
            {
                mapName.text = _MapName;
                grade.text = _Grade;
                score.text = _Score.ToString();
                maxCombo.text = _MaxCombo.ToString();
                hpRemain.text = FindObjectOfType<Health>().GetHealthNum().ToString();//GameManager.Instance.GetHealthCount().ToString();
                playAt.text = _PlayAt;
                accurancy.text = (_accurancy * 100) + "%";
            }
            
            // score Graph의 애니메이션을 보여줍니다.
            {
                
            }
        }

        // Update is called once per frame
        void Update()
        {
            updateInput();
        }

        private void updateInput()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                onDestroy();
                SceneManager.LoadScene("SelectMap");
            }
        }
        
        private void calculateAccurancy()
        {
            int totalHit = FindObjectOfType<Sword>().GetTotalHit();
            int obstacleCount = GameManager.Instance._levelObstacleSpawner.FragileObstacleCount;
            
            _accurancy = 0f;
            if (obstacleCount > 0)
            {
                _accurancy = (float)totalHit / obstacleCount; // 정확도 계산식: 총 명중 수 / 장애물 수
            }
        }
        
        private void calculateGrade(float accurancy)
        {
            if (accurancy >= 1f)
            {
                _Grade = "S";
            }
            else if(accurancy >= 0.9f)
            {
                _Grade = "A";
            }
            else if (accurancy >= 0.8f)
            {
                _Grade = "B";
            }
            else
            {
                _Grade = "C";
            }
        }

        private void calculateScore()
        {
            _Score = (int)(_accurancy * 100f * _MaxCombo);
        }

        private void onShowScoreGraph()
        {
            StartCoroutine(drawScoreGraph());
        }

        private IEnumerator drawScoreGraph()
        {
            float targetFillAmount = _accurancy;
            float duration = 0.8f; // 애니메이션 진행 시간
            float timer = 0f; // 경과 시간

            float startFillAmount = scoreGraph.fillAmount;
            float currentFillAmount = startFillAmount; // 현재 fillAmount 값

            while (timer < duration)
            {
                timer += Time.deltaTime;
                float progress = timer / duration; // 진행도 비율

                currentFillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, progress);
                scoreGraph.fillAmount = currentFillAmount;

                yield return null;
            }

            // 애니메이션 완료 후 최종 값 설정
            scoreGraph.fillAmount = targetFillAmount;
        }

        private void onDestroy()
        {
            Destroy(gameObject);
        }
    }
}
