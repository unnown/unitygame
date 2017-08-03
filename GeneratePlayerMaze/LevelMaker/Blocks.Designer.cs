namespace LevelMaker
{
    partial class Blocks
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            this.spike = new System.Windows.Forms.Button();
            this.block = new System.Windows.Forms.Button();
            this.Start = new System.Windows.Forms.Button();
            this.Finish = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // spike
            // 
            this.spike.Image = global::LevelMaker.Properties.Resources.spikesBottom;
            this.spike.Location = new System.Drawing.Point(12, 12);
            this.spike.Name = "spike";
            this.spike.Size = new System.Drawing.Size(72, 70);
            this.spike.TabIndex = 0;
            this.spike.Tag = "s";
            this.spike.Text = "Spike";
            this.spike.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.spike.UseVisualStyleBackColor = true;
            this.spike.Click += new System.EventHandler(this.block_Click);
            // 
            // block
            // 
            this.block.Image = global::LevelMaker.Properties.Resources.brickWall;
            this.block.Location = new System.Drawing.Point(90, 12);
            this.block.Name = "block";
            this.block.Size = new System.Drawing.Size(72, 70);
            this.block.TabIndex = 1;
            this.block.Tag = "1";
            this.block.Text = "Block";
            this.block.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.block.UseVisualStyleBackColor = true;
            this.block.Click += new System.EventHandler(this.block_Click);
            // 
            // Start
            // 
            this.Start.Image = global::LevelMaker.Properties.Resources.spikesBottom;
            this.Start.Location = new System.Drawing.Point(12, 88);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(72, 70);
            this.Start.TabIndex = 2;
            this.Start.Tag = "st";
            this.Start.Text = "Start";
            this.Start.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Start.UseVisualStyleBackColor = true;
            this.Start.Click += new System.EventHandler(this.block_Click);
            // 
            // Finish
            // 
            this.Finish.Image = global::LevelMaker.Properties.Resources.spikesBottom;
            this.Finish.Location = new System.Drawing.Point(94, 88);
            this.Finish.Name = "Finish";
            this.Finish.Size = new System.Drawing.Size(72, 70);
            this.Finish.TabIndex = 3;
            this.Finish.Tag = "fn";
            this.Finish.Text = "Finish";
            this.Finish.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Finish.UseVisualStyleBackColor = true;
            this.Finish.Click += new System.EventHandler(this.block_Click);
            // 
            // Blocks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(178, 185);
            this.Controls.Add(this.Finish);
            this.Controls.Add(this.Start);
            this.Controls.Add(this.block);
            this.Controls.Add(this.spike);
            this.Name = "Blocks";
            this.Text = "Blocks";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button spike;
        private System.Windows.Forms.Button block;
        private System.Windows.Forms.Button Start;
        private System.Windows.Forms.Button Finish;
    }
}