using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharSelect : MonoBehaviour
{
    [SerializeField] private Transform titleTran;
    [SerializeField] private Transform[] charTrans;

    void Start()
    {

        foreach (var item in charTrans)
        {
            item.gameObject.SetActive(false);
        }

        titleTran.DOMoveY(570, 1f)
            .SetDelay(0.2f)
            .SetEase(Ease.InOutBounce)
            .OnComplete(
                () =>
                {
                    Invoke("CharacterAnimation", 0.4f);
                }
            );

    }

    void CharacterAnimation()
    {
        for (int i = 0; i < charTrans.Length; i++)
        {
            charTrans[i].gameObject.SetActive(true);
            charTrans[i].DOMoveY(210, 0.2f)
                .SetDelay(0.2f * i);
        }
    }

    //IEnumerator CharacterAnimation()
    //{
    //    Vector2[] aniStartVecs = { new Vector2(-150, 410), new Vector2(795, 850), new Vector2(1430, 150), new Vector2(485, -130) };
    //    Vector2[] aniEndVecs = { new Vector2(485, 410), new Vector2(795, 410), new Vector2(795, 150), new Vector2(485, 150) };

    //    for (int i = 0; i < charTrans.Length; i++)
    //    {
    //        charTrans[i].transform.position = aniStartVecs[i];
    //        charTrans[i].gameObject.SetActive(true);
    //        charTrans[i].DOMove(aniEndVecs[i], 0.1f)
    //            .SetEase(Ease.Linear);

    //        yield return new WaitForSeconds(0.2f);
    //    }
    //}

    void Update()
    {
        
    }

    public void CharSel(int index)
    {
        GameManager.instance.charSelectIndex = index;
        GameManager.instance.OnGame();
    }
}
