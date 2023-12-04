using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace TestItextsharp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PrintPagePdf(dgvPage);
        }

        private void PrintPagePdf(DataGridView dgvPage)
        {
            if (dgvPage.Rows.Count <= 0)
            {
                MessageBox.Show("No Record To Export !!!", "Info");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PDF (*.pdf)|*.pdf";
            sfd.FileName = "Output.pdf";
            bool fileError = false;

            if (sfd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            if (File.Exists(sfd.FileName))
            {
                try
                {
                    File.Delete(sfd.FileName);
                }
                catch (IOException ex)
                {
                    fileError = true;
                    MessageBox.Show("It wasn't possible to write the data to the disk." + ex.Message);
                }
            }

            if (fileError)
            {
                return;
            }

            try
            {
                PdfPTable pdfTable = new PdfPTable(dgvPage.Columns.OfType<DataGridViewColumn>().Where(x => x.Visible).Count() + 1);
                pdfTable.DefaultCell.Padding = 3;
                pdfTable.WidthPercentage = 100;
                pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfTable.HeaderRows = 1;


                //var a = dgvPage.Columns.OfType<DataGridViewColumn>().Where(x => x.Visible).Select(x => x.Width).ToList();
                //a.Insert(0, 50);
                ////a[2] = 40;
                ////a[3] = 60;
                ////a[9] = 90;
                ////a[10] = 90;
                ////a[12] = 40;
                ////a[15] = 80;


                //pdfTable.SetWidths(a.ToArray());

                iTextSharp.text.Font fontTable = FontFactory.GetFont("Arial", 5, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);


                //add row number

                pdfTable.AddCell(new PdfPCell(new Phrase("No.", fontTable)));

                foreach (DataGridViewColumn column in dgvPage.Columns)
                {
                    if (column.Visible)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, fontTable)) { Border=PdfPCell.BOTTOM_BORDER};
                        pdfTable.AddCell(cell);
                    }
                }

                var num = 1;
                foreach (DataGridViewRow row in dgvPage.Rows)
                {
                    //add cell for row number
                    var cel=new PdfPCell(new Phrase(num.ToString(), fontTable)) { Border = PdfPCell.BOTTOM_BORDER };
                    pdfTable.AddCell(cel);
                    num++;

                    //add other cells
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        var col = dgvPage.Columns[cell.ColumnIndex];
                        if (col.Visible)
                        {
                            var c = new PdfPCell(new Phrase(cell.Value?.ToString(), fontTable)) { Border = PdfPCell.BOTTOM_BORDER };
                            pdfTable.AddCell(c);
                        }
                    }
                }

                using (FileStream stream = new FileStream(sfd.FileName, FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.A4, 10f, 20f, 80f, 80f);
                    var writer = PdfWriter.GetInstance(pdfDoc, stream);
                    writer.PageEvent = new ITextPdfFooter();

                    pdfDoc.Open();
                    pdfDoc.Add(pdfTable);

                    pdfDoc.Close();
                    stream.Close();

                    //show pdf file
                    System.Diagnostics.Process.Start(sfd.FileName);
                }

                MessageBox.Show("Data Exported Successfully !!!", "Info");

                ////show pdf file

                //System.Diagnostics.Process.Start(sfd.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :" + ex.Message);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for(int i = 0; i < 100; i++)
            {
                DataGridViewRow row = new DataGridViewRow();
                for(int j = 1; j <= 7; j++)
                {
                    var cell = new DataGridViewTextBoxCell();
                    cell.Value = i.ToString();
                    if (j % 3 == 0)
                    {
                        cell.Value = cell.Value + "aasdas asdasd haurqyuwr hqhfjhafuha s hasdjasdkjkjs asdasdr rfsadfaf fdfadf";
                    }
                    row.Cells.Add(cell);
                }
                dgvPage.Rows.Add(row);
            }
        }
    }
}
