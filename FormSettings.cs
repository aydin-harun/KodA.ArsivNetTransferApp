using KodA.Business;
using KodA.Forms;
using KodA.Types.CommonObjects;
using KodA.Types.GD5AktarimObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KodA.ArsivNetTransferApp
{
    public partial class FormSettings : Form
    {
        List<AArsivTipi> arsivTipis = null;
        List<FieldMatch> fieldMatches = new List<FieldMatch>();
        AArsivTipi currentKlasor = null;
        public FormSettings()
        {
            InitializeComponent();
            if (!DesignMode)
                LoadStaticData();
        }

        private void LoadStaticData()
        {
            var deps = AdminBS.GetDepartmanByType(EnumDepertmanType.DokumanTipi);
            FormsUtility.LoadComboFromList(comboBoxEditSBDep, deps.ToList<object>(), "DepartmanName", "tDepartmanId", true);
            arsivTipis = InstitueBS.GetArsivNetArsivTipi();
            var disArsivTipiList = new List<AArsivTipi>();
            foreach (var item in arsivTipis)
            {
                if (disArsivTipiList.Where(e => e.ARSIV_TIP_ID == item.ARSIV_TIP_ID).FirstOrDefault() == null)
                    disArsivTipiList.Add(item);
            }
            FormsUtility.LoadComboFromList(comboBoxEditGDDep, disArsivTipiList.ToList<object>(), "ARSIV_TIPI_ADI", "ARSIV_TIP_ID", true);
        }

        private void LoadIndexFields()
        {
            if (Convert.ToInt32(((ComboItem)comboBoxEditGDDep.SelectedItem).Value) == -1 || Convert.ToInt32(((ComboItem)comboBoxEditSBDep.SelectedItem).Value) == -1)
            {
                fieldMatches.Clear();
                gridControlFields.RefreshDataSource();
                simpleButtonSaveFieldMap.Enabled = false;
                return;
            }

            var sbIndexFileds = AdminBS.GetDocIndexFieldByDepartmanId(Convert.ToInt32(((ComboItem)comboBoxEditSBDep.SelectedItem).Value), true, EnumFieldType.UstAlan);
            sbIndexFileds.Add(new DocTypeIndexField() { tDocTypeIndexMapId = 0, FieldName = "BarcodeValue" });
            var aKlasorler = (from a in (arsivTipis.Where(e => e.ARSIV_TIP_ID == Convert.ToInt32(((ComboItem)comboBoxEditGDDep.SelectedItem).Value))) select a.Clone()).ToList();
            currentKlasor = (AArsivTipi)aKlasorler.FirstOrDefault();

            var list = (from sbf in sbIndexFileds select new FieldMatch() { SBFieldName = sbf.FieldName, tDocTypeIndexMapId = sbf.tDocTypeIndexMapId, FieldType = sbf.FieldType }).ToList();

            //FormsUtility.LoadRepositoryItemLookUpEditFromList(sbIndexFileds.ToList<object>(), repositoryItemLookUpEditSBField, "tDocTypeIndexMapId", "FieldName");
            FormsUtility.LoadRepositoryItemLookUpEditFromList(aKlasorler.ToList<object>(), repositoryItemLookUpEditGDFileds, "USTVER_ALAN_ADI", "USTVER_ALAN_ADI");
            //FormsUtility.LoadRepositoryItemLookUpEditFromList(gdFields.ToList<object>(), repositoryItemLookUpEditGDFileds, "Value", "Name");
            fieldMatches = list;
            gridControlFields.DataSource = fieldMatches;
            gridControlFields.RefreshDataSource();
            simpleButtonSaveFieldMap.Enabled = true;
        }

        private void simpleButtonSaveFieldMap_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(((ComboItem)comboBoxEditGDDep.SelectedItem).Value) == -1 || Convert.ToInt32(((ComboItem)comboBoxEditSBDep.SelectedItem).Value) == -1 || gridViewFields.RowCount == 0)
            {
                FormDialog fdResult = new FormDialog("Uyarı", "Tüm aktarım ayarları yaplımalıdır...", false, false, FormDialog.MessageType.Warning);
                fdResult.ShowDialog();
                return;
            }
            GDTransferSettingDep transferSettingDep = AdminBS.GetDepartmanGDTransferSetting(Convert.ToInt32(((ComboItem)comboBoxEditSBDep.SelectedItem).Value));
            if (transferSettingDep == null)
                transferSettingDep = new GDTransferSettingDep();

            var selectedKlasor =(from a in (arsivTipis.Where(ee => ee.ARSIV_TIP_ID == Convert.ToInt32(((ComboItem)comboBoxEditGDDep.SelectedItem).Value))) select (AArsivTipi)a.Clone()).FirstOrDefault();
            if (selectedKlasor == null)
            {
                FormDialog fdResult = new FormDialog("Uyarı", "ArsivNet Kalsor Bulunamadı...", false, false, FormDialog.MessageType.Warning);
                fdResult.ShowDialog();
                return;
            }

            transferSettingDep.tDepartmanId = Convert.ToInt32(((ComboItem)comboBoxEditSBDep.SelectedItem).Value);
            transferSettingDep.BatchGDSeriesId = selectedKlasor.ARSIV_TIP_ID;
            transferSettingDep.IsUseBatchBarcode = false;
            transferSettingDep.IsUseLocation = false;
            transferSettingDep.PageGroupGDSeriesId = selectedKlasor.USTVERI_ID;
            transferSettingDep.FolderKeys = textEditArsivNetIdPath.Text;
            transferSettingDep.FolderDetailTypeId =Convert.ToInt32(textEditFolderTypeDetailId.Text);
            transferSettingDep.CreatedById = Convert.ToInt32(textEditCreatedById.Text);

            List<GDTransferSettingFieldMatch> transferSettingFieldMatches = new List<GDTransferSettingFieldMatch>();
            for (int i = 0; i < gridViewFields.RowCount; i++)
            {
                FieldMatch fieldMatch = (FieldMatch)gridViewFields.GetRow(i);
                int metaId = 0;
                string USTVER_ALAN_ADI = fieldMatch.USTVER_ALAN_ADI;
                if (USTVER_ALAN_ADI == null)
                    USTVER_ALAN_ADI = fieldMatch.USTVER_ALAN_ADI = "";
                if (fieldMatch.USTVER_ALAN_ADI.Contains("RefId"))
                {
                    string[] metaDataParts = fieldMatch.USTVER_ALAN_ADI.Split('=');
                    MatchCollection matches = Regex.Matches(metaDataParts[1], @"\d+");
                    if (matches.Count > 0)
                        metaId = Convert.ToInt32(matches[0].Value);
                    USTVER_ALAN_ADI = fieldMatch.USTVER_ALAN_ADI.Split('(')[0].Trim();

                }
                transferSettingFieldMatches.Add(new GDTransferSettingFieldMatch()
                {
                    FieldType = fieldMatch.FieldType,
                    GDFieldName = USTVER_ALAN_ADI,
                    SBFieldName = fieldMatch.SBFieldName,
                    GDMetadataId = metaId,
                    tDocTypeIndexMapId = fieldMatch.tDocTypeIndexMapId
                });
            }

            AdminBS.SaveGDTransferSettingDep(transferSettingDep, transferSettingFieldMatches);
        }

        private void comboBoxEditGDDep_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadIndexFields();
        }
    }
}
