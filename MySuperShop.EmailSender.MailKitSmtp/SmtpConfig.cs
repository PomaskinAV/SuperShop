﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySuperShop.EmailSender.MailKitSmtp
{
    public class SmtpConfig
    {
        [Required, RegularExpression(@"[^@\s]+\.[^@\s]+\.[^@\s]+$")] public string Host { get; set; }
        // [Required] public string Host { get; set; }

        [EmailAddress, Required] public string UserName { get; set; }

        [Required] public string Password { get; set; }

        [Range(1, ushort.MaxValue)] public int Port { get; set; }
        [EmailAddress, Required] public string SendFrom { get; set; }
    }
}
