public class XRPCToggler : XRToggler
{

    public override bool InitOnly { get; } = false;
    public override ToggleMode Toggle { get; } = ToggleMode.Toggle;

    public XRPCToggler() : base()
    {
    }

    protected override bool ShouldEnable()
    {
        return XRManager.HasXRDevices;
    }
}
