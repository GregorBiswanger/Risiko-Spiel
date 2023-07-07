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

    public AngriffErgebnis Angreifen(Land verteidiger, Angriff angriff, Verteidigung verteidigung)
    {
        var angreiferVerluste = 0;
        var verteidigerVerluste = 0;
        var eingenommen = false;

        for(int i = 0; i < verteidigung.Wuerfel.Count; i++)
        {
            if(verteidigung.Wuerfel[i].Augen >= angriff.Wuerfel[i].Augen)
            {
                angreiferVerluste++;
            }
            else
            {
                verteidigerVerluste++;
            }
        }

        if(Einheiten - angreiferVerluste < 1)
        {
            throw new InvalidOperationException("Es muss mindestens eine Einheit im Land verbleiben.");
        }
        Einheiten -= (uint)angreiferVerluste;

        if(verteidiger.Einheiten - verteidigerVerluste < 0)
        {
            throw new InvalidOperationException("Es dürfen nicht mehr Einheiten verloren gehen als vorhanden sind.");
        }

        verteidiger.Einheiten -= (uint)verteidigerVerluste;

        if(verteidiger.Einheiten == 0)
        {
            verteidiger.SetzeEinheitenUndBesetze(BesitzerSpielerId, 1);
            Einheiten--;
            eingenommen = true;
        }

        return new AngriffErgebnis(angreiferVerluste, verteidigerVerluste, eingenommen);
    }
}