using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainTitleAni : MonoBehaviour
{
    [SerializeField] private Transform mainTitleTrans;
    [SerializeField] private Transform startBtnTrans;

    // Start is called before the first frame update
    void Start()
    {
        startBtnTrans.gameObject.SetActive(false);
        mainTitleTrans.DOMoveY(580, 1f)
            .SetDelay(0.2f)
            .SetEase(Ease.OutBack)
            .OnComplete(() => Invoke("StartBtnAnimation", 0.2f));
    }


    void StartBtnAnimation()
    {
        startBtnTrans.gameObject.SetActive(true);
        startBtnTrans.DOMoveY(205, 1f)
            .SetEase(Ease.OutBounce);
    }
    
}
