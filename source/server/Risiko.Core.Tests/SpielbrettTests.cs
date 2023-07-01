using FluentAssertions;

namespace Risiko.Core.Tests
{
    public class SpielbrettTests
    {
        [Fact]
        public void StarteSpiel_MitZweiSpielern_SpielBereitschaftUndVerteilungDerLaender()
        {
            var spieler1 = new Spieler("André", Farbe.Blau);
            var spieler2 = new Spieler("Gregor", Farbe.Rot);

            var spielbrett = new Spielbrett();
            spielbrett.StarteSpiel(spieler1, spieler2);

            spielbrett.Spieler.Count.Should().Be(2);
            spielbrett.Spieler.First().Name.Should().Be("André");
            spielbrett.Spieler.First().Farbe.Should().Be(Farbe.Blau);
            spielbrett.Spieler.Last().Name.Should().Be("Gregor");
            spielbrett.Spieler.Last().Farbe.Should().Be(Farbe.Rot);
            spielbrett.Kontinente.Count.Should().Be(6);

            spielbrett.Kontinente.SelectMany(kontinent => kontinent.Laender).Count().Should().Be(42);
            spielbrett.AktiverSpieler.Should().Be(spieler1);

            var spieler1Laender = spielbrett.Kontinente.SelectMany(kontinent => kontinent.Laender)
                .Count(land => land.BesitzerSpielerId == spieler1.Id);
            spieler1Laender.Should().Be(21);

            var spieler2Laender = spielbrett.Kontinente.SelectMany(kontinent => kontinent.Laender)
                .Count(land => land.BesitzerSpielerId == spieler2.Id);
            spieler2Laender.Should().Be(21);
        }

        [Fact]
        public void BeendeSpielzug_NachErstemSpielzug_EinheitenDesZweitenSpielers()
        {
            var spieler1 = new Spieler("André", Farbe.Blau);
            var spieler2 = new Spieler("Gregor", Farbe.Rot);

            var no = new Land("Nordeuropa");
            no.SetzeEinheitenUndBesetze(spieler1.Id, 2);
            var wo = new Land("Westeuropa");
            wo.SetzeEinheitenUndBesetze(spieler2.Id, 2);

            var europa = new Kontinent("Europa", 3, new List<Land>
            {
                no, 
                wo
            });

            var spielbrett = new Spielbrett();
            spielbrett.LadeSpielstand(new List<Spieler> { spieler1, spieler2 }, new List<Kontinent>{ europa }, spieler1);
            spielbrett.BeendeSpielzug();
            spielbrett.AktiverSpieler.Should().Be(spieler2);
            spieler2.FreieEinheiten.Should().Be(3);
        }

        [Fact]
        public void BeendeSpielzug_NachZweitenSpielzug_EinheitenDesErstenSpielers()
        {
            var spieler1 = new Spieler("André", Farbe.Blau);
            var spieler2 = new Spieler("Gregor", Farbe.Rot);

            var no = new Land("Nordeuropa");
            no.SetzeEinheitenUndBesetze(spieler1.Id, 2);
            var wo = new Land("Westeuropa");
            wo.SetzeEinheitenUndBesetze(spieler2.Id, 2);

            var europa = new Kontinent("Europa", 3, new List<Land>
            {
                no,
                wo
            });

            var spielbrett = new Spielbrett();
            spielbrett.LadeSpielstand(new List<Spieler> { spieler1, spieler2 }, new List<Kontinent> { europa }, spieler1);
            spielbrett.BeendeSpielzug();
            spielbrett.BeendeSpielzug();
            spielbrett.AktiverSpieler.Should().Be(spieler1);
            spieler1.FreieEinheiten.Should().Be(3);
        }

        [Fact]
        public void BeendeSpielzug_ZweiterSpielerHatMehrereLaender_EinheitenDesErstenSpielers()
        {
            var spieler1 = new Spieler("André", Farbe.Blau);
            var spieler2 = new Spieler("Gregor", Farbe.Rot);

            // Spieler 1 mit 2 Ländern
            var no = new Land("Nordeuropa");
            no.SetzeEinheitenUndBesetze(spieler1.Id, 2);
            var gb = new Land("Großbritannien");
            gb.SetzeEinheitenUndBesetze(spieler1.Id, 2);

            // Spieler 2 mit 1 Land
            var wo = new Land("Westeuropa");
            wo.SetzeEinheitenUndBesetze(spieler2.Id, 2);

            var europa = new Kontinent("Europa", 3, new List<Land>
            {
                no,
                gb,
                wo
            });

            var spielbrett = new Spielbrett();
            spielbrett.LadeSpielstand(new List<Spieler> { spieler1, spieler2 }, new List<Kontinent> { europa }, spieler1);
            spielbrett.BeendeSpielzug();
            spielbrett.BeendeSpielzug();
            spielbrett.AktiverSpieler.Should().Be(spieler1);
            spieler1.FreieEinheiten.Should().Be(3);
        }

        [Fact]
        public void BeendeSpielzug_KomplettesKontinentEinemSpielerZugehoerig_FreieEinheitenDesSpielers()
        {
            var spieler1 = new Spieler("André", Farbe.Blau);
            var spieler2 = new Spieler("Gregor", Farbe.Rot);

            var land1 = new Land("Land1");
            land1.SetzeEinheitenUndBesetze(spieler1.Id, 1);
            var land2 = new Land("Land2");
            land2.SetzeEinheitenUndBesetze(spieler1.Id, 1);
            var land3 = new Land("Land3");
            land3.SetzeEinheitenUndBesetze(spieler1.Id, 1);
            var land4 = new Land("Land4");
            land4.SetzeEinheitenUndBesetze(spieler1.Id, 1);
            var land5 = new Land("Land5");
            land5.SetzeEinheitenUndBesetze(spieler1.Id, 1);
            var land6 = new Land("Land6");
            land6.SetzeEinheitenUndBesetze(spieler1.Id, 1);

            var kontinent = new Kontinent("Kontinent", 3, new List<Land>
            {
                land1,
                land2,
                land3,
                land4,
                land5,
                land6
            });

            var spielbrett = new Spielbrett();
            spielbrett.LadeSpielstand(new List<Spieler> { spieler1, spieler2 }, new List<Kontinent> { kontinent }, spieler2);
            spielbrett.BeendeSpielzug();
            spielbrett.AktiverSpieler.Should().Be(spieler1);
            spieler1.FreieEinheiten.Should().Be(6);
        }
    }
}