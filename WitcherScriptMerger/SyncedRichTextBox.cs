using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WitcherScriptMerger
{
    public class SyncedRichTextBox : RichTextBox
    {
        #region Types

        public enum ScrollBarType : int
        {
            Horizontal = 0,
            Vertical,
            Control,
            Both
        }

        public enum VScrollType
        {
            LineUp = 0,
            LineDown,
            PageDown,
            PageUp,
            ThumbPosition
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct Dword
        {
            [FieldOffset(0)]
            public uint Number;

            [FieldOffset(0)]
            public ushort Low;

            [FieldOffset(2)]
            public ushort High;
        }

        #endregion

        #region Members

        public const int WM_HSCROLL = 0x0114;
        public const int WM_VSCROLL = 0x0115;
        public const int WM_MOUSEWHEEL = 0x020A;

        public List<SyncedRichTextBox> Peers = new List<SyncedRichTextBox>();

        #endregion

        [DllImport("user32.dll")]
        private static extern int GetScrollPos(IntPtr hWnd, int nBar);

        public void AddPeer(SyncedRichTextBox otherBox)
        {
            Peers.Add(otherBox);
        }

        protected override void OnSelectionChanged(EventArgs e)
        {
            SyncScrollBars();
            base.OnSelectionChanged(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
                SyncScrollBars();
            base.OnMouseUp(e);
        }

        public void SyncScrollBars()
        {
            SyncVerticalScroll();
            SyncHorizontalScroll();
        }

        public void SyncVerticalScroll()
        {
            int pos = GetScrollPos(this.Handle, (int)ScrollBarType.Vertical);
            Dword test = new Dword
            {
                High = (ushort)pos,
                Low = (ushort)VScrollType.ThumbPosition
            };
            var m = Message.Create(this.Handle, WM_VSCROLL, (IntPtr)test.Number, (IntPtr)0);
            WndProc(ref m);
        }

        public void SyncHorizontalScroll()
        {
            int pos = GetScrollPos(this.Handle, (int)ScrollBarType.Horizontal);
            Dword test = new Dword
            {
                High = (ushort)pos,
                Low = (ushort)VScrollType.ThumbPosition
            };
            var m = Message.Create(this.Handle, WM_HSCROLL, (IntPtr)test.Number, (IntPtr)0);
            WndProc(ref m);
        }

        private void SyncPeers(IntPtr wParam)
        {
            var msg = new Message();
            msg.Msg = WM_VSCROLL;
            msg.WParam = (IntPtr)wParam;
            WndProc(ref msg);
        }

        private void DirectWndProc(ref Message m)
        {
            base.WndProc(ref m);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HSCROLL || m.Msg == WM_VSCROLL)
            {
                foreach (var peer in Peers)
                {
                    var peerMsg = Message.Create(peer.Handle, m.Msg, m.WParam, m.LParam);
                    peer.DirectWndProc(ref peerMsg);
                }
            }
            if (m.Msg == WM_MOUSEWHEEL)
            {
                var msgData = new Dword();
                if ((int)m.WParam < 0)
                    msgData.Low = (ushort)VScrollType.LineDown;
                else
                    msgData.Low = (ushort)VScrollType.LineUp;
                
                var msg = new Message
                {
                    HWnd = this.Handle,
                    Msg = WM_VSCROLL,
                    WParam = (IntPtr)msgData.Number
                };
                WndProc(ref msg);

                return;
            }
            base.WndProc(ref m);
        }
    }
}
