namespace Risiko.Core;

public class Wuerfel
{
    public WuerfelAugen Augen { get; private set; }

    public Wuerfel(WuerfelAugen augen)
    {
        this.Augen = augen;
    }
}