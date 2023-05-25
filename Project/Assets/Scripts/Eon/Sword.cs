using System;
using UnityEngine;
using TMPro;

public class Sword : MonoBehaviour
{
    [SerializeField] Animator playerAnim;

    [SerializeField] TMP_Text comboText;

    [SerializeField] private AudioSource hitSound;

    [SerializeField] private GameObject healthPrefab;


    int comboCount = 0;
    private int maxComboCount = 0;
    private int totalHit;

    private Collider _hitBox;

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
        
        
        if (collision.gameObject.tag == "Fragile Obstacle")
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