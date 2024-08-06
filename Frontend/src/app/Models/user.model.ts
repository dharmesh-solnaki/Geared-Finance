import { ColumnType, IGridSettings } from "src/app/Models/common-grid.model";
import { Vendor } from "./common-models";
import { ManagerLevel } from "./ManagerLevel.model";

export class User {
  constructor(
    public id:number,
    public name: string,
    public surName: string,
    public email: string,
    public mobile: string,
    public password?: string,
    public notificationPreferences: number = 0,
    public status: boolean = true,
    public isPortalLogin?: boolean,
    public isUserInGafsalesRepList?: boolean,
    public dayOfBirth?: number,
    public monthOfBirth?: number,
    public relationshipManager?: number,
    public reportingTo?: number,
    public isUserInVendorSalesRepList?: boolean,
    public unassignedApplications?: boolean,
    public roleName?: number,
    public isSendEndOfTermReport?: boolean,
    public isFunderProfile?: boolean,
    public isProceedBtnInApp?: boolean,
    public isCalcRateEditor?: boolean,
    public staffCode?:string,
    public vendorId?:number,
    public vendorManagerLevelId?:number,
    public vendor?:Vendor,
    public managerLevels?:ManagerLevel,
    public venodrName?:string,
    public relationshipManagerName?:string
  )
  {}
}


export const UserGridSetting:IGridSettings={
  columns: [
    { name: 'name', title: 'name', sort: true },
    { name: 'surName', title: 'surName', sort: true },
    { name: 'staffCode', title: 'staff code', sort: false },

    { name: 'venodrName', title: 'vendor', sort: false },
    { name: 'relationshipManagerName', title: 'relationship Manager', sort: false },
    { name: 'roleName', title: 'role', sort: true },

    { name: 'email', title: 'email', sort: true },
    { name: 'status', title: 'status', sort: false, type: ColumnType.STATUS },
    {
      name: 'isPortalLogin',
      title: 'portalLogin',
      sort: true,
      type: ColumnType.LOGINSTATUS,
    },
  ],
  showPagination:true,
  pageSizeValues:[
    { pageNo: 10, text: '10 per pager' },
    { pageNo: 25, text: '25 per pager' },
    { pageNo: 50, text: '50 per pager' },
    { pageNo: 100, text: '100 per pager' },
  ]
}