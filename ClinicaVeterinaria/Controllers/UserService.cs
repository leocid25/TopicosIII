using ClinicaVeterinaria.Data;
using ClinicaVeterinaria.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class UserService
{
    private readonly ClinicaVeterinariaContext _context;

    public UserService()
    {
        _context = new ClinicaVeterinariaContext();
    }

    public RoleManager<ApplicationRole> GetRoleManager()
    {
        return new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(_context));
    }

    public UserManager<ApplicationUser> GetUserManager()
    {
        return new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var smtpClient = new SmtpClient("smtp.seuServidorDeEmail.com")
        {
            Port = 587, // ou a porta correta para o servidor
            Credentials = new NetworkCredential("seuEmail@dominio.com", "suaSenha"),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress("seuEmail@dominio.com"),
            Subject = subject,
            Body = message,
            IsBodyHtml = true,
        };

        mailMessage.To.Add(email);

        await smtpClient.SendMailAsync(mailMessage);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
