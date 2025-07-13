using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;
using MAMS.Models.ViewModels;

namespace MAMS.Services.Reports
{
    public class BookingSlipGenerator : IDocument
    {
        private readonly LabResultViewModel _model;

        public BookingSlipGenerator(LabResultViewModel model)
        {
            _model = model;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A5.Landscape());
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12).FontFamily("Arial"));

                // Header
                page.Header().Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("MedEase™ Lab Booking Slip")
                            .FontSize(18)
                            .Bold()
                            .FontColor(Colors.Blue.Darken2);

                        col.Item().Text($"Reference No: {_model.ReferenceNo}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken2);
                    });

                    row.ConstantItem(70).Image("wwwroot/img/MedEase.png");
                });

                // Divider
                page.Content().PaddingVertical(10).Column(col =>
                {
                    col.Spacing(8);

                    col.Item().LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten2);

                    // Booking Details in two-column layout
                    col.Item().Grid(grid =>
                    {
                        grid.Columns(2);

                        grid.Item().Text(txt => {
                            txt.Span("Patient Name: ").SemiBold();
                            txt.Span(_model.PatientName);
                        });

                        grid.Item().Text(txt => {
                            txt.Span("Lab Test: ").SemiBold();
                            txt.Span(_model.LabTypeName);
                        });

                        grid.Item().Text(txt => {
                            txt.Span("Date: ").SemiBold();
                            txt.Span($"{_model.BookedDate:yyyy-MM-dd}");
                        });

                        grid.Item().Text(txt => {
                            txt.Span("Time: ").SemiBold();
                            txt.Span(_model.TimeSlot.ToString());
                        });

                        grid.Item().Text(txt => {
                            txt.Span("Fee: ").SemiBold();
                            txt.Span($"Rs. {_model.BookedPrice:N2}");
                        });
                    });

                    col.Item().PaddingTop(10).LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten2);

                    col.Item().PaddingTop(8).Text("Please arrive 10 minutes before your scheduled time.")
                        .FontColor(Colors.Grey.Darken1);
                });

                // Footer
                page.Footer().AlignCenter().Column(col =>
                {
                    col.Spacing(4);

                    col.Item().Text(txt =>
                    {
                        txt.Span("Thank you for choosing ").FontColor(Colors.Grey.Darken2).FontSize(10);
                        txt.Span("MedEase").Bold().FontColor(Colors.Blue.Medium).FontSize(10);
                        txt.Span(" – We care for your health.").FontColor(Colors.Grey.Darken2).FontSize(10);
                    });

                    col.Item().AlignCenter().Text(txt =>
                    {
                        txt.Span("Need help? Call us at ").FontSize(9).FontColor(Colors.Black);
                        txt.Span("+94 112 345 678").FontSize(9).FontColor(Colors.Blue.Darken2).Underline();
                        txt.Span(" for support.").FontSize(9).FontColor(Colors.Black);
                    });
                });
            });
        }
    }
}
