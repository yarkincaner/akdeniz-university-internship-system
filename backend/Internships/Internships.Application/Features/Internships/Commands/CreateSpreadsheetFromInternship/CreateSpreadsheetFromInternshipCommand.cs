using AutoMapper;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using Internships.Core.Entities;
using Internships.Core.Interfaces.Repositories;
using Internships.Core.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internships.Core.Features.Internships.Commands.CreateSpreadsheetFromInternship
{
    public class CreateSpreadsheetFromInternshipCommand : IRequest<Response<string>>
    {
        public List<int> InternshipIds { get; set; }
    }

    public class CreateSpreadsheetFromInternshipCommandHandler : IRequestHandler<CreateSpreadsheetFromInternshipCommand, Response<string>> 
    {
        private readonly IMapper _mapper;
        private readonly IInternshipRepositoryAsync _internshipRepository;

        public CreateSpreadsheetFromInternshipCommandHandler(IMapper mapper, IInternshipRepositoryAsync internshipRepository, IUserRepositoryAsync userRepository)
        {
            _mapper = mapper;
            _internshipRepository = internshipRepository;
        }

        public async Task<Response<string>> Handle(CreateSpreadsheetFromInternshipCommand request, CancellationToken cancellationToken)
        {
            var internships = new List<Internship>();
            foreach (var internshipId in request.InternshipIds)
            {
                internships.Add(await _internshipRepository.GetByIdAsync(internshipId));
            }

            CreateSpreadsheet(internships);

            return new Response<string> { Message = "Spreadsheet created successfully", Succeeded = true };
        }

        private void CreateSpreadsheet(List<Internship> internships)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Controllers", "Assets");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, "Internships.xlsx");

            using (SpreadsheetDocument document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                Worksheet worksheet = new Worksheet();

                // Define the widths of the columns
                Columns columns = new Columns(
                    new Column { Min = 1, Max = 1, Width = 5, CustomWidth = true }, // Sıra No
                    new Column { Min = 2, Max = 2, Width = 15, CustomWidth = true }, // TCKimlik Numarası
                    new Column { Min = 3, Max = 3, Width = 15, CustomWidth = true }, // Sigortalı Adı
                    new Column { Min = 4, Max = 4, Width = 15, CustomWidth = true }, // Sigortalı Soyadı
                    new Column { Min = 5, Max = 5, Width = 15, CustomWidth = true }  // İşe Giriş Tarihi
                );

                worksheet.Append(columns);

                worksheet.Append(new SheetData());
                worksheetPart.Worksheet = worksheet;

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                Sheet sheet = new Sheet()
                {
                    Id = workbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Internships"
                };
                sheets.Append(sheet);

                SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                // Create header row
                Row headerRow = new Row();
                headerRow.Append(
                    new Cell { CellValue = new CellValue("Sıra No"), DataType = CellValues.String },
                    new Cell { CellValue = new CellValue("TCKimlik Numarası"), DataType = CellValues.String },
                    new Cell { CellValue = new CellValue("Sigortalı Adı"), DataType = CellValues.String },
                    new Cell { CellValue = new CellValue("Sigortalı Soyadı"), DataType = CellValues.String },
                    new Cell { CellValue = new CellValue("İşe Giriş Tarihi"), DataType = CellValues.String }
                );
                sheetData.Append(headerRow);

                int index = 1;
                // Add data rows
                foreach (var internship in internships)
                {
                    Row dataRow = new Row();
                    dataRow.Append(
                        new Cell { CellValue = new CellValue(index), DataType = CellValues.Number },
                        new Cell { CellValue = new CellValue(internship.User.TcKimlikNo.ToString()), DataType = CellValues.Number },
                        new Cell { CellValue = new CellValue(internship.User.FirstName), DataType = CellValues.String },
                        new Cell { CellValue = new CellValue(internship.User.LastName), DataType = CellValues.String },
                        new Cell { CellValue = new CellValue(internship.StartDate.ToString("yyyy-MM-dd")), DataType = CellValues.String }
                    );
                    sheetData.Append(dataRow);
                    index++;
                }

                workbookPart.Workbook.Save();
            }
        }
    }
}
