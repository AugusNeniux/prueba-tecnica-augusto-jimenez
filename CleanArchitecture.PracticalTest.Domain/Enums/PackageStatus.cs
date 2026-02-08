using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.PracticalTest.Domain.Enums
{
    public enum PackageStatus
    {
        Registrado = 1,
        EnBodega = 2,
        EnTransito = 3,
        EnReparto = 4,
        Entregado = 5,
        Devuelto = 6
    }
}
