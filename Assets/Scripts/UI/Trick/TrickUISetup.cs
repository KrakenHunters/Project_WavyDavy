using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrickUISetup : MonoBehaviour
{
    [Header("Trick UI")]
    [SerializeField] private Image trickIcon;
    [SerializeField] private GameObject comboBox;

    [Header("Trick Arrows")]
    [SerializeField] private Image trickUP;
    [SerializeField] private Image trickDOWN;
    [SerializeField] private Image trickLEFT;
    [SerializeField] private Image trickRIGHT;

    public void SetupTrick(TrickSO trick)
    {
       trickIcon.sprite = trick.Icon;
       for (int i = 0; i < trick.trickCombo.Count; i++)
       {
           switch (trick.trickCombo[i])
           {
               case TrickCombo.UP:
                   Instantiate(trickUP, comboBox.transform);
                   break;
               case TrickCombo.DOWN:
                   Instantiate(trickDOWN, comboBox.transform);
                   break;
               case TrickCombo.LEFT:
                   Instantiate(trickLEFT, comboBox.transform);
                   break;
               case TrickCombo.RIGHT:
                   Instantiate(trickRIGHT, comboBox.transform);
                   break;
           }
       }
    }
}
