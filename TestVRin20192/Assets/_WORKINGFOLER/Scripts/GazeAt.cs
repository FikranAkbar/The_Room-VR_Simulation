using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GazeAt : MonoBehaviour
{
    // images for icon
    private Image whiteIcon;
    private Image redIcon;

    // coroutine for triggering animation
    private Coroutine lastGazingCoroutine;

    // times property for gazing
    private float gazeTime;
    [SerializeField]
    private float clickDuration;

    // animations
    private int animLayer = 0;
    public Animator animator;
    public bool firstState = false;
    public string firstStateName;
    public string secondStateName;

    // sounds
    public GameObject interactionSound;

    // transforms
    public Transform playerVR;
    public float invisibleAfter;

    // booleans property for gazing
    private bool isGazing = false;

    protected void Start()
    {
        whiteIcon = transform.GetChild(0).GetComponent<Image>();
        redIcon = transform.GetChild(1).GetComponent<Image>();
    }

    private void Update()
    {
        if (isGazing)
        {
            gazeTime += Time.deltaTime;
            redIcon.fillAmount = gazeTime / clickDuration;
        }
        else
        {
            if (lastGazingCoroutine != null)
            {
                StopCoroutine(lastGazingCoroutine);
                lastGazingCoroutine = null;
            }
            gazeTime = 0f;
            redIcon.fillAmount = gazeTime;
        }

        if (redIcon.fillAmount >= 1f)
        {
            whiteIcon.enabled = false;
            redIcon.enabled = false;
        }
        else
        {
            whiteIcon.enabled = true;
            redIcon.enabled = true;
        }
    }

    public void StartGazing()
    {
        if(Vector3.Distance(playerVR.position, transform.position) <= invisibleAfter)
        {
            isGazing = true;
            if (lastGazingCoroutine == null)
            {
                lastGazingCoroutine = StartCoroutine(WaitGazingTillFinish());
            }
        }
    }

    public void StopGazing()
    {
        isGazing = false;
    }

    private IEnumerator WaitGazingTillFinish()
    {
        Debug.Log("Start Coroutine");
        yield return new WaitUntil(() => redIcon.fillAmount >= 1f);
        if (firstState)
        {
            interactionSound.SetActive(false);
            interactionSound.SetActive(true);
            animator.SetTrigger("TriggerSecondState");
            firstState = false;
        } 
        else if (!firstState)
        {
            interactionSound.SetActive(false);
            interactionSound.SetActive(true);
            animator.SetTrigger("TriggerFirstState");
            firstState = true;
        }
    }

    private bool IsPlaying(Animator anim, string stateName)
    {
        if(anim.GetCurrentAnimatorStateInfo(animLayer).IsName(stateName) &&
            anim.GetCurrentAnimatorStateInfo(animLayer).normalizedTime < 1.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
