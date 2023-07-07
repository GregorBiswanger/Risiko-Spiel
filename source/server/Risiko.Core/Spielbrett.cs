using Risiko.Core.Kontinente;
using System.Collections.Immutable;

namespace Risiko.Core;

public class Spielbrett
{
    public ImmutableList<Spieler> Spieler { get; private set; } = ImmutableList<Spieler>.Empty;
    public ImmutableList<Kontinent> Kontinente { get; private set; } = ImmutableList<Kontinent>.Empty;
    public Spieler AktiverSpieler => Spieler[_aktiverSpielerIndex];
    private int _aktiverSpielerIndex = 0;

    private readonly Random _rand = new();

    public void StarteSpiel(Spieler spieler, Spieler spieler2)
    {
        Spieler = ImmutableList.Create(spieler, spieler2);
        Kontinente = ImmutableList.Create<Kontinent>(new Europa(), new Afrika(), new Asien(), new Australien(), new Suedamerika(), new Nordamerika());
        LadeLaenderVerbindungen();

        if (Spieler.Count == 2)
        {
            Spieler.First().ErhalteEinheiten(40);
            Spieler.Last().ErhalteEinheiten(40);
        }

        VerteileEinheitenAufLaender();
        VergibNeueEinheitenAnAktivenSpieler();
    }

