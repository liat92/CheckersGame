using System.Windows.Forms;

namespace Checkers.UI
{
    public partial class FormCheckersGame
    {
        private enum eSizeAndLocations
        {
            ButtonSize = 50,
            ButtonXStartPosition = 13,
            ButtonYStartPosition = 50,
        }

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        /// 
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanelPlayerInfo = new System.Windows.Forms.TableLayoutPanel();
            this.ucPlayer1Information = new Checkers.UI.UCPlayerInformation();
            this.ucPlayer2Information = new Checkers.UI.UCPlayerInformation();
            this.tableLayoutPanelPlayerInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelPlayerInfo
            // 
            this.tableLayoutPanelPlayerInfo.ColumnCount = 2;
            this.tableLayoutPanelPlayerInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelPlayerInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelPlayerInfo.Controls.Add(this.ucPlayer1Information, 0, 0);
            this.tableLayoutPanelPlayerInfo.Controls.Add(this.ucPlayer2Information, 1, 0);
            this.tableLayoutPanelPlayerInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelPlayerInfo.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelPlayerInfo.Name = "tableLayoutPanelPlayerInfo";
            this.tableLayoutPanelPlayerInfo.RowCount = 1;
            this.tableLayoutPanelPlayerInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelPlayerInfo.Size = new System.Drawing.Size(466, 32);
            this.tableLayoutPanelPlayerInfo.TabIndex = 0;
            // 
            // ucPlayer1Information
            // 
            this.ucPlayer1Information.BackColor = System.Drawing.SystemColors.Control;
            this.ucPlayer1Information.Location = new System.Drawing.Point(25, 3);
            this.ucPlayer1Information.Name = "ucPlayer1Information";
            this.ucPlayer1Information.PlayerName = "::";
            this.ucPlayer1Information.PlayerScore = "0";
            this.ucPlayer1Information.Size = new System.Drawing.Size(194, 50);
            this.ucPlayer1Information.TabIndex = 0;
            // 
            // ucPlayer2Information
            // 
            this.ucPlayer2Information.BackColor = System.Drawing.SystemColors.Control;
            this.ucPlayer2Information.Location = new System.Drawing.Point(236, 3);
            this.ucPlayer2Information.Name = "ucPlayer2Information";
            this.ucPlayer2Information.PlayerName = "::";
            this.ucPlayer2Information.PlayerScore = "0";
            this.ucPlayer2Information.Size = new System.Drawing.Size(194, 50);
            this.ucPlayer2Information.TabIndex = 1;
            // 
            // FormCheckers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(466, 400);
            this.Controls.Add(this.tableLayoutPanelPlayerInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "FormCheckers";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Checkers";
            this.tableLayoutPanelPlayerInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private readonly byte r_BoardSize; 
        private readonly byte r_NumberOfPiecesInOneSide;
        private readonly ButtonSquare[,] buttonSquares;
        private readonly PictureBoxCheckersPiece[] pictureBoxCheckersPieceWhitePieces; 
        private readonly PictureBoxCheckersPiece[] pictureBoxCheckersPieceBlackPieces; 
        private TableLayoutPanel tableLayoutPanelPlayerInfo;
        private UCPlayerInformation ucPlayer1Information;
        private UCPlayerInformation ucPlayer2Information;
    }
}