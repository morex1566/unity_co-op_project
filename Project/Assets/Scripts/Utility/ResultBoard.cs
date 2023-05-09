using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utility
{
    public class ResultBoard : MonoBehaviour
    {
        public string MapName;
        
        public int Grade;
        public int Score;

        public int MaxCombo;
        public int ComboCount;
        public int MaxHp;
        public int HpRemain;

        // tag가 들어가는곳
        public GameObject TagPanel;
        public List<GameObject> Tags;

        public string PlayAt;

        private void Awake()
        {
            MapName = Regex.Replace(GameManager.Instance.MapData.Filename, @"(\.txt|\.wav|\.mp3)", "");
            
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("SelectMap");
            }
        }
    }
}
