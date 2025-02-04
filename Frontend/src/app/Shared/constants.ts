export interface selectMenu {
  option: string;
  value: string | number | boolean;
}

export const menuBarItems = [
  {
    menuItem: 'Dashboard',
    imagePath: '../../assets/Images/icon-home.svg',
  },
  {
    menuItem: 'Applications',
    imagePath: '../../assets/Images/icon-applications.svg',
    routerPath: '/application',
  },
  {
    menuItem: 'Clients',
    imagePath: '../../assets/Images/icon-clients.svg',
  },
  {
    menuItem: 'Funders',
    imagePath: '../../assets/Images/icon-funder.svg',
    routerPath: '/funder',
  },
  {
    menuItem: 'Vendors',
    imagePath: '../../assets/Images/icon-vendors.svg',
  },
  {
    menuItem: 'Settings',
    imagePath: '../../assets/Images/icon-settings.svg',
    routerPath: '/settings',
  },
];

export interface SettingMenuType {
  menuItem: string;
  routerPath?: string;
}

export const settingSystemProperties: SettingMenuType[] = [
  { menuItem: 'Role Permission', routerPath: 'role' },
  { menuItem: 'Funding categories', routerPath: 'equipmentType' },
  { menuItem: 'Vendor industries' },
  { menuItem: 'Geared doc fee ' },
  { menuItem: 'Vendor rate charts' },
  { menuItem: 'Clients in arrears' },
  { menuItem: 'Delete a deal' },
  { menuItem: 'Configuration' },
  { menuItem: 'Privacy funder list' },
  { menuItem: 'Website Calculator' },
];

export const settingsSalesAndMarketing = [
  'Email Management',
  'Templates',
  'SMS Template',
  'Vendor user links',
];

export enum RoleEnum {
  GearedAdmin = 'Geared Admin',
  GearedSalesRep = 'Geared Sales Rep',
  GearedSuperAdmin = 'Geared Super Admin',
  VendorGuestUser = 'Vendor Guest User',
  VendorManager = 'Vendor Manager',
  VendorSalesRep = 'Vendor Sales Rep',
}

export const SiteRolesId = {
  GearedAdmin: 1,
  GearedSalesRep: 2,
  GearedSuperAdmin: 3,
  VendorGuestUser: 4,
  VendorManager: 5,
  VendorSalesRep: 6,
};
export enum MonthEnum {
  Jan = 1,
  Feb = 2,
  Mar = 3,
  Apr = 4,
  May = 5,
  Jun = 6,
  Jul = 7,
  Aug = 8,
  Sep = 9,
  Oct = 10,
  Nov = 11,
  Dec = 12,
}

export const roleSelectionMenu = [
  { option: RoleEnum.GearedAdmin, value: RoleEnum.GearedAdmin },
  {
    option: RoleEnum.GearedSalesRep,
    value: RoleEnum.GearedSalesRep,
  },
  {
    option: RoleEnum.GearedSuperAdmin,
    value: RoleEnum.GearedSuperAdmin,
  },
  {
    option: RoleEnum.VendorGuestUser,
    value: RoleEnum.VendorGuestUser,
  },
  {
    option: RoleEnum.VendorManager,
    value: RoleEnum.VendorManager,
  },
  {
    option: RoleEnum.VendorSalesRep,
    value: RoleEnum.VendorSalesRep,
  },
];

export const dateSelectionMenu = (days: number) => {
  let dateArray = [];
  for (let i = 1; i <= days; i++) {
    dateArray.push({ option: i.toString(), value: i });
  }

  return dateArray;
};
export const monthSelectionMenu = (month: number): selectMenu[] => {
  let monthArray: selectMenu[] = [];
  if (month == -1) {
    Object.keys(MonthEnum).forEach((key) => {
      const value = MonthEnum[key as keyof typeof MonthEnum];
      if (typeof value === 'number') {
        monthArray.push({ option: key, value: value });
      }
    });

    return monthArray;
  }

  let monthDays: number;
  switch (month) {
    case MonthEnum.Feb:
      monthDays = isLeapYear(new Date().getFullYear()) ? 29 : 28;
      break;
    case MonthEnum.Apr:
    case MonthEnum.Jun:
    case MonthEnum.Sep:
    case MonthEnum.Nov:
      monthDays = 30;
      break;
    default:
      monthDays = 31;
  }
  Object.keys(MonthEnum).forEach((key) => {
    const value = MonthEnum[key as keyof typeof MonthEnum];
    if (typeof value === 'number') {
      if (
        monthDays == new Date(new Date().getFullYear(), value + 1, 0).getDate()
      ) {
        monthArray.push({ option: key, value: value });
      }
    }
  });
  return monthArray;
};

function isLeapYear(year: number): boolean {
  return (year % 4 === 0 && year % 100 !== 0) || year % 400 === 0;
}

export const notificationPreSelectionMenu = [
  { option: 'None', value: 0 },
  { option: 'Email', value: 1 },
  { option: 'SMS', value: 2 },
  { option: 'Email & SMS', value: 3 },
];

export const validationRegexes = {
  EMAIL_REGEX:
    /^(?!\.)(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|"(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])$/,
  PASSWORD_REGEX:
    /^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,})/,
};

export const recordsPerPage = [
  { option: '25 Per Page', value: 25 },
  { option: '50 Per Page', value: 50 },
  { option: '100 Per Page', value: 100 },
];

