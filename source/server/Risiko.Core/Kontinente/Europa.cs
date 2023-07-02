namespace Risiko.Core.Kontinente;

public record Europa() : Kontinent("Europa", 5, new List<Land>
{
    new("Island"),
    new("Skandinavien"),
    new("Russland"),
    new("Großbritannien"),
    new("Nordeuropa"),
    new("Westeuropa"),
    new("Südeuropa")
});
