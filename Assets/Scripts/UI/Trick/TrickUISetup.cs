using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TrickUISetup : MonoBehaviour
{
    [Header("Trick UI")]
    [SerializeField] private Image trickIcon;
    [SerializeField] private GameObject comboBox;

    [Header("Trick Arrows Image")]
    [SerializeField] private Image trickUP;
    [SerializeField] private Image trickDOWN;
    [SerializeField] private Image trickLEFT;
    [SerializeField] private Image trickRIGHT;

    private Image[] _images;
    public void SetupTrick(TrickSO trick,Color color)
    {
       trickIcon.sprite = trick.Icon;
        _images = new Image[trick.trickCombo.Count];
       for (int i = 0; i < trick.trickCombo.Count; i++)
       {
           switch (trick.trickCombo[i])
           {
               case TrickCombo.UP:
                   _images[i] = Instantiate(trickUP, comboBox.transform);
                   break;
               case TrickCombo.DOWN:
                    _images[i] = Instantiate(trickDOWN, comboBox.transform);
                   break;
               case TrickCombo.LEFT:
                    _images[i] = Instantiate(trickLEFT, comboBox.transform);
                   break;
               case TrickCombo.RIGHT:
                    _images[i] = Instantiate(trickRIGHT, comboBox.transform);
                   break;
           }
            _images[i].color = color;
       }
    }

    public void HighLightArrows(int index,Color litColor)
    {
        for(int i= index; i >= 0; i--)
        {
            _images[i].color = litColor;
        }
    }

    public void UnHighLightAllArrows(Color unlitColor)
    {
        foreach (Image image in _images)
        {
            image.color = unlitColor;
        }
    }
}
