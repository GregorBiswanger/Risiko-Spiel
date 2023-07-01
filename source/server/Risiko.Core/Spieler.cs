namespace Risiko.Core;

public class Spieler
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; }
    public Farbe Farbe { get; }
    public uint FreieEinheiten { get; private set; } = 0;

    public Spieler(string name, Farbe farbe)
    {
        Name = name;
        Farbe = farbe;
    }

    public void ErhalteEinheiten(uint einheiten)
    {
        FreieEinheiten += einheiten;
    }

    public uint NimmEinheiten(uint einheiten)
    {
        if (FreieEinheiten < einheiten)
        {
            throw new Exception("Nicht genügend Einheiten vorhanden");
        }

        FreieEinheiten -= einheiten;
        return einheiten;
    }
}