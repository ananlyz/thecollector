using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFristAttackController : MonoBehaviour
{
    [Header("水滴生成相关")]
    public Animator animator;
    public float dropWaterDru = 1f;
    public GameObject dropPrefab;       
    public Transform dropSpawnPoint;    
    public float dropSpawnDelay = 1f;   
    public float dropSpawnInterval = 3f;
    public int dropCount = 0;           // 0=无限

    [Header("对象显示控制")]
    public GameObject[] targetObject;     
    public float showDuration = 2f;
    public float stopDuration = 2.5f;

    void Start()
    {
        StartCoroutine(SpawnDropsRoutine());
        ShowObjectForSeconds();
    }

    IEnumerator SpawnDropsRoutine()
    {
        
        yield return new WaitForSeconds(dropSpawnDelay);

        int count = 0;
        while (dropCount == 0 || count < dropCount)
        {
            animator.SetTrigger("Start");
            yield return new WaitForSeconds(dropWaterDru);
            SpawnDrop();
            count++;
            yield return new WaitForSeconds(dropSpawnInterval);
        }
    }

    void SpawnDrop()
    {
        if (dropPrefab != null && dropSpawnPoint != null)
        {
            var dropwater = Instantiate(dropPrefab, dropSpawnPoint.position, Quaternion.identity);
            ImageFillTimer imageFillTimer = FindObjectOfType<ImageFillTimer>();
            if (imageFillTimer != null)
            {
                dropwater.GetComponent<DropWaterAttack>().imageFillTimer = imageFillTimer; 
            }
        }
    }

    public void ShowObjectForSeconds()
    {
        if (targetObject != null)
            StartCoroutine(ShowObjectRoutine());
    }

    IEnumerator ShowObjectRoutine()
    {
        while (true)
        {
            foreach (var targ in targetObject)
            {
                targ.SetActive(true);
            }
            yield return new WaitForSeconds(showDuration);
            foreach (var targ in targetObject)
            {
                targ.SetActive(false);
            }
            yield return new WaitForSeconds(stopDuration);
        }
        
    }
}
