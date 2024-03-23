using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PageManagement
{
    public class PageChangeScript : MonoBehaviour
    {
        private float moveDetectRange;
        private float timeDetectRange;
        System.Random random = new System.Random();
        private PageManagerScript.PageEnumerator nextPage { get; set; }
        private void Awake()
        {
            //Пока поставил чтобы на авейке оно рандомно выбирало какой пейдж открыть, нужно будет поменять как то на ассигнмент конкретных пейджей
            var pageEnumSize = Enum.GetNames(typeof(PageManagerScript.PageEnumerator)).Length;
            nextPage = (PageManagerScript.PageEnumerator) random.Next(0,pageEnumSize-1);
        }

        public void OpenOtherPage()
        {
            if (Input.touchCount > 0)
            {

                if (Input.GetTouch(0).phase != TouchPhase.Moved)
                {
                    Debug.Log($"Changed page to {nextPage}");
                    PageManagerScript.Instance.ChangeCurrentPage(nextPage);
                }
            }
        }
    }
}
