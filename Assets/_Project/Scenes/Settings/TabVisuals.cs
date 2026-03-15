using UnityEngine;
using UnityEngine.UI;

public class TabVisuals : MonoBehaviour
{

    private void Start()
    {
        Select(0);
    }

    [System.Serializable]
    public class TabSet
    {
        public Image targetImage;      // the button Image
        public Sprite normalSprite;    // from normal PNG
        public Sprite selectedSprite;  // from selected PNG
    }

    [SerializeField] private TabSet[] tabs;

    public void Select(int index)
    {
        for (int i = 0; i < tabs.Length; i++)
            tabs[i].targetImage.sprite = (i == index) ? tabs[i].selectedSprite : tabs[i].normalSprite;
    }
}