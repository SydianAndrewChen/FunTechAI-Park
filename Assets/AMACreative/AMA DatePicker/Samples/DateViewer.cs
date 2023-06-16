using TMPro;
using System.Text;
using UnityEngine;
using AMADatePicker;
using UnityEngine.SceneManagement;

public class DateViewer : MonoBehaviour
{
    [SerializeField] private TMP_Text _DateViewer;
    [SerializeField] private TMP_Text _AgeViewer;

    [SerializeField] private ADP _ADPInstance;

    public void _DateUpdated(AMADate _AMADate)
    {
        StringBuilder _SB = new StringBuilder();
        _SB.Append("--- Date of Birth ---\n");
        _SB.Append("Month: ");
        _SB.Append(_AMADate.MM);
        _SB.Append("\nDay: ");
        _SB.Append(_AMADate.dd);
        _SB.Append("\nYear: ");
        _SB.Append(_AMADate.yyyy);

        _DateViewer.text = _SB.ToString();

        _SB = new StringBuilder();
        _SB.Append("--- AGE ---\n");
        _SB.Append("Years: ");
        _SB.Append(_AMADate.Age.Years);
        _SB.Append("\nMonths: ");
        _SB.Append(_AMADate.Age.Months);
        _SB.Append("\nDays: ");
        _SB.Append(_AMADate.Age.Days);
        _SB.Append("\nHours: ");
        _SB.Append(_AMADate.Age.Hours);
        _SB.Append("\nMinutes: ");
        _SB.Append(_AMADate.Age.Minutes);
        _SB.Append("\nSeconds: ");
        _SB.Append(_AMADate.Age.Seconds);

        _AgeViewer.text = _SB.ToString();
    }

    public void _Button_ReloadScene() => SceneManager.LoadScene(0);
}