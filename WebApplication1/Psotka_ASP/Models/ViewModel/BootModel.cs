
namespace WebApplication1.Models;

public class BootModel
{
    public int BootId { get; set; }
    public string Name { get; set; }
    
    public IFormFile Image { get; set; }
    public List<TbColor> Colors { get; set; } = new List<TbColor>();
    
    public int PickedColorId { get; set; } = -1 ;
    
    public int PickedVariationId { get; set; } = -1;
    public List<TbPhoto> Photos { get; set; } = new List<TbPhoto>();

    public List<TbCategory> Categories { get; set; } = new List<TbCategory>();
    
    public List<TbCategory> NonCategories { get; set; } = new List<TbCategory>();

    public List<TbVariation> Variations { get; set; } = new List<TbVariation>();

    public string ShortDescription { get; set; }

    public string LongDescription { get; set; }

    public string Material { get; set; }

    public int Code { get; set; }

    public BootModel(TbBoot boot)
    {
        
        this.BootId = boot.BootId;
        this.Name = boot.Name;
        MyContext mc = new MyContext();
        Categories = mc.TbBootCategories.Where(x => x.BootId == boot.BootId).Select(x => x.Category).ToList();
        NonCategories = mc.TbCategories.ToList().Except(Categories).ToList();

        foreach (TbPhoto phot in mc.TbPhotos)
        {
            if (phot.BootId == BootId)
            {
                this.Photos.Add(phot);
            }
        }
        this.Colors = mc.TbVariations.Where(x => x.BootId == boot.BootId).Select(y => y.Color).Distinct().ToList();
        this.Variations = mc.TbVariations.Where(x => x.BootId == boot.BootId).ToList();
        this.ShortDescription = boot.ShortDescription;
        this.LongDescription = boot.LongDescription;
        this.Material = boot.Material;
        this.Code = boot.Code;
    }
    public BootModel()
    {
        
    }
}
