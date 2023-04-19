namespace WebApplication1.Models.ViewModel;

public class CustomerModel
{
    public List<Tuple<TbVariation,int>> Boots { get; set; } = new List<Tuple<TbVariation,int>>();
    public int Discount { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Address { get; set; }
    public string Country { get; set; }
    public int PostalCode { get; set; }
    public string Email { get; set; }
    public int Phone { get; set; }
}