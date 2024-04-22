using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    public Text CountRed;
    public Text CountGreen;
    public Text CountBlue;
    public Text CountSky;

    private Dictionary<CrystalType, Text> scoreTexts;
    // Start is called before the first frame update
    void Start()
    {
        scoreTexts = new Dictionary<CrystalType, Text>
        {
            { CrystalType.Red, CountRed },
            { CrystalType.Green, CountGreen },
            { CrystalType.Blue, CountBlue },
            { CrystalType.Sky, CountSky }
        };

        Bus.Instance.UpdateCrystal += UpdateText;
    }

    private void UpdateText(int score, CrystalType type)
    {
        if (scoreTexts.ContainsKey(type))
            scoreTexts[type].text = score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
