﻿namespace SonyBankUsageRecordParse.src
{
	partial class StoreNameTagLists
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
			treeViewStoreConfig = new TreeView();
			ExpandStoreNameTagListsbutton = new Button();
			SuspendLayout();
			// 
			// treeViewStoreConfig
			// 
			treeViewStoreConfig.Location = new Point(12, 41);
			treeViewStoreConfig.Name = "treeViewStoreConfig";
			treeViewStoreConfig.Size = new Size(797, 539);
			treeViewStoreConfig.TabIndex = 0;
			// 
			// ExpandStoreNameTagListsbutton
			// 
			ExpandStoreNameTagListsbutton.Location = new Point(12, 12);
			ExpandStoreNameTagListsbutton.Name = "ExpandStoreNameTagListsbutton";
			ExpandStoreNameTagListsbutton.Size = new Size(75, 23);
			ExpandStoreNameTagListsbutton.TabIndex = 1;
			ExpandStoreNameTagListsbutton.Text = "すべて展開";
			ExpandStoreNameTagListsbutton.UseVisualStyleBackColor = true;
			ExpandStoreNameTagListsbutton.Click += ExpandStoreNameTagListsbutton_Click;
			// 
			// StoreNameTagLists
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(821, 592);
			Controls.Add(ExpandStoreNameTagListsbutton);
			Controls.Add(treeViewStoreConfig);
			Name = "StoreNameTagLists";
			Text = "StoreNameTagLists";
			ResumeLayout(false);
		}

		#endregion

		private TreeView treeViewStoreConfig;
		private Button ExpandStoreNameTagListsbutton;
	}
}