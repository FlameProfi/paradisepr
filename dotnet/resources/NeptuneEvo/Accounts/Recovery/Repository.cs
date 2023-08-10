using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Database;
using GTANetworkAPI;
using NeptuneEvo.Handles;
using LinqToDB;
using NeptuneEvo.Character;
using NeptuneEvo.Chars;
using NeptuneEvo.Players;
using Izumrud.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Localization;

namespace NeptuneEvo.Accounts.Recovery
{
    class Repository
    {
        private static readonly nLog Log = new nLog("Accounts.Repository.Events");

        public static async Task SendEmailAsync(ExtPlayer player, string login)
        {
            try
            {
                var sessionData = player.GetSessionData();
                if (sessionData == null) return;
                var auntificationData = sessionData.AuntificationData;

                login = login.ToLower();

                if (!auntificationData.IsCreateAccount)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, LangFunc.GetText(LangType.Ru, DataName.RecoveryCantFind), 4500);
                    Trigger.ClientEvent(player, "restorepassstep", 2);
                    return;
                }
                else if (!auntificationData.Login.ToLower().Equals(login) && !auntificationData.Email.ToLower().Equals(login))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, LangFunc.GetText(LangType.Ru, DataName.RecoveryCantFind), 4500);
                    Trigger.ClientEvent(player, "restorepassstep", 2);
                    return;
                }
                else if (!auntificationData.Email.Contains("@"))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, LangFunc.GetText(LangType.Ru, DataName.RecoveryEmailCant), 4500);
                    Trigger.ClientEvent(player, "restorepassstep", 2);
                    return;
                }

                string recoveryCode = Generate.RandomOneTimePassword();
                if (recoveryCode == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, LangFunc.GetText(LangType.Ru, DataName.RecoveryError), 5000);
                    Trigger.ClientEvent(player, "restorepassstep", 2);
                    return;
                }

                string email = auntificationData.Email;
                string subject = "Izumrud RP - восстановление пароля";
                string body = $"Ваш код для восстановления: {recoveryCode}";

                using (var smtpClient = new SmtpClient("Твой почтовый хост"))
                {
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new NetworkCredential("Твоя полная почта , пример - IzumrudRP@gmail.com", "IzumrudRP");
                    smtpClient.EnableSsl = true;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("Твоя полная почта , пример - IzumrudRP@gmail.com"),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = false
                    };

                    mailMessage.To.Add(email);

                    await smtpClient.SendMailAsync(mailMessage);
                }

                sessionData.RecoveryCode = recoveryCode;
                Utils.Analytics.HelperThread.AddUrl($"recovery?email={auntificationData.Email}&name={login}&code={recoveryCode}&ip={sessionData.Address}");
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Сообщение с кодом для восстановления пароля успешно выслано на {Generate.ObfuscateEmail(auntificationData.Email)}.", 5000);
                Trigger.ClientEvent(player, "restorepassstep", 1);
            }
            catch (Exception e)
            {
                Debugs.Repository.Exception(e);
            }
        }

        public static async void RecoveryPassword(ExtPlayer player, string code)
        {
            try
            {
                var sessionData = player.GetSessionData();
                if (sessionData == null) return;
                var auntificationData = sessionData.AuntificationData;

                if (!auntificationData.IsCreateAccount)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, LangFunc.GetText(LangType.Ru, DataName.RecoveryCantFind), 4500);
                    Trigger.ClientEvent(player, "restorepassstep", 2);
                    return;
                }
                if (code == sessionData.RecoveryCode)
                {
                    Log.Debug($"{sessionData.RealSocialClub} удачно восстановил пароль!", nLog.Type.Info);
                    sessionData.RecoveryCode = null;
                    string newPassword = Generate.RandomString(9);
                    if (newPassword == null)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, LangFunc.GetText(LangType.Ru, DataName.RecoveryError), 5000);
                        Trigger.ClientEvent(player, "restorepassstep", 2);
                        return;
                    }

                    Utils.Analytics.HelperThread.AddUrl($"newpassword?email={auntificationData.Email}&name={auntificationData.Login}&pass={newPassword}&ip={sessionData.Address}");

                    auntificationData.Password = Accounts.Repository.GetSha256(newPassword.ToString());

                    await using var db = new ServerBD("MainDB"); // В отдельном потоке

                    await db.Accounts
                        .Where(v => v.Login == auntificationData.Login)
                        .Set(v => v.Password, auntificationData.Password)
                        .UpdateAsync();

                    Autorization.Repository.AutorizationAccount(player, auntificationData.Login, auntificationData.Password).Wait();

                    Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Вы успешно восстановили доступ к аккаунту. Новый пароль выслан на емайл!", 5000);
                    Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Помимо этого Вы можете изменить его прямо сейчас из игры введя /password и новый желаемый пароль через пробел. Пример [/password 123] без скобок.", 10000);
                }
                else
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, LangFunc.GetText(LangType.Ru, DataName.CodeDoesntMatter), 4500);
                    Trigger.ClientEvent(player, "restorepassstep", 1);
                }
            }
            catch (Exception e)
            {
                Debugs.Repository.Exception(e);
            }
        }
    }
}
