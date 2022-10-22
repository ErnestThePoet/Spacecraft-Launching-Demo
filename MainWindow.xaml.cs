using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpacecraftLaunching
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private const double k1 = 1.8;
        private const double k2 = 0.6;

        private const double CosmicSpeed1 = 7.9;
        private const double CosmicSpeed2 = 11.2;
        private const double CosmicSpeed3 = 16.7;

        private const double EarthSizeLarge = 350.0;
        private const double EarthSizeMedium = 150.0;
        private const double EarthSizeSmall = 100.0;

        private const double PathHide = 0.0;
        private const double PathShow = 1.0;
        

        private Point CanvasCenter;
        private Point CanvasLeftSide;


        private EarthPosition CurrentEarthPosition = EarthPosition.CenterLarge;
        private DemoMode CurrentDemoMode=DemoMode.OnEnterProgram;

        private DispatcherTimer timer = new DispatcherTimer();

        private double roundedSpeedValue = CosmicSpeed1;

        //指示地球处于画板上的哪个预设位置
        private enum EarthPosition
        {
            CenterLarge,
            CenterSmall,
            LeftSide
        }

        //指示当前程序请求的功能状态
        private enum DemoMode
        {
            OnEnterProgram,
            BelowV1,
            EqualsV1,
            V1toV2,
            V2toV3,
            AboveV3,

            OneTrackChangePart1,
            OneTrackChangePart2,

            TwoTrackChangePart1,
            TwoTrackChangePart2,
            TwoTrackChangePart3,

            SyncSimPart1,
            SyncSimPart2,
            SyncSimPart3,
            SyncSimPart4,
            SyncSimPart5,

            CE1SimPart1,
            CE1SimPart2,
            CE1SimPart3,
            CE1SimPart4,
            CE1SimPart5,
            CE1SimPart6,
            CE1SimPart7,
            CE1SimPart8,
            CE1SimPart9,

            CE5SimPart1,
            CE5SimPart2,
            CE5SimPart3,
            CE5SimPart4,
            CE5SimPart5,
            CE5SimPart6
        }



        //【通用】根据目标地球位置、大小，使用动画移动地球
        private void BeginEarthAnimation()
        {
            switch (CurrentEarthPosition)
            {
                case EarthPosition.CenterLarge:
                    animEarthMoveWidth.To=animEarthMoveHeight.To=EarthSizeLarge;

                    animEarthMoveLeft.To=CanvasCenter.X-EarthSizeLarge/2;
                    animEarthMoveTop.To=CanvasCenter.Y-EarthSizeLarge/2;

                    break;

                case EarthPosition.CenterSmall:
                    animEarthMoveWidth.To=animEarthMoveHeight.To=EarthSizeSmall;

                    animEarthMoveLeft.To=CanvasCenter.X-EarthSizeSmall/2;
                    animEarthMoveTop.To=CanvasCenter.Y-EarthSizeSmall/2;

                    break;

                case EarthPosition.LeftSide:
                    animEarthMoveWidth.To=animEarthMoveHeight.To=EarthSizeMedium;

                    animEarthMoveLeft.To=CanvasLeftSide.X-EarthSizeMedium/2;
                    animEarthMoveTop.To=CanvasCenter.Y-EarthSizeMedium/2;

                    break;
            }

            sbEarthMove.Begin();
        }

        //【通用】使用动画显示/隐藏月球
        private void BeginMoonAnimation(bool show)
        {
            animMoonShowHide.To=show?1.0:0.0;
            sbMoonShowHide.Begin();
        }

        //【通用】根据当前程序的功能状态，设置相应路径组的显示隐藏
        private void SetShapesShowHide()
        {
            timer.Stop();

            imgSat.Visibility=Visibility.Hidden;
            
            BeginMoonAnimation(false);

            pthArc.Opacity=CurrentDemoMode==DemoMode.V2toV3||CurrentDemoMode==DemoMode.AboveV3 ? PathShow : PathHide;
            
            pthEllipse.Opacity=CurrentDemoMode==DemoMode.EqualsV1||CurrentDemoMode==DemoMode.V1toV2 ? PathShow : PathHide;
            
            pthPolyLine.Opacity=CurrentDemoMode==DemoMode.BelowV1 ? PathShow : PathHide;


            bool isOneTrackChange = CurrentDemoMode==DemoMode.OneTrackChangePart1||CurrentDemoMode==DemoMode.OneTrackChangePart2;
            pthOneTrackChangePart1.Opacity=
                pthOneTrackChangePart2.Opacity=isOneTrackChange ? PathShow : PathHide;


            bool isTwoTrackChange = 
                CurrentDemoMode==DemoMode.TwoTrackChangePart1||
                CurrentDemoMode==DemoMode.TwoTrackChangePart2||
                CurrentDemoMode==DemoMode.TwoTrackChangePart3;
            pthTwoTrackChangePart1.Opacity=
                pthTwoTrackChangePart2.Opacity=
                pthTwoTrackChangePart3.Opacity=isTwoTrackChange ? PathShow : PathHide;


            bool isSyncSim =
                CurrentDemoMode==DemoMode.SyncSimPart1||
                CurrentDemoMode==DemoMode.SyncSimPart2||
                CurrentDemoMode==DemoMode.SyncSimPart3||
                CurrentDemoMode==DemoMode.SyncSimPart4||
                CurrentDemoMode==DemoMode.SyncSimPart5;
            pthSyncSimPart1.Opacity=
                pthSyncSimPart24.Opacity=
                pthSyncSimPart3.Opacity=
                pthSyncSimPart5.Opacity=isSyncSim ? PathShow : PathHide;


            bool isCE1Sim =
                CurrentDemoMode==DemoMode.CE1SimPart1||
                CurrentDemoMode==DemoMode.CE1SimPart2||
                CurrentDemoMode==DemoMode.CE1SimPart3||
                CurrentDemoMode==DemoMode.CE1SimPart4||
                CurrentDemoMode==DemoMode.CE1SimPart5||
                CurrentDemoMode==DemoMode.CE1SimPart6||
                CurrentDemoMode==DemoMode.CE1SimPart7||
                CurrentDemoMode==DemoMode.CE1SimPart8||
                CurrentDemoMode==DemoMode.CE1SimPart9;

            pthCE1SimPart1.Opacity=
                pthCE1SimPart2.Opacity=
                pthCE1SimPart3.Opacity=
                pthCE1SimPart4.Opacity=
                pthCE1SimPart5.Opacity=
                pthCE1SimPart6.Opacity=
                pthCE1SimPart7.Opacity=
                pthCE1SimPart8.Opacity=
                pthCE1SimPart9.Opacity=isCE1Sim ? PathShow : PathHide;


            bool isCE5Sim =
                CurrentDemoMode==DemoMode.CE5SimPart1||
                CurrentDemoMode==DemoMode.CE5SimPart2||
                CurrentDemoMode==DemoMode.CE5SimPart3||
                CurrentDemoMode==DemoMode.CE5SimPart4||
                CurrentDemoMode==DemoMode.CE5SimPart5||
                CurrentDemoMode==DemoMode.CE5SimPart6;

            pthCE5SimPart1.Opacity=
                pthCE5SimPart2.Opacity=
                pthCE5SimPart3.Opacity=
                pthCE5SimPart4.Opacity=
                pthCE5SimPart5.Opacity=
                pthCE5SimPart6.Opacity=isCE5Sim ? PathShow : PathHide;
        }

        //【发射】根据调定的不同速度，更新路径形状数据
        private void SetPathData()
        {
            switch (CurrentDemoMode)
            {
                case DemoMode.BelowV1:

                    List<Point> points = new List<Point>();

                    double radius = EarthSizeLarge/2+20;

                    if (roundedSpeedValue==0)
                    {
                        points.Add(new Point(CanvasCenter.X, CanvasCenter.Y-radius+40));
                    }
                    else
                    {
                        for (double angle = 0; angle<=2*Math.PI&&
                            (CosmicSpeed1-roundedSpeedValue)/CosmicSpeed1*angle<=1; angle+=0.05)
                        {
                            double localRadius = roundedSpeedValue==0 ? 0 : radius-40*(CosmicSpeed1-roundedSpeedValue)/CosmicSpeed1*angle;
                            points.Add(new Point(CanvasCenter.X+localRadius*Math.Sin(angle),
                                CanvasCenter.Y-localRadius*Math.Cos(angle)));
                        }
                    }

                    pfPolyLine.StartPoint=new Point(CanvasCenter.X, CanvasCenter.Y-radius);

                    plsPolyLine.Points=new PointCollection(points);

                    break;

                case DemoMode.EqualsV1:
                    egEllipse.Center=CanvasCenter;
                    egEllipse.RadiusX=egEllipse.RadiusY=EarthSizeLarge/2+20;

                    break;

                case DemoMode.V1toV2:

                    egEllipse.RadiusX=(EarthSizeMedium/2+10)*Math.Exp((roundedSpeedValue-CosmicSpeed1)/k1);
                    double deltaRX = egEllipse.RadiusX-EarthSizeMedium/2-10;
                    egEllipse.RadiusY=
                        (1-(1-k2)*Math.Sqrt((roundedSpeedValue-CosmicSpeed1)/(CosmicSpeed2-CosmicSpeed1)))*egEllipse.RadiusX;
                    egEllipse.Center=new Point(CanvasLeftSide.X+deltaRX, CanvasCenter.Y);

                    break;

                case DemoMode.V2toV3:
                case DemoMode.AboveV3:
                    double radiusX = (EarthSizeMedium/2+10)*Math.Exp((roundedSpeedValue-CosmicSpeed1)/k1);
                    double deltaRX1 = radiusX-EarthSizeMedium/2-10;
                    double radiusY =
                        (1-(1-k2)*Math.Sqrt((roundedSpeedValue-CosmicSpeed1)/(CosmicSpeed2-CosmicSpeed1)))*radiusX;
                    pthArc.Data=Geometry.Parse($"M {CanvasLeftSide.X-EarthSizeMedium/2-10},{CanvasCenter.Y} "+
                    $"A {radiusX},{radiusY} 0 0 1 "+
                    $"{-radiusX*Math.Sqrt(1-(CanvasCenter.Y*CanvasCenter.Y)/(radiusY*radiusY))+CanvasLeftSide.X+deltaRX1},0");

                    break;

            }
        }

        //【发射】根据调定的不同速度，更新视图中的地球、轨迹、文字显示
        private void UpdateEarthTrackLabel()
        {

            if (roundedSpeedValue<CosmicSpeed1)
            {
                CurrentDemoMode=DemoMode.BelowV1;
                SetPathData();

                SetShapesShowHide();

                lblSpeed.Foreground=new SolidColorBrush(Colors.White);

                lblState.Foreground=new SolidColorBrush(Colors.LightYellow);

                lblState.Content=FindResource("strLaunchBelowV1");

                if (CurrentEarthPosition!=EarthPosition.CenterLarge)
                {
                    CurrentEarthPosition=EarthPosition.CenterLarge;
                    BeginEarthAnimation();
                }

            }
            else if (roundedSpeedValue==CosmicSpeed1)
            {
                CurrentDemoMode=DemoMode.EqualsV1;
                SetPathData();

                SetShapesShowHide();

                lblSpeed.Foreground=new SolidColorBrush(Colors.LawnGreen);

                lblState.Foreground=new SolidColorBrush(Colors.LightYellow);

                lblState.Content=FindResource("strLaunchV1");

                if (CurrentEarthPosition!=EarthPosition.CenterLarge)
                {
                    CurrentEarthPosition=EarthPosition.CenterLarge;
                    BeginEarthAnimation();
                }

            }
            else if (roundedSpeedValue>CosmicSpeed1&&roundedSpeedValue<CosmicSpeed2)
            {
                CurrentDemoMode=DemoMode.V1toV2;
                SetPathData();

                SetShapesShowHide();

                lblSpeed.Foreground=new SolidColorBrush(Colors.Cyan);

                lblState.Foreground=new SolidColorBrush(Colors.LightYellow);

                lblState.Content=FindResource("strLaunchV1toV2");

                if (CurrentEarthPosition!=EarthPosition.LeftSide)
                {
                    CurrentEarthPosition=EarthPosition.LeftSide;
                    BeginEarthAnimation();
                }
            }
            else if (roundedSpeedValue>=CosmicSpeed2&&roundedSpeedValue<CosmicSpeed3)
            {
                CurrentDemoMode=DemoMode.V2toV3;
                SetPathData();

                SetShapesShowHide();

                lblSpeed.Foreground=new SolidColorBrush(Colors.Gold);
                pthArc.Stroke=new SolidColorBrush(Colors.Gold);
                lblState.Foreground=new SolidColorBrush(Colors.Gold);

                lblState.Content=FindResource("strLaunchV2toV3");

                if (CurrentEarthPosition!=EarthPosition.LeftSide)
                {
                    CurrentEarthPosition=EarthPosition.LeftSide;
                    BeginEarthAnimation();
                }
            }
            else
            {
                CurrentDemoMode=DemoMode.AboveV3;

                SetPathData();

                SetShapesShowHide();

                lblSpeed.Foreground=new SolidColorBrush(Colors.Red);
                pthArc.Stroke=new SolidColorBrush(Colors.Red);
                lblState.Foreground=new SolidColorBrush(Colors.Red);

                lblState.Content=FindResource("strLaunchAboveV3");

                if (CurrentEarthPosition!=EarthPosition.LeftSide)
                {
                    CurrentEarthPosition=EarthPosition.LeftSide;
                    BeginEarthAnimation();
                }
            }
        }

        //【示意】设置下一阶段卫星移动的时间、轨迹、加减速参数
        private void SetSatMove(TimeSpan timeSpan,bool isForever,Path path,double acc,double dec)
        {
            animSatMoveLeft.Duration=animSatMoveTop.Duration=new Duration(timeSpan);
            animSatMoveLeft.RepeatBehavior=animSatMoveTop.RepeatBehavior=
                isForever ? RepeatBehavior.Forever : new RepeatBehavior(1);
            animSatMoveLeft.PathGeometry=animSatMoveTop.PathGeometry=path.Data.GetFlattenedPathGeometry();
            animSatMoveLeft.AccelerationRatio=animSatMoveTop.AccelerationRatio=acc;
            animSatMoveLeft.DecelerationRatio=animSatMoveTop.DecelerationRatio=dec;
        }

        //【示意】本阶段计时器完成后，进入下一阶段
        private void BeginDemoNextPart(object sender, EventArgs e)
        {
            switch (CurrentDemoMode)
            {
                case DemoMode.V1toV2:
                    sbSatMove.Begin();
                    break;

                ////////////////////////////////////////////////////////////////////////////////

                case DemoMode.OneTrackChangePart1:
                    CurrentDemoMode=DemoMode.OneTrackChangePart2;

                    lblState.Content=FindResource("strOneTrackChange2");

                    SetSatMove(new TimeSpan(0, 0, 7), true, pthOneTrackChangePart2, 0, 0);

                    timer.Stop();

                    sbSatMove.Begin();

                    break;

                ////////////////////////////////////////////////////////////////////////////////
                
                case DemoMode.TwoTrackChangePart1:
                    CurrentDemoMode=DemoMode.TwoTrackChangePart2;

                    lblState.Content=FindResource("strTwoTrackChange2");

                    SetSatMove(new TimeSpan(0, 0, 0, 0, 3600), false, pthTwoTrackChangePart2, 0, 1);

                    timer.Interval=new TimeSpan(0, 0, 0, 0, 3000);
                    timer.Start();

                    sbSatMove.Begin();
                    break;

                case DemoMode.TwoTrackChangePart2:
                    CurrentDemoMode=DemoMode.TwoTrackChangePart3;

                    lblState.Content=FindResource("strTwoTrackChange3");

                    SetSatMove(new TimeSpan(0, 0, 20), true, pthTwoTrackChangePart3, 0, 0);

                    timer.Stop();

                    sbSatMove.Begin();
                    break;

                ////////////////////////////////////////////////////////////////////////////////

                case DemoMode.SyncSimPart1:
                    CurrentDemoMode=DemoMode.SyncSimPart2;

                    lblState.Content=FindResource("strSyncSim2");

                    SetSatMove(new TimeSpan(0, 0, 0, 0, 3600), false, pthSyncSimPart24, 0, 1);

                    timer.Interval=new TimeSpan(0, 0, 0, 0, 3000);
                    timer.Start();

                    sbSatMove.Begin();
                    break;

                case DemoMode.SyncSimPart2:
                    CurrentDemoMode=DemoMode.SyncSimPart3;

                    SetSatMove(new TimeSpan(0, 0, 0, 0, 3600), false, pthSyncSimPart3, 1, 0);

                    timer.Interval=new TimeSpan(0, 0, 0, 0, 3600);
                    timer.Start();

                    sbSatMove.Begin();
                    break;

                case DemoMode.SyncSimPart3:
                    CurrentDemoMode=DemoMode.SyncSimPart4;

                    SetSatMove(new TimeSpan(0, 0, 0, 0, 3600), false, pthSyncSimPart24, 0, 1);

                    timer.Interval=new TimeSpan(0, 0, 0, 0, 3200);
                    timer.Start();

                    sbSatMove.Begin();
                    break;


                case DemoMode.SyncSimPart4:
                    CurrentDemoMode=DemoMode.SyncSimPart5;

                    lblState.Content=FindResource("strSyncSim3");

                    SetSatMove(new TimeSpan(0, 0, 16), true, pthSyncSimPart5, 0, 0);

                    timer.Stop();

                    sbSatMove.Begin();
                    break;

                ////////////////////////////////////////////////////////////////////////////////

                case DemoMode.CE1SimPart1:
                    CurrentDemoMode=DemoMode.CE1SimPart2;

                    SetSatMove(new TimeSpan(0, 0, 0, 0, 3600), false, pthCE1SimPart2, 1, 0);

                    timer.Interval=new TimeSpan(0, 0, 0, 0, 3600);
                    timer.Start();

                    sbSatMove.Begin();
                    break;

                case DemoMode.CE1SimPart2:
                    CurrentDemoMode=DemoMode.CE1SimPart3;

                    lblState.Content=FindResource("strCE1Sim2");

                    SetSatMove(new TimeSpan(0, 0, 0, 0, 2700), false, pthCE1SimPart3, 0, 1);

                    timer.Interval=new TimeSpan(0, 0, 0, 0, 2100);
                    timer.Start();

                    sbSatMove.Begin();
                    break;

                case DemoMode.CE1SimPart3:
                    CurrentDemoMode=DemoMode.CE1SimPart4;

                    SetSatMove(new TimeSpan(0, 0, 0, 0, 2700), false, pthCE1SimPart4, 1, 0);

                    timer.Interval=new TimeSpan(0, 0, 0, 0, 2700);
                    timer.Start();

                    sbSatMove.Begin();
                    break;

                case DemoMode.CE1SimPart4:
                    CurrentDemoMode=DemoMode.CE1SimPart5;

                    lblState.Content=FindResource("strCE1Sim3");

                    SetSatMove(new TimeSpan(0, 0, 0, 0, 1800), false, pthCE1SimPart5, 0, 0);

                    timer.Interval=new TimeSpan(0, 0, 0, 0, 1800);
                    timer.Start();

                    sbSatMove.Begin();
                    break;

                case DemoMode.CE1SimPart5:
                    CurrentDemoMode=DemoMode.CE1SimPart6;

                    lblState.Content=FindResource("strCE1Sim4");

                    SetSatMove(new TimeSpan(0, 0, 0, 0, 5500), false, pthCE1SimPart6, 0, 1);

                    timer.Interval=new TimeSpan(0, 0, 0, 0, 4500);
                    timer.Start();

                    sbSatMove.Begin();
                    break;

                case DemoMode.CE1SimPart6:
                    CurrentDemoMode=DemoMode.CE1SimPart7;

                    SetSatMove(new TimeSpan(0, 0, 0, 0, 3000), false, pthCE1SimPart7, 1, 0);

                    timer.Interval=new TimeSpan(0, 0, 0, 0, 3000);
                    timer.Start();

                    sbSatMove.Begin();
                    break;

                case DemoMode.CE1SimPart7:
                    CurrentDemoMode=DemoMode.CE1SimPart8;

                    lblState.Content=FindResource("strCE1Sim5");

                    SetSatMove(new TimeSpan(0, 0, 0, 0, 5500), false, pthCE1SimPart8, 0, 1);

                    timer.Interval=new TimeSpan(0, 0, 0, 0, 4500);
                    timer.Start();

                    sbSatMove.Begin();
                    break;

                case DemoMode.CE1SimPart8:
                    CurrentDemoMode=DemoMode.CE1SimPart9;

                    SetSatMove(new TimeSpan(0, 0, 0, 0, 4000), false, pthCE1SimPart9, 1, 0);

                    timer.Interval=new TimeSpan(0, 0, 0, 0, 4000);
                    timer.Start();

                    sbSatMove.Begin();
                    break;

                case DemoMode.CE1SimPart9:
                    CurrentDemoMode=DemoMode.CE1SimPart8;

                    lblState.Content=FindResource("strCE1Sim6");

                    SetSatMove(new TimeSpan(0, 0, 0, 0, 4000), false, pthCE1SimPart8, 0, 1);

                    timer.Interval=new TimeSpan(0, 0, 0, 0, 3400);
                    timer.Start();

                    sbSatMove.Begin();
                    break;


                ////////////////////////////////////////////////////////////////////////////////

                case DemoMode.CE5SimPart1:
                    CurrentDemoMode=DemoMode.CE5SimPart2;

                    lblState.Content=FindResource("strCE5Sim2");

                    SetSatMove(new TimeSpan(0, 0, 0, 0, 5500), false, pthCE5SimPart2, 0, 0.8);

                    timer.Interval=new TimeSpan(0, 0, 0, 0, 4500);
                    timer.Start();

                    sbSatMove.Begin();
                    break;

                case DemoMode.CE5SimPart2:
                    CurrentDemoMode=DemoMode.CE5SimPart3;

                    SetSatMove(new TimeSpan(0, 0, 0, 0, 3500), false, pthCE5SimPart3, 1, 0);

                    timer.Interval=new TimeSpan(0, 0, 0, 0, 3500);
                    timer.Start();

                    sbSatMove.Begin();
                    break;

                case DemoMode.CE5SimPart3:
                    CurrentDemoMode=DemoMode.CE5SimPart4;

                    lblState.Content=FindResource("strCE5Sim3");

                    SetSatMove(new TimeSpan(0, 0, 0, 0, 6500), false, pthCE5SimPart4, 0, 0.8);

                    timer.Interval=new TimeSpan(0, 0, 0, 0, 5500);
                    timer.Start();

                    sbSatMove.Begin();
                    break;

                case DemoMode.CE5SimPart4:
                    CurrentDemoMode=DemoMode.CE5SimPart5;

                    SetSatMove(new TimeSpan(0, 0, 0, 0, 3500), false, pthCE5SimPart5, 1, 0);

                    timer.Interval=new TimeSpan(0, 0, 0, 0, 3500);
                    timer.Start();

                    sbSatMove.Begin();
                    break;

                case DemoMode.CE5SimPart5:
                    CurrentDemoMode=DemoMode.CE5SimPart6;

                    lblState.Content=FindResource("strCE5Sim4");

                    SetSatMove(new TimeSpan(0, 0, 0, 0, 6000), true, pthCE5SimPart6, 0, 0);

                    timer.Stop();

                    sbSatMove.Begin();
                    break;

                default:
                    timer.Stop();
                    break;
            }

        }

 

        private void sldSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            roundedSpeedValue = Math.Round(sldSpeed.Value, 1);

            lblSpeed.Content=string.Format("{0}km/s", roundedSpeedValue);

            UpdateEarthTrackLabel();
        }

        private void btnSpeedDown_Click(object sender, RoutedEventArgs e)
        {
            sldSpeed.Value-=0.1;
        }

        private void btnSpeedUp_Click(object sender, RoutedEventArgs e)
        {
            sldSpeed.Value+=0.1;
        }

        private void btnCosmicSpeed1_Click(object sender, RoutedEventArgs e)
        {
            sldSpeed.Value=CosmicSpeed1;
        }

        private void btnCosmicSpeed2_Click(object sender, RoutedEventArgs e)
        {
            sldSpeed.Value=CosmicSpeed2;
        }

        private void btnCosmicSpeed3_Click(object sender, RoutedEventArgs e)
        {
            sldSpeed.Value=CosmicSpeed3;
        }

        //在这里执行所有xaml中未完成的初始化操作
        private void cvAnimation_Loaded(object sender, RoutedEventArgs e)
        {
            imgSat.Visibility=Visibility.Hidden;

            CanvasCenter.X=cvAnimation.ActualWidth/2;
            CanvasCenter.Y=cvAnimation.ActualHeight/2;

            CanvasLeftSide.X=cvAnimation.ActualWidth/4;
            CanvasLeftSide.Y=CanvasCenter.Y;

            egEllipse.Center=CanvasCenter;
            egEllipse.RadiusX=egEllipse.RadiusY=EarthSizeLarge/2+20;

            imgMoon.SetValue(Canvas.LeftProperty, 3*cvAnimation.ActualWidth/4-imgMoon.ActualWidth/2);
            imgMoon.SetValue(Canvas.TopProperty, cvAnimation.ActualHeight/2-imgMoon.ActualHeight/2);

            ttPathCommom.X=CanvasCenter.X;
            ttPathCommom.Y=CanvasCenter.Y;

            ttPathCommomLeft.X=CanvasLeftSide.X;
            ttPathCommomLeft.Y=CanvasLeftSide.Y;

            //MessageBox.Show(CanvasCenter.Y.ToString());

            timer.Tick+=BeginDemoNextPart;

            BeginEarthAnimation();

            sbEllipseShowHide.Begin();
        }

        private void btnLaunch_Click(object sender, RoutedEventArgs e)
        {
            UpdateEarthTrackLabel();

            ttSat.X=ttSat.Y=-imgSat.Width/2;

            if (roundedSpeedValue<CosmicSpeed1)
            {
                SetSatMove(new TimeSpan(0, 0, 4), false, pthPolyLine, 0, 0);
            }
            else if (roundedSpeedValue==CosmicSpeed1)
            {
                SetSatMove(new TimeSpan(0, 0, 4), true, pthEllipse, 0, 0);
            }
            else if (roundedSpeedValue>CosmicSpeed1&&roundedSpeedValue<CosmicSpeed2)
            {
                SetSatMove(new TimeSpan(0, 0, 5), true, pthEllipse, 0.5, 0.5);

                timer.Interval=new TimeSpan(0, 0, 0, 0, 4800);
                timer.Start();
            }
            else
            {
                TimeSpan timeSpan;
                if (roundedSpeedValue<CosmicSpeed3)
                {
                    timeSpan=new TimeSpan(0, 0, 0,0,300);
                }
                else
                {
                    timeSpan=new TimeSpan(0, 0, 0,0,100);
                }

                SetSatMove(timeSpan, false, pthArc, 0, 0);
            }

            sbSatMove.Begin();
            
            imgSat.Visibility=Visibility.Visible;
        }

        private void btnOneTrackChange_Click(object sender, RoutedEventArgs e)
        {
            CurrentDemoMode=DemoMode.OneTrackChangePart1;

            CurrentEarthPosition=EarthPosition.CenterSmall;

            BeginEarthAnimation();

            SetShapesShowHide();

            lblState.Foreground=new SolidColorBrush(Colors.LightYellow);

            lblState.Content=FindResource("strOneTrackChange1");

            ttSat.X=CanvasCenter.X-imgSat.Width/2;
            ttSat.Y=CanvasCenter.Y-imgSat.Height/2;

            SetSatMove(new TimeSpan(0, 0, 3), false, pthOneTrackChangePart1, 0, 1);

            timer.Interval=new TimeSpan(0, 0, 0, 0, 2500);
            timer.Start();

            sbSatMove.Begin();

            imgSat.Visibility=Visibility.Visible;

        }

        private void btnTwoTrackChange_Click(object sender, RoutedEventArgs e)
        {
            CurrentDemoMode=DemoMode.TwoTrackChangePart1;

            CurrentEarthPosition=EarthPosition.CenterSmall;

            BeginEarthAnimation();

            SetShapesShowHide();

            lblState.Foreground=new SolidColorBrush(Colors.LightYellow);

            lblState.Content=FindResource("strTwoTrackChange1");

            ttSat.X=CanvasCenter.X-imgSat.Width/2;
            ttSat.Y=CanvasCenter.Y-imgSat.Height/2;

            SetSatMove(new TimeSpan(0, 0, 3), false, pthTwoTrackChangePart1, 0, 0);

            timer.Interval=new TimeSpan(0, 0, 3);
            timer.Start();

            sbSatMove.Begin();

            imgSat.Visibility=Visibility.Visible;
        }

        private void btnSyncSim_Click(object sender, RoutedEventArgs e)
        {
            CurrentDemoMode=DemoMode.SyncSimPart1;

            CurrentEarthPosition=EarthPosition.CenterSmall;

            BeginEarthAnimation();

            SetShapesShowHide();

            lblState.Foreground=new SolidColorBrush(Colors.LightYellow);

            lblState.Content=FindResource("strSyncSim1");

            ttSat.X=CanvasCenter.X-imgSat.Width/2;
            ttSat.Y=CanvasCenter.Y-imgSat.Height/2;

            SetSatMove(new TimeSpan(0, 0, 3), false, pthSyncSimPart1, 0, 0);

            timer.Interval=new TimeSpan(0, 0, 3);
            timer.Start();

            sbSatMove.Begin();

            imgSat.Visibility=Visibility.Visible;

        }

        private void btnCE1Sim_Click(object sender, RoutedEventArgs e)
        {
            CurrentDemoMode=DemoMode.CE1SimPart1;

            CurrentEarthPosition=EarthPosition.LeftSide;

            BeginEarthAnimation();

            SetShapesShowHide();

            BeginMoonAnimation(true);

            lblState.Foreground=new SolidColorBrush(Colors.LightYellow);

            lblState.Content=FindResource("strCE1Sim1");

            ttSat.X=CanvasLeftSide.X-imgSat.Width/2;
            ttSat.Y=CanvasLeftSide.Y-imgSat.Height/2;

            SetSatMove(new TimeSpan(0, 0, 0,0,3600), false, pthCE1SimPart1, 0, 1);

            timer.Interval=new TimeSpan(0, 0, 0,0,2800);
            timer.Start();

            sbSatMove.Begin();

            imgSat.Visibility=Visibility.Visible;
        }

        private void btnCE5Sim_Click(object sender, RoutedEventArgs e)
        {
            CurrentDemoMode=DemoMode.CE5SimPart1;

            CurrentEarthPosition=EarthPosition.LeftSide;

            BeginEarthAnimation();

            SetShapesShowHide();

            BeginMoonAnimation(true);

            lblState.Foreground=new SolidColorBrush(Colors.LightYellow);

            lblState.Content=FindResource("strCE5Sim1");

            ttSat.X=CanvasLeftSide.X-imgSat.Width/2;
            ttSat.Y=CanvasLeftSide.Y-imgSat.Height/2;

            SetSatMove(new TimeSpan(0, 0, 0, 0, 2000), false, pthCE5SimPart1, 0,0);

            timer.Interval=new TimeSpan(0, 0, 0, 0, 2000);
            timer.Start();

            sbSatMove.Begin();

            imgSat.Visibility=Visibility.Visible;
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        private void sbSatMove_Completed(object sender, EventArgs e)
        {
            switch (CurrentDemoMode)
            {
                case DemoMode.BelowV1:
                case DemoMode.V2toV3:
                case DemoMode.AboveV3:
                    imgSat.Visibility=Visibility.Hidden;
                    break;
            }
        }

        private void sbEllipseShowHide_Completed(object sender, EventArgs e)
        {
            pthEllipse.Opacity=(double)animEllipseShowHide.To;
        }
    }
}
