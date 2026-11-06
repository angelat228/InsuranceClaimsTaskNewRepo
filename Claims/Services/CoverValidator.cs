
using Claims.Models;

namespace Claims.Services;

public static class CoverValidator
{
    public static void Validate(Cover cover)
    {
        if (cover.StartDate.Date < DateTime.Today)
        {
            throw new ArgumentException("Cover start date cannot be in the past.");
        }

        if (cover.EndDate.Date > cover.StartDate.Date.AddYears(1))
        {
            throw new ArgumentException("The insurance period cannot exceed one year.");
        }
    }
}
