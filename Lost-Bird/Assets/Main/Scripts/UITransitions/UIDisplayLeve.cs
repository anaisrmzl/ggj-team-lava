using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIDisplayLeve : MonoBehaviour
{
    [SerializeField]
    private Collector collector;
    [SerializeField]
    private Text feathers;
    [SerializeField]
    private GameObject key;
    [SerializeField]
    private int max_feathers, timesCaught;

    private void Awake()
    {
        collector.onUpdateUI += UpdateUI;
    }

    private void UpdateUI()
    {
        //Checks what scene is active and sets the appropriate num of feathers
        if (SceneManager.GetActiveScene().name == "Map1") max_feathers = 4;
        else if (SceneManager.GetActiveScene().name == "Map2") max_feathers = 6;

        feathers.text = collector.Feathers.ToString() + "/" + max_feathers;
        key.SetActive(collector.HasKey);
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
