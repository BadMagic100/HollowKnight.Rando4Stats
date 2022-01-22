using UnityEngine;

namespace RandoStats.Util
{
    public class CanvasGroupLinearFade : MonoBehaviour
    {
        public float duration = 1f;

        private float currentLerpTime;
        private CanvasGroup? group;

        private void Awake()
        {
            group = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            currentLerpTime = 0;
        }

        private void Update()
        {
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > duration)
            {
                currentLerpTime = duration;
                gameObject.SetActive(false);
            }
            // in the unity lifecycle this should be non-null; if it is, we should throw because we're not allowed to have this component on the GO anyway.
            group!.alpha = 1 - currentLerpTime / duration;
        }
    }
}
