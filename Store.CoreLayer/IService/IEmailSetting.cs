using Store.CoreLayer.Entirty.Mail;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.CoreLayer.IServices
{
    public interface IEmailSetting
    {
        Task SendAsyncByUsingMailKite(Email email);
    }
}
