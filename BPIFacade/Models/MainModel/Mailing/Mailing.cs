namespace BPIFacade.Models.MainModel.Mailing
{
    public class Mailing
    {
        public string ModuleName { get; set; } = string.Empty;
        public string ActionName { get; set; } = string.Empty;
        public string Receiver { get; set; } = string.Empty;
        public string LocationID { get; set; } = string.Empty;
        public string MailSubject { get; set; } = string.Empty;
        public string MailBeginningBody { get; set; } = string.Empty;
        public string MailMainBody { get; set; } = string.Empty;
        public string MailFooter { get; set; } = string.Empty;
        public string MailNote { get; set; } = string.Empty;
    }

    public class EmailLine
    {
        public int LineNo { get; set; } = 0;
        public string userEmail { get; set; } = string.Empty;
    }

    public class CustomMailing
    {
        public string moduleName { get; set; } = string.Empty;
        public string actionName { get; set; } = string.Empty;
        public string locationId { get; set; } = string.Empty;

        public string from { get; set; } = string.Empty;
        public List<EmailLine> to { get; set; } = new();
        public List<EmailLine> cc { get; set; } = new();
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;

        public string OtherString { get; set; } = string.Empty;
        public DateTime OtherDate { get; set; } = DateTime.Now;
        public List<string> OtherListString { get; set; } = new();
    }
}
