using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectMoveByDrag : MonoBehaviour
{
    [SerializeField] List<GameObject> particleVFXs;

    private Vector3 startPos;
    private Transform target;
    private bool isPickup = false;

    private void OnEnable()
    {
        startPos = transform.position;
    }

    public void PickUp()
    {
        isPickup = true;
        //transform.rotation = new Quaternion(0,0,0,0);
    }

    public void CheckOnMouseUp()
    {
        //transform.position = startPos;
        isPickup = false;
        if (target)
        {
            GameObject explosion = Instantiate(particleVFXs[Random.Range(0, particleVFXs.Count)], transform.position,
                transform.rotation);
            Destroy(explosion, .75f);
            GameManager.Instance.levels[GameManager.Instance.GetCurrentIndex()].RemoveObject(gameObject);
            GameManager.Instance.levels[GameManager.Instance.GetCurrentIndex()].CheckOk(target.gameObject);
            gameObject.SetActive(false);
        }
        else
        {
            transform.position = startPos;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isPickup)
            if (collision.gameObject.GetComponent<SpriteRenderer>().sprite.name ==
                GetComponent<SpriteRenderer>().sprite.name)
            {
                target = collision.transform;
            }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (isPickup)
        {
            if (collision.gameObject.GetComponent<SpriteRenderer>().sprite.name ==
                GetComponent<SpriteRenderer>().sprite.name)
            {
                target = null;
            }
        }
    }
}