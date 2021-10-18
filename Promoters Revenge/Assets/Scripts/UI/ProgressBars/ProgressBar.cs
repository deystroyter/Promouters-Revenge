using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.ProgressBars
{
    [ExecuteInEditMode]
    public class ProgressBar : MonoBehaviour
    {
        [Header("Title Setting")]
        public string Title;

        public Color TitleColor;
        public Font TitleFont;
        public int TitleFontSize = 10;

        [Header("Bar Setting")]
        public Color BarColor;

        public Color BarDelayColor;
        public Color BarBackGroundColor;
        public Sprite BarBackGroundSprite;

        [Header("DelayedBar Settings")]
        [Range(0.01f, 5f)] public float DelayTime = 1f;

        [Range(0.00f, 1f)] public float DelayUpdateTime = 0.5f;


        [SerializeField] private Image bar, barDelay, barBackground;


        private AudioSource _audiosource;

        //private Text txtTitle;
        private float barValue;

        public float BarValue
        {
            get { return barValue; }
            private set { barValue = Mathf.Clamp(value, 0.00f, 1.00f); }
        }


        private void Awake()
        {
            //bar = transform.Find("Bar").GetComponent<Image>();
            //barDelay = transform.Find("BarDelay").GetComponent<Image>();
            //barBackground = transform.Find("BarBackground").GetComponent<Image>();

            //txtTitle = transform.Find("Text").GetComponent<Text>();
            _audiosource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            //txtTitle.text = Title;
            //txtTitle.color = TitleColor;
            //txtTitle.font = TitleFont;
            //txtTitle.fontSize = TitleFontSize;
            BarValue = 1;
            bar.color = BarColor;
            barDelay.color = BarDelayColor;
            barBackground.color = BarBackGroundColor;
            barBackground.sprite = BarBackGroundSprite;

            UpdateValue();
        }

        public void ChangeValue(float val)
        {
            BarValue = val;
            UpdateValue();
        }


        private void UpdateValue()
        {
            bar.fillAmount = Mathf.Lerp(bar.fillAmount, BarValue, 100 * Time.deltaTime);
            //txtTitle.text = Title + " " + val + "%";

            StartCoroutine("DelayedUpdateValue");
        }

        private IEnumerator DelayedUpdateValue()
        {
            yield return new WaitForSeconds(DelayTime);

            while (Mathf.Abs(barDelay.fillAmount - barValue) >= 1e-3)
            {
                barDelay.fillAmount = Mathf.Lerp(barDelay.fillAmount, BarValue, Time.deltaTime);
                yield return null;
            }

            StopCoroutine("DelayedUpdateValue");
            yield return new WaitForSeconds(DelayUpdateTime);
        }


        private void Update()
        {
            if (!Application.isPlaying)
            {
                //txtTitle.color = TitleColor;
                //txtTitle.font = TitleFont;
                //txtTitle.fontSize = TitleFontSize;

                bar.color = BarColor;
                barBackground.color = BarBackGroundColor;

                barBackground.sprite = BarBackGroundSprite;
            }
        }
    }
}