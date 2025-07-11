using QuestPDF.Infrastructure;
using MAMS.Models.ViewModels;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace MAMS.Services.Reports
{
    public class LabReceiptPdfDocument : IDocument
    {
        private readonly TransactionReceiptViewModel _model;

        public LabReceiptPdfDocument(TransactionReceiptViewModel model)
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

                // Header with logo and title
                page.Header()
                    .Row(row =>
                    {
                        row.ConstantItem(60).Image("wwwroot/img/MedEase.png");

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignMiddle().Text("MedEase - Lab Receipt").FontSize(18).Bold().FontColor(Colors.Blue.Medium);
                            col.Item().AlignMiddle().Text("Your Trusted Health Partner").FontSize(10).FontColor(Colors.Grey.Medium);
                        });
                    });

                // Content section
                page.Content()
                    .PaddingVertical(20)
                    .Column(col =>
                    {
                        col.Spacing(10);

                        col.Item().BorderBottom(1).PaddingBottom(5).Text($"Reference No: {_model.ReferenceNo}").Bold();
                        col.Item().Text($"👤 Patient Name: {_model.PatientName}");
                        col.Item().Text($"🧪 Lab Test: {_model.LabTestName}");
                        col.Item().Text($"📅 Booked Date: {_model.BookedDate}");
                        col.Item().Text($"⏰ Time Slot: {_model.TimeSlot}");
                        col.Item().Text($"💳 Payment Method: {_model.PaymentMethod}");
                        col.Item().Text($"💰 Amount Paid: Rs. {_model.AmountPaid:F2}");
                        col.Item().Text($"🕒 Issued Date: {_model.IssuedDate}");
                    });

                // Footer
                page.Footer()
                    .AlignCenter()
                    .Text(txt =>
                    {
                        txt.Span("Thank you for choosing ").FontSize(10).FontColor(Colors.Grey.Darken2);
                        txt.Span("MedEase").Bold().FontColor(Colors.Blue.Medium).FontSize(10);
                        txt.Span(" – We care for your health.").FontSize(10).FontColor(Colors.Grey.Darken2);
                    });
            }); ;
        }
    }
}
