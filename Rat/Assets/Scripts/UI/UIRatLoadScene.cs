using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class UIRatLoadScene : MonoBehaviour
{
    public string targetScene = "";
    private Button target;

    private void Awake()
    {
        target = this.GetComponent<UnityEngine.UI.Button>();
    }

    private void OnEnable()
    {
        target.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        target.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        SceneManager.LoadScene(targetScene);
    }
}
