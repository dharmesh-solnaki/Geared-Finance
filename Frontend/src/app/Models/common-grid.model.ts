export interface IGridSettings {
  columns: GridColumn[];
  showPagination?: boolean;
  showEquipmentTypeDelete?: boolean;
  showEquipmentTypeSave?: boolean;
  showEquipmentTypeEdit?: boolean;
  showRolePermissionEdit?: boolean;
  isShowIndex?: boolean;
  showDelete?: boolean;
  showDownload?: boolean;
  showDocument?: boolean;
  showNoteEdit?: boolean;
  showNoteDelete?: boolean;
  pageSizeValues?: { pageNo: number; text: string }[];
}

export class PaginationSetting {
  public totalRecords: number;
  public currentPage: number;
  selectedPageSize?: string[];
  constructor() {
    this.currentPage = 1;
    this.totalRecords = 0;
  }
}

export class GridColumn {
  name: string;
  title: string;
  type?: ColumnType;
  sort: boolean;
  filterType?: string;

  constructor() {
    this.sort = false;
    this.name = '';
    this.title = '';
  }
}

export enum ColumnType {
  STATUS = 'STATUS',
  LOGINSTATUS = 'LOGINSTATUS',
  EQUIPMENTTYPE = 'EQUIPMENTTYPE',
  NOTE = 'NOTE',
  DATE = 'DATE',
}

export enum SortOrder {
  ASC = 'ASC',
  DESC = 'DESC',
  Default = 'Default',
}
export class SortConfiguration {
  constructor() {
    this.sort = '';
    this.sortOrder = SortOrder.Default;
  }
  sort: string;
  sortOrder: SortOrder;
}

// export const dummyDataGeneration = () => {
//   const hundredRecords = [];
//   for (let i = 0; i < 200; i++) {
//     const record = {
//       name: `User${i + 1}`,
//       sureName: 'Doe',
//       staffCode: `USR${i + 1}`,
//       vendor: i % 2 === 0 ? `Vendor ${(i % 5) + 1}` : '',
//       relationshipManager: i % 3 === 0 ? `Manager ${(i % 4) + 1}` : '',
//       role:
//         i % 6 === 0
//           ? RoleEnum.GearedAdmin
//           : i % 5 === 0
//           ? RoleEnum.VendorManager
//           : RoleEnum.GearedSalesRep,
//       email: `user${i + 1}@example.com`,
//       status: i % 3 === 0 ? 1 : 0,
//       portalLogin: i % 4 === 0 ? 1 : 0,
//     };
//     hundredRecords.push(record);
//   }
//   return hundredRecords;
// };

// export const dummyColumnGridSetting: IGridSettings = {
//   columns: [
//     { name: 'name', title: 'name', sort: true },
//     { name: 'sureName', title: 'sureName', sort: false },
//     { name: 'staffCode', title: 'staffcode', sort: false },

//     { name: 'vendor', title: 'vendor', sort: false },
//     { name: 'relationshipManager', title: 'relationshipManager', sort: false },
//     { name: 'role', title: 'role', sort: true },

//     { name: 'email', title: 'email', sort: true },
//     { name: 'status', title: 'status', sort: false, type: ColumnType.STATUS },
//     {
//       name: 'portalLogin',
//       title: 'portalLogin',
//       sort: true,
//       type: ColumnType.LOGINSTATUS,
//     },
//   ],
//   showPagination: true,
//   pageSizeValues: [
//     { pageNo: 10, text: '10 per page' },
//     { pageNo: 25, text: '25 per page' },
//     { pageNo: 50, text: '50 per page' },
//     { pageNo: 100, text: '100 per page' },
//   ],
// };
