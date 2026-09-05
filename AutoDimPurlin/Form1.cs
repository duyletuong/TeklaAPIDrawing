using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Tekla.Structures.Drawing;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Tekla.Structures.Model.UI;
using Tekla.Structures.Solid;
using TSD = Tekla.Structures.Drawing;
using TSG = Tekla.Structures.Geometry3d;
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

            CreateDimension(activeDrawing);

            ////trả workplane về lại cho người dùng
            workPlaneHandler.SetCurrentTransformationPlane(current);
            model.CommitChanges();
        }

        private void CreateDimension(Drawing drawing)
        {
            ContainerView containerView = drawing.GetSheet();
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
                        XuLiFrontView(view, drawing);
                    }
                    //xử lí dim cho top view
                    else if (view.ViewType is TSD.View.ViewTypes.TopView)
                    {
                        //XuLiFrontView(view, model, activeDrawing);
                    }
                }
            }
        }

        private void XuLiFrontView(TSD.View view, Drawing activeDrawing)
        {
            //xoa dim cu
            DeleteOldDim(view);
            //lấy ModelObjects trong front view
            DrawingObjectEnumerator drawingObjectEnumerator = view.GetModelObjects();
            while (drawingObjectEnumerator.MoveNext())
            {
                if (drawingObjectEnumerator.Current is TSD.Part tsdPart)
                {
                    //lấy model part từ drawing part
                    TSM.Part modelPart = new Model().SelectModelObject(tsdPart.ModelIdentifier) as TSM.Part;

                    CreateOverallDimension(activeDrawing, view, modelPart);
                    CreateWebHoleDimension(activeDrawing, view, modelPart);
                    CreateOverallVerticalDimension(activeDrawing, view, modelPart);
                    CreateWebHoleVertcalDimension(activeDrawing, view, modelPart);
                    CreateFlangeHoleDimension(activeDrawing, view, modelPart);
                    TSD.View section = CreateSection(view, modelPart);
                    CreateSectionDimension(section, modelPart);

                    List<TSG.Point> boltPosParallelViewPlane = GetBoltPositionsParallelViewPlane(modelPart, view);
                    var minX = SelectMinX(boltPosParallelViewPlane);
                    Point startPoint = SelectMaxY(minX).FirstOrDefault();

                    var maxX = SelectMaxX(boltPosParallelViewPlane);
                    Point endPoint = SelectMinY(maxX).FirstOrDefault();

                    Rectangle rectangle = new Rectangle(view, startPoint, endPoint);
                    rectangle.Insert();

                    activeDrawing.CommitChanges();
                }
            }
        }

        private void CreateOverallDimension(Drawing activeDrawing, TSD.View view, TSM.Part modelPart)
        {
            Solid solid = modelPart.GetSolid();

            //tạo danh sách để chứa các điểm
            List<TSG.Point> pointsInViewCoords = LayViTriBatDiemXaGo(modelPart, view);

            //khai bao PointList
           

            //Linq
            //dùng linq sắp xếp Point theo thứ tự trên trục Y
            //OrderBy là sắp xếp theo thứ tự từ nhỏ đến lớn
            //OrderByDescending => sắp xếp từ lớn đến nhỏ
            // sắp xếp các điểm theo phương Y, thứ tự từ lớn đến nhỏ
            List<TSG.Point> danhSachDaSapXep = pointsInViewCoords.OrderByDescending(p => p.Y).ToList();
            PointList pointList = ListToPointList(danhSachDaSapXep);

            // tạo vector chỉ hướng dim tổng
            Vector vector = new Vector(0, 1, 0);

            //khai báo chiều cao đường dim
            double distance = 300;

            CreateDim(view, pointList, vector, distance);

            activeDrawing.CommitChanges();
        }

        private void CreateWebHoleDimension(Drawing activeDrawing, TSD.View view, TSM.Part purlin)
        {
            List<TSG.Point> boltPosParallelViewPlane = GetBoltPositionsParallelViewPlane(purlin, view);
            List<TSG.Point> purlinPointsInView = LayViTriBatDiemXaGo(purlin, view);

            //dùng Linq sắp xếp danh sách theo trục Y từ trên xuống
            List<TSG.Point> allPoints = purlinPointsInView.OrderByDescending(p => p.Y).Concat(boltPosParallelViewPlane).ToList();

            PointList pointList = ListToPointList(allPoints);

            Vector vector = new Vector(0,1,0);
            double distance = 100;

            CreateDim(view, pointList, vector, distance);
            activeDrawing.CommitChanges();
        }

        private void CreateFlangeHoleDimension(Drawing activeDrawing, TSD.View view, TSM.Part purlin)
        {
            List<TSG.Point> flangeHolePos = GetFlangeBoltPosInFrontView(purlin, view);
            List<TSG.Point> purlinPointsInView = LayViTriBatDiemXaGo(purlin, view);

            List<TSG.Point> topHoles = new List<Point>();
            List<TSG.Point> bottomHoles = new List<Point>();

            foreach (var p in flangeHolePos)
            {
                if (p.Y > 0)
                {
                    topHoles.Add(p);
                }
                else
                {
                    bottomHoles.Add(p);
                }
            }

            List<TSG.Point> allTopPoints = purlinPointsInView.OrderByDescending(p => p.Y).Concat(topHoles).ToList();
            PointList pointListTop = ListToPointList(allTopPoints);

            List<TSG.Point> allBottomPoints = purlinPointsInView.OrderBy(p => p.Y).Concat(bottomHoles).ToList();
            PointList pointListBottom = ListToPointList(allBottomPoints);

            Vector vectorUp = new Vector(0, 1, 0);
            double distanceTop = 200;

            CreateDim(view, pointListTop, vectorUp, distanceTop);
            CreateDim(view, pointListBottom, new Vector(0, -1, 0), 100);
            activeDrawing.CommitChanges();
        }

        private void CreateOverallVerticalDimension(Drawing activeDrawing, TSD.View view, TSM.Part purlin)
        {
            var purlinPoints = LayViTriBatDiemXaGo(purlin, view);
            List<TSG.Point> maxY = SelectMaxY(purlinPoints);
            List<TSG.Point> minY = SelectMinY(purlinPoints);

            List<TSG.Point> danhSachBatDiem = maxY.Concat(minY).OrderBy(p => p.X).ToList();

            PointList pointList = ListToPointList(danhSachBatDiem);
            Vector left = new Vector(-1,0,0);
            double distance = 200;

            CreateDim(view, pointList, left, distance);
        }

        private void CreateWebHoleVertcalDimension(Drawing activeDrawing, TSD.View view, TSM.Part purlin)
        {
            var purlinPoints = LayViTriBatDiemXaGo(purlin, view);
            List<TSG.Point> maxY = SelectMaxY(purlinPoints);
            List<TSG.Point> minY = SelectMinY(purlinPoints);

            List<TSG.Point> danhSachBatDiem = maxY.Concat(minY).OrderBy(p => p.X).ToList();
            List<TSG.Point> boltPosParallelViewPlane = GetBoltPositionsParallelViewPlane(purlin, view);

            List<TSG.Point> allPoints = danhSachBatDiem.Concat(boltPosParallelViewPlane).ToList();

            PointList pointList = ListToPointList(allPoints);

            Vector left = new Vector(-1, 0, 0);
            double distance = 100;

            CreateDim(view, pointList, left, distance);
        }

        /// <summary>
        /// Chuyển các điểm trong danh sách về hệ trục tọa độ chỉ định
        /// </summary>
        /// <param name="coordinateSystem">Hệ trục tọa độ cần chuyển về</param>
        /// <param name="points">Các điểm cần chuyển</param>
        /// <returns>Danh sách điểm trong hệ trục tọa độ mới</returns>
        private List<TSG.Point> TranformPointsTo(CoordinateSystem coordinateSystem, List<TSG.Point> points)
        {
            // tạo 1 list chứa kết quả
            List<TSG.Point> result = new List<TSG.Point>();

            //tạo matrix để tranform về coords
            Matrix toCoords = MatrixFactory.ToCoordinateSystem(coordinateSystem);
            //duyệt các điểm trong danh sách và chuyển về hệ trục tọa độ
            foreach (var p in points)
            {
                TSG.Point pointInNewCoords = new TSG.Point();

                //dùng matrix chuyển p về coords
                pointInNewCoords = toCoords.Transform(p);

                //thêm điểm đã chuyển vào danh sách
                result.Add(pointInNewCoords);
            }

            //trả về danh sách kết quả
            return result;
        }

        /// <summary>
        /// Trả về danh sách điểm của part, với tọa độ hiện hành trên model
        /// </summary>
        /// <param name="part">Part trên model</param>
        /// <returns></returns>
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

        private List<TSG.Point> GetBoltPositionsParallelViewPlane(TSM.Part purlin, TSD.View view)
        {
            List<BoltGroup> boltGroups = GetBoltGroupsFrom(purlin);
            List<BoltGroup> parallelBoltGroup = new List<BoltGroup>();

            foreach (var bolt in boltGroups)
            {
                CoordinateSystem boltCoords = bolt.GetCoordinateSystem();
                GeometricPlane boltPlane = new GeometricPlane(boltCoords);
                GeometricPlane viewPlane = new GeometricPlane(view.ViewCoordinateSystem);

                if (TSG.Parallel.PlaneToPlane(boltPlane, viewPlane))
                {
                    //chỉ thêm những boltgroup song song với view coords
                    parallelBoltGroup.Add(bolt);
                }
            }

            List<TSG.Point> points = new List<TSG.Point>();
            foreach (BoltGroup boltGroup in parallelBoltGroup)
            {
                //duyệt qua các điểm bolt đơn lẻ
                foreach (TSG.Point point in boltGroup.BoltPositions)
                {
                    points.Add(point);
                }
            }

            //lấy điểm bolt position trong tọa độ view
            List<TSG.Point> pointsInViewCoords = TranformPointsTo(view.ViewCoordinateSystem, points);

            return pointsInViewCoords;
        }

        private List<TSG.Point> GetFlangeBoltPosInFrontView(TSM.Part purlin, TSD.View view)
        {
            List<BoltGroup> boltGroups = GetBoltGroupsFrom(purlin);
            List<TSG.Point> flangeHoles = new List<TSG.Point>();

            foreach (var bolt in boltGroups)
            {
                CoordinateSystem boltCoords = bolt.GetCoordinateSystem();
                Vector zBolt = boltCoords.AxisX.Cross(boltCoords.AxisY);
                Vector yView = view.ViewCoordinateSystem.AxisY;

                if (TSG.Parallel.VectorToVector(zBolt, yView))
                {
                    //chỉ thêm những boltgroup nam tren canh xa go
                    foreach (TSG.Point p in bolt.BoltPositions)
                    {
                        //chuyen p ve view coords
                        flangeHoles.Add(MatrixFactory.ToCoordinateSystem(view.ViewCoordinateSystem)
                                                     .Transform(p));
                    }
                }
            }

            return flangeHoles;
        }

        private List<TSG.Point> GetWebBoltPosInSectionView(TSM.Part purlin, TSD.View view)
        {
            List<BoltGroup> boltGroups = GetBoltGroupsFrom(purlin);
            List<TSG.Point> WebHoles = new List<TSG.Point>();

            foreach (var bolt in boltGroups)
            {
                CoordinateSystem boltCoords = bolt.GetCoordinateSystem();
                Vector zBolt = boltCoords.AxisX.Cross(boltCoords.AxisY);
                Vector xView = view.ViewCoordinateSystem.AxisX;

                if (TSG.Parallel.VectorToVector(zBolt, xView))
                {
                    //chỉ thêm những boltgroup nam tren bung xa go
                    foreach (TSG.Point p in bolt.BoltPositions)
                    {
                        //chuyen p ve view coords
                        WebHoles.Add(MatrixFactory.ToCoordinateSystem(view.ViewCoordinateSystem)
                                                     .Transform(p));
                    }
                }
            }

            return WebHoles;
        }

        private List<TSG.Point> LayViTriBatDiemXaGo(TSM.Part purlin, TSD.View view)
        {
            List<TSG.Point> points = GetPointsOfPart(purlin);

            List<TSG.Point> pointsInView = TranformPointsTo(view.ViewCoordinateSystem, points);

            return pointsInView;
        }

        /// <summary>
        /// Dùng để lấy các boltgroup có trong part
        /// </summary>
        /// <param name="part">Part cần lấy boltgroup</param>
        /// <returns>Danh sách bolt group</returns>
        private List<BoltGroup> GetBoltGroupsFrom(TSM.Part part) 
        {
            //tạo danh sách chứa các bolt group trong purlin
            List<BoltGroup> boltGroupList = new List<BoltGroup>();

            //lấy bolt từ purlin
            ModelObjectEnumerator modelObjectEnumerator = part.GetBolts();
            while (modelObjectEnumerator.MoveNext())
            {
                if (modelObjectEnumerator.Current is BoltGroup boltGroup)
                {
                    //thêm boltgroup vào danh sách
                    boltGroupList.Add(boltGroup);
                }
            }

            //trả về kết quả
            return boltGroupList;
        }

        private PointList ListToPointList(List<TSG.Point> points)
        {
            PointList pointList = new PointList();
            foreach (var p in points)
            {
                pointList.Add(p);
            }

            return pointList;
        }

        private List<TSG.Point> SelectMinX(List<TSG.Point> points)
        {
            //Tạo danh sách kết quả
            List<TSG.Point> result = new List<Point>();

            double minX = FindMinX(points);

            //Lọc ra các điểm có X bằng X nhỏ nhất
            result = points.Where(p => p.X == minX).ToList();

            return result;
        }

        private List<TSG.Point> SelectMaxX(List<TSG.Point> points)
        {
            //Tạo danh sách kết quả
            List<TSG.Point> result = new List<Point>();

            double maxX = FindMaxX(points);

            //Lọc ra các điểm có X bằng X lớn nhất
            result = points.Where(p => p.X == maxX).ToList();

            return result;
        }

        private List<TSG.Point> SelectMinY(List<TSG.Point> points)
        {
            //Tạo danh sách kết quả
            List<TSG.Point> result = new List<Point>();

            double minY = FindMinY(points);

            //Lọc ra các điểm có X bằng X lớn nhất
            result = points.Where(p => p.Y == minY).ToList();

            return result;
        }

        private List<TSG.Point> SelectMaxY(List<TSG.Point> points)
        {
            //Tạo danh sách kết quả
            List<TSG.Point> result = new List<Point>();

            double maxY = FindMaxY(points);

            //Lọc ra các điểm có X bằng X lớn nhất
            result = points.Where(p => p.Y == maxY).ToList();

            return result;
        }

        private double FindMinX(List<TSG.Point> points)
        {
            return points.Min(p => p.X);
        }

        private double FindMaxX(List<TSG.Point> points)
        {
            return points.Max(p => p.X);
        }

        private double FindMinY(List<TSG.Point> points)
        {
            return points.Min(p => p.Y);
        }

        private double FindMaxY(List<TSG.Point> points)
        {
            return points.Max(p => p.Y);
        }

        private void CreateDim(TSD.View view, PointList pointList, Vector vector, double distance)
        {
            // lay attribute dimension
            StraightDimensionSet.StraightDimensionSetAttributes attributes =
                new StraightDimensionSet.StraightDimensionSetAttributes("standard");

            StraightDimensionSetHandler sdh = new StraightDimensionSetHandler();
            sdh.CreateDimensionSet(view, pointList, vector, distance, attributes);
            view.Modify();
        }

        private void DeleteOldDim(TSD.View view)
        {
            var dimensions = view.GetAllObjects(typeof(DimensionBase));

            while (dimensions.MoveNext())
            {
                if (dimensions.Current is DimensionBase dimensionBase)
                {
                    dimensionBase.Delete();
                }
            }

            view.Modify();
        }

        private void button1_Click(object sender, EventArgs e)
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

            //kiểm tra có đang chọn bản vẽ không
            DrawingEnumerator drawingEnum = drawingHandler.GetDrawingSelector().GetSelected();
            if (drawingEnum.GetSize() == 0)
            {
                MessageBox.Show("Không có bản vẽ nào đang chọn");
                return;
            }

            while (drawingEnum.MoveNext())
            {
                if (drawingEnum.Current is Drawing drawing)
                {
                    drawingHandler.SetActiveDrawing(drawing);
                    CreateDimension(drawing);
                    drawingHandler.SaveActiveDrawing();
                }
            }

            ////trả workplane về lại cho người dùng
            workPlaneHandler.SetCurrentTransformationPlane(current);
            model.CommitChanges();
        }

        private TSD.View CreateSection(TSD.View view, TSM.Part purlin)
        {
            List<TSG.Point> points = LayViTriBatDiemXaGo(purlin, view);

            double minX = FindMinX(points);
            double minY = FindMinY(points);
            double maxY = FindMaxY(points);

            TSG.Point startPoint = new Point(minX, maxY);
            Point endPoint = new Point(minX, minY);
            Point insertionPoint = new Point(0, 0, 0); // goc 0 cua ban ve
            double depthUp = 0;
            purlin.GetReportProperty("LENGTH", ref depthUp);

            double depthDown = 0;
            TSD.View.ViewAttributes sectionAtt = new TSD.View.ViewAttributes("standard");
            SectionMarkBase.SectionMarkAttributes sectionMarkAttributes =
                new SectionMarkBase.SectionMarkAttributes("standard");

            TSD.View.CreateSectionView(
                view,
                startPoint,
                endPoint,
                insertionPoint,
                depthUp,
                depthDown,
                sectionAtt,
                sectionMarkAttributes,
                out TSD.View sectionView,
                out SectionMark sectionMark);

            return sectionView;
        }

        private void CreateSectionDimension(TSD.View sectionView, TSM.Part purlin)
        {
            List<Point> purlinPoints = LayViTriBatDiemXaGo(purlin, sectionView);

            List<Point> topFlange = SelectMaxY(purlinPoints);
            List<Point> bottomFlange = SelectMinY(purlinPoints);
            List<Point> leftOverall = topFlange.Concat(bottomFlange).ToList();
            List<Point> leftFold = SelectMinX(purlinPoints);
            List<Point> rightFold = SelectMaxX(purlinPoints);
            List<Point> webHoles = leftOverall.Concat(GetWebBoltPosInSectionView(purlin, sectionView))
                                              .ToList();

            CreateDim(sectionView, ListToPointList(topFlange), new Vector(0, 1, 0), 200);
            CreateDim(sectionView, ListToPointList(bottomFlange), new Vector(0, -1, 0), 200);
            CreateDim(sectionView, ListToPointList(leftOverall), new Vector(-1, 0, 0), 300);
            CreateDim(sectionView, ListToPointList(leftFold), new Vector(-1, 0, 0), 100);
            CreateDim(sectionView, ListToPointList(rightFold), new Vector(1, 0, 0), 100);
            CreateDim(sectionView, ListToPointList(webHoles), new Vector(-1,0,0), 200);
        }
    }
}
