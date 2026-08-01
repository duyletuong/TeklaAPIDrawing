using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tekla.Structures.Drawing;
using Tekla.Structures.Drawing.UI;
using TSD = Tekla.Structures.Drawing;

namespace TeklaAPIDrawing
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            DrawingHandler drawingHandler = new DrawingHandler();
            DrawingSelector drawingSelector = drawingHandler.GetDrawingSelector();
            DrawingEnumerator drawingEnumerator = drawingSelector.GetSelected();
            int num = Convert.ToInt32(txtStartNumber.Text);
            int digit = Convert.ToInt32(txtDigits.Text);   // đọc số digit

            while (drawingEnumerator.MoveNext())
            {
                TSD.Drawing drawing = drawingEnumerator.Current;

                // tạo chuỗi số đã pad 0
                string numStr = num.ToString().PadLeft(digit, '0');
                string value = txtPrefix.Text + numStr + txtPostfix.Text;

                switch (cbWriteTo.SelectedIndex)
                {
                    case 0:
                        drawing.Name = value; break;
                    case 1:
                        drawing.Title1 = value; break;
                    case 2:
                        drawing.Title2 = value; break;
                    case 3:
                        drawing.Title3 = value; break;
                    case 4:
                        drawing.SetUserProperty("DRAWING_USERFIELD_1", value);
                        break;
                    default:
                        break;
                }

                //if (cbWriteTo.SelectedIndex == 0)
                //    drawing.Name = value;
                //else if (cbWriteTo.SelectedIndex == 1)
                //    drawing.Title1 = value;
                //else if (cbWriteTo.SelectedIndex == 2)
                //    drawing.Title2 = value;
                //else if (cbWriteTo.SelectedIndex == 3)
                //    drawing.Title3 = value;
                //else if (cbWriteTo.SelectedIndex == 4)
                //    drawing.SetUserProperty("DRAWING_USERFIELD_1", value);

                num++;
                drawing.Modify();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbWriteTo.SelectedIndex = 0;
        }
    }
}
