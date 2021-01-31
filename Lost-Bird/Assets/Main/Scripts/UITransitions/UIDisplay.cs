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
        {
            feathers.text = "Feathers: " + PlayerPrefs.GetInt("Feathers").ToString() + "/" + max_feathers.ToString();
            return;
        }

        if (SceneManager.GetActiveScene().name == "Map1") max_feathers = 4;
        else if (SceneManager.GetActiveScene().name == "Map2") max_feathers = 6;

        feathers.text = "Feathers: " + collector.Feathers.ToString() + "/" + max_feathers.ToString();
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
