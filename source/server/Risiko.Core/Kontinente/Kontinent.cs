namespace Risiko.Core.Kontinente;

public abstract record Kontinent(string Name, uint KontrollBonus, IReadOnlyCollection<Land> Laender);