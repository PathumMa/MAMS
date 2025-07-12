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
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header().Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("MedEase - Lab Booking Slip").Bold().FontSize(16).FontColor(Colors.Blue.Darken2);
                        col.Item().Text($"Reference: {_model.ReferenceNo}").FontSize(10).FontColor(Colors.Grey.Darken2);
                    });

                    row.ConstantItem(60).Image("wwwroot/img/MedEase.png");
                });

                page.Content().Column(col =>
                {
                    col.Item().Text($"Patient Name: {_model.PatientName}");
                    col.Item().Text($"Lab Test: {_model.LabTypeName}");
                    col.Item().Text($"Date: {_model.BookedDate}");
                    col.Item().Text($"Time: {_model.TimeSlot}");
                    col.Item().Text($"Fee: Rs. {_model.BookedPrice:N2}");

                    col.Item().PaddingTop(15).LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten2);
                    col.Item().PaddingTop(10).Text("Thank you for choosing MedEase!");
                });
            });
        }
    }
}
