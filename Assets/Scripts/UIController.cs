using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text points;
    public Text level;
    void Update()
    {
        points.text = "Очки: " + PlayerPrefs.GetInt("CurrentPoints");
        level.text = "Уровень: " + PlayerPrefs.GetInt("Currentlvl");
    }
}
