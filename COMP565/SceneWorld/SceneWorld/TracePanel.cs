using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SceneWorld
{

    public partial class TracePanel : Form
    {
        private SceneWorld world;

        public TracePanel(SceneWorld w)
        {
            InitializeComponent();
            world = w;
        }

        // Properties

        public string Trace
        {
            get { return traceRTB.Text; }
            set { traceRTB.AppendText(value); }
        }  // AppendText focus on end of trace

    }
}