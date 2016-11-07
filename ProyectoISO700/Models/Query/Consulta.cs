using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoISO700.Models.Query
{
    public static class Consulta
    {
        public static string ConsultaPaginado(string consulta)
        {
            return String.Format(@"
                    SELECT * FROM ({0}) Tabla 
                    WHERE Tabla.RowID 
                    BETWEEN (@tamañoPagina * (@pagina-1)) + 1 AND (@tamañoPagina * @pagina)", consulta);
        }

        public static string ConsultaCantidad(string consulta)
        {
            return String.Format(@"SELECT COUNT(*) FROM ({0})  k", consulta);
        }

        public static string ObtenerOfertasListado()
        {
            return @"SELECT ROW_NUMBER() OVER(ORDER BY j.DateCreated DESC) AS RowID 
                              , j.[id]
                              ,[category_id]
	                          ,c.[name]
                              ,j.[typeID]
                              ,tj.[Type]
                              ,[company]
                              ,[position]
                              ,[location]
                              ,[DateCreated]
                              ,[DateUpdated]
                              ,[DateExpires]
                    FROM [BolsaEmpleo].[dbo].[Job] j
                        INNER JOIN [BolsaEmpleo].[dbo].[Category] c
                            ON j.category_id = c.id
                        INNER JOIN [BolsaEmpleo].[dbo].[TypeJob] tj
                            ON j.typeID = tj.typeID
                    WHERE j.id > 0 ";
        }
    }
}