using TMPro;
using UnityEngine;

public class MapEditorMessageBox : MonoBehaviour
{
    public TMP_Text textmeshpro;
    public RectTransform rectTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        textmeshpro = transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
        rectTransform = transform.GetComponent<RectTransform>();
        
        this.gameObject.SetActive(true);
    }
}