export const alertResponses = {
  DELETION_CONFIRMATION: 'Are you sure you want to delete this record?',
  ADD_RECORD: 'Record added successfully',
  UPDATE_RECORD: 'Record updated successfully',
  DELETE_RECORD: 'Record deleted successfully',
  ERROR: 'Something went wrong',
  ON_LOGIN_ERROR: 'Please provide valid credentials',
  ON_OTP_SUCCESS: 'Otp sent successfully',
  ON_OTP_INVALID: 'Invalid Otp!',
  ON_EMAIL_NOT_EXIST: 'Given username not exists',
  ON_PASSWORD_CHANGE_SUCCESS: 'Password updated successfully',
  ON_FORM_INVALID: 'Please enter all mandatory details: \n',
  INVALID_DOC_TYPE_IMG: 'Please upload valid image doc',
  INVALID_DOC_TYPE_PDF: 'Please upload valid pdf file',
  UNSAVE_CONFIRMATION:
    'You have unsaved changes. Do you want to leave without saving?',
  DOC_UPLOAD_SUCCESS: 'Document uploaded successfully',
  DOC_DELETE_SUCCESS: 'Document deleted successfully',
  INTEREST_CHART_SAVED: 'Interest chart(s) saved',
  INTEREST_CHART_DELETE: 'Interest chart deleted',
};

export const errorResponses = {
  CLIEENTSIDE_ERROR: 'Client-side error',
  BAD_REQUEST: 'Bad Request',
  FORBIDDEN: 'Forbidden',
  UNAUTHORIZED: 'Forbidden',
  NOT_FOUND: 'Not Found',
  INTERNAL_SERVER_ERROR: 'Internal Server Error',
  CONFLICT_ERROR: "Request can't be fulfilled ",
  UNKNOWN_ERROR: 'Unknown Error',
};

export const modalTitles = {
  ADDEQUIPMENTTYPE: 'ADD FUNDING/EQUIPMENT',
  DELETEEQUIPMENTTYPE: 'DELETE EQUIPMENT TYPE',
};

export const statusSelectMenu: selectMenu[] = [
  { option: 'Active', value: true },
  { option: 'Inactive', value: false },
];

export const MAPS_ADDRESS_TYPES = {
  ADMINISTRATIVE_AREA_LEVEL_1: 'administrative_area_level_1',
  POSTAL_CODE: 'postal_code',
  LOCALITY: 'locality',
  ROUTE: 'route',
  STREET_NUMBER: 'street_number',
};

export const AddressOptions = {
  componentRestrictions: {
    country: ['AU'],
  },
};
export const ckEditorConfig: CKEDITOR.config = {
  forcePasteAsPlainText: true,
  toolbar: [
    { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline'] },
    { name: 'lists', items: ['NumberedList', 'BulletedList'] },
    { name: 'insert', items: ['HorizontalRule'] },
  ],
};
export const FunderModuleConstants = {
  ACTIVE_OVERVIEW_TAB: 'overview',
  ACTIVE_FUNDER_PRODUCT_GUIDE_TAB: 'funderProductGuide',
  CHOSEN_FUNDING_TITLE: 'Chosen Funding <span class="text-danger">*</span>',
  FUNDER_OVERVIEW: 'funderOverview',
  ACTIVE_FUNDER_INTEREST_RATE_CHART_TAB: 'funderInterestRateChart',
};
export const FunderChartConstants = {
  TERM_DEFAULT: 'Term',
  TERM_CHATTEL_MORTGAGE: 'Term - Chattel mortgage',
  TERM_CHATTEL_RENTAL: 'Term - Chattel mortgage & Rental',
  TERM_RATE_DEFAULT: 'Interest rates',
  TERM_RATE_CHATTEL_MORTGAGE: 'chattel mortgage interest rates',
  TERM_RATE_CHATTEL_RENTAL: 'chattel mortgage & rental interest rates',
  INTEREST_CHART_CHATTEL_MORTGAGE: 'interestChartChattelMortgage',
  INTEREST_CHART_RENTAL: 'interestChartRental',
  CHATTEL_MORTGAGE_TERMS: 'chattelMortgageTerms',
  RENTAL_TERMS: 'rentalTerms',
};

export const pipelineList: selectMenu[] = [
  { option: 'All Pipelines', value: 0 },
  { option: 'Internal Pipeline', value: 1 },
  { option: 'External Pipeline', value: 2 },
];

export const leadTypeList: selectMenu[] = [
  { option: 'All Leads', value: 0 },
  { option: 'My Leads', value: 1 },
];

export const applicationTypelist: selectMenu[] = [
  { option: 'Hide unassigned applications', value: 0 },
  { option: 'Show unassigned applications', value: 1 },
  { option: 'Show unassigned applications only', value: 2 },
];

export const recordTypeList: string[] = [
  'Today',
  'Yesterday',
  'This week',
  'Last week',
  'This month',
  'Last month',
  'This quarter',
  'Last quarter',
  'This year',
  'Last year',
  'Last 7 days',
  'Last 14 days',
  'Last 30 days',
  'Last 60 days',
  'Last 90 days',
];

// extensions methods..

declare global {
  interface Array<T> {
    isNotEmpty(): boolean;
  }
  interface NumberConstructor {
    INT_MAX_VALUE: number;
  }
  interface StringConstructor {
    Empty: string;
  }
}

Number.INT_MAX_VALUE = 2147483647;
String.Empty = '';
Array.prototype.isNotEmpty = function (): boolean {
  return this && this.length > 0;
};