    private void LadeLaenderVerbindungen()
    {
        var europa = Kontinente.First(k => k.Name == "Europa");
        // var afrika = Kontinente.First(k => k.Name == "Afrika");
        // var asien = Kontinente.First(k => k.Name == "Asien");
        // var australien = Kontinente.First(k => k.Name == "Australien");
        // var suedamerika = Kontinente.First(k => k.Name == "Südamerika");
        // var nordamerika = Kontinente.First(k => k.Name == "Nordamerika");

        // Europa
        var island = europa.Laender.First(l => l.Name == "Island");
        var skandinavien = europa.Laender.First(l => l.Name == "Skandinavien");
        var russland = europa.Laender.First(l => l.Name == "Russland");
        var großbritannien = europa.Laender.First(l => l.Name == "Großbritannien");
        var nordeuropa = europa.Laender.First(l => l.Name == "Nordeuropa");
        var westeuropa = europa.Laender.First(l => l.Name == "Westeuropa");
        var südeuropa = europa.Laender.First(l => l.Name == "Südeuropa");

        // Europa Verbindungen
        island.AngrenzendeLaender.AddRange(new List<Land> { großbritannien });
        großbritannien.AngrenzendeLaender.AddRange(new List<Land> { island, skandinavien, nordeuropa, westeuropa });
        skandinavien.AngrenzendeLaender.AddRange(new List<Land> { island, großbritannien, russland, nordeuropa });
        russland.AngrenzendeLaender.AddRange(new List<Land> { skandinavien, nordeuropa, südeuropa });
        nordeuropa.AngrenzendeLaender.AddRange(new List<Land> { skandinavien, großbritannien, russland, westeuropa, südeuropa });
        westeuropa.AngrenzendeLaender.AddRange(new List<Land> { großbritannien, nordeuropa, südeuropa });
        südeuropa.AngrenzendeLaender.AddRange(new List<Land> { nordeuropa, westeuropa, russland });


        // // Afrika
        // var nordafrika = afrika.Laender.First(l => l.Name == "Nordafrika");
        // var ostafrika = afrika.Laender.First(l => l.Name == "Ostafrika");
        // var zentralafrika = afrika.Laender.First(l => l.Name == "Zentralafrika");
        // var suedafrika = afrika.Laender.First(l => l.Name == "Südafrika");
        // var aegypten = afrika.Laender.First(l => l.Name == "Ägypten");
        // var madagaskar = afrika.Laender.First(l => l.Name == "Madagaskar");

        // // Afrika Verbindungen
        // nordafrika.AngrenzendeLaender.AddRange(new List<Land> { westeuropa, aegypten, zentralafrika });
        // ostafrika.AngrenzendeLaender.AddRange(new List<Land> { nordafrika, zentralafrika, suedafrika, aegypten, madagaskar });
        // zentralafrika.AngrenzendeLaender.AddRange(new List<Land> { nordafrika, ostafrika, suedafrika });
        // suedafrika.AngrenzendeLaender.AddRange(new List<Land> { zentralafrika, ostafrika, madagaskar });
        // aegypten.AngrenzendeLaender.AddRange(new List<Land> { nordafrika, ostafrika, mitteleuropa });
        // madagaskar.AngrenzendeLaender.AddRange(new List<Land> { ostafrika, suedafrika });

        // // Asien
        // var sibirien = asien.Laender.First(l => l.Name == "Sibirien");
        // var ural = asien.Laender.First(l => l.Name == "Ural");
        // var tatarstan = asien.Laender.First(l => l.Name == "Tatarstan");
        // var jakutien = asien.Laender.First(l => l.Name == "Jakutien");
        // var kamtschatka = asien.Laender.First(l => l.Name == "Kamtschatka");
        // var japan = asien.Laender.First(l => l.Name == "Japan");

        // // Asien Verbindungen
        // sibirien.AngrenzendeLaender.AddRange(new List<Land> { ural, tatarstan, jakutien });
        // ural.AngrenzendeLaender.AddRange(new List<Land> { sibirien, tatarstan });
        // tatarstan.AngrenzendeLaender.AddRange(new List<Land> { ural, sibirien, kamtschatka });
        // jakutien.AngrenzendeLaender.AddRange(new List<Land> { sibirien, kamtschatka });
        // kamtschatka.AngrenzendeLaender.AddRange(new List<Land> { tatarstan, jakutien, japan });
        // japan.AngrenzendeLaender.AddRange(new List<Land> { kamtschatka });

        // // Australien
        // var westaustralien = australien.Laender.First(l => l.Name == "Westaustralien");
        // var ostaustralien = australien.Laender.First(l => l.Name == "Ostaustralien");
        // var indonesien = australien.Laender.First(l => l.Name == "Indonesien");
        // var neuguinea = australien.Laender.First(l => l.Name == "Neuguinea");

        // // Australien Verbindungen
        // westaustralien.AngrenzendeLaender.AddRange(new List<Land> { ostaustralien, indonesien });
        // ostaustralien.AngrenzendeLaender.AddRange(new List<Land> { westaustralien, neuguinea });
        // indonesien.AngrenzendeLaender.AddRange(new List<Land> { westaustralien, neuguinea });
        // neuguinea.AngrenzendeLaender.AddRange(new List<Land> { ostaustralien, indonesien });

        // // Südamerika
        // var venezuela = suedamerika.Laender.First(l => l.Name == "Venezuela");
        // var peru = suedamerika.Laender.First(l => l.Name == "Peru");
        // var argentinien = suedamerika.Laender.First(l => l.Name == "Argentinien");
        // var brasilien = suedamerika.Laender.First(l => l.Name == "Brasilien");

        // // Südamerika Verbindungen
        // venezuela.AngrenzendeLaender.AddRange(new List<Land> { peru, brasilien });
        // peru.AngrenzendeLaender.AddRange(new List<Land> { venezuela, argentinien, brasilien });
        // argentinien.AngrenzendeLaender.AddRange(new List<Land> { peru, brasilien });
        // brasilien.AngrenzendeLaender.AddRange(new List<Land> { venezuela, peru, argentinien });

        // // Nordamerika
        // var alaska = nordamerika.Laender.First(l => l.Name == "Alaska");
        // var nordwestterritorium = nordamerika.Laender.First(l => l.Name == "Nordwest-Territorium");
        // var grönland = nordamerika.Laender.First(l => l.Name == "Grönland");
        // var ostkanada = nordamerika.Laender.First(l => l.Name == "Ostkanada");
        // var weststaaten = nordamerika.Laender.First(l => l.Name == "Weststaaten");
        // var oststaaten = nordamerika.Laender.First(l => l.Name == "Oststaaten");
        // var mittelamerika = nordamerika.Laender.First(l => l.Name == "Mittelamerika");

        // // Nordamerika Verbindungen
        // alaska.AngrenzendeLaender.AddRange(new List<Land> { nordwestterritorium, weststaaten });
        // nordwestterritorium.AngrenzendeLaender.AddRange(new List<Land> { alaska, grönland, ostkanada, weststaaten });
        // grönland.AngrenzendeLaender.AddRange(new List<Land> { nordwestterritorium, ostkanada });
        // ostkanada.AngrenzendeLaender.AddRange(new List<Land> { nordwestterritorium, grönland, weststaaten, oststaaten });
        // weststaaten.AngrenzendeLaender.AddRange(new List<Land> { alaska, nordwestterritorium, ostkanada, oststaaten, mittelamerika });
        // oststaaten.AngrenzendeLaender.AddRange(new List<Land> { ostkanada, weststaaten, mittelamerika });
        // mittelamerika.AngrenzendeLaender.AddRange(new List<Land> { weststaaten, oststaaten });

        // // Verbindungen zwischen den Kontinenten
        // westeuropa.AngrenzendeLaender.Add(nordafrika);
        // nordafrika.AngrenzendeLaender.Add(westeuropa);
        // nordafrika.AngrenzendeLaender.Add(ostafrika); // Verbindung zu Asien über das Mittelmeer
        // ostafrika.AngrenzendeLaender.Add(nordafrika);
        // aegypten.AngrenzendeLaender.Add(mitteleuropa); // Verbindung zu Europa über das Mittelmeer
        // mitteleuropa.AngrenzendeLaender.Add(aegypten);
        // alaska.AngrenzendeLaender.Add(kamtschatka); // Verbindung zu Asien über die Beringstraße
        // kamtschatka.AngrenzendeLaender.Add(alaska);
        // mittelamerika.AngrenzendeLaender.Add(venezuela); // Verbindung zu Südamerika
        // venezuela.AngrenzendeLaender.Add(mittelamerika);
        // indonesien.AngrenzendeLaender.Add(westaustralien); // Verbindung zu Australien
        // westaustralien.AngrenzendeLaender.Add(indonesien);
    }

