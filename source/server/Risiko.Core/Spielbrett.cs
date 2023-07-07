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