using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
 
[RequireComponent(typeof(TMP_Text))]
public class TMP_Link : MonoBehaviour, IPointerClickHandler
{
    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int linkIndex = TMP_TextUtilities.
            FindIntersectingLink(_text, eventData.position, null);

        if (linkIndex != -1)
        { 
            TMP_LinkInfo linkInfo = _text.textInfo.linkInfo[linkIndex];
            Application.OpenURL(linkInfo.GetLinkID());
        }
    }
}
