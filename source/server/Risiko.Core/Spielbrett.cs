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

        var europa = new Kontinent("Europa", 5, new List<Land>
        {
            new("Island"),
            new("Skandinavien"),
            new("Russland"),
            new("Großbritannien"),
            new("Nordeuropa"),
            new("Westeuropa"),
            new("Südeuropa")
        });
        var afrika = new Kontinent("Afrika", 4, new List<Land>
        {
            new("Nordafrika"),
            new("Ägypten"),
            new("Ostafrika"),
            new("Zentralafrika"),
            new("Südafrika"),
            new("Madagaskar")
        });
        var asien = new Kontinent("Asien", 7, new List<Land>
        {
            new("Ural"),
            new("Sibirien"),
            new("Jakutien"),
            new("Kamtschatka"),
            new("Irkutsk"),
            new("Mongolei"),
            new("Japan"),
            new("China"),
            new("Mittlerer Osten"),
            new("Indien"),
            new("Siam"),
            new("Afghanistan")
        });
        var australien = new Kontinent("Australien", 3, new List<Land>
        {
            new("Indonesien"),
            new("Neu-Guinea"),
            new("West-Australien"),
            new("Ost-Australien")
        });
        var suedamerika = new Kontinent("Südamerika", 3, new List<Land>
        {
            new("Venezuela"),
            new("Peru"),
            new("Brasilien"),
            new("Argentinien")
        });
        var nordamerika = new Kontinent("Nordamerika", 6, new List<Land>
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

        Kontinente = ImmutableList.Create(europa, afrika, asien, australien, suedamerika, nordamerika);

        if (Spieler.Count == 2)
        {
            Spieler.First().ErhalteEinheiten(40);
            Spieler.Last().ErhalteEinheiten(40);
        }

        VerteileEinheitenAufLaender();
        VergibNeueEinheitenAnAktivenSpieler();
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