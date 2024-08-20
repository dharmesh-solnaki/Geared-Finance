namespace Utilities
{
    public static class Constants
    {
        public const string EMAIL_REGEX = "/^(?!\\.)(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)\\])$/";
        public const string RECORD_NOT_FOUND = "Record(s) Not Found";
        public const string BAD_REQUEST = "Bad Request";
        public const string INTERNAL_SERVER_ERR = "An unexpected error occurred. Please try again later.";
    }

    public static class JWTTokenClaims
    {
        public const string USER_ROLE = "userRole";
        public const string USER_ID = "userId";
        public const string USER_NAME = "userName";
        public const string REF_TOKEN_EXP_TIME = "refreshTokenExp";
        public const string ROLE_ID = "roleId";
    }

    public static class PermissionConstants
    {
        public const string CAN_VIEW = "CanView";
        public const string CAN_ADD = "CanAdd";
        public const string CAN_EDIT = "CanEdit";
        public const string CAN_DELETE = "CanDelete";
        public const string CAN_UPSERT = "CanUpsert";
    }

    public static class RoleConstants
    {
        public const int GEARED_ADMIN = 1;
        public const int GEARED_SALES_REP = 2;
        public const int GEARED_SUPERADMIN = 3;
        public const int VENDOR_GUEST_USER = 4;
        public const int VENDOR_MANAGER = 5;
        public const int VENDOR_SALES_REP = 6;
    }
    public static class ModuleConstants
    {
        public const string DASHBOARD = "Dashboard";
        public const string KANBAN = "Kanban";
        public const string LEADS_TAB = "Leads Tab";
        public const string QUOTE_TABGAF = "Quote Tab Gaf";
        public const string QUOTE_TABVEND = "Quote Tab Vend";
        public const string APPLICATION_TAB = "Application Tab";
        public const string DEAL_TAB = "Deal Tab";
        public const string LEADS_LIST = "Leads List";
        public const string QUOTES_LIST = "Quotes List";
        public const string APPLICATION_LIST = "Application List";
        public const string DEAL_LIST = "Deal List";
        public const string CLIENTS = "Clients";
        public const string DIRECTORS = "Directors";
        public const string FUNDERS = "Funders";
        public const string VENDORS = "Vendors";
        public const string DOCUMNETS = "Documents";
        public const string TASKS = "Tasks";
        public const string TICKETS = "Tickets";
        public const string REPORTS = "Reports";
        public const string HELP = "Help";
        public const string EDIT_PROFILE = "Edit profile";
        public const string SETTINGS = "Settings";
        public const string COMMISSION = "Commission";
        public const string BROCHURES = "Brochures";
    }

}
