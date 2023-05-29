using System;
using UnityEngine;
using TMPro;

public class Sword : MonoBehaviour
{
    [SerializeField] Animator playerAnim;

    [SerializeField] TMP_Text comboText;

    [SerializeField] private AudioSource hitSound;

    [SerializeField] private GameObject healthPrefab;

    [SerializeField] Health health;


    int comboCount = 0;
    private int maxComboCount = 0;
    private int totalHit;

    private Collider _hitBox;
    private bool hitFlag = true;

    public int ComboCount { get { return comboCount; } set { comboCount = value; } }

    private void Awake()
    {
        _hitBox = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        comboText.text = comboCount.ToString();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Fragile Obstacle" &&
            (playerAnim.GetCurrentAnimatorStateInfo(0).IsName("RightAttack") ||
             playerAnim.GetCurrentAnimatorStateInfo(0).IsName("LeftAttack")) &&
             hitFlag)
        {
            string blockDirection = collision.GetComponent<FragileObstacle>().direction;
            if ((playerAnim.GetCurrentAnimatorStateInfo(0).IsName("RightAttack") && blockDirection == "Right") ||
                (playerAnim.GetCurrentAnimatorStateInfo(0).IsName("LeftAttack") && blockDirection == "Left"))
            {
                Renderer renderer = collision.gameObject.GetComponent<Renderer>();
                Material material = renderer.materials[0];


                collision.gameObject.transform.SetParent(null);
                collision.enabled = false;

                hitSound.Play();

                MeshCut.Cut(collision.gameObject, collision.transform.position, Vector3.up, material);

                //체력
                health.AddHp();

                // 콤보 관련 정리
                {
                    totalHit++;

                    if (comboCount >= maxComboCount)
                    {
                        maxComboCount++;
                    }

                    comboCount++;
                }

                // UI 관련 정리
                {
                    // 적중 시, 점수 ui 출력
                    // TODO : 점수 시스템에 대해서 설정해야할듯.
                    GameManager.Instance.ShowPopup(collision.gameObject.transform, comboCount.ToString());
                }

                comboText.text = comboCount.ToString();

                hitFlag = false;
            }
        }
    }

    public int GetCombo()
    {
        return comboCount;
    }

    public int GetTotalHit()
    {
        return totalHit;
    }

    public int GetMaxCombo()
    {
        return maxComboCount;
    }


    private void Update()
    {
        hitFlag = true;
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            playerAnim.SetTrigger("LeftAttack");
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            playerAnim.SetTrigger("RightAttack");
        }
    }

    private void onAttack(bool toggle)
    {
        _hitBox.enabled = toggle;
    }
}