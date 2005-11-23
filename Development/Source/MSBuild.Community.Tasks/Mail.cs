// $Id$

using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;

namespace MSBuild.Community.Tasks
{
    /// <summary>
    /// Sends an email message
    /// </summary>
    /// <example>Example of sending an email.
    /// <code><![CDATA[
    /// <Target Name="Mail">
    ///     <Mail SmtpServer="localhost"
    ///         To="user@email.com"
    ///         From="user@email.com"
    ///         Subject="Test Mail Task"
    ///         Body="This is a test of the mail task." />
    /// </Target>
    /// ]]>
    /// </code>
    /// </example>
    public class Mail : Task
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Mail"/> class.
        /// </summary>
        public Mail()
        {
            _isBodyHtml = false;
            _priority = "Normal";
        }

        #region Properties
        string[] _attachments;

        /// <summary>
        /// List of files to attach to message
        /// </summary>
        public string[] Attachments
        {
            get { return _attachments; }
            set { _attachments = value; }
        }

        private string[] _bcc;
        /// <summary>
        /// List of addresss that contains the blind carbon copy (BCC) recipients for this e-mail message
        /// </summary>
        public string[] Bcc
        {
            get { return _bcc; }
            set { _bcc = value; }
        }

        string[] _cc;
        /// <summary>
        /// List of addresss that contains the carbon copy (CC) recipients for this e-mail message
        /// </summary>
        public string[] CC
        {
            get { return _cc; }
            set { _cc = value; }
        }

        string _body;
        /// <summary>
        /// The email message body
        /// </summary>
        public string Body
        {
            get { return _body; }
            set { _body = value; }
        }

        string _from;
        /// <summary>
        /// The from address for this e-mail message
        /// </summary>
        /// <remarks>
        /// This property is required.
        /// </remarks>
        [Required]
        public string From
        {
            get { return _from; }
            set { _from = value; }
        }

        bool _isBodyHtml;
        /// <summary>
        /// A value indicating whether the mail message body is in Html
        /// </summary>
        public bool IsBodyHtml
        {
            get { return _isBodyHtml; }
            set { _isBodyHtml = value; }
        }

        string _priority;
        /// <summary>
        /// The priority of this e-mail message
        /// </summary>
        /// <remarks>
        /// Possible values are High, Normal, and Low
        /// </remarks>
        public string Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        string _subject;
        /// <summary>
        /// The subject line for this e-mail message
        /// </summary>
        /// <remarks>
        /// This property is required.
        /// </remarks>
        [Required]
        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        string _smtpServer;
        /// <summary>
        /// The name or IP address of the host used for SMTP transactions
        /// </summary>
        /// <remarks>
        /// This property is required.
        /// </remarks>
        [Required]
        public string SmtpServer
        {
            get { return _smtpServer; }
            set { _smtpServer = value; }
        }

        string[] _to;
        /// <summary>
        /// List of addresss that contains the recipients of this e-mail message
        /// </summary>
        /// <remarks>
        /// This property is required.
        /// </remarks>
        [Required]
        public string[] To
        {
            get { return _to; }
            set { _to = value; }
        }

        #endregion

        /// <summary>Sends an email message</summary>
        /// <returns>Returns true if successful</returns>
        public override bool Execute()
        {
            Log.LogMessage("Emailing \"{0}\".", _to);

            SmtpClient smtp = new SmtpClient();
            smtp.Host = _smtpServer;
            smtp.UseDefaultCredentials = true;

            MailMessage message = new MailMessage();

            message.From = new MailAddress(_from);
            message.Subject = _subject;
            message.IsBodyHtml = _isBodyHtml;
            message.Body = _body;

            foreach (string to in _to)
            {
                message.To.Add(new MailAddress(to));
            }

            if (_cc != null && _cc.Length > 0)
            {
                foreach (string cc in _cc)
                {
                    message.CC.Add(new MailAddress(cc));
                }
            }

            if (_bcc != null && _bcc.Length > 0)
            {
                foreach (string bcc in _bcc)
                {
                    message.Bcc.Add(new MailAddress(bcc));
                }
            }

            if (_attachments != null && _attachments.Length > 0)
            {
                foreach (string attachment in _attachments)
                {
                    message.Attachments.Add(new Attachment(attachment));
                }
            }

            if (string.Compare(_priority, "High", true) == 0)
                message.Priority = MailPriority.High;
            else if (string.Compare(_priority, "Low", true) == 0)
                message.Priority = MailPriority.Low;
            else
                message.Priority = MailPriority.Normal;

            smtp.Send(message);

            return true;
        }
    }
}
