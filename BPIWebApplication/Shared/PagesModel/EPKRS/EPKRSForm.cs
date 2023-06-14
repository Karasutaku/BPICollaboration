using System.ComponentModel.DataAnnotations;

namespace BPIWebApplication.Shared.PagesModel.EPKRS
{
    public class ItemCaseForm
    {
        public string SubRiskID { get; set; } = string.Empty;
        public string DocumentID { get; set; } = string.Empty;
        [Required(ErrorMessage = "Kolom Site Pelapor Kosong!")]
        public string SiteReporter { get; set; } = string.Empty;
        [Required(ErrorMessage = "Kolom Site Pengirim Kosong!")]
        public string SiteSender { get; set; } = string.Empty;
        [Required(ErrorMessage = "Kolom Tanggal Laporan Kosong!")]
        public DateTime ReportDate { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Kolom Tanggal Pengambilan Barang Kosong!")]
        public DateTime ItemPickupDate { get; set; } = DateTime.Now;
        public string LoadingDocumentID { get; set; } = string.Empty;
        public DateTime LoadingDocumentDate { get; set; } = DateTime.Now;
        public string ExtendedMitigationPlan { get; set; } = string.Empty;
        public string DocumentStatus { get; set; } = string.Empty;
    }

    public class ItemLineForm
    {
        private bool late = false;
        private bool cctv = false;
        private bool reported = false;

        public string DocumentID { get; set; } = string.Empty;
        public int LineNum { get; set; } = 0;
        public string TRID { get; set; } = string.Empty;
        public DateTime TRDate { get; set; } = DateTime.Now;
        public string ItemCode { get; set; } = string.Empty;
        public string ItemDescription { get; set; } = string.Empty;
        public string ItemRiskCategoryID { get; set; } = string.Empty;
        public string CategoryID { get; set; } = string.Empty;
        public int ItemQuantity { get; set; } = 0;
        public string UOM { get; set; } = string.Empty;
        public decimal ItemValue { get; set; } = decimal.Zero;
        public int ItemStock { get; set; } = 0;
        public int VarianceDate { get; set; } = 0;
        public string isLate { get => late ? "TRUE" : "FALSE"; set { if (bool.TryParse(value.ToString(), out var result)) late = result; } }
        public string isCCTVCoverable { get => cctv ? "TRUE" : "FALSE"; set { if (bool.TryParse(value.ToString(), out var result)) cctv = result; } }
        public string isReportedtoSender { get => reported ? "TRUE" : "FALSE"; set { if (bool.TryParse(value.ToString(), out var result)) reported = result; } }
    }

    public class IncidentAccidentForm
    {
        public string SubRiskID { get; set; } = string.Empty;
        public string DocumentID { get; set; } = string.Empty;
        [Required(ErrorMessage = "Tanggal Pelaporan Kosong!")]
        public DateTime ReportDate { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Tanggal Kejadian Kosong!")]
        public DateTime OccurenceDate { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Site Pelapor Kosong!")]
        public string SiteReporter { get; set; } = string.Empty;
        [Required(ErrorMessage = "Departemen Pelapor Kosong!")]
        public string DepartmentReporter { get; set; } = string.Empty;
        [Required(ErrorMessage = "Nama Risk RP Kosong!")]
        public string RiskRPName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email Risk RP Kosong!")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Format Harus EMAIL!")]
        [EmailAddress]
        public string RiskRPEmail { get; set; } = string.Empty;
        [Required(ErrorMessage = "Nama DORM Kosong!")]
        public string DORMName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Tanggal Pelaporan Kosong!")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Format Harus EMAIL!")]
        [EmailAddress]
        public string DORMEmail { get; set; } = string.Empty;
        [Required(ErrorMessage = "Deskripsi Kejadian Kosong!")]
        public string CaseDescription { get; set; } = string.Empty;
        [Required(ErrorMessage = "Departemen Terdampak Kosong!")]
        public string DepartmentAffected { get; set; } = string.Empty;
        [Required(ErrorMessage = "Kronologi Kosong!")]
        public string Cronology { get; set; } = string.Empty;
        [Required(ErrorMessage = "Root Cause Kosong!")]
        public string RootCause { get; set; } = string.Empty;
        [Required(ErrorMessage = "Deskripsi Kerugian Kosong!")]
        public string LossDescription { get; set; } = string.Empty;
        public decimal LossEstimation { get; set; } = decimal.Zero;
        public decimal ReturnAmount { get; set; } = decimal.Zero;
        [Required(ErrorMessage = "Deskripsi Risiko Kosong!")]
        public string RiskDescription { get; set; } = string.Empty;
        [Required(ErrorMessage = "Dampak Kejadian Kosong!")]
        public string CauseDescription { get; set; } = string.Empty;
        [Required(ErrorMessage = "Tanggal Pelaporan Kosong!")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Format Harus EMAIL!")]
        [EmailAddress]
        public string PIC { get; set; } = string.Empty;
        [Required(ErrorMessage = "Action Plan Kosong!")]
        public string ActionPlan { get; set; } = string.Empty;
        [Required(ErrorMessage = "Tanggal Target Kosong!")]
        public DateTime TargetDate { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Tanggal Mitigasi Kosong!")]
        public string MitigationPlan { get; set; } = string.Empty;
        [Required(ErrorMessage = "Tanggal Mitigasi Kosong!")]
        public DateTime MitigationDate { get; set; } = DateTime.Now;
        public string ExtendedRootCause { get; set; } = string.Empty;
        public string ExtendedMitigationPlan { get; set; } = string.Empty;
        public string DocumentStatus { get; set; } = string.Empty;
    }

    public class CaseAttachmentForm
    {
        public string DocumentID { get; set; } = string.Empty;
        public int LineNum { get; set; } = 0;
        public DateTime UploadDate { get; set; } = DateTime.Now;
        public string FileExtension { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
    }

    public class DocumentDiscussionForm
    {
        public string formId { get; set; } = string.Empty;
        public string rowGuid { get; set; } = string.Empty;
        public string DocumentID { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime CommentDate { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Data is required")]
        public string Comment { get; set; } = string.Empty;
        public bool isEdited { get; set; } = false;
        public string ReplyRowGuid { get; set; } = string.Empty;
    }
}
