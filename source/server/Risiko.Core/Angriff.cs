using System.Collections.Immutable;

namespace Risiko.Core;

public record Angriff {
    public ImmutableList<Wuerfel> Wuerfel;

    public Angriff(Wuerfel wuerfel1, Wuerfel wuerfel2, Wuerfel wuerfel3)
    {
        Wuerfel = new List<Wuerfel> { wuerfel1, wuerfel2, wuerfel3 }.OrderByDescending(wuerfel => (int)wuerfel.Augen).ToImmutableList();
    }
}