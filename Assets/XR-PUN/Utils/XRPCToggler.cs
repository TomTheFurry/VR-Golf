public class XRPCToggler : XRToggler 
{
    public bool OnlyOnInit = true;
    public new ToggleMode ToggleMode = ToggleMode.EnableOrDestry;

    public bool PCMode = false;

    public override bool InitOnly => OnlyOnInit;
    public override ToggleMode Toggle => ToggleMode;

    public XRPCToggler() : base()
    {
    }

    protected override bool ShouldEnable()
    {
        return PCMode ^ XRManager.HasXRDevices;
    }
}
