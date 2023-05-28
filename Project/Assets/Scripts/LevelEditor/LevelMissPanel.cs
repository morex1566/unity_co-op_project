using TMPro;
using UnityEngine;

public class LevelMissPanel : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private TextMeshProUGUI combo;
    [SerializeField] private AudioSource missSound;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Fragile Obstacle" )
        {
            FindObjectOfType<Sword>().ComboCount = 0;
            
            missSound.Play();

            combo.text = FindObjectOfType<Sword>().ComboCount.ToString();

            //Destroy(health.transform.GetChild(health.transform.childCount-1).gameObject);
            health.SubHp();
            
            Destroy(other.gameObject);
        }
    }
}