    public void VerteileEinheitenAufLaender()
    {
        var laender = Kontinente.SelectMany(k => k.Laender)
            .OrderBy(x => _rand.Next())
            .ToList();

        foreach (var spieler in Spieler)
        {
            foreach (var land in laender)
            {
                if (land.BesitzerSpielerId == Guid.Empty && spieler.FreieEinheiten > 0)
                {
                    var einheiten = spieler.FreieEinheiten > 2 ? spieler.NimmEinheiten(2) : spieler.NimmEinheiten(1);
                    land.SetzeEinheitenUndBesetze(spieler.Id, einheiten);
                }
            }
        }
    }

    public void LadeSpielstand(List<Spieler> spieler, List<Kontinent> kontinente, Spieler aktiverSpieler)
    {
        Spieler = spieler.ToImmutableList();
        Kontinente = kontinente.ToImmutableList();
        _aktiverSpielerIndex = spieler.IndexOf(aktiverSpieler);
    }

    public void BeendeSpielzug()
    {
        WechsleZuNächstemSpieler();
        VergibNeueEinheitenAnAktivenSpieler();
    }

    private void WechsleZuNächstemSpieler()
    {
        _aktiverSpielerIndex = (_aktiverSpielerIndex + 1) % Spieler.Count;
    }

    private void VergibNeueEinheitenAnAktivenSpieler()
    {
        var extraEinheiten = BerechneNeueEinheiten(AktiverSpieler);
        AktiverSpieler.ErhalteEinheiten(extraEinheiten);
    }

    private uint BerechneNeueEinheiten(Spieler spieler)
    {
        var anzahlLänder = HoleAnzahlKontrollierteLänder(spieler);
        var basisEinheiten = Math.Max(anzahlLänder / 3, 3);
        var kontinentBonus = BerechneKontinentKontrollBonus(spieler);

        return basisEinheiten + kontinentBonus;
    }

    private uint HoleAnzahlKontrollierteLänder(Spieler spieler)
    {
        return (uint)Kontinente.SelectMany(kontinent => kontinent.Laender)
                .Count(land => land.BesitzerSpielerId == spieler.Id);
    }

    private uint BerechneKontinentKontrollBonus(Spieler spieler)
    {
        return (uint)Kontinente.Where(kontinent => kontinent.Laender.All(land => land.BesitzerSpielerId == spieler.Id)).Sum(kontinent => kontinent.KontrollBonus);
    }
}