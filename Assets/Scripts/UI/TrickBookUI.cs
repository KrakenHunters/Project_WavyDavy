using UnityEngine;

public class TrickBookUI : MonoBehaviour
{
    [SerializeField] private TrickSO[] tricks;
    [SerializeField] private GameObject trickBoxPanel;
    [SerializeField] private TrickUISetup trickUIPrefab;
    [SerializeField] private Color color;


    private void Start()
    {
        Debug.Log("TrickBookMenu Start");
        foreach (TrickSO trick in tricks)
        {
            TrickUISetup trickUI = Instantiate(trickUIPrefab, trickBoxPanel.transform);
            trickUI.SetupTrick(trick, color);
            trickUI.gameObject.SetActive(true);
        }
    }

}
