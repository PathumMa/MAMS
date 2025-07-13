using MAMS.Models.ViewModels;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Composition;
using System.IO;

namespace MAMS.Services.Reports
{
    public class LabReportPdfGenerator : IDocument
    {
        private readonly LabResultViewModel _model;

        public LabReportPdfGenerator(LabResultViewModel model)
        {
            _model = model;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A5);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                // Header
                page.Header().Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("MedEase™ - Lab Report")
                            .Bold().FontSize(18).FontColor(Colors.Blue.Darken2);

                        col.Item().Text($"Reference No: {_model.ReferenceNo}")
                            .FontSize(10).FontColor(Colors.Grey.Darken2);
                    });

                    row.ConstantItem(60).Image("wwwroot/img/MedEase.png");
                });

                // Content
                page.Content().Column(col =>
                {
                    col.Spacing(6);

                    col.Item().PaddingTop(10).LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten2);

                    col.Item().Row(r =>
                    {
                        r.RelativeItem().Text("Patient Name:").Bold().FontColor(Colors.Blue.Darken2);
                        r.RelativeItem().Text(_model.PatientName).FontColor(Colors.Grey.Darken3);
                    });

                    col.Item().Row(r =>
                    {
                        r.RelativeItem().Text("Lab Test:").Bold().FontColor(Colors.Blue.Darken2);
                        r.RelativeItem().Text(_model.LabTypeName).FontColor(Colors.Grey.Darken3);
                    });

                    col.Item().Row(r =>
                    {
                        r.RelativeItem().Text("Amount:").Bold().FontColor(Colors.Blue.Darken2);
                        r.RelativeItem().Text($"Rs. {_model.BookedPrice:N2}").FontColor(Colors.Grey.Darken3);
                    });

                    col.Item().Row(r =>
                    {
                        r.RelativeItem().Text("Booked Date:").Bold().FontColor(Colors.Blue.Darken2);
                        r.RelativeItem().Text(_model.BookedDate.ToString("dd/MM/yyyy")).FontColor(Colors.Grey.Darken3);
                    });

                    col.Item().Row(r =>
                    {
                        r.RelativeItem().Text("Performed Date:").Bold().FontColor(Colors.Blue.Darken2);
                        r.RelativeItem().Text(_model.PerformedDate.HasValue
                            ? _model.PerformedDate.Value.ToString("dd/MM/yyyy")
                            : "Test pending")
                            .FontColor(_model.PerformedDate.HasValue ? Colors.Grey.Darken3 : Colors.Green.Medium)
                            .Italic(!_model.PerformedDate.HasValue);
                    });

                    col.Item().Row(r =>
                    {
                        r.RelativeItem().Text("Result Values:").Bold().FontColor(Colors.Blue.Darken2);
                        r.RelativeItem().Text(!string.IsNullOrWhiteSpace(_model.ResultValue)
                            ? _model.ResultValue
                            : "N/A")
                            .FontColor(!string.IsNullOrWhiteSpace(_model.ResultValue) ? Colors.Grey.Darken3 : Colors.Red.Medium)
                            .Italic(string.IsNullOrWhiteSpace(_model.ResultValue));
                    });

                    col.Item().Row(r =>
                    {
                        r.RelativeItem().Text("Comments:").Bold().FontColor(Colors.Blue.Darken2);
                        r.RelativeItem().Text(!string.IsNullOrWhiteSpace(_model.Comments)
                            ? _model.Comments
                            : "N/A")
                            .FontColor(!string.IsNullOrWhiteSpace(_model.Comments) ? Colors.Grey.Darken3 : Colors.Red.Medium)
                            .Italic(string.IsNullOrWhiteSpace(_model.Comments));
                    });

                    

                    //col.Item().Row(r =>
                    //{
                    //    r.RelativeItem().Text("Issued Date:").Bold().FontColor(Colors.Blue.Darken2);
                    //    r.RelativeItem().Text(_model.IssuedDate.ToString("dd/MM/yyyy")).FontColor(Colors.Grey.Darken3);
                    //});

                    col.Item().PaddingTop(15).LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten2);
                });

                // Footer (optional)
                page.Footer()
                    .AlignCenter()
                    .Column(col =>
                    {
                        col.Spacing(5);

                        col.Item().Text(txt =>
                        {
                            txt.Span("Thank you for choosing ").FontSize(10).FontColor(Colors.Grey.Darken2);
                            txt.Span("MedEase").Bold().FontColor(Colors.Blue.Medium).FontSize(10);
                            txt.Span(" – We care for your health.").FontSize(10).FontColor(Colors.Grey.Darken2);
                        });

                        col.Item().AlignCenter().Text(txt =>
                        {
                            txt.Span("Call us at ").FontSize(9).FontColor(Colors.Black);
                            txt.Span("+94 112 345 678")
                                .FontSize(9)
                                .FontColor(Colors.Blue.Darken2)
                                .Underline();
                            txt.Span(" for any queries.").FontSize(9).FontColor(Colors.Black);
                        });
                    });
            });

        }
    }
}
