using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AulaVirtual.App.Models
{
    public class CursoGrupo : ObservableCollection<CursoHistorialDto>
    {
        public string NombreGrupo { get; private set; }

        public CursoGrupo(string nombreGrupo, IEnumerable<CursoHistorialDto> cursos) : base(cursos)
        {
            NombreGrupo = nombreGrupo;
        }
    }
}
