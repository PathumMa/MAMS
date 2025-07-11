using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;
using MAMS.Models.ViewModels;

namespace MAMS.Services.Reports
{
    public class BookingSlipGenerator
    {
        public byte[] Generate(LabBookingResponseViewModel model)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A5);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("MedEase&#8482; - Lab Booking Slip").Bold().FontSize(16).FontColor(Colors.Blue.Darken2);
                            col.Item().Text($"Reference: {model.ReferenceNo}").FontSize(10).FontColor(Colors.Grey.Darken2);
                        });

                        row.ConstantItem(50).Height(50).Image("wwwroot/img/MedEase.png");
                    });

                    page.Content().Column(col =>
                    {
                        col.Item().Text($"Patient Name: {model.Patient}");
                        col.Item().Text($"Lab Test: {model.LabName}");
                        col.Item().Text($"Date: {model.BookedDate}");
                        col.Item().Text($"Time: {model.Time}");
                        col.Item().Text($"Fee: Rs. {model.Price:N2}");

                        col.Item().PaddingTop(15).LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten2);
                        col.Item().PaddingTop(10).Text("Thank you for choosing MAMS!");
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}
