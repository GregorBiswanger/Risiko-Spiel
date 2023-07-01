using System.Collections.Immutable;

namespace Risiko.Core;

public class Kontinent
{
    public string Name { get; }
    public ImmutableList<Land> Laender { get; }
    public uint KontrollBonus { get; }

    public Kontinent(string name, uint kontrollBonus, List<Land> lands)
    {
        Name = name;
        Laender = ImmutableList.CreateRange(lands);
        KontrollBonus = kontrollBonus;
    }
}