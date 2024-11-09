using CNSVM.Models.ModelView;
using iText.Html2pdf;
namespace CNSVM.Services
{
    public class ReportService
    {
        public string CreatePdf(string nombreDoc,string espeDoc, string nombrePac,List<MedicamentJustification> medicaments)
        {
            string pathReports = Path.Combine(Directory.GetCurrentDirectory(), "Reportes");
            try
            {
                if (!Directory.Exists(pathReports))
                {
                    Directory.CreateDirectory(pathReports);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            string htmlContent = @"
            <html>
                <head>
                    <style>
                        body {
                            font-family: Arial, sans-serif;
                            margin: 2px,10px,10px,10px;
                            font-size: 16px; /* Tamaño de fuente general */
                        }
                        .header {
                            display: flex;
                            align-items: center;
                            justify-content: space-between;
                            margin-bottom: 20px;
                        }
                        .header img {
                            width: 120px; /* Tamaño del logo */
                            margin-right: 20px; /* Espacio a la derecha del logo */
                        }
                        .header .text-container {
                            display: flex;
                            flex-direction: column;
                            align-items: center;
                            justify-content: center;
                            text-align: center;
                            flex-grow: 1; /* Permite que el bloque de texto ocupe todo el espacio disponible */
                        }

                        .content {
                            margin-top: 20px;
                            font-size: 18px; /* Tamaño de fuente del contenido */
                        }
                        .date {
                            text-align: right; /* Fecha alineada a la derecha */
                            margin-bottom: 10px;
                        }
                        .ref {
                            margin-top: 50px;
                            margin-bottom: 50px;
                            font-weight: bold;
                        }
                        .table-container {
                            margin-top: 30px;
                        }
                        table {
                            width: 100%;
                            border-collapse: collapse;
                            font-size: 16px; /* Tamaño de fuente de la tabla */
                        }
                        th, td {
                            border: 1px solid black;
                            padding: 10px;
                            text-align: left;
                        }
                        th {
                            background-color: #f2f2f2;
                        }
                        .signature-section {
                            margin-top: 50px;
                            text-align: center;
                        }
                        .signature-line {
                            margin-top: 160px;
                            border-top: 1px solid black;
                            width: 200px;
                            margin-left: auto;
                            margin-right: auto;
                            padding-top: 1px;
                        }
                        /* Media query para ajustar el tamaño de los títulos si es necesario */
                        @media (max-width: 768px) {
                            .header h1 {
                                font-size: 30px;
                                margin: 0;
                            }
                            .header h2 {
                                font-size: 26px;
                                margin: 0;
                            }
                        }
                    </style>
                </head>
                <body>
                    <div class='header'>
                        <img src='wwwroot/images/logo_CNS_header.png' alt='Logotipo'>
                        <div class='text-container'>
                            <h1>CAJA NACIONAL DE SALUD</h1>
                            <h2>GERENCIA GENERAL</h2>
                        </div>
                    </div>
                    <div class='content'>
                        <div class='date'>Cochabamba, " + DateTime.Now.ToString("dd-MM-yyyy") + @"</div> <!-- Alineada a la derecha -->
                        <p>Señor(a):</p>
                        <p><strong>" + nombrePac + @"</strong></p>
                        <div class='ref'>REF: Rechazo de Medicamentos</div>
                        <p>Estimado(a):</p>
                        <p>Mediante la presente, le informamos sobre los medicamentos que han sido rechazados por el consejo de médicos, junto con la justificación correspondiente.</p>
                    </div>
                    <div class='table-container'>
                        <table>
                            <thead>
                                <tr>
                                    <th>Nombre del Medicamento</th>
                                    <th>Justificación del Rechazo</th>
                                </tr>
                            </thead>
                            <tbody>";
            foreach (var item in medicaments)
            {
                htmlContent += @"<tr>
                                    <td>"+item.MedicamentName+@"</td>
                                    <td>"+item.Justification+@"</td>
                                </tr>";
            }
            htmlContent += @"</tbody>
                        </table>
                    </div>
                    <div class='signature-section'>
                        <div class='signature-line'></div>
                        <p>"+nombreDoc+@"</p>
                        <p>"+espeDoc+@"</p>
                    </div>
                </body>
            </html>";

            string pdfPath = Path.Combine(pathReports, $"{nombrePac}.pdf");
            try
            {
                HtmlConverter.ConvertToPdf(htmlContent, new FileStream(pdfPath, FileMode.Create));
                return pdfPath;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
