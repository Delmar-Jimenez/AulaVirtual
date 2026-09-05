using AulaVirtual.App.Services;
using AulaVirtual.App.ViewModels;
using AulaVirtual.App.Views;
using Microsoft.Extensions.Logging;

namespace AulaVirtual.App;

public static class MauiProgram
{
    public static IServiceProvider Services { get; private set; }

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        
        builder.Services.AddSingleton<ApiService>();

        
        builder.Services.AddTransient<InicioSesionViewModel>();
        builder.Services.AddTransient<CarrerasViewModel>();
        builder.Services.AddTransient<SemestresViewModel>();
        builder.Services.AddTransient<CursosViewModel>();
        builder.Services.AddTransient<UsuariosViewModel>();
        builder.Services.AddTransient<AsignacionesViewModel>();
        builder.Services.AddTransient<CursosDocenteViewModel>();
        builder.Services.AddTransient<DetalleCursoDocenteViewModel>();
        builder.Services.AddTransient<RevisionEntregasViewModel>();
        builder.Services.AddTransient<CursosEstudianteViewModel>();
        builder.Services.AddTransient<DetalleCursoEstudianteViewModel>();
        builder.Services.AddTransient<TareasPendientesViewModel>();
        builder.Services.AddTransient<PanelAdminViewModel>();
        builder.Services.AddTransient<CargarTareaViewModel>();
        builder.Services.AddTransient<NotasSemestreActualViewModel>();
        builder.Services.AddTransient<ResumenNotasViewModel>();
        builder.Services.AddTransient<HistorialAcademicoViewModel>();
        
        builder.Services.AddTransient<EntregasTareaViewModel>();
        builder.Services.AddTransient<CalificacionRapidaViewModel>();
        builder.Services.AddTransient<ResumenNotasDocenteViewModel>();

        
        builder.Services.AddTransient<InicioSesionView>();
        builder.Services.AddTransient<PanelAdminView>();
        builder.Services.AddTransient<CarrerasView>();
        builder.Services.AddTransient<SemestresView>();
        builder.Services.AddTransient<CursosView>();
        builder.Services.AddTransient<UsuariosView>();
        builder.Services.AddTransient<AsignacionesView>();
        builder.Services.AddTransient<CursosDocenteView>();
        builder.Services.AddTransient<DetalleCursoDocenteView>();
        builder.Services.AddTransient<RevisionEntregasView>();
        builder.Services.AddTransient<CursosEstudianteView>();
        builder.Services.AddTransient<DetalleCursoEstudianteView>();
        builder.Services.AddTransient<TareasPendientesView>();
        builder.Services.AddTransient<CargarTareaView>();
        builder.Services.AddTransient<NotasSemestreActualView>();
        builder.Services.AddTransient<ResumenNotasView>();
        builder.Services.AddTransient<HistorialAcademicoView>();

        builder.Services.AddTransient<EntregasTareaView>();
        builder.Services.AddTransient<CalificacionRapidaView>();
        builder.Services.AddTransient<ResumenNotasDocenteView>();

        try
        {
            var app = builder.Build();
            Services = app.Services;
            return app;
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"MauiProgram Error: {ex}");
            throw;
        }
    }
}

