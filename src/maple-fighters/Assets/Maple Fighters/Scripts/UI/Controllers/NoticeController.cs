﻿using System;
using Scripts.UI.Windows;
using UI.Manager;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class NoticeController : MonoBehaviour
    {
        private INoticeView noticeView;
        private Action onOkButtonClicked;

        public void Show(string message, Action onClicked = null, bool background = true)
        {
            var noticeWindow = CreateOrShowNoticeView();
            if (noticeWindow != null)
            {
                noticeWindow.Message = message;

                if (onClicked != null)
                {
                    onOkButtonClicked = onClicked;

                    noticeWindow.OkButtonClicked += onOkButtonClicked;
                }
                else
                {
                    onOkButtonClicked = null;
                }

                if (background == false)
                {
                    noticeWindow.HideBackground();
                }
            }
        }

        public void Hide()
        {
            SubscribeToUIFadeAnimation();
            HideNoticeWindow();
        }

        private void OnDestroy()
        {
            HideNoticeWindow();
        }

        private void HideNoticeWindow()
        {
            if (noticeView != null)
            {
                noticeView.OkButtonClicked -= Hide;

                if (onOkButtonClicked != null)
                {
                    noticeView.OkButtonClicked -= onOkButtonClicked;
                }

                noticeView.Hide();
            }
        }

        private void SubscribeToUIFadeAnimation()
        {
            if (noticeView != null)
            {
                noticeView.FadeOutCompleted += OnFadeOutCompleted;
            }
        }

        private void UnsubscribeFromUIFadeAnimation()
        {
            if (noticeView != null)
            {
                noticeView.FadeOutCompleted -= OnFadeOutCompleted;
            }
        }

        private void OnFadeOutCompleted()
        {
            UnsubscribeFromUIFadeAnimation();
        }

        private INoticeView CreateOrShowNoticeView()
        {
            if (noticeView == null)
            {
                noticeView = UIElementsCreator.GetInstance()
                    .Create<NoticeWindow>(UILayer.Foreground, UIIndex.End);
                noticeView.OkButtonClicked += Hide;
            }

            noticeView.Show();

            return noticeView;
        }
    }
}