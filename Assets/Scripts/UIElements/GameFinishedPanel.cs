using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameFinishedPanel : MonoBehaviour
{
    [SerializeField] private string youWonString;
    [SerializeField] private string youLostString;



    [SerializeField] private TextMeshProUGUI youWonOrLostText;


    public void SetGameFinishedPanel(bool win)
    {
        if (win)
        {
            youWonOrLostText.color = Color.green;
            youWonOrLostText.text = youWonString;
        }
        else
        {
            youWonOrLostText.color = Color.red;
            youWonOrLostText.text = youLostString;
        }
    }
}
