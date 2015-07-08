using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Input;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace WinStoreDrawTools
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        InkManager _inkManager = new Windows.UI.Input.Inking.InkManager(); //インクマネージャ
        private uint _penID; //ペンID
        private uint _touchID; //タッチID
        private Point _previousContactPt; //事前コンタクトポインター
        private Point currentContactPt; //接続時コンタクトポインター
        private double X1; //Distance関数用befX
        private double Y1; //Distance関数用befY
        private double X2; //Distance関数用aftX
        private double Y2; //Distance関数用aftY

//        private PointerDeviceType pointerDevType;
//        private double STROKETHINESS;

        //メインページ
        public MainPage()
        {
            this.InitializeComponent();
            
            InkCanvas.PointerPressed += new PointerEventHandler(InkCanvas_PointerPressed);
            InkCanvas.PointerMoved += new PointerEventHandler(InkCanvas_PointerMoved);
            InkCanvas.PointerReleased += new PointerEventHandler(InkCanvas_PointerReleased);
            InkCanvas.PointerExited += new PointerEventHandler(InkCanvas_PointerReleased);
        }

        //クリック時操作
        private void InkCanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // Get information about the pointer location.
            PointerPoint pt = e.GetCurrentPoint(InkCanvas);
            _previousContactPt = pt.Position;

            //Accept input only form a pen or mouse with the left button pressed.
            PointerDeviceType pointerDevType = e.Pointer.PointerDeviceType;
            if (pointerDevType == PointerDeviceType.Pen ||
                  pointerDevType == PointerDeviceType.Mouse &&
                  pt.Properties.IsLeftButtonPressed)
            {
                // Pass the pointer information to the InkManager.
                _inkManager.ProcessPointerDown(pt);
                _penID = pt.PointerId;

                e.Handled = true;
            }

            else if (pointerDevType == PointerDeviceType.Touch)
            {
                // Process touch input
                _inkManager.ProcessPointerDown(pt);
                _penID = pt.PointerId;

                e.Handled = true;
            }
        }
        //PointerPressedイベントに関連付けられているポインターがCanvas上に移動すると発生する処理
        private void InkCanvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if(e.Pointer.PointerId == _penID)
            {
                PointerPoint pt = e.GetCurrentPoint(InkCanvas);

                //
                //
                //
                //
                //Point
                currentContactPt = pt.Position;
                if (Distance(currentContactPt, _previousContactPt) > 2)
                {
                    Line line = new Line()
                    {
                        X1 = _previousContactPt.X,
                        Y1 = _previousContactPt.Y,
                        X2 = currentContactPt.X,
                        Y2 = currentContactPt.Y,
                        StrokeThickness = 4.0,
                        Stroke = new SolidColorBrush(Windows.UI.Colors.Red)
                    };

                    _previousContactPt = currentContactPt;

                    //
                    //
                    InkCanvas.Children.Add(line);

                    //
                    _inkManager.ProcessPointerUpdate(pt);
                }
            }

            else if (e.Pointer.PointerId == _touchID)
            {
                // Process touch input
                PointerPoint pt = e.GetCurrentPoint(InkCanvas);

                //
                //
                //
                //
                //Point
                currentContactPt = pt.Position;
                if (Distance(currentContactPt, _previousContactPt) > 2)
                {
                    Line line = new Line()
                    {
                        X1 = _previousContactPt.X,
                        Y1 = _previousContactPt.Y,
                        X2 = currentContactPt.X,
                        Y2 = currentContactPt.Y,
                        StrokeThickness = 4.0,
                        Stroke = new SolidColorBrush(Windows.UI.Colors.Red)
                    };

                    _previousContactPt = currentContactPt;

                    //
                    //
                    InkCanvas.Children.Add(line);

                    //
                    _inkManager.ProcessPointerUpdate(pt);
                }
            }

            e.Handled = true;
        }

        private double Distance(Point currentContactPt, Point _previousContactPt)
        {
            double dx = currentContactPt.X - _previousContactPt.X;
            double dy = currentContactPt.Y - _previousContactPt.Y;
            return Math.Sqrt(dx*dx + dy*dy);
        }

        //Pointerリリース時処理
        private void InkCanvas_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerId == _penID)
            {
                PointerPoint pt = e.GetCurrentPoint(InkCanvas);

                //
                _inkManager.ProcessPointerUp(pt);
            }

            else if (e.Pointer.PointerId == _touchID)
            {
                // Process touch input
                PointerPoint pt = e.GetCurrentPoint(InkCanvas);

                //
                _inkManager.ProcessPointerUp(pt);
            }

            _touchID = 0;
            _penID = 0;

            //
            //RenderAllStroke(); //test case で消されてた。

            e.Handled = true;
        }
    }
}
