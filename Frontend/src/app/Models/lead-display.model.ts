import { ColumnType, IGridSettings } from './common-grid.model';

export class LeadsDisplay {
  constructor(
    public id: number,
    public created: Date,
    public vendor: string,
    public leadSource: string,
    public clientName: string,
    public contactPerson: string,
    public amount: number,
    public phone: string,
    public email: string,
    public gafSalesRep: string,
    public substage: string,
    public lastActivity: Date,
    public createdBy: string
  ) {}
}

export const leadListGridSetting: IGridSettings = {
  columns: [
    { name: 'created', title: 'created', sort: true, type: ColumnType.DATEORG },
    { name: 'vendor', title: 'vendor', sort: false },
    { name: 'leadSource', title: 'lead Source', sort: true },
    { name: 'clientName', title: 'client Name', sort: true },
    { name: 'contactPerson', title: 'contact Person', sort: true },
    { name: 'amount', title: 'amount', sort: true },
    { name: 'phone', title: 'phone', sort: false },
    { name: 'email', title: 'email', sort: false },
    { name: 'gafSalesRep', title: 'gaf Sales Rep', sort: true },
    { name: 'substage', title: 'substage', sort: true },
    {
      name: 'lastActivity',
      title: 'lastActivity',
      sort: true,
      type: ColumnType.DATEORG,
    },
    {
      name: 'createdBy',
      title: 'created By',
      sort: true,
    },
  ],
  showPagination: true,
  pageSizeValues: [
    { pageNo: 10, text: '10 per page' },
    { pageNo: 25, text: '25 per page' },
    { pageNo: 50, text: '50 per page' },
    { pageNo: 100, text: '100 per page' },
  ],
};
