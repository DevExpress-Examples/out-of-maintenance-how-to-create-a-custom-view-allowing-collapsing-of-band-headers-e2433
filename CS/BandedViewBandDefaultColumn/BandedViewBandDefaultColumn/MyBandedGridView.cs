using System.Drawing;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.Utils.Drawing;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Drawing;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.BandedGrid.ViewInfo;

namespace BandedViewBandDefaultColumn {
    public class MyBandedGridView : BandedGridView {
        public MyBandedGridView(GridControl ownerGrid)
            : base(ownerGrid) {
        }
        public MyBandedGridView()
            : base() {
            CustomDrawBandHeader += new BandHeaderCustomDrawEventHandler(OnCustomDrawBandHeader);
            MouseMove += new MouseEventHandler(OnMouseMove);
            MouseDown += new MouseEventHandler(OnMouseDown);
            MouseUp += new MouseEventHandler(OnMouseUp);
        }

        private Rectangle CalcButtonBounds(Rectangle headerBounds) {
            Rectangle buttonBounds = headerBounds;
            buttonBounds.Inflate(-1, -1);
            buttonBounds.Width = 21;

            return buttonBounds;
        }

        private Rectangle CalcCaptionRectWithButton(BandHeaderCustomDrawEventArgs e, bool addButtonWidth) {
            Rectangle captionRect = e.Info.CaptionRect;
            if (addButtonWidth)
                captionRect.X += CalcButtonBounds(e.Bounds).Width;
            else
                captionRect.X -= CalcButtonBounds(e.Bounds).Width;

            return captionRect;
        }

        private EditorButtonObjectInfoArgs FindButtonInnerElement(GridBandInfoArgs bandInfo) {
            EditorButtonObjectInfoArgs retValue = null;
            for (int i = 0; i < bandInfo.InnerElements.Count; i++)
                if (bandInfo.InnerElements[i].ElementInfo is EditorButtonObjectInfoArgs) {
                    retValue = (EditorButtonObjectInfoArgs)bandInfo.InnerElements[i].ElementInfo;
                    break;
                }

            return retValue;
        }

        private bool GetBandColumnsDefaultOnlyVisibility(GridBand band) {
            for (int i = 0; i < band.Columns.Count; i++)
                if (!((MyBandedGridColumn)band.Columns[i]).DefaultBandColumn && (band.Columns[i]).Visible)
                    return false;

            return true;
        }

        private void OnCustomDrawBandHeader(object sender, BandHeaderCustomDrawEventArgs e) {
            if (e.Band == null) return;
            //...
            DrawElementInfo buttonElementInfo = null;
            for (int i = 0; i < e.Info.InnerElements.Count; i++)
                if (e.Info.InnerElements[i].ElementInfo is EditorButtonObjectInfoArgs) {
                    buttonElementInfo = e.Info.InnerElements[i];
                    break;
                }

            if (buttonElementInfo == null) {
                ButtonPredefines buttonKind = ButtonPredefines.Minus;
                if (GetBandColumnsDefaultOnlyVisibility(e.Band))
                    buttonKind = ButtonPredefines.Plus;

                EditorButton button = new EditorButton(buttonKind);
                EditorButtonPainter buttonPainter = EditorButtonHelper.GetPainter(BorderStyles.Default);
                EditorButtonObjectInfoArgs buttonInfoArgs = new EditorButtonObjectInfoArgs(e.Cache, button, e.Info.Appearance);

                buttonElementInfo = new DrawElementInfo(buttonPainter, buttonInfoArgs);
                buttonElementInfo.ElementInfo.Bounds = CalcButtonBounds(e.Bounds);
                buttonElementInfo.ElementInfo.State = ObjectState.Normal;

                e.Info.InnerElements.Add(buttonElementInfo);
                e.Info.CaptionRect = CalcCaptionRectWithButton(e, true);
            }

            e.Painter.DrawObject(e.Info);
            e.Handled = true;
        }

