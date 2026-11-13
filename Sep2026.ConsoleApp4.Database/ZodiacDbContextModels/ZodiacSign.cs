using System;
using System.Collections.Generic;

namespace Sep2026.ConsoleApp4.Database.ZodiacDbContextModels;

public partial class ZodiacSign
{
    public int ZodiacId { get; set; }

    public string ZodiacName { get; set; } = null!;

    public string Symbol { get; set; } = null!;

    public string Element { get; set; } = null!;

    public string DateRange { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Horoscope> Horoscopes { get; set; } = new List<Horoscope>();
}
