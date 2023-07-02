namespace Risiko.Core.Kontinente;

public record Australien() : Kontinent("Australien", 3, new List<Land>
{
    new("Indonesien"),
    new("Neu-Guinea"),
    new("West-Australien"),
    new("Ost-Australien")
});
