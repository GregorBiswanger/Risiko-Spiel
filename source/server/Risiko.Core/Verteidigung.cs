using System.Collections.Immutable;
using System.Linq;

namespace Risiko.Core;

public record Verteidigung {
    public ImmutableList<Wuerfel> Wuerfel;

    public Verteidigung(Wuerfel wuerfel1, Wuerfel wuerfel2)
    {
        Wuerfel = new List<Wuerfel> { wuerfel1, wuerfel2 }.OrderByDescending(wuerfel => (int)wuerfel.Augen).ToImmutableList();
    }
}