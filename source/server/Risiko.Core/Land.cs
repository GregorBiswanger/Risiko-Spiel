namespace Risiko.Core;

public class Land
{
    public string Name { get; }
    public Guid BesitzerSpielerId { get; private set; } = Guid.Empty;
    public uint Einheiten { get; private set; }

    public Land(string name)
    {
        Name = name;
    }

    public void SetzeEinheitenUndBesetze(Guid spielerId, uint einheiten)
    {
        BesitzerSpielerId = spielerId;
        Einheiten = einheiten;
    }
}