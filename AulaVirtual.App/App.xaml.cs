namespace AulaVirtual.App;

public partial class App : Application
{
	public App()
	{
		try
        {
            InitializeComponent();
            MainPage = new AppShell();
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"App.xaml.cs Error: {ex}");
            throw;
        }
	}
}