using KoiCare.Domain.Enums;

namespace KoiCare.Application.Helpers
{
    public static class AgeRangeHelper
    {
        public static bool IsInAgeRange(this decimal age, EAgeRange? ageRange)
        {
            if (ageRange == null)
            {
                return true;
            }
            if (age < 1)
            {
                return ageRange == EAgeRange.UnderYear;
            }
            if (age == 1)
            {
                return ageRange == EAgeRange.UnderYear || ageRange == EAgeRange.YearToThree;
            }
            if (age < 3)
            {
                return ageRange == EAgeRange.YearToThree;
            }
            if (age == 3)
            {
                return ageRange == EAgeRange.YearToThree || ageRange == EAgeRange.AboveThree;
            }
            return ageRange == EAgeRange.AboveThree;
        }
    }
}
