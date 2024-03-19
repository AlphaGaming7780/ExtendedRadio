using System.Collections;
using Game.SceneFlow;
using UnityEngine;

namespace ExtendedRadio;

public class ExtendedRadioMono : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    internal void ChangeUiNextFrame(string js) {
        StartCoroutine(ChangeUI(js));
    }

    private IEnumerator ChangeUI(string js) {
        yield return new WaitForEndOfFrame();
        GameManager.instance.userInterface.view.View.ExecuteScript(js);
        yield return null;
    }
}