using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Controller : MonoBehaviour
{
    public float scaleSpeed = 1f;

    bool islogin;
    [SerializeField] GameObject botonJugar;
    [SerializeField] GameObject botonLogin;
    [SerializeField] GameObject botonLogin2;
    [SerializeField] GameObject EnterPass;
    [SerializeField] GameObject EnterUser;
    [SerializeField] GameObject Pass;
    [SerializeField] GameObject User;

    public void Jugar()
    {
        if (!islogin)
        {
            Debug.Log("Not Login");
        }
        else
        {
            SceneManager.LoadScene("MainScene");
        }
    }

    public void Login1()
    {
        StartCoroutine(changeFields());
    }

    public void Login2()
    {
        string username = EnterUser.GetComponent<TMP_InputField>().text;
        string password = EnterPass.GetComponent<TMP_InputField>().text;

        if (username == "Meminbb" && password == "Jugar")
        {
            StartCoroutine(revertFields());
            islogin = true;
        }
        else
        {
            Debug.Log("No existe");
            StartCoroutine(Shake(botonLogin2));
        }
    }

    IEnumerator changeFields()
    {
        botonLogin.GetComponent<ScaleObject>().StartScaleTo(0f);
        yield return new WaitForSeconds(1);
        User.GetComponent<ScaleObject>().StartScaleTo(1f);
        Pass.GetComponent<ScaleObject>().StartScaleTo(1f);
        EnterPass.GetComponent<ScaleObject>().StartScaleTo(2f);
        EnterUser.GetComponent<ScaleObject>().StartScaleTo(2f);
        botonLogin2.GetComponent<ScaleObject>().StartScaleTo(1f);
    }

    IEnumerator revertFields()
    {
        User.GetComponent<ScaleObject>().StartScaleTo(0f);
        Pass.GetComponent<ScaleObject>().StartScaleTo(0f);
        EnterPass.GetComponent<ScaleObject>().StartScaleTo(0f);
        EnterUser.GetComponent<ScaleObject>().StartScaleTo(0f);
        botonLogin2.GetComponent<ScaleObject>().StartScaleTo(0f);
        yield return new WaitForSeconds(1);
        botonJugar.GetComponent<ScaleObject>().StartScaleTo(1f);
    }

    IEnumerator Shake(GameObject obj, float duration = 0.3f, float magnitude = 10f)
    {
        RectTransform rt = obj.GetComponent<RectTransform>();
        Vector3 originalPos = rt.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            rt.anchoredPosition = originalPos + new Vector3(x, 0f, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        rt.anchoredPosition = originalPos;
    }
}
