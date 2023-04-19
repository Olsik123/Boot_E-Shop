namespace WebApplication1.Models;
public class Authenticator
{
    public bool Login(string login, string password)
    {
        MyContext mc = new MyContext();
        TbAdmin? pass = mc.TbAdmins.FirstOrDefault(x => login == x.Login);
        
        if(pass == null)
            return false;
        
        return BCrypt.Net.BCrypt.Verify(password, pass.Password);
    }
}