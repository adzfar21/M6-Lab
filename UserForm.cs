using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GraphApp
{
    public class UserForm : Form
    {
        private readonly string userName;
        private Graph_Manager manager;

        // controls
        private Panel graphPanel;
        private ListBox graphListBox;
        private Label statusLabel;

        // input fields
        private TextBox txtGraphID, txtVertexID, txtVX, txtVY;
        private TextBox txtEdgeID, txtFromV, txtToV;
        private TextBox txtCopyID;
        private TextBox txtUpdateType, txtUpdateID, txtUpdateX, txtUpdateY;

        public UserForm(string name)
        {
            userName = name;
            manager = Graph_Manager.GetInstance();
            InitializeUI();
        }

        private void InitializeUI()
        {
            Text = $"Graph Manager — {userName}";
            Width = 950;
            Height = 680;
            StartPosition = FormStartPosition.Manual;

            // graph display panel 
            graphPanel = new Panel
            {
                Location = new Point(10, 10),
                Size = new Size(500, 400),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };
            Controls.Add(graphPanel);

            // graph list 
            var lblList = new Label { Text = "All Graphs:", Location = new Point(520, 10), AutoSize = true };
            Controls.Add(lblList);

            graphListBox = new ListBox
            {
                Location = new Point(520, 30),
                Size = new Size(390, 120)
            };
            graphListBox.DoubleClick += (s, e) => DisplaySelectedGraph();
            Controls.Add(graphListBox);

            // status label
            statusLabel = new Label
            {
                Location = new Point(10, 420),
                Size = new Size(900, 20),
                ForeColor = Color.DarkGreen,
                Font = new Font("Arial", 9, FontStyle.Italic)
            };
            Controls.Add(statusLabel);

            int top = 165;

            // create grapgh
            AddSectionLabel("CREATE GRAPH", 520, top); top += 22;
            var btnCreate = new Button { Text = "Create New Graph", Location = new Point(520, top), Width = 150 };
            btnCreate.Click += BtnCreate_Click;
            Controls.Add(btnCreate);
            top += 35;

            // add vertex
            AddSectionLabel("ADD VERTEX  [Graph ID | Vertex ID | X | Y]", 520, top); top += 22;
            txtGraphID = AddTextBox(520, top, 50, "GID");
            txtVertexID = AddTextBox(575, top, 50, "VID");
            txtVX = AddTextBox(630, top, 50, "X");
            txtVY = AddTextBox(685, top, 50, "Y");
            var btnAddV = new Button { Text = "Add Vertex", Location = new Point(740, top), Width = 90 };
            btnAddV.Click += BtnAddVertex_Click;
            Controls.Add(btnAddV);
            top += 35;

            // add edge
            AddSectionLabel("ADD EDGE  [Graph ID | Edge ID | From VID | To VID]", 520, top); top += 22;
            txtEdgeID = AddTextBox(520, top, 50, "EID");
            txtFromV = AddTextBox(575, top, 50, "From");
            txtToV = AddTextBox(630, top, 50, "To");
            var btnAddE = new Button { Text = "Add Edge", Location = new Point(685, top), Width = 90 };
            btnAddE.Click += BtnAddEdge_Click;
            Controls.Add(btnAddE);
            // reuse txtGraphID for graph id - add a separate one
            var lblNote = new Label { Text = "(uses Graph ID above)", Location = new Point(520, top + 25), AutoSize = true, ForeColor = Color.Gray, Font = new Font("Arial", 7) };
            Controls.Add(lblNote);
            top += 50;

            // update vertex
            AddSectionLabel("UPDATE VERTEX  [Graph ID | Vertex ID | New X | New Y]", 520, top); top += 22;
            txtUpdateType = AddTextBox(520, top, 50, "GID");
            txtUpdateID = AddTextBox(575, top, 50, "VID");
            txtUpdateX = AddTextBox(630, top, 50, "X");
            txtUpdateY = AddTextBox(685, top, 50, "Y");
            var btnUpdV = new Button { Text = "Update Vertex", Location = new Point(740, top), Width = 110 };
            btnUpdV.Click += BtnUpdateVertex_Click;
            Controls.Add(btnUpdV);
            top += 35;

            // copy graph
            AddSectionLabel("COPY GRAPH  [Source Graph ID]", 520, top); top += 22;
            txtCopyID = AddTextBox(520, top, 60, "Graph ID");
            var btnCopy = new Button { Text = "Copy Graph", Location = new Point(585, top), Width = 100 };
            btnCopy.Click += BtnCopy_Click;
            Controls.Add(btnCopy);
            top += 35;

            // display
            AddSectionLabel("DISPLAY GRAPH  [Graph ID — or double-click list]", 520, top); top += 22;
            var txtDisplayID = new TextBox { Location = new Point(520, top), Width = 60, PlaceholderText = "GID" };
            Controls.Add(txtDisplayID);
            var btnDisplay = new Button { Text = "Display", Location = new Point(585, top), Width = 80 };
            btnDisplay.Click += (s, e) =>
            {
                if (int.TryParse(txtDisplayID.Text, out int id)) DisplayGraph(id);
            };
            Controls.Add(btnDisplay);

            RefreshList();
        }

        // helpers

        private void AddSectionLabel(string text, int x, int y)
        {
            var lbl = new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Arial", 8, FontStyle.Bold),
                ForeColor = Color.DarkSlateBlue
            };
            Controls.Add(lbl);
        }

        private TextBox AddTextBox(int x, int y, int width, string placeholder)
        {
            var tb = new TextBox { Location = new Point(x, y), Width = width, PlaceholderText = placeholder };
            Controls.Add(tb);
            return tb;
        }

        private void RefreshList()
        {
            graphListBox.Items.Clear();
            foreach (var g in manager.GetAllGraphs())
                graphListBox.Items.Add(g.ToString());
        }

        private void SetStatus(string msg) => statusLabel.Text = $"[{userName}] {msg}";

        // event handlers

        private void BtnCreate_Click(object? sender, EventArgs e)
        {
            var g = manager.CreateGraph();
            SetStatus($"Created {g}");
            RefreshList();
        }

        private void BtnAddVertex_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtGraphID.Text, out int gid) ||
                !int.TryParse(txtVertexID.Text, out int vid) ||
                !int.TryParse(txtVX.Text, out int x) ||
                !int.TryParse(txtVY.Text, out int y))
            {
                SetStatus("Invalid input for Add Vertex."); return;
            }
            var v = new Vertex(vid, x, y);
            bool ok = manager.AddVertexToGraph(gid, v);
            SetStatus(ok ? $"Added {v} to Graph {gid}" : $"Graph {gid} not found.");
            RefreshList();
        }

        private void BtnAddEdge_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtGraphID.Text, out int gid) ||
                !int.TryParse(txtEdgeID.Text, out int eid) ||
                !int.TryParse(txtFromV.Text, out int fid) ||
                !int.TryParse(txtToV.Text, out int tid))
            {
                SetStatus("Invalid input for Add Edge."); return;
            }
            var g = manager.GetGraphByID(gid);
            if (g == null) { SetStatus($"Graph {gid} not found."); return; }

            var from = g.FindVertex(fid);
            var to = g.FindVertex(tid);
            if (from == null || to == null) { SetStatus("Vertex not found. Add vertices first."); return; }

            bool ok = manager.AddEdgeToGraph(gid, new Edge(eid, from, to));
            SetStatus(ok ? $"Added Edge {eid} ({fid}->{tid}) to Graph {gid}" : "Failed.");
            RefreshList();
        }

        private void BtnUpdateVertex_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtUpdateType.Text, out int gid) ||
                !int.TryParse(txtUpdateID.Text, out int vid) ||
                !int.TryParse(txtUpdateX.Text, out int x) ||
                !int.TryParse(txtUpdateY.Text, out int y))
            {
                SetStatus("Invalid input for Update Vertex."); return;
            }
            bool ok = manager.UpdateVertex(gid, vid, x, y);
            SetStatus(ok ? $"Updated Vertex {vid} in Graph {gid} to ({x},{y})" : $"Graph {gid} not found.");
            RefreshList();
        }

        private void BtnCopy_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtCopyID.Text, out int id))
            {
                SetStatus("Invalid graph ID for copy."); return;
            }
            var copy = manager.CopyGraph(id);
            SetStatus(copy != null ? $"Copied Graph {id} → new {copy}" : $"Graph {id} not found.");
            RefreshList();
        }

        private void DisplaySelectedGraph()
        {
            int idx = graphListBox.SelectedIndex;
            if (idx < 0) return;
            var g = manager.GetAllGraphs()[idx];
            g.Display(graphPanel);
            SetStatus($"Displaying {g}");
        }

        private void DisplayGraph(int id)
        {
            var g = manager.GetGraphByID(id);
            if (g == null) { SetStatus($"Graph {id} not found."); return; }
            g.Display(graphPanel);
            SetStatus($"Displaying {g}");
        }
    }
}
