using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameLevel : MonoBehaviour
{
    public List<GameObject> gameObjects;
    [SerializeField] private Transform parentListObj;
    [SerializeField] private Transform parentListOk;
    [SerializeField] private Transform parentListEmpty;
    public List<GameObject> listOk;
    public List<GameObject> listEmpty;
    
    private bool canCheck = true;
    public void Start()
    {
        canCheck = true;
        foreach (Transform tr in parentListObj)
        {
            if (tr.gameObject.activeSelf)
            {
                gameObjects.Add(tr.gameObject);
            }
        }
        
        foreach (Transform tr in parentListOk)
        {
            listOk.Add(tr.gameObject);
        }
        
        foreach (Transform tr in parentListEmpty)
        {
            listEmpty.Add(tr.gameObject);
        }
        
        StartLevel();
    }

    public void StartLevel()
    {
        int count = gameObjects.Count;
        List<int> list = new List<int>();
        for (int i = 0; i < count; i++)
        {
            var t = Random.Range(0, listEmpty.Count);
            while (list.Exists(l=>l == t))
            {
                t = Random.Range(0, listEmpty.Count);
            }
            list.Add(t);
        }

        for (int i = 0; i < count; i++)
        {
            var it = listEmpty[list[i]];
            gameObjects[i].GetComponent<SpriteRenderer>().sprite = it.GetComponent<SpriteRenderer>().sprite;
            it.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public void CheckOk(GameObject g)
    {
        int t = listEmpty.IndexOf(g);
        //listOk[t].gameObject.SetActive(true);
        g.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void RemoveObject(GameObject obj)
    {
        gameObjects.Remove(obj);
        if (canCheck)
        {
            if (gameObjects.Count <= 0)
            {
                StartCoroutine(CheckShowAll());
                canCheck = false;
            }
        }
    }

    IEnumerator CheckShowAll()
    {
        foreach (var tr in listOk)
        {
            int sl = listOk.IndexOf(tr);
            yield return new WaitForSeconds(0.15f);
            listEmpty[sl].SetActive(false);
            tr.gameObject.SetActive(true);
        }
        //yield return new WaitForSeconds(1.25f);
        GameManager.Instance.CheckLevelUp();
    }
}
