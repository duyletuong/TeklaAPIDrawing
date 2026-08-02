using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tekla.Structures.Model;
using Tekla.Structures.Drawing;
using Tekla.Structures.Geometry3d;
using TSD = Tekla.Structures.Drawing;
using TSM = Tekla.Structures.Model;

namespace AutoDimPurlin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            Model model = new Model();
            DrawingHandler drawingHandler = new DrawingHandler();

            if (drawingHandler.GetConnectionStatus() == false)
            {
                MessageBox.Show("Không thể kết nối đến Tekla!");
                return; //thoát chương trình
            }

            //kiểm tra bản vẽ có đang mở không
            Drawing activeDrawing = drawingHandler.GetActiveDrawing();
            if (activeDrawing == null) 
            {
                MessageBox.Show("Không có bản vẽ nào để dim");
                return;
            }

            // lấy khung bản vẽ
            ContainerView containerView = activeDrawing.GetSheet(); 
            // lấy các view có trong khung bản vẽ
            DrawingObjectEnumerator views = containerView.GetViews();
            while (views.MoveNext())
            {
                //kiểm tra có phải view không
                if (views.Current is TSD.View view)
                {
                    //xử lí dim cho front view
                    if (view.ViewType == TSD.View.ViewTypes.FrontView)
                    {
                        //lấy ModelObjects trong front view
                        DrawingObjectEnumerator drawingObjectEnumerator = view.GetModelObjects();
                        while (drawingObjectEnumerator.MoveNext())
                        {
                            if (drawingObjectEnumerator.Current is TSD.Part tsdPart)
                            {
                                //lấy model part từ drawing part
                                TSM.Part modelPart = model.SelectModelObject(tsdPart.ModelIdentifier) as TSM.Part;

                                Solid solid = modelPart.GetSolid();

                                // lay attribute dimension
                                StraightDimensionSet.StraightDimensionSetAttributes attributes =
                                    new StraightDimensionSet.StraightDimensionSetAttributes("standard");

                                //thêm 2 diểm vào point list
                                PointList pointList = new PointList();
                                pointList.Add(solid.MinimumPoint);
                                pointList.Add(solid.MaximumPoint);

                                // tạo vector chỉ hướng dim tổng
                                Vector vector = new Vector(0, 1, 0);

                                //khai báo chiều cao đường dim
                                double distance = 200;

                                // tạo dim
                                StraightDimensionSetHandler sdh = new StraightDimensionSetHandler();
                                sdh.CreateDimensionSet(view, pointList, vector, distance, attributes);

                                activeDrawing.CommitChanges();
                            }
                        }
                    }
                    //xử lí dim cho top view
                    else if (view.ViewType is TSD.View.ViewTypes.TopView)
                    {

                    }
                }
            }
        }
    }
}
