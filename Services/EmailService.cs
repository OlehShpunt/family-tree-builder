using FluentEmail.Core;
using System.Threading.Tasks;

public class EmailService
{
    private readonly IFluentEmailFactory _emailFactory;
    
   
    public EmailService(IFluentEmailFactory emailFactory)
    {
        _emailFactory = emailFactory;
    }

        public async Task SendSimpleEmailAsync(string toEmail, string subject, string body)
    {
        
        var email = _emailFactory.Create(); 

        await email
           
            .To(toEmail)
            .Subject(subject)
            
            .Body(body, isHtml: true) 
            .SendAsync();
    }
}