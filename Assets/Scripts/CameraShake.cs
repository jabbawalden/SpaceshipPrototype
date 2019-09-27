using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private Vector3 originalPos;

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            CallCamShake(0.7f, 0.58f);
        }
    }

    public void CallCamShake(float duration, float magnitude)
    {
        StartCoroutine(Shake(duration, magnitude));
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            magnitude *= 0.955f;
            float x = Random.Range(-1, 1) * magnitude;
            float y = Random.Range(-1, 1) * magnitude;
            transform.localPosition = new Vector3(x + originalPos.x, y + originalPos.y, originalPos.z);
            elapsed += Time.deltaTime;
            //wait until the next frame starts before updating
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
