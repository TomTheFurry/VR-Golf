public class XRPCToggler : XRToggler
{

    public override bool InitOnly { get; set; } = false;
    public override ToggleMode Toggle { get; set; } = ToggleMode.Toggle;

    public XRPCToggler() : base()
    {
    }

    protected override bool ShouldEnable()
    {
        return XRManager.HasXRDevices;
    }
}
