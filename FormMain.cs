using KodA.Business;
using KodA.FileProvider;
using KodA.Forms;
using KodA.Types.CommonObjects;
using KodA.Types.GD5AktarimObjects;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KodA.ArsivNetTransferApp
{
    public partial class FormMain : Form
    {
        List<Departman> Departmans = null;
        List<Departman> AllDepartmas = null;
        DataTable dtAnalize = null;
        Departman currentDepartman = null;
        DataTable dtExportPg = null;
        GDTransferSettingDep transferSettingDep = null;
        List<GDTransferSettingFieldMatch> fieldMatches = null;
        List<DocTypeIndexField> docTypeIndexMaps = null;
        //List<KodA.ArsivNetTransferApp.ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc> arsivDizinleri = new List<ArsivPlanlamaSvc.arsivDiziniCocuklariniListeleSonuc>();

        #region LooukupEsleme
        List<GDTransferSettingLookup> gDTransferSettingLookups = null;

        #endregion
        public FormMain()
        {
            InitializeComponent();
            if (!DesignMode)
                LoadStaticData();
        }

        void LoadStaticData()
        {
            try
            {
                AllDepartmas = AdminBS.GetDepartmanList(1);
                Departmans = AllDepartmas.Where(d => d.NodeType == 2).OrderBy(d => d.DepartmanName).ToList();
                listBoxDeps.Items.Clear();
                foreach (Departman departman in Departmans)
                {
                    listBoxDeps.Items.Add(departman);
                }
                gDTransferSettingLookups = AdminBS.GetGDTransferSettingLookups();

            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        void ShowError(Exception ex)
        {
            Clipboard.SetDataObject(ex, true);
            MessageBox.Show(ex.ToString(), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void buttonPrepare_Click(object sender, EventArgs e)
        {
            PrepareExport();
        }

        void PrepareExport()
        {
            try
            {
                if (listBoxDeps.SelectedItems.Count != 1)
                {
                    MessageBox.Show("Departman Seçmelisiniz...", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                currentDepartman = (Departman)listBoxDeps.SelectedItem;
                dtAnalize = CommonBS.GetDepartmanExportAnalize(currentDepartman.tDepartmanId, true);
                textBoxStatus.Clear();
                textBoxStatus.AppendText("DepartmanId : " + currentDepartman.tDepartmanId);
                foreach (DataRow dataRow in dtAnalize.Rows)
                {
                    textBoxStatus.AppendText(Environment.NewLine);
                    textBoxStatus.AppendText(dataRow["Aciklama"].ToString());
                    textBoxStatus.AppendText(Environment.NewLine);
                }
                if (dtAnalize.Select("Tip=4").Length > 0)
                {
                    toolStripButtonStartExport.Enabled = (int)dtAnalize.Select("Tip=4")[0]["Sayi"] > 0;
                    toolStripButtonResetError.Enabled = (int)dtAnalize.Select("Tip=5")[0]["Sayi"] > 0;
                    if ((int)dtAnalize.Select("Tip=4")[0]["Sayi"] == 0)
                    {
                        MessageBox.Show("Export Edilecek PageGroup Bulunamadı...", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void toolStripButtonResetError_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxDeps.SelectedItems.Count != 1)
                {
                    MessageBox.Show("Departman Seçmelisiniz...", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                currentDepartman = (Departman)listBoxDeps.SelectedItem;
                CommonBS.DeletePageGroupExportLogsByDepIdAndStatus(currentDepartman.tDepartmanId, 2);
                PrepareExport();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void toolStripButtonStartExport_Click(object sender, EventArgs e)
        {
            try
            {
                docTypeIndexMaps = AdminBS.GetDocIndexFieldByDepartmanId(currentDepartman.tDepartmanId, true, EnumFieldType.UstAlan);
                ServiceHelper.LoginService();
                listViewLog.Items.Clear();
                List<Batch> lBatches = new List<Batch>();
                List<Storage> lStorages = new List<Storage>();
                //DataTable dtDepArsivNetEslestirme = null;// InstitueBS.GetDepartmanArsivEsletirme(currentDepartman.tDepartmanId);
                //if (dtDepArsivNetEslestirme.Rows.Count == 0)
                //{
                //    MessageBox.Show("Export Eşleştirme Bilgisi Bulunamadı...", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}

                transferSettingDep = AdminBS.GetDepartmanGDTransferSetting(currentDepartman.tDepartmanId);
                if (transferSettingDep == null)
                {
                    FormDialog fdResult = new FormDialog("Uyarı", "Departman Aktarım Ayarlarını Yapmalısınız...", false, false, FormDialog.MessageType.Warning);
                    fdResult.ShowDialog();
                    return;
                }

                fieldMatches = AdminBS.GetGDTransferSettingFieldMatches(transferSettingDep.tGDTransferSettingDepId);
                if (fieldMatches == null || fieldMatches.Count == 0)
                {
                    FormDialog fdResult = new FormDialog("Uyarı", "Departman İçin Alan Eşleştirmesi Yapmalısınız...", false, false, FormDialog.MessageType.Warning);
                    fdResult.ShowDialog();
                    return;
                }
                var docTypeIndexFields = AdminBS.GetDocIndexFieldByDepartmanId(transferSettingDep.tDepartmanId, true, EnumFieldType.UstAlan);


                int arsivNetDetayTipId = transferSettingDep.FolderDetailTypeId;
                int arsivNetTipId = transferSettingDep.BatchGDSeriesId;
                int arsivNetUstVeriTanimId = transferSettingDep.PageGroupGDSeriesId;
                int arsivNetUreticiId = transferSettingDep.CreatedById;

                ArsivPlanlamaSvc.arsivDizini arsivDizini = null;

                arsivDizini = ServiceHelper.GetArchiveFolder(transferSettingDep.FolderKeys);
                if (arsivDizini == null)
                {
                    MessageBox.Show("Export Edilecek Departman Karşılığı Bulunamadı...", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                dtExportPg = null;// CommonBS.GetExportPG(currentDepartman.tDepartmanId);
                if (dtExportPg.Rows.Count == 0)
                {
                    MessageBox.Show("Export Edilecek Kayıt Bulunamadı...", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                toolStripButtonStartExport.Enabled = false;
                toolStripButtonResetError.Enabled = false;
                buttonPrepare.Enabled = false;
                progressBarStatus.Minimum = 0;
                progressBarStatus.Maximum = dtExportPg.Rows.Count;
                progressBarStatus.Value = 0;
                foreach (DataRow row in dtExportPg.Rows)
                {
                    try
                    {
                        Batch batch = null;
                        Storage storage = null;
                        if (lBatches.All(s => s.tBatchId != (int)row["tBatchId"]))
                        {
                            lBatches.Add(CommonBS.GetBatchById(true, (int)row["tBatchId"]));
                        }

                        batch = lBatches.SingleOrDefault(s => s.tBatchId == (int)row["tBatchId"]);
                        if (lStorages.All(s => s.tStorageId != batch.tStorageId))
                        {
                            lStorages.Add(CommonBS.GetStorage(true, batch.tStorageId)[0]);
                        }
                        storage = lStorages.SingleOrDefault(s => s.tStorageId == batch.tStorageId);

                        // batch ıçın klasor olustur

                        // indeks datası alma
                        DataTable dataTableIndex = CommonBS.GetDepartmanDataWithLookUp(currentDepartman.tDepartmanId, EnumFieldType.UstAlan, (int)row["tBatchId"],
                            (int)row["tPageGroupId"], currentDepartman, docTypeIndexFields);

                        //int birimKodu = 0;
                        //isNumeric(AllDepartmas.Where(d => d.tDepartmanId == currentDepartman.UpDepartmanId).First().Description, out birimKodu);
                        ArsivMalzemeSvc.arsivMalzemesiEkleMTOMParametre arsivMalzemesi = CreateArsivMalzemesi((int)row["tPageGroupId"], currentDepartman,
                            dataTableIndex.Rows[0], Path.Combine(storage.PdfPath, batch.RelativePath, batch.BatchName, row["tPageGroupId"].ToString() + ".pdf"), 
                            arsivNetDetayTipId, arsivNetTipId,
                            arsivDizini.id, arsivNetUstVeriTanimId, arsivNetUreticiId);
                        var malzemeResult = ServiceHelper.CreateArsivMalzeme(arsivMalzemesi);
                        arsivMalzemesi.arsivMalzemesiMTOMDetay[0].binaryDosya = null;
                        arsivMalzemesi = null;
                        GC.Collect();
                        if (malzemeResult == null)
                        {
                            // pg export logu oluşturma
                            CommonBS.InsertPageGroupExportLog((int)row["tPageGroupId"], DateTime.Now, 2, 0, "", 0, 0);
                            AddLogItem((int)row["tPageGroupId"], 2, DateTime.Now, 0);
                        }
                        else
                        {
                            KodA.ArsivNetTransferApp.ArsivMalzemeSvc.arsivMalzemesiEkleSonuc mr = (KodA.ArsivNetTransferApp.ArsivMalzemeSvc.arsivMalzemesiEkleSonuc)((Object[])malzemeResult)[0];
                            // pg export logu oluşturma
                            CommonBS.InsertPageGroupExportLog((int)row["tPageGroupId"], DateTime.Now, 1, mr.id, mr.erisimNo, mr.ustveriId, mr.arsivMalzemesiDetayIds[0].value);
                            AddLogItem((int)row["tPageGroupId"], 1, DateTime.Now, mr.id);
                            CreateArsivMalzemeOcr(mr.arsivMalzemesiDetayIds[0].value, mr.id, (int)row["tPageGroupId"]);
                        }
                        //}
                    }
                    catch (Exception exExport)
                    {
                        if (exExport.ToString().Contains("Could not find file"))
                        {
                            PageGroup pageGroup = CommonBS.GetPageGroupByPageGroupId(true, (int)row["tPageGroupId"])[0];
                            pageGroup.PdfCreatedDate = DateTime.MinValue;
                            pageGroup.PdfCreatState = 0;
                            CommonBS.SavePageGroup(pageGroup, false, false, false);
                        }
                        CommonBS.InsertPageGroupExportLog((int)row["tPageGroupId"], DateTime.Now, 2, 0, "", 0, 0);
                    }
                    progressBarStatus.Value = progressBarStatus.Value + 1;
                    Application.DoEvents();
                }
                toolStripButtonStartExport.Enabled = true;
                toolStripButtonResetError.Enabled = true;
                buttonPrepare.Enabled = true;
            }
            catch (Exception ex)
            {
                ShowError(ex);
                toolStripButtonStartExport.Enabled = true;
                toolStripButtonResetError.Enabled = true;
                buttonPrepare.Enabled = true;
            }
        }

        bool CreateArsivMalzemeOcr(int arsivMalzemesiDetayId, int arsivMalzesiId, int tPageGroupId)
        {
            try
            {
                PageGroupOcr pageGroupOcr = CommonBS.GetPageGroupOcr(tPageGroupId);
                if (pageGroupOcr != null)
                {

                    ArsivMalzemeSvc.arsivMalzemesiDetayOCRIcerikYazMTOMParametre oCRIcerikYazMTOMParametre = new ArsivMalzemeSvc.arsivMalzemesiDetayOCRIcerikYazMTOMParametre()
                    {
                        arsivMalzemesiDetayId = arsivMalzemesiDetayId,
                        arsivMalzemesiDetayIdSpecified = true,
                        arsivMalzemesiId = arsivMalzesiId,
                        arsivMalzemesiIdSpecified = true,
                        ocrBinaryDosya = System.Text.ASCIIEncoding.UTF8.GetBytes(pageGroupOcr.OcrResult)
                    };
                    ServiceHelper.CreateArsivMalzemeOcr(oCRIcerikYazMTOMParametre);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void AddLogItem(int tPageGroupId, byte status, DateTime exportDate, int outherId)
        {
            ListViewItem item = new ListViewItem(tPageGroupId.ToString());
            item.SubItems.Add(status == 1 ? "Başarılı" : "Başarısız");
            item.SubItems.Add(exportDate.ToString("dd.MM.yyyy HH:mm:ss"));
            item.SubItems.Add(outherId.ToString());
            item.BackColor = status == 1 ? Color.GreenYellow : Color.IndianRed;
            listViewLog.Items.Add(item);
            listViewLog.EnsureVisible(item.Index);
        }


        ArsivMalzemeSvc.arsivMalzemesiEkleMTOMParametre CreateArsivMalzemesi(int tPageGroupId, Departman departman, DataRow row, string pdfPath,
            int arsivNetDetayTipId, int arsivNetTipId, int arsivNetArsivDizinId, int arsivNetUstVeriTanimId, int arsivNetUreticiId)
        {
            ArsivMalzemeSvc.arsivMalzemesiEkleMTOMParametre parametre = new ArsivMalzemeSvc.arsivMalzemesiEkleMTOMParametre();
            parametre.tipId = arsivNetTipId;
            parametre.tipIdSpecified = true;
            parametre.kapsamBitisZamaniSpecified = false;
            parametre.remoteId = tPageGroupId.ToString();
            parametre.kod = tPageGroupId.ToString();
            parametre.ad = tPageGroupId.ToString();
            if (currentDepartman.DepartmanCode != "DIA" && currentDepartman.DepartmanCode != "Fotograf")
            {
                parametre.ustveri = new ArsivMalzemeSvc.arsivMalzemesiUstveri()
                {
                    ustveri = CreateJsonFromRow(row),
                    ustveriTanimId = arsivNetUstVeriTanimId,
                    ustveriTanimIdSpecified = true
                };
            }
            parametre.ustveri.ustveri = "{" + parametre.ustveri.ustveri + "}";
            FileInfo fileInfo = new FileInfo(pdfPath);
            parametre.arsivMalzemesiMTOMDetay = new ArsivMalzemeSvc.arsivMalzemesiDetayEkleMTOMParametre[1]
            {
                new ArsivMalzemeSvc.arsivMalzemesiDetayEkleMTOMParametre()
                {
                    aciklama = currentDepartman.DepartmanName,
                    binaryDosya =File.ReadAllBytes(pdfPath),
                    tempId =-1,
                    sira = 1,
                    siraSpecified = true,
                    dosyaAdi=tPageGroupId.ToString()+".pdf",
                    detayTipiId=arsivNetDetayTipId,
                    detayTipiIdSpecified = false
                    //detayTipi="FakulteKurulKararlari"
                }
            };
            parametre.fizikselDurumu = "H";
            parametre.gizlilikDerecesiKod = "H";
            parametre.ureticiId = arsivNetUreticiId;
            parametre.ureticiIdSpecified = true;
            int?[] dIds = new int?[1];
            dIds[0] = arsivNetArsivDizinId;
            parametre.arsivDiziniIdListe = dIds;
            int?[] kIds = new int?[0];
            parametre.arsivKlasorIdListe = kIds;
            return parametre;
        }

        string CreateJsonFromRow(DataRow row)
        {
            JObject fileData = new JObject();
            fieldMatches.ForEach(e =>
            {
                var docTypeIndexMap = docTypeIndexMaps.Where(ee => ee.tDocTypeIndexMapId == e.tDocTypeIndexMapId).FirstOrDefault();
                if (docTypeIndexMap != null)
                {
                    //if (docTypeIndexMap.FieldDataType == EnumFieldDataType.Sayi.GetHashCode())
                    //{
                    //    if (row[e.SBFieldName] != DBNull.Value)
                    //    {
                    //        fileData[e.GDFieldName] = row[e.SBFieldName].ToString();
                    //    }
                    //    else
                    //    {
                    //        fileData[e.GDFieldName] = "";
                    //    }
                    //}
                    //else if (docTypeIndexMap.FieldDataType == EnumFieldDataType.OndalıkSayi.GetHashCode())
                    //{
                    //    if (row[e.SBFieldName] != DBNull.Value)
                    //    {
                    //        fileData[e.GDFieldName] = (decimal)row[e.SBFieldName];
                    //    }
                    //    else
                    //    {
                    //        fileData[e.GDFieldName] = null;
                    //    }
                    //}
                    //else if (docTypeIndexMap.FieldDataType == EnumFieldDataType.DogruYanlis.GetHashCode())
                    //{
                    //    if (row[e.SBFieldName] != DBNull.Value)
                    //    {
                    //        fileData[e.GDFieldName] = (bool)row[e.SBFieldName];
                    //    }
                    //    else
                    //    {
                    //        fileData[e.GDFieldName] = null;
                    //    }
                    //}
                    //else if (docTypeIndexMap.FieldDataType == EnumFieldDataType.Metin.GetHashCode())
                    //{
                    //    if (row[e.SBFieldName] != DBNull.Value)
                    //    {
                    //        fileData[e.GDFieldName] = row[e.SBFieldName].ToString();
                    //    }
                    //    else
                    //    {
                    //        fileData[e.GDFieldName] = null;
                    //    }
                    //}
                    if (docTypeIndexMap.FieldDataType == EnumFieldDataType.Tarih.GetHashCode() || docTypeIndexMap.FieldDataType == EnumFieldDataType.TarihSaat.GetHashCode())
                    {
                        if (row[e.SBFieldName] != DBNull.Value)
                        {
                            fileData[e.GDFieldName] = (DateTime)row[e.SBFieldName];
                        }
                        else
                        {
                            fileData[e.GDFieldName] = null;
                        }
                    }
                    else
                    {
                        if (row[e.SBFieldName] != DBNull.Value)
                        {
                            fileData[e.GDFieldName] = row[e.SBFieldName].ToString();
                        }
                        else
                        {
                            fileData[e.GDFieldName] = null;
                        }
                    }
                }
                else// barkod alanı icin istisna
                {
                    if (row["BarcodeValue"] != DBNull.Value)
                    {
                        fileData["Barkod"] = row["BarcodeValue"].ToString();
                    }
                    else
                    {
                        fileData["Barkod"] = null;
                    }
                }
            });
            return fileData.ToString();
        }

        string CiftTirnakArasinaAl(string deger)
        {
            return "\"" + deger + "\"";
        }

        bool isNumeric(string inputVal, out int numberValue)
        {
            if (string.IsNullOrEmpty(inputVal))
            {
                numberValue = 0;
                return false;
            }
            return int.TryParse(inputVal, out numberValue);
        }


        int GetLookupMatchValue(int localValue, int GDTypeId)
        {
            return gDTransferSettingLookups.Where(e => e.GDTypeId == GDTypeId && e.tLookUpDataDetailsId == localValue).FirstOrDefault().tGDLookUpDataDetailsId;
        }

        private void toolStripButtonSettings_Click(object sender, EventArgs e)
        {
            FormSettings formSettings = new FormSettings();
            formSettings.ShowDialog();
        }
    }
}
