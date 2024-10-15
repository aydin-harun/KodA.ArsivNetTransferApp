
namespace KodA.ArsivNetTransferApp
{
    partial class FormSettings
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
            this.gridControlFields = new DevExpress.XtraGrid.GridControl();
            this.gridViewFields = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumnId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnSBField = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnGDField = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemLookUpEditGDFileds = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.panel2 = new System.Windows.Forms.Panel();
            this.textEditArsivNetIdPath = new DevExpress.XtraEditors.TextEdit();
            this.comboBoxEditGDDep = new DevExpress.XtraEditors.ComboBoxEdit();
            this.comboBoxEditSBDep = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.simpleButtonSaveFieldMap = new DevExpress.XtraEditors.SimpleButton();
            this.label4 = new System.Windows.Forms.Label();
            this.textEditFolderTypeDetailId = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlFields)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewFields)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEditGDFileds)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEditArsivNetIdPath.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEditGDDep.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEditSBDep.Properties)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEditFolderTypeDetailId.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControlFields
            // 
            this.gridControlFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlFields.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gridControlFields.Location = new System.Drawing.Point(0, 125);
            this.gridControlFields.MainView = this.gridViewFields;
            this.gridControlFields.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gridControlFields.Name = "gridControlFields";
            this.gridControlFields.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpEditGDFileds});
            this.gridControlFields.Size = new System.Drawing.Size(800, 275);
            this.gridControlFields.TabIndex = 7;
            this.gridControlFields.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewFields});
            // 
            // gridViewFields
            // 
            this.gridViewFields.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumnId,
            this.gridColumnSBField,
            this.gridColumnGDField});
            this.gridViewFields.GridControl = this.gridControlFields;
            this.gridViewFields.Name = "gridViewFields";
            // 
            // gridColumnId
            // 
            this.gridColumnId.Caption = "tDocTypeIndexMapId";
            this.gridColumnId.FieldName = "tDocTypeIndexMapId";
            this.gridColumnId.MinWidth = 25;
            this.gridColumnId.Name = "gridColumnId";
            this.gridColumnId.OptionsColumn.AllowEdit = false;
            this.gridColumnId.OptionsColumn.ReadOnly = true;
            this.gridColumnId.Visible = true;
            this.gridColumnId.VisibleIndex = 0;
            this.gridColumnId.Width = 93;
            // 
            // gridColumnSBField
            // 
            this.gridColumnSBField.Caption = "SB Indeks Alanı";
            this.gridColumnSBField.FieldName = "SBFieldName";
            this.gridColumnSBField.MinWidth = 25;
            this.gridColumnSBField.Name = "gridColumnSBField";
            this.gridColumnSBField.OptionsColumn.AllowEdit = false;
            this.gridColumnSBField.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumnSBField.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumnSBField.OptionsColumn.ReadOnly = true;
            this.gridColumnSBField.Visible = true;
            this.gridColumnSBField.VisibleIndex = 1;
            this.gridColumnSBField.Width = 93;
            // 
            // gridColumnGDField
            // 
            this.gridColumnGDField.Caption = "ArsivNet Indeks Alanı";
            this.gridColumnGDField.ColumnEdit = this.repositoryItemLookUpEditGDFileds;
            this.gridColumnGDField.FieldName = "USTVER_ALAN_ADI";
            this.gridColumnGDField.MinWidth = 25;
            this.gridColumnGDField.Name = "gridColumnGDField";
            this.gridColumnGDField.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumnGDField.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumnGDField.Visible = true;
            this.gridColumnGDField.VisibleIndex = 2;
            this.gridColumnGDField.Width = 93;
            // 
            // repositoryItemLookUpEditGDFileds
            // 
            this.repositoryItemLookUpEditGDFileds.AutoHeight = false;
            this.repositoryItemLookUpEditGDFileds.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEditGDFileds.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("USTVER_ALAN_ADI", "USTVER_ALAN_ADI"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("USTVER_ALAN_ADI", "ArsivNet Indeks Alanı")});
            this.repositoryItemLookUpEditGDFileds.Name = "repositoryItemLookUpEditGDFileds";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.textEditFolderTypeDetailId);
            this.panel2.Controls.Add(this.textEditArsivNetIdPath);
            this.panel2.Controls.Add(this.comboBoxEditGDDep);
            this.panel2.Controls.Add(this.comboBoxEditSBDep);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(800, 125);
            this.panel2.TabIndex = 5;
            // 
            // textEditArsivNetIdPath
            // 
            this.textEditArsivNetIdPath.Location = new System.Drawing.Point(318, 64);
            this.textEditArsivNetIdPath.Name = "textEditArsivNetIdPath";
            this.textEditArsivNetIdPath.Size = new System.Drawing.Size(368, 22);
            this.textEditArsivNetIdPath.TabIndex = 6;
            // 
            // comboBoxEditGDDep
            // 
            this.comboBoxEditGDDep.Location = new System.Drawing.Point(129, 34);
            this.comboBoxEditGDDep.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxEditGDDep.Name = "comboBoxEditGDDep";
            this.comboBoxEditGDDep.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEditGDDep.Properties.Sorted = true;
            this.comboBoxEditGDDep.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.comboBoxEditGDDep.Size = new System.Drawing.Size(368, 22);
            this.comboBoxEditGDDep.TabIndex = 5;
            this.comboBoxEditGDDep.SelectedIndexChanged += new System.EventHandler(this.comboBoxEditGDDep_SelectedIndexChanged);
            // 
            // comboBoxEditSBDep
            // 
            this.comboBoxEditSBDep.Location = new System.Drawing.Point(129, 6);
            this.comboBoxEditSBDep.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxEditSBDep.Name = "comboBoxEditSBDep";
            this.comboBoxEditSBDep.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEditSBDep.Properties.Sorted = true;
            this.comboBoxEditSBDep.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.comboBoxEditSBDep.Size = new System.Drawing.Size(368, 22);
            this.comboBoxEditSBDep.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(307, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "ArşivNet Klasör idleri Tam Yol(123|1234|12345)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Diğer Birim";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Departman";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.simpleButtonSaveFieldMap);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 400);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(800, 50);
            this.panel3.TabIndex = 6;
            // 
            // simpleButtonSaveFieldMap
            // 
            this.simpleButtonSaveFieldMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButtonSaveFieldMap.Location = new System.Drawing.Point(656, 7);
            this.simpleButtonSaveFieldMap.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.simpleButtonSaveFieldMap.Name = "simpleButtonSaveFieldMap";
            this.simpleButtonSaveFieldMap.Size = new System.Drawing.Size(141, 37);
            this.simpleButtonSaveFieldMap.TabIndex = 0;
            this.simpleButtonSaveFieldMap.Text = "Kaydet";
            this.simpleButtonSaveFieldMap.Click += new System.EventHandler(this.simpleButtonSaveFieldMap_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(253, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "ArşivNet Arşiv Malzzemesi Detay Tip Id";
            // 
            // textEditFolderTypeDetailId
            // 
            this.textEditFolderTypeDetailId.Location = new System.Drawing.Point(318, 93);
            this.textEditFolderTypeDetailId.Name = "textEditFolderTypeDetailId";
            this.textEditFolderTypeDetailId.Size = new System.Drawing.Size(368, 22);
            this.textEditFolderTypeDetailId.TabIndex = 6;
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.gridControlFields);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Name = "FormSettings";
            this.Text = "Eşleştirme Ayarları";
            ((System.ComponentModel.ISupportInitialize)(this.gridControlFields)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewFields)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEditGDFileds)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEditArsivNetIdPath.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEditGDDep.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEditSBDep.Properties)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.textEditFolderTypeDetailId.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControlFields;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewFields;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnId;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnSBField;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnGDField;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEditGDFileds;
        private System.Windows.Forms.Panel panel2;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEditGDDep;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEditSBDep;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private DevExpress.XtraEditors.SimpleButton simpleButtonSaveFieldMap;
        private DevExpress.XtraEditors.TextEdit textEditArsivNetIdPath;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.TextEdit textEditFolderTypeDetailId;
        private System.Windows.Forms.Label label4;
    }
}