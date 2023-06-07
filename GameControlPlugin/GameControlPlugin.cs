namespace Loupedeck.GameControlPlugin
{
    using System;
    using System.Diagnostics;
    using System.IO;

    using vJoyInterfaceWrap;

    public class GameControlPlugin : Plugin
    {
        public static vJoy joystick;
        public static vJoy.JoystickState iReport;
        public static uint id = 1;
        public static int X;
        public static int Y;
        public static int Z;
        public static int RX;
        public static int RY;
        public static int RZ;
        public static int SL0;
        public static int SL1;
        public static bool[] buttons = new bool[200];
        public static uint count = 0;
        public static long maxValue;
        public static int nButtons;
        public static int ContPovNumber;
        public static int DiscPovNumber;
        public static bool DefaultDrawNumbers = true;
        public static bool DefaultDrawToggleIndicators = true;
        public static BitmapColor DefaultLabelBackgroundColor = BitmapColor.Transparent;
        public static BitmapColor DefaultLabelColor = BitmapColor.White;
        public static int DefaultLabelSize = 14;
        public static int DefaultLabelPos = 7;
        public static int DefaultDXSendType;
        public static bool DefaultToggleAsButton;
        public static int DefaultAdjustmentSpeed = 100;
        public static string ToggleOnOffOnUpResourcePath;
        public static string ToggleOnOffOnMiddleResourcePath;
        public static string ToggleOnOffOnDownResourcePath;
        public static string ToggleOnOnOnUpResourcePath;
        public static string ToggleOnOnOnMiddleResourcePath;
        public static string ToggleOnOnOnDownResourcePath;
        public static string ToggleOnOnUpResourcePath;
        public static string ToggleOnOnDownResourcePath;
        public static string ToggleOnOffUpResourcePath;
        public static string ToggleOnOffDownResourcePath;
        public static string ToggleDownResourcePath;
        public static string ToggleUpResourcePath;
        public static string ToggleMiddleResourcePath;
        public static string DefaultButtonPath;
        public static string DefaultRotaryPath;
        public static string PluginError;
        public static string PluginWarning;
        public static Stopwatch PluginWarningStopwatch;
        public static bool InWarning;

        public override bool UsesApplicationApiOnly => true;

        public override bool HasNoApplication => true;

        public override void Load()
        {
            this.Info.DisplayName = "GameControl";
            this.Info.Homepage = "https://loupedeck.com/developer/";
            this.Info.Icon16x16 = EmbeddedResources.ReadImage("Loupedeck.GameControlPlugin.Resources.Icon16x16.png");
            this.Info.Icon32x32 = EmbeddedResources.ReadImage("Loupedeck.GameControlPlugin.Resources.Icon32x32.png");
            this.Info.Icon48x48 = EmbeddedResources.ReadImage("Loupedeck.GameControlPlugin.Resources.Icon48x48.png");
            this.Info.Icon256x256 = EmbeddedResources.ReadImage("Loupedeck.GameControlPlugin.Resources.Icon256x256.png");
            ToggleOnOffOnUpResourcePath = EmbeddedResources.FindFile("ToggleOnOffOnUpDark.png");
            ToggleOnOffOnMiddleResourcePath = EmbeddedResources.FindFile("ToggleOnOffOnMiddleDark.png");
            ToggleOnOffOnDownResourcePath = EmbeddedResources.FindFile("ToggleOnOffOnDownDark.png");
            ToggleOnOnOnUpResourcePath = EmbeddedResources.FindFile("ToggleOnOnOnUpDark.png");
            ToggleOnOnOnMiddleResourcePath = EmbeddedResources.FindFile("ToggleOnOnOnMiddleDark.png");
            ToggleOnOnOnDownResourcePath = EmbeddedResources.FindFile("ToggleOnOnOnDownDark.png");
            ToggleOnOnUpResourcePath = EmbeddedResources.FindFile("ToggleOnOnUpDark.png");
            ToggleOnOnDownResourcePath = EmbeddedResources.FindFile("ToggleOnOnDownDark.png");
            ToggleOnOffUpResourcePath = EmbeddedResources.FindFile("ToggleOnOffUpDark.png");
            ToggleOnOffDownResourcePath = EmbeddedResources.FindFile("ToggleOnOffDownDark.png");
            ToggleUpResourcePath = EmbeddedResources.FindFile("ToggleUpDark.png");
            ToggleDownResourcePath = EmbeddedResources.FindFile("ToggleDownDark.png");
            ToggleMiddleResourcePath = EmbeddedResources.FindFile("ToggleMiddleDark.png");
            DefaultButtonPath = EmbeddedResources.FindFile("BlackSquareButton.png");
            DefaultRotaryPath = EmbeddedResources.FindFile("GrayRotary.png");
            PluginError = "";
            PluginWarning = "";
            PluginWarningStopwatch = new Stopwatch();
            InWarning = false;
            joystick = new vJoy();
            iReport = new vJoy.JoystickState();
            if (!joystick.vJoyEnabled())
            {
                Console.WriteLine("vJoy driver not enabled: Failed Getting vJoy attributes.\n");
            }
            else
            {
                
                
                int result1 = 1;
                string pluginDataDirectory = this.GetPluginDataDirectory();
                if (IoHelpers.EnsureDirectoryExists(pluginDataDirectory))
                {
                    string path = Path.Combine(pluginDataDirectory, "Settings.txt");
                    if (File.Exists(path))
                    {
                        using (StreamReader streamReader = new StreamReader(path))
                        {
                            for (string text = streamReader.ReadLine(); text != null; text = streamReader.ReadLine())
                            {
                                string str = text.Trim();
                                if (str.Length > 0 && str[0] != '#')
                                {
                                    string[] strArray = text.Split("=");
                                    string lower1 = strArray[0].Trim().ToLower();
                                    if (strArray.Length == 2)
                                    {
                                        string lower2 = strArray[1].Trim().ToLower();
                                        switch (lower1)
                                        {
                                            case "adjustmentspeed":
                                            case "as":
                                                switch (lower2)
                                                {
                                                    case "f":
                                                    case "fast":
                                                        DefaultAdjustmentSpeed = 500;
                                                        continue;
                                                    case "n":
                                                    case "norm":
                                                    case "normal":
                                                        DefaultAdjustmentSpeed = 100;
                                                        continue;
                                                    case "s":
                                                    case "slow":
                                                        DefaultAdjustmentSpeed = 25;
                                                        continue;
                                                    default:
                                                        if (!int.TryParse(lower2, out DefaultAdjustmentSpeed))
                                                        {
                                                            this.OnPluginStatusChanged(Loupedeck.PluginStatus.Error, "Invalid Default Adjustment Speed (not a number). Fix in settings.txt and restart.");
                                                        }

                                                        continue;
                                                }
                                            case "bt":
                                            case "buttontype":
                                                switch (lower2)
                                                {
                                                    case "black round":
                                                    case "br":
                                                    case "bround":
                                                        DefaultButtonPath = EmbeddedResources.FindFile("BlackRoundButton.png");
                                                        continue;
                                                    case "ccw":
                                                    case "counterclockwise":
                                                        DefaultButtonPath = EmbeddedResources.FindFile("BlackSquareCCWButton.png");
                                                        continue;
                                                    case "clockwise":
                                                    case "cw":
                                                        DefaultButtonPath = EmbeddedResources.FindFile("BlackSquareCWButton.png");
                                                        continue;
                                                    case "d":
                                                    case "down":
                                                        DefaultButtonPath = EmbeddedResources.FindFile("BlackSquareDownButton.png");
                                                        continue;
                                                    case "gr":
                                                    case "gray round":
                                                    case "ground":
                                                        DefaultButtonPath = EmbeddedResources.FindFile("GrayRoundButton.png");
                                                        continue;
                                                    case "gray square":
                                                    case "gs":
                                                    case "gsquare":
                                                        DefaultButtonPath = EmbeddedResources.FindFile("GraySquareButton.png");
                                                        continue;
                                                    case "l":
                                                    case "left":
                                                        DefaultButtonPath = EmbeddedResources.FindFile("BlackSquareLeftButton.png");
                                                        continue;
                                                    case "r":
                                                    case "right":
                                                        DefaultButtonPath = EmbeddedResources.FindFile("BlackSquareRightButton.png");
                                                        continue;
                                                    case "red round":
                                                    case "rr":
                                                    case "rround":
                                                        DefaultButtonPath = EmbeddedResources.FindFile("RedRoundButton.png");
                                                        continue;
                                                    case "u":
                                                    case "up":
                                                        DefaultButtonPath = EmbeddedResources.FindFile("BlackSquareUpButton.png");
                                                        continue;
                                                    default:
                                                        DefaultButtonPath = EmbeddedResources.FindFile("BlackSquareButton.png");
                                                        continue;
                                                }
                                            case "dn":
                                            case "drawnumbers":
                                                switch (lower2)
                                                {
                                                    case "true":
                                                    case "on":
                                                    case "yes":
                                                        DefaultDrawNumbers = true;
                                                        continue;
                                                    case "false":
                                                    case "off":
                                                    case "no":
                                                        DefaultDrawNumbers = false;
                                                        continue;
                                                    default:
                                                        continue;
                                                }
                                            case "drawtoggleindicators":
                                            case "dti":
                                                switch (lower2)
                                                {
                                                    case "true":
                                                    case "on":
                                                    case "yes":
                                                        DefaultDrawToggleIndicators = true;
                                                        continue;
                                                    case "false":
                                                    case "off":
                                                    case "no":
                                                        DefaultDrawToggleIndicators = false;
                                                        continue;
                                                    default:
                                                        continue;
                                                }
                                            case "dx":
                                            case "dxsendtype":
                                            case "dxst":
                                                switch (lower2)
                                                {
                                                    case "pulse":
                                                    case "p":
                                                        DefaultDXSendType = 0;
                                                        continue;
                                                    case "hold":
                                                    case "h":
                                                        DefaultDXSendType = 1;
                                                        continue;
                                                    default:
                                                        continue;
                                                }
                                            case "labelbackgroundcolor":
                                            case "lb":
                                            case "lbc":
                                                switch (lower2)
                                                {
                                                    case "black":
                                                        DefaultLabelBackgroundColor = BitmapColor.Black;
                                                        continue;
                                                    case "blue":
                                                        DefaultLabelBackgroundColor = new BitmapColor(50, 50, 200);
                                                        continue;
                                                    case "gray":
                                                        DefaultLabelBackgroundColor = new BitmapColor(128, 128, 128);
                                                        continue;
                                                    case "green":
                                                        DefaultLabelBackgroundColor = new BitmapColor(50, 200, 50);
                                                        continue;
                                                    case "none":
                                                        DefaultLabelBackgroundColor = BitmapColor.Transparent;
                                                        continue;
                                                    case "purple":
                                                        DefaultLabelBackgroundColor = new BitmapColor(150, 50, 200);
                                                        continue;
                                                    case "red":
                                                        DefaultLabelBackgroundColor = new BitmapColor(200, 50, 50);
                                                        continue;
                                                    default:
                                                        uint result2 = 0;
                                                        if (!uint.TryParse(lower2, out result2))
                                                        {
                                                            this.OnPluginStatusChanged(Loupedeck.PluginStatus.Error, "Invalid Default Label Background Color. Fix in settings.txt and restart.");
                                                            continue;
                                                        }

                                                        DefaultLabelBackgroundColor = new BitmapColor(result2);
                                                        continue;
                                                }
                                            case "labelcolor":
                                            case "lc":
                                                switch (lower2)
                                                {
                                                    case "black":
                                                        DefaultLabelColor = BitmapColor.Black;
                                                        continue;
                                                    case "blue":
                                                        DefaultLabelColor = new BitmapColor(50, 50, 200);
                                                        continue;
                                                    case "gray":
                                                        DefaultLabelColor = new BitmapColor(128, 128, 128);
                                                        continue;
                                                    case "green":
                                                        DefaultLabelColor = new BitmapColor(50, 200, 50);
                                                        continue;
                                                    case "purple":
                                                        DefaultLabelColor = new BitmapColor(150, 50, 200);
                                                        continue;
                                                    case "red":
                                                        DefaultLabelColor = new BitmapColor(200, 50, 50);
                                                        continue;
                                                    case "white":
                                                        DefaultLabelColor = BitmapColor.White;
                                                        continue;
                                                    default:
                                                        uint result3 = 0;
                                                        if (!uint.TryParse(lower2, out result3))
                                                        {
                                                            this.OnPluginStatusChanged(Loupedeck.PluginStatus.Error, "Invalid Default Label Color. Fix in settings.txt and restart.");
                                                            continue;
                                                        }

                                                        DefaultLabelColor = new BitmapColor(result3);
                                                        continue;
                                                }
                                            case "labelpos":
                                            case "lp":
                                                switch (lower2)
                                                {
                                                    case "top":
                                                    case "t":
                                                        DefaultLabelPos = 7;
                                                        continue;
                                                    case "center":
                                                    case "c":
                                                        DefaultLabelPos = 40;
                                                        continue;
                                                    case "bottom":
                                                    case "b":
                                                        DefaultLabelPos = 73;
                                                        continue;
                                                    default:
                                                        if (!int.TryParse(lower2, out DefaultLabelPos))
                                                        {
                                                            this.OnPluginStatusChanged(Loupedeck.PluginStatus.Error, "Invalid Default Label Position (must be top, center, bottom, or a number). Fix in settings.txt and restart.");
                                                        }

                                                        continue;
                                                }
                                            case "labelsize":
                                            case "ls":
                                                int result4 = 14;
                                                if (!int.TryParse(lower2, out result4))
                                                {
                                                    this.OnPluginStatusChanged(Loupedeck.PluginStatus.Error, "Invalid Default Label Size (must be a number). Fix in settings.txt and restart.");
                                                    continue;
                                                }

                                                DefaultLabelSize = 14;
                                                continue;
                                            case "rotarytype":
                                            case "rt":
                                                if (!(lower2 == "gray"))
                                                {
                                                    int num = lower2 == "g" ? 1 : 0;
                                                }

                                                DefaultRotaryPath = EmbeddedResources.FindFile("GrayRotary.png");
                                                continue;
                                            case "tab":
                                            case "toggleasbutton":
                                                switch (lower2)
                                                {
                                                    case "true":
                                                    case "on":
                                                    case "yes":
                                                        DefaultToggleAsButton = true;
                                                        continue;
                                                    case "false":
                                                    case "off":
                                                    case "no":
                                                        DefaultToggleAsButton = false;
                                                        continue;
                                                    default:
                                                        continue;
                                                }
                                            case "vjoy number":
                                                if (int.TryParse(lower2, out result1))
                                                {
                                                    if (result1 > 0 && result1 < 17)
                                                    {
                                                        id = (uint)result1;
                                                        continue;
                                                    }

                                                    this.OnPluginStatusChanged(Loupedeck.PluginStatus.Error, "Invalid vJoy ID.  Value must be between 1 and 16.  Please fix in settings.txt and restart");
                                                    continue;
                                                }

                                                this.OnPluginStatusChanged(Loupedeck.PluginStatus.Error, "Invalid vJoy ID.  A number between 1 and 16 must be set.  Please fix in settings.txt and restart");
                                                continue;
                                            default:
                                                continue;
                                        }
                                    }

                                    this.OnPluginStatusChanged(Loupedeck.PluginStatus.Error, "Invalid settings.txt option format (must be [Option] = [Value]). Fix in settings.txt and restart.");
                                }
                            }
                        }
                    }
                    else
                    {
                        using (StreamWriter streamWriter = new StreamWriter(path))
                        {
                            streamWriter.WriteLine("vJoy Number = 1");
                            streamWriter.WriteLine("");
                            streamWriter.WriteLine("## Defaults ##");
                            streamWriter.WriteLine("LabelBackgroundColor = None");
                            streamWriter.WriteLine("LabelColor = White");
                            streamWriter.WriteLine("LabelSize = 14");
                            streamWriter.WriteLine("DrawNumbers = True");
                            streamWriter.WriteLine("ButtonType = Black Square");
                            streamWriter.WriteLine("RotaryType = Gray");
                            streamWriter.WriteLine("LabelPos = Top");
                            streamWriter.WriteLine("DrawToggleIndicators = True");
                            streamWriter.WriteLine("DXSendType = Pulsed");
                            streamWriter.WriteLine("ToggleAsButton = False");
                            streamWriter.WriteLine("AdjustmentSpeed = 100");
                        }
                    }
                }

                VjdStat vjdStatus = joystick.GetVJDStatus(id);
                switch (vjdStatus)
                {
                    case VjdStat.VJD_STAT_OWN:
                        this.OnPluginStatusChanged(Loupedeck.PluginStatus.Error, "vJoy Device is already owned by this feeder");
                        goto case VjdStat.VJD_STAT_FREE;
                    case VjdStat.VJD_STAT_FREE:
                        joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_X);
                        joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_Y);
                        joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_Z);
                        joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_RX);
                        joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_RY);
                        joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_RZ);
                        joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_SL0);
                        joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_SL1);
                        nButtons = joystick.GetVJDButtonNumber(id);
                        ContPovNumber = joystick.GetVJDContPovNumber(id);
                        DiscPovNumber = joystick.GetVJDDiscPovNumber(id);
                        uint DllVer = 0;
                        uint DrvVer = 0;
                        if (!joystick.DriverMatch(ref DllVer, ref DrvVer))
                            this.OnPluginStatusChanged(Loupedeck.PluginStatus.Error, "Version of Driver does NOT match DLL Version.");
                        switch (vjdStatus)
                        {
                            case VjdStat.VJD_STAT_OWN:
                                this.OnPluginStatusChanged(Loupedeck.PluginStatus.Error, "Failed to acquire vJoy device");
                                return;
                            case VjdStat.VJD_STAT_FREE:
                                if (joystick.AcquireVJD(id))
                                    break;
                                goto case VjdStat.VJD_STAT_OWN;
                        }

                        joystick.GetVJDAxisMax(id, HID_USAGES.HID_USAGE_X, ref maxValue);
                        joystick.ResetVJD(id);
                        for (uint nBtn = 0; nBtn < nButtons; ++nBtn)
                            joystick.SetBtn(false, id, nBtn);
                        int num1;
                        RX = num1 = (int)maxValue / 2;
                        RX = num1;
                        RX = num1;
                        Z = num1;
                        Y = num1;
                        X = num1;
                        SL0 = SL1 = 0;
                        break;
                    case VjdStat.VJD_STAT_BUSY:
                        this.OnPluginStatusChanged(Loupedeck.PluginStatus.Error, "vJoy Device is already owned by another feeder. Cannot continue");
                        break;
                    case VjdStat.VJD_STAT_MISS:
                        this.OnPluginStatusChanged(Loupedeck.PluginStatus.Error, "vJoy Device is not installed or disabled.Cannot continue");
                        break;
                    default:
                        this.OnPluginStatusChanged(Loupedeck.PluginStatus.Error, "vJoy Device general error. Cannot continue");
                        break;
                }
            }
        }

        public override void Unload()
        {
        }

        private void OnApplicationStarted(object sender, EventArgs e)
        {
        }

        private void OnApplicationStopped(object sender, EventArgs e)
        {
        }

        public override void RunCommand(string commandName, string parameter)
        {
        }

        public override void ApplyAdjustment(string adjustmentName, string parameter, int diff)
        {
        }

        public static CommandInfoType GetCommandInfo(string parameter)
        {
            CommandInfoType commandInfo = new CommandInfoType();
            commandInfo.Value = -1;
            commandInfo.Label = "";
            commandInfo.LabelBackgroundColor = DefaultLabelBackgroundColor;
            commandInfo.LabelColor = DefaultLabelColor;
            commandInfo.LabelSize = DefaultLabelSize;
            commandInfo.LabelPos = DefaultLabelPos;
            commandInfo.DrawNumbers = DefaultDrawNumbers;
            commandInfo.DrawToggleIndicators = DefaultDrawToggleIndicators;
            commandInfo.ButtonPath = DefaultButtonPath;
            commandInfo.RotaryPath = DefaultRotaryPath;
            commandInfo.DXSendType = DefaultDXSendType;
            commandInfo.ToggleAsButton = DefaultToggleAsButton;
            string[] strArray1;
            if (parameter != null)
                strArray1 = parameter.Split(";");
            else
                strArray1 = new string[1] { "" };
            for (int index = 0; index < strArray1.Length; ++index)
            {
                int result1 = -1;
                string[] strArray2 = strArray1[index].Split("=");
                string lower1 = strArray2[0].Trim().ToLower();
                if (strArray2.Length == 1)
                {
                    if (int.TryParse(lower1, out result1))
                    {
                        commandInfo.Value = result1;
                    }
                    else
                    {
                        switch (lower1)
                        {
                            case "c":
                            case "center":
                                commandInfo.Value = 0;
                                continue;
                            case "d":
                            case "down":
                                commandInfo.Value = 2;
                                commandInfo.ButtonPath = EmbeddedResources.FindFile("BlackSquareDownButton.png");
                                continue;
                            case "f":
                            case "fast":
                                commandInfo.Value = 500;
                                continue;
                            case "l":
                            case "left":
                                commandInfo.Value = 3;
                                commandInfo.ButtonPath = EmbeddedResources.FindFile("BlackSquareLeftButton.png");
                                continue;
                            case "max":
                                commandInfo.Value = 100;
                                continue;
                            case "min":
                                commandInfo.Value = -100;
                                continue;
                            case "n":
                            case "norm":
                            case "normal":
                                commandInfo.Value = 100;
                                continue;
                            case "r":
                            case "right":
                                commandInfo.Value = 1;
                                commandInfo.ButtonPath = EmbeddedResources.FindFile("BlackSquareRightButton.png");
                                continue;
                            case "s":
                            case "slow":
                                commandInfo.Value = 25;
                                continue;
                            case "u":
                            case "up":
                                commandInfo.Value = 0;
                                commandInfo.ButtonPath = EmbeddedResources.FindFile("BlackSquareUpButton.png");
                                continue;
                            default:
                                commandInfo.Value = -1;
                                PluginWarning = "Invalid Value";
                                PluginWarningStopwatch.Restart();
                                continue;
                        }
                    }
                }
                else if (strArray2.Length == 2)
                {
                    string str = strArray2[1].Trim();
                    string lower2 = str.ToLower();
                    switch (lower1)
                    {
                        case "bt":
                        case "buttontype":
                            switch (lower2)
                            {
                                case "black round":
                                case "br":
                                case "bround":
                                    commandInfo.ButtonPath = EmbeddedResources.FindFile("BlackRoundButton.png");
                                    continue;
                                case "black square":
                                case "bs":
                                case "bsquare":
                                    commandInfo.ButtonPath = EmbeddedResources.FindFile("BlackSquareButton.png");
                                    continue;
                                case "ccw":
                                case "counterclockwise":
                                    commandInfo.ButtonPath = EmbeddedResources.FindFile("BlackSquareCCWButton.png");
                                    continue;
                                case "clockwise":
                                case "cw":
                                    commandInfo.ButtonPath = EmbeddedResources.FindFile("BlackSquareCWButton.png");
                                    continue;
                                case "d":
                                case "down":
                                    commandInfo.ButtonPath = EmbeddedResources.FindFile("BlackSquareDownButton.png");
                                    continue;
                                case "gr":
                                case "gray round":
                                case "ground":
                                    commandInfo.ButtonPath = EmbeddedResources.FindFile("GrayRoundButton.png");
                                    continue;
                                case "l":
                                case "left":
                                    commandInfo.ButtonPath = EmbeddedResources.FindFile("BlackSquareLeftButton.png");
                                    continue;
                                case "r":
                                case "right":
                                    commandInfo.ButtonPath = EmbeddedResources.FindFile("BlackSquareRightButton.png");
                                    continue;
                                case "red round":
                                case "rr":
                                case "rround":
                                    commandInfo.ButtonPath = EmbeddedResources.FindFile("RedRoundButton.png");
                                    continue;
                                case "u":
                                case "up":
                                    commandInfo.ButtonPath = EmbeddedResources.FindFile("BlackSquareUpButton.png");
                                    continue;
                                default:
                                    commandInfo.ButtonPath = EmbeddedResources.FindFile("GraySquareButton.png");
                                    continue;
                            }
                        case "dn":
                        case "drawnumbers":
                            switch (lower2)
                            {
                                case "true":
                                case "on":
                                case "yes":
                                    commandInfo.DrawNumbers = true;
                                    continue;
                                case "false":
                                case "off":
                                case "no":
                                    commandInfo.DrawNumbers = false;
                                    continue;
                                default:
                                    continue;
                            }
                        case "drawtoggleindicators":
                        case "dti":
                            switch (lower2)
                            {
                                case "true":
                                case "on":
                                case "yes":
                                    commandInfo.DrawToggleIndicators = true;
                                    continue;
                                case "false":
                                case "off":
                                case "no":
                                    commandInfo.DrawToggleIndicators = false;
                                    continue;
                                default:
                                    continue;
                            }
                        case "dx":
                        case "dxsendtype":
                        case "dxst":
                            switch (lower2)
                            {
                                case "hold":
                                case "h":
                                    commandInfo.DXSendType = 1;
                                    continue;
                                default:
                                    commandInfo.DXSendType = 0;
                                    continue;
                            }
                        case "l":
                        case "label":
                            commandInfo.Label = str;
                            continue;
                        case "labelbackgroundcolor":
                        case "lb":
                        case "lbc":
                            switch (lower2)
                            {
                                case "black":
                                    commandInfo.LabelBackgroundColor = BitmapColor.Black;
                                    continue;
                                case "blue":
                                    commandInfo.LabelBackgroundColor = new BitmapColor(50, 50, 200);
                                    continue;
                                case "gray":
                                    commandInfo.LabelBackgroundColor = new BitmapColor(128, 128, 128);
                                    continue;
                                case "green":
                                    commandInfo.LabelBackgroundColor = new BitmapColor(50, 200, 50);
                                    continue;
                                case "none":
                                    commandInfo.LabelBackgroundColor = BitmapColor.Transparent;
                                    continue;
                                case "purple":
                                    commandInfo.LabelBackgroundColor = new BitmapColor(150, 50, 200);
                                    continue;
                                case "red":
                                    commandInfo.LabelBackgroundColor = new BitmapColor(200, 50, 50);
                                    continue;
                                case "white":
                                    commandInfo.LabelBackgroundColor = BitmapColor.White;
                                    continue;
                                default:
                                    uint result2 = 0;
                                    if (!uint.TryParse(lower2, out result2))
                                    {
                                        PluginWarning = "Invalid Color";
                                        PluginWarningStopwatch.Restart();
                                        continue;
                                    }

                                    commandInfo.LabelBackgroundColor = new BitmapColor(result2);
                                    continue;
                            }
                        case "labelcolor":
                        case "lc":
                            switch (lower2)
                            {
                                case "black":
                                    commandInfo.LabelColor = BitmapColor.Black;
                                    continue;
                                case "blue":
                                    commandInfo.LabelColor = new BitmapColor(50, 50, 200);
                                    continue;
                                case "gray":
                                    commandInfo.LabelColor = new BitmapColor(128, 128, 128);
                                    continue;
                                case "green":
                                    commandInfo.LabelColor = new BitmapColor(50, 200, 50);
                                    continue;
                                case "none":
                                    commandInfo.LabelColor = BitmapColor.Transparent;
                                    continue;
                                case "purple":
                                    commandInfo.LabelColor = new BitmapColor(150, 50, 200);
                                    continue;
                                case "red":
                                    commandInfo.LabelColor = new BitmapColor(200, 50, 50);
                                    continue;
                                case "white":
                                    commandInfo.LabelColor = BitmapColor.White;
                                    continue;
                                default:
                                    uint result3 = 0;
                                    if (!uint.TryParse(lower2, out result3))
                                    {
                                        PluginWarning = "Invalid Color";
                                        PluginWarningStopwatch.Restart();
                                        continue;
                                    }

                                    commandInfo.LabelColor = new BitmapColor(result3);
                                    continue;
                            }
                        case "labelpos":
                        case "lp":
                            switch (lower2)
                            {
                                case "top":
                                case "t":
                                    commandInfo.LabelPos = 7;
                                    continue;
                                case "center":
                                case "c":
                                    commandInfo.LabelPos = 40;
                                    continue;
                                case "bottom":
                                case "b":
                                    commandInfo.LabelPos = 73;
                                    continue;
                                default:
                                    if (!int.TryParse(lower2, out commandInfo.LabelPos))
                                    {
                                        PluginWarning = "Invalid Label Position (must be top, center, bottom, or a number)";
                                        PluginWarningStopwatch.Restart();
                                    }

                                    continue;
                            }
                        case "labelsize":
                        case "ls":
                            int result4 = 14;
                            if (!int.TryParse(lower2, out result4))
                            {
                                PluginWarning = "Invalid Label Size (must be a number)";
                                PluginWarningStopwatch.Restart();
                                continue;
                            }

                            commandInfo.LabelSize = result4;
                            continue;
                        case "rotarytype":
                        case "rt":
                            if (!(lower2 == "gray"))
                            {
                                int num = lower2 == "g" ? 1 : 0;
                            }

                            commandInfo.RotaryPath = EmbeddedResources.FindFile("GrayRotary.png");
                            continue;
                        case "tab":
                        case "toggleasbutton":
                            switch (lower2)
                            {
                                case "true":
                                case "on":
                                case "yes":
                                    commandInfo.ToggleAsButton = true;
                                    continue;
                                case "false":
                                case "off":
                                case "no":
                                    commandInfo.ToggleAsButton = false;
                                    continue;
                                default:
                                    continue;
                            }
                        default:
                            PluginWarning = "Unknown Option";
                            PluginWarningStopwatch.Restart();
                            continue;
                    }
                }
                else
                {
                    PluginWarning = "Invalid option format. Option format is <option> = <optionvalue>";
                    PluginWarningStopwatch.Restart();
                    break;
                }
            }

            return commandInfo;
        }
    }
}