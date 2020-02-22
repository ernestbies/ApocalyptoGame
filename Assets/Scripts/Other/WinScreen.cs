using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
 
public class WinScreen : MonoBehaviour {
 
    public AudioClip winScreenSound;
    public float speed;
    AudioSource source;
    Image win;
    Text winText;
    GameObject buttons;
    bool isFaded = false;
 
    void OnEnable()
    {
        win = transform.Find("Image").GetComponent<Image>();
        winText = transform.Find("Text").GetComponent<Text>();
        buttons = transform.Find("Buttons").gameObject;
        source = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
 
        winText.canvasRenderer.SetAlpha(0.0f);
        foreach(Transform button in buttons.transform)
        {
            button.Find("Text").GetComponent<Text>().canvasRenderer.SetAlpha(0.0f);
        }
        source.PlayOneShot(winScreenSound);
        win.material.SetFloat("_Level", 1.0f);
        StartCoroutine(FadeScreen());
    }
 
    void Update()
    {
        if(win.material.GetFloat("_Level") <= 0 && isFaded == false)
        {
            isFaded = true;
            winText.CrossFadeAlpha(1.0f, 1.0f, false);
            foreach (Transform button in buttons.transform)
            {
                button.Find("Text").GetComponent<Text>().CrossFadeAlpha(1.0f, 1.0f, false);
            }
        }
    }
 
    IEnumerator FadeScreen()
    {
        float t = 1f;
        while (t > 0)
        {
            t -= Time.deltaTime * speed;
            win.material.SetFloat("_Level", t);
            yield return null;
        }
    }
 
    public void RestartButton()
    {
        SceneManager.LoadScene("Game");
    }
 
    public void MenuButton()
    {
        SceneManager.LoadScene("Menu");
    }
 
    public void ExitButton()
    {
        Application.Quit();
    }
 
}