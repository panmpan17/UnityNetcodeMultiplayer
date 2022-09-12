using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestMath : MonoBehaviour
{
    public Text resultText;
    
    public void SwitchToOne()
    {
        resultText.text = "1";
    }

    public void SwitchToTwo()
    {
        resultText.text = "2";
    }

    public void SwitchToResult()
    {
        resultText.text = "3";
    }
}
