namespace Risiko.Core.Kontinente;

public record Nordamerika() : Kontinent("Nordamerika", 6, new List<Land>
{
    new("Alaska"),
    new("Nordwest-Territorium"),
    new("Grönland"),
    new("Alberta"),
    new("Quebec"),
    new("Ontario"),
    new("Westsstaaten"),
    new("Oststaaten"),
    new("Zentralamerika")
});