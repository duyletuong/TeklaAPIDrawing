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
using TSG = Tekla.Structures.Geometry3d;
using TSD = Tekla.Structures.Drawing;
using TSM = Tekla.Structures.Model;
using Tekla.Structures.Model.UI;
using Tekla.Structures.Solid;

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

            //tạo workplanehandler để điều khiển coordinatesystem
            WorkPlaneHandler workPlaneHandler = model.GetWorkPlaneHandler();

            //lấy hệ trục tọa độ hiện hành
            TransformationPlane current = workPlaneHandler.GetCurrentTransformationPlane();

            //đưa hệ trục tọa độ về global
            workPlaneHandler.SetCurrentTransformationPlane(new TransformationPlane());

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
                        XuLiFrontView(view, model, activeDrawing);
                    }
                    //xử lí dim cho top view
                    else if (view.ViewType is TSD.View.ViewTypes.TopView)
                    {

                    }
                }
            }

            ////trả workplane về lại cho người dùng
            workPlaneHandler.SetCurrentTransformationPlane(current);
            model.CommitChanges();
        }

        private void XuLiFrontView(TSD.View view, Model model, Drawing activeDrawing)
        {
            //lấy ModelObjects trong front view
            DrawingObjectEnumerator drawingObjectEnumerator = view.GetModelObjects();
            while (drawingObjectEnumerator.MoveNext())
            {
                if (drawingObjectEnumerator.Current is TSD.Part tsdPart)
                {
                    //lấy model part từ drawing part
                    TSM.Part modelPart = model.SelectModelObject(tsdPart.ModelIdentifier) as TSM.Part;

                    CreateOverallDimension(activeDrawing, view, modelPart);
                }
            }
        }

        private void CreateOverallDimension(Drawing activeDrawing, TSD.View view, TSM.Part modelPart)
        {
            Solid solid = modelPart.GetSolid();

            //thêm 2 diểm vào point list
            //Tạo matrix để tranform điểm về view coordinate system
            Matrix toViewCoordinateSystem = MatrixFactory.ToCoordinateSystem(view.ViewCoordinateSystem);

            //tạo danh sách để chứa các điểm
            List<TSG.Point> points = GetPointsOfPart(modelPart);

            //khai bao PointList
            PointList pointList = new PointList();

            //duyet qua cac cạnh của xà gồ
            EdgeEnumerator edgeEnumerator = solid.GetEdgeEnumerator();
            while (edgeEnumerator.MoveNext())
            {
                if (edgeEnumerator.Current is Edge edge)
                {
                    //chuyển startpoint về view coords
                    TSG.Point startPoint = toViewCoordinateSystem.Transform(edge.StartPoint);
                    //thêm điểm đầu mỗi cạnh vào danh sách
                    points.Add(startPoint);
                }
            }

            //Linq
            //dùng linq sắp xếp Point theo thứ tự trên trục Y
            //OrderBy là sắp xếp theo thứ tự từ nhỏ đến lớn
            //OrderByDescending => sắp xếp từ lớn đến nhỏ
            // sắp xếp các điểm theo phương Y, thứ tự từ lớn đến nhỏ
            List<TSG.Point> danhSachDaSapXep = points.OrderByDescending(p => p.Y).ToList();

            foreach (var p in danhSachDaSapXep)
            {
                pointList.Add(p);
            }

            // tạo vector chỉ hướng dim tổng
            Vector vector = new Vector(0, 1, 0);

            //khai báo chiều cao đường dim
            double distance = 200;

            CreateDim(view, pointList, vector, distance);

            activeDrawing.CommitChanges();
        }

        private List<TSG.Point> GetPointsOfPart(TSM.Part part)
        {
            //lay solid từ part
            Solid solid = part.GetSolid();

            //lấy các cạnh của solid
            EdgeEnumerator edgeEnumerator = solid.GetEdgeEnumerator();

            //tạo danh sách chứa điểm
            List<TSG.Point> points = new List<TSG.Point>();
            while (edgeEnumerator.MoveNext())
            {
                if (edgeEnumerator.Current is Edge edge)
                {
                    points.Add(edge.StartPoint);
                }
            }

            return points;
        }

        private void CreateDim(TSD.View view, PointList pointList, Vector vector, double distance)
        {
            // lay attribute dimension
            StraightDimensionSet.StraightDimensionSetAttributes attributes =
                new StraightDimensionSet.StraightDimensionSetAttributes("standard");

            StraightDimensionSetHandler sdh = new StraightDimensionSetHandler();
            sdh.CreateDimensionSet(view, pointList, vector, distance, attributes);
        }
    }
}
