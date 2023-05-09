using UnityEngine;
using TMPro;

public class Sword : MonoBehaviour
{
    [SerializeField]
    Animator playerAnim;

    [SerializeField]
    TMP_Text comboText;

    int comboCount = 0;

    private void Start()
    {
        comboText.text = comboCount.ToString();
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Fragile Obstacle" && 
             (playerAnim.GetCurrentAnimatorStateInfo(0).IsName("RightAttack") ||
             playerAnim.GetCurrentAnimatorStateInfo(0).IsName("LeftAttack")))
        {
            Debug.Log("충돌");
            Renderer renderer = collision.gameObject.GetComponent<Renderer>();
            Material material = renderer.materials[0];
            MeshCut.Cut(collision.gameObject, collision.transform.position, Vector3.right, material);

            comboText.text = comboCount++.ToString();
            
        }
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
}
