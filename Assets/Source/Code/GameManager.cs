using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<GameLevel> levels;

    private int startIndex = 0;

    private int currentIndex;
    [SerializeField] List<GameObject> particleVFXs;
    public bool isStartGame = false;


    private bool canDrag = true;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        currentIndex = startIndex;

        levels[currentIndex].gameObject.SetActive(true);
        canDrag = true;
        isStartGame = true;
    }

    public int GetCurrentIndex()
    {
        return currentIndex;
    }

    public void CheckLevelUp()
    {
        if (isStartGame)
        {
            if (levels[currentIndex].gameObjects.Count == 0)
            {
                canDrag = false;
                isStartGame = false;
                selectedObject = null;
                GameObject explosion = Instantiate(particleVFXs[Random.Range(0,particleVFXs.Count)], transform.position, transform.rotation);
                Destroy(explosion, .75f);
                currentIndex += 1;
                StartCoroutine(LevelUp());
            }
        }
    }
    
    IEnumerator LevelUp()
    {
        yield return new WaitForSeconds(1);
        levels[currentIndex-1].gameObject.SetActive(false);
        if (currentIndex == 5)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            currentIndex = 0;
            canDrag = true;
            isStartGame = true;
        }
        else
        {
            levels[currentIndex].gameObject.SetActive(true);
            canDrag = true;
            isStartGame = true;
        }

        
        
    }
    
    
    // drag obj
    public ObjectMoveByDrag selectedObject;
    Vector3 offset;
    void Update()
    {
        if(!canDrag) return;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);
            if (targetObject && targetObject.GetComponent<ObjectMoveByDrag>() != null)
            {
                selectedObject = targetObject.GetComponent<ObjectMoveByDrag>();
                selectedObject.PickUp();
                offset = selectedObject.transform.position - mousePosition;
            }
        }
        if (selectedObject)
        {
            selectedObject.transform.position = mousePosition + offset;
        }
        if (Input.GetMouseButtonUp(0) && selectedObject)
        {
            if (selectedObject)
            {
                selectedObject.CheckOnMouseUp();
            }
            selectedObject = null;
        }
    }
}