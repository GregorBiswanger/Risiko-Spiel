namespace Risiko.Core.Kontinente;

public record Afrika() : Kontinent("Afrika", 4, new List<Land>
{
    new("Nordafrika"),
    new("Ägypten"),
    new("Ostafrika"),
    new("Zentralafrika"),
    new("Südafrika"),
    new("Madagaskar")
});
