import { ColumnType, IGridSettings } from './common-grid.model';

export class Funder {
  constructor(
    public id: number,
    public funder: string,
    public legalName: string,
    public financeType: string,
    public bdmName: string,
    public bdmEmail: string,
    public bdmPhone: string,
    public status: boolean
  ) {}
}

export const FunderGridSettings: IGridSettings = {
  columns: [
    { name: 'funder', title: 'funder', sort: true },
    { name: 'legalName', title: 'legalName', sort: true },
    { name: 'financeType', title: 'financeType', sort: false },

    { name: 'bdmName', title: 'bdmName', sort: true },
    {
      name: 'bdmEmail',
      title: 'bdmEmail',
      sort: true,
    },
    { name: 'bdmPhone', title: 'bdmPhone', sort: true },
    { name: 'status', title: 'status', sort: false, type: ColumnType.STATUS },
  ],
  showPagination: true,
  pageSizeValues: [
    { pageNo: 10, text: '10 per pager' },
    { pageNo: 25, text: '25 per pager' },
    { pageNo: 50, text: '50 per pager' },
    { pageNo: 100, text: '100 per pager' },
  ],
};
