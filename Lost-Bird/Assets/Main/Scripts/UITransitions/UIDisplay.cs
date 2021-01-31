using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIDisplay : MonoBehaviour
{
    [SerializeField]
    private Collector collector = null;
    [SerializeField]
    private int max_feathers;
    [SerializeField]
    private Text feathers;

    void Start()
    {
        if (collector == null)
            return;

        max_feathers = 10;
        feathers.text = collector.Feathers.ToString() + "/" + max_feathers;
    }

    public void GoToScreen(string _name)
    {
        SceneManager.LoadScene(_name);
    }
    public void Next()
    {
        if (SceneManager.GetActiveScene().name == "Menu") SceneManager.LoadScene("Map1");
        else if (SceneManager.GetActiveScene().name == "Map1") SceneManager.LoadScene("Map2");
        else if (SceneManager.GetActiveScene().name == "Map2") SceneManager.LoadScene("Win");
        else if (SceneManager.GetActiveScene().name == "Win") SceneManager.LoadScene("Menu");

    }


}
