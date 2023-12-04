using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestItextsharp
{
    public class ITextPdfFooter : PdfPageEventHelper
    {
        private readonly Font _pageNumberFont = FontFactory.GetFont("Arial", 5, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            var text = writer.PageNumber.ToString();

            var numberTable = new PdfPTable(1);
            numberTable.DefaultCell.Border = 0;
            var numberCell = new PdfPCell(new Phrase(text, _pageNumberFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };

            numberTable.AddCell(numberCell);
            numberTable.TotalWidth = 50;
            numberTable.WriteSelectedRows(0, -1, document.Right - 40, document.Bottom + 5, writer.DirectContent);
        }
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            AddPageNumber(writer, document);
        }

        private void AddPageNumber(PdfWriter writer, Document document)
        {
            var text = writer.PageNumber.ToString();

            var numberTable = new PdfPTable(1);
            numberTable.DefaultCell.Border = 0;
            var numberCell = new PdfPCell(new Phrase(text, _pageNumberFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };

            numberTable.AddCell(numberCell);
            numberTable.TotalWidth = 50;
            numberTable.WriteSelectedRows(0, -1, document.Right - 40, document.Bottom + 5, writer.DirectContent);
        }
    }
}
