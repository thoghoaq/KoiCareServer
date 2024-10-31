using System.ComponentModel.DataAnnotations;

namespace KoiCare.Domain.Enums
{
    public enum EAgeRange
    {
        [Display(Name = "Cá dưới 1 tuổi")]
        UnderYear = 1,
        [Display(Name = "Cá 1-3 tuổi")]
        YearToThree = 2,
        [Display(Name = "Cá trên 3 tuổi")]
        AboveThree = 3
    }
}