        private void OnMouseMove(object sender, MouseEventArgs e) {
            if (!(sender is MyBandedGridView))
                return;

            MyBandedGridView view = (MyBandedGridView)sender;
            BandedGridHitInfo hitInfo = view.CalcHitInfo(e.X, e.Y);

            if (hitInfo.HitTest == BandedGridHitTest.Band) {
                GridBandInfoArgs bandInfo = view.ViewInfo.BandsInfo[hitInfo.Band];
                EditorButtonObjectInfoArgs bandButtonInfo = FindButtonInnerElement(bandInfo);
                if (bandButtonInfo != null)
                    if (CalcButtonBounds(bandInfo.Bounds).Contains(e.X, e.Y)) {
                        bandButtonInfo.State = ObjectState.Hot;
                        view.InvalidateBandHeader(hitInfo.Band);

                        ((DXMouseEventArgs)e).Handled = true;
                    }
                    else {
                        bandButtonInfo.State = ObjectState.Normal;
                        view.InvalidateBandHeader(hitInfo.Band);
                    }
            }
            else {
                for (int i = 0; i < view.ViewInfo.BandsInfo.Count; i++) {
                    EditorButtonObjectInfoArgs bandButtonInfo = FindButtonInnerElement(view.ViewInfo.BandsInfo[i]);
                    if (bandButtonInfo != null)
                        bandButtonInfo.State = ObjectState.Normal;
                }
            }
        }

        private void OnMouseDown(object sender, MouseEventArgs e) {
            if (e.Button != MouseButtons.Left || !(sender is MyBandedGridView))
                return;

            MyBandedGridView view = (MyBandedGridView)sender;
            BandedGridHitInfo hitInfo = view.CalcHitInfo(e.X, e.Y);

            if (hitInfo.HitTest == BandedGridHitTest.Band) {
                GridBandInfoArgs bandInfo = view.ViewInfo.BandsInfo[hitInfo.Band];
                if (CalcButtonBounds(bandInfo.Bounds).Contains(e.X, e.Y)) {
                    EditorButtonObjectInfoArgs bandButtonInfo = FindButtonInnerElement(bandInfo);
                    if (bandButtonInfo != null) {
                        bandButtonInfo.State = ObjectState.Pressed;
                        view.InvalidateBandHeader(hitInfo.Band);
                    }

                    ((DXMouseEventArgs)e).Handled = true;
                }
                else
                    view.InvalidateBandHeader(hitInfo.Band);
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e) {
            if (e.Button != MouseButtons.Left || !(sender is MyBandedGridView))
                return;

            MyBandedGridView view = (MyBandedGridView)sender;
            BandedGridHitInfo hitInfo = view.CalcHitInfo(e.X, e.Y);

            if (hitInfo.HitTest == BandedGridHitTest.Band) {
                GridBandInfoArgs bandInfo = view.ViewInfo.BandsInfo[hitInfo.Band];
                if (CalcButtonBounds(bandInfo.Bounds).Contains(e.X, e.Y)) {
                    EditorButtonObjectInfoArgs bandButtonInfo = FindButtonInnerElement(bandInfo);
                    if (bandButtonInfo != null) {
                        for (int i = 0; i < hitInfo.Band.Columns.Count; i++)
                            if (!((MyBandedGridColumn)hitInfo.Band.Columns[i]).DefaultBandColumn)
                                hitInfo.Band.Columns[i].Visible = !hitInfo.Band.Columns[i].Visible;
                    }

                    ((DXMouseEventArgs)e).Handled = true;
                }
            }
        }

        protected override GridColumnCollection CreateColumnCollection() {
            return new MyBandedGridColumnCollection(this);
        }

        protected override string ViewName {
            get {
                return "MyBandedGridView";
            }
        }

        public new BandedGridViewInfo ViewInfo {
            get {
                return base.ViewInfo as BandedGridViewInfo;
            }
        }
    }
}