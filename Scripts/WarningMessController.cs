using UnityEngine.UI;
using System;

namespace Mkey
{
    public enum MessageAnswer { Yes, Cancel, No, None }
    public class WarningMessController : PopUpsController
    {
        public Text caption;
        public Text message;
        public Button yesButton;
        public Button noButton;
        public Button cancelButton;

        private MessageAnswer answer = MessageAnswer.None;
        public MessageAnswer Answer
        {
            get { return answer; }
        }

        public void Cancel_Click()
        {
            answer = MessageAnswer.Cancel;
            CloseWindow();
        }

        public void Yes_Click()
        {
            answer = MessageAnswer.Yes;
            CloseWindow();
        }

        public void No_Click()
        {
            answer = MessageAnswer.No;
            CloseWindow();
        }

        public string Caption
        {
            get { if (caption) return caption.text; else return string.Empty; }
            set { if (caption) caption.text = value; }
        }

        public string Message
        {
            get { if (message) return message.text; else return string.Empty; }
            set { if (message) message.text = value; }
        }

        internal void SetMessage(string caption, string message, bool yesButtonActive, bool cancelButtonActive, bool noButtonActive)
        {
            Caption = caption;
            Message = message;
            yesButton.gameObject.SetActive(yesButtonActive);
            cancelButton.gameObject.SetActive(cancelButtonActive);
            noButton.gameObject.SetActive(noButtonActive);
        }
    }
}