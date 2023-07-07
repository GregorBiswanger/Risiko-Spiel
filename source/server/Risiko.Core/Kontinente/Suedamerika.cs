namespace Risiko.Core.Kontinente;

public record Suedamerika() : Kontinent("Südamerika", 3, new List<Land>
{
    new("Venezuela"),
    new("Peru"),
    new("Brasilien"),
    new("Argentinien")
});
