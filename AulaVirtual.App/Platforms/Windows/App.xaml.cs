using Microsoft.UI.Xaml;




namespace AulaVirtual.App.WinUI;

public partial class App : MauiWinUIApplication
{
		public App()
	{
		this.InitializeComponent();
        this.UnhandledException += (sender, args) =>
        {
            System.IO.File.WriteAllText(@"C:\Users\delma\OneDrive\Documents\AulaVirtual\error_winui.txt", args.Exception.ToString() + "\n" + args.Message);
            args.Handled = true;
        };
	}

	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}

