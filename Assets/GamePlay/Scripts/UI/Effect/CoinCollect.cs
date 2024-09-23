using System.Collections;
using System.Collections.Generic;
using ReuseSystem.ObjectPooling;
using Unity.VisualScripting;
using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    [SerializeField] private RectTransform rect;
    [SerializeField] private float moveSpeed;
    public void Init(Vector2 position,RectTransform parent,Vector2 destination,float timeWait)
    {
        rect.rotation = Quaternion.Euler(0f,0f,Random.Range(0,180f));
        rect.SetParent(parent);
        rect.position = position;
        StartCoroutine(ToDestination(destination,timeWait));
    }
    
    IEnumerator ToDestination(Vector2 destination,float timeWait)
    {
        rect.localScale = Vector3.zero;
        float currentScale = 0f;
        while (currentScale <= 1f)
        {
            currentScale += Time.deltaTime / timeWait;
            rect.localScale = currentScale * Vector3.one;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        while (Vector2.Distance(rect.position, destination) >= 0.3f)
        {
            rect.position = Vector2.Lerp(rect.position, destination, moveSpeed * Time.deltaTime);
            yield return null;
        }
        LazyPool.Instance.AddObjectToPool(gameObject);
    }
}
