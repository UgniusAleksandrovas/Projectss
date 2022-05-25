using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class startScript : MonoBehaviour
{
    [SerializeField] GameObject E;
    [SerializeField] GameObject N;
    [SerializeField] GameObject L;
    [SerializeField] GameObject I;
    [SerializeField] GameObject G;
    [SerializeField] GameObject H;
    [SerializeField] GameObject T;
    [SerializeField] GameObject E1;
    [SerializeField] GameObject N1;
    [SerializeField] GameObject M;
    [SerializeField] GameObject E2;
    [SerializeField] GameObject N2;
    [SerializeField] GameObject T1;


    [SerializeField] GameObject S;
    [SerializeField] GameObject T2;
    [SerializeField] GameObject O;
    [SerializeField] GameObject N3;
    [SerializeField] GameObject E3;
    [SerializeField] GameObject startMenu;
    [SerializeField] GameObject minimapopen;


    public void Update()
    {
        StartCoroutine(changingScene());
    }
    private void Awake()
    {
        minimapopen.SetActive(false);
    }

    public IEnumerator changingScene()
    {
        yield return new WaitForSeconds(0.4f);
        E.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        N.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        L.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        I.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        G.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        H.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        T.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        E1.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        N1.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        M.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        E2.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        N2.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        T1.SetActive(true);
        yield return new WaitForSeconds(0.4f);


        S.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        T2.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        O.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        N3.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        E3.SetActive(true);
        yield return new WaitForSeconds(0.4f);

        startMenu.SetActive(false);

        Time.timeScale = 1f;
        minimapopen.SetActive(true);
    }
}
