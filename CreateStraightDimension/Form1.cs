using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tekla.Structures.Drawing;
using TSD = Tekla.Structures.Drawing;
using Tekla.Structures.Drawing.UI;
using Tekla.Structures.Model;
using Tekla.Structures.Geometry3d;


namespace CreateStraightDimension
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCreateDim_Click(object sender, EventArgs e)
        {
            Point startPoint = new Point(1000,0,0);
            Point endPoint = new Point(2000,0,0);
            Vector upDirection = new Vector(0,1,0);
            double distance = double.Parse(txtDimDistance.Text);
            ViewBase viewBase = null;

            //lay view dang duoc chon trong ban ve de tao dim
            DrawingHandler drawingHandler = new DrawingHandler();
            //lay ban ve dang mo
            Drawing activeDrawing = drawingHandler.GetActiveDrawing();

            //lay selector
            DrawingObjectSelector drawingObjectSelector = drawingHandler.GetDrawingObjectSelector();
            //lay cac object duoc chon trong ban ve
            DrawingObjectEnumerator drawingObjects = drawingObjectSelector.GetSelected();

            //duyet qua tung object
            while (drawingObjects.MoveNext())
            {
                //kiem tra object dang chon co phai la view ko
                if (drawingObjects.Current is TSD.View view)
                {
                    viewBase = view;
                    //thoat vong lap
                    break;
                }
            }

            //kiem tra co lay duoc viewbase chua
            if (viewBase != null)
            {
                // lay attribute dimension
                StraightDimensionSet.StraightDimensionSetAttributes attributes =
                    new StraightDimensionSet.StraightDimensionSetAttributes("standard2");

                //khoi tao straight dimension
                StraightDimension straightDimension = new StraightDimension(
                    viewBase, startPoint, endPoint, upDirection, distance, attributes);

                //them straightdimension vao ban ve
                straightDimension.Insert();
                //refresh lai ban ve de thay dim
                activeDrawing.CommitChanges();
            }
            else
            {
                MessageBox.Show("Khong tim thay view can tao!");
            }
        }
    }
}
