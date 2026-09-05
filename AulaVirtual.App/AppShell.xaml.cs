using AulaVirtual.App.Views;

namespace AulaVirtual.App;

public partial class AppShell : Shell
{
    public AppShell()
    {
        try
        {
            InitializeComponent();
            Routing.RegisterRoute("AdminDashboard", typeof(PanelAdminView));

            Routing.RegisterRoute(nameof(CarrerasView), typeof(CarrerasView));
            Routing.RegisterRoute(nameof(SemestresView), typeof(SemestresView));
            Routing.RegisterRoute(nameof(CursosView), typeof(CursosView));
            Routing.RegisterRoute(nameof(UsuariosView), typeof(UsuariosView));
            Routing.RegisterRoute(nameof(AsignacionesView), typeof(AsignacionesView));

            Routing.RegisterRoute(nameof(CursosDocenteView), typeof(CursosDocenteView));
            Routing.RegisterRoute(nameof(DetalleCursoDocenteView), typeof(DetalleCursoDocenteView));
            Routing.RegisterRoute(nameof(RevisionEntregasView), typeof(RevisionEntregasView));
            Routing.RegisterRoute(nameof(ResumenNotasView), typeof(ResumenNotasView));
            
            Routing.RegisterRoute(nameof(EntregasTareaView), typeof(EntregasTareaView));
            Routing.RegisterRoute(nameof(CalificacionRapidaView), typeof(CalificacionRapidaView));
            Routing.RegisterRoute(nameof(ResumenNotasDocenteView), typeof(ResumenNotasDocenteView));

            Routing.RegisterRoute(nameof(CursosEstudianteView), typeof(CursosEstudianteView));
            Routing.RegisterRoute(nameof(DetalleCursoEstudianteView), typeof(DetalleCursoEstudianteView));
            Routing.RegisterRoute(nameof(CargarTareaView), typeof(CargarTareaView));
            Routing.RegisterRoute(nameof(TareasPendientesView), typeof(TareasPendientesView));
            Routing.RegisterRoute(nameof(NotasSemestreActualView), typeof(NotasSemestreActualView));
            Routing.RegisterRoute(nameof(HistorialAcademicoView), typeof(HistorialAcademicoView));
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"AppShell.xaml.cs Error: {ex}");
            throw;
        }
    }
}
