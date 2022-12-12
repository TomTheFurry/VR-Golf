public class XRPCToggler : XRToggler 
{
    public bool OnlyOnInit = true;
    public new ToggleMode ToggleMode = ToggleMode.Destory;

<<<<<<< HEAD
    public override bool InitOnly { get; } = false;
    public override ToggleMode Toggle { get; } = ToggleMode.Toggle;
=======
    public bool PCMode = false;

    public override bool InitOnly => OnlyOnInit;
    public override ToggleMode Toggle => ToggleMode;
>>>>>>> 6955565a3ebc6c3bee8eff621efc8ea04b6427e5

    public XRPCToggler() : base()
    {
    }

    protected override bool ShouldEnable()
    {
        return PCMode ^ XRManager.HasXRDevices;
    }
}
