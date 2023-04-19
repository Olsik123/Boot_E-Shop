using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

public class ProductRepository
{
    public List<TbBoot> FindAll()
    {
        MyContext mc = new MyContext();
        
        return mc.TbBoots.Include(x=> x.TbPhotos).Include(x=> x.TbVariations).ToList();
    }
    public  TbBoot FindById(int id)
    {
        return this.FindAll().Find(x => x.BootId == id);
    }
}