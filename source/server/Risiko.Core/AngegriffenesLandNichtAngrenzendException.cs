using Risiko.Core;

public class AngegriffenesLandNichtAngrenzendException : Exception 
{
    public AngegriffenesLandNichtAngrenzendException(Land angegriffenesLand, Land angreifendesLand) : base($"Das Land {angegriffenesLand.Name} ist nicht angrenzend zu {angreifendesLand.Name}") {
    }
}