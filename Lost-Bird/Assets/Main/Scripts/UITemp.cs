using UnityEngine;
using UnityEngine.UI;

public class UITemp : MonoBehaviour
{
    #region FIELDS

    [SerializeField] private Collector collector;
    [SerializeField] private Text feathersText;
    [SerializeField] private Text keyText;

    #endregion

    #region  BEHAVIORS

    private void Awake()
    {
        collector.onUpdateUI += UpdateUI;
    }

    private void OnDestroy()
    {
        collector.onUpdateUI -= UpdateUI;
    }

    private void UpdateUI()
    {
        feathersText.text = collector.Feathers.ToString();
        keyText.text = collector.HasKey ? "1" : "0";
    }

    #endregion
}
